using AutoMapper;
using CORE.DTOs.Interest;
using CORE.DTOs.UserMatch;
using CORE.Exceptions;
using CORE.Services.IServices;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;
using DATA.Models.Enums;
using Microsoft.Extensions.Logging;

namespace CORE.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MatchingService> _logger;
        public MatchingService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MatchingService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserMatchResponse> FindMatchAsync(UserMatchRequest request)
        {
            if (request == null) throw new ArgumentNullException("Request can't be null");

            var user = await _unitOfWork.AppUsers.FindAsync(
                u => u.Id == request.UserId && !u.IsDeleted,
                includes: new[] { "LanguagePreferences", "UserInterests.Interest" })
                ?? throw new NotFoundException("User not found");

            var candidates = await _unitOfWork.AppUsers.GetAllAsync(
                u => u.Id != request.UserId && !u.IsDeleted && u.IsOnline,
                includes: new[] { "LanguagePreferences", "UserInterests.Interest" });

            var bestMatch = candidates
                .Select(c => new
                {
                    Candidate = c,
                    Score = CalculateMatchScore(user, c)
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (bestMatch == null || bestMatch.Score < 0.25) // Minimum score for valid matching
                throw new NotFoundException("No suitable match found");

            var match = await CreateMatchAsync(user.Id, bestMatch.Candidate.Id, request.MatchType, bestMatch.Score);
            return match;
        }

        public async Task<UserMatchResponse> CreateMatchAsync(int userId1, int userId2, string matchType, double matchScore)
        {
            if (userId1 == userId2) throw new ArgumentException("Users cannot match themselves");

            var user1 = await _unitOfWork.AppUsers.FindAsync(u => u.Id == userId1)
                ?? throw new NotFoundException("User1 not found");
            var user2 = await _unitOfWork.AppUsers.FindAsync(u => u.Id == userId2)
                ?? throw new NotFoundException("User2 not found");

            var existingMatch = await _unitOfWork.UserMatches.FindAsync(
                m => (m.UserId1 == userId1 && m.UserId2 == userId2) || (m.UserId1 == userId2 && m.UserId2 == userId1) && m.IsActive);

            if (existingMatch != null)
                return _mapper.Map<UserMatchResponse>(existingMatch);

            var match = new UserMatch
            {
                UserId1 = userId1,
                UserId2 = userId2,
                MatchType = matchType,
                MatchScore = matchScore,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.UserMatches.AddOrUpdateAsync(match);
            await _unitOfWork.CommitAsync();

            match.User1 = user1;
            match.User2 = user2;
            return _mapper.Map<UserMatchResponse>(match);
        }

        public async Task<IEnumerable<UserMatchResponse>> GetMatchesAsync(int userId)
        {
            var matches = await _unitOfWork.UserMatches.GetAllAsync(
                m => (m.UserId1 == userId || m.UserId2 == userId) && m.IsActive,
                includes: new[] { "User1", "User2" });

            return _mapper.Map<IEnumerable<UserMatchResponse>>(matches);
        }

        public async Task<bool> EndMatchAsync(int matchId)
        {
            var match = await _unitOfWork.UserMatches.FindAsync(m => m.Id == matchId)
                ?? throw new NotFoundException("Match not found");

            match.IsActive = false;
            await _unitOfWork.UserMatches.AddOrUpdateAsync(match);
            var changes = await _unitOfWork.CommitAsync();
            return changes > 0;
        }

       

        private double CalculateMatchScore(AppUser user1, AppUser user2)
        {
            double score = 0;

            // Language compatibility (50% weight)
            var user1Prefs = user1.LanguagePreferences.Select(x =>
            {
                return new
                {
                    Id = x.LanguageId,
                    ProficiencyLevel = x.ProficiencyLevel,
                    IsLearning = x.IsLearning
                };
            });
            var user2Prefs = user2.LanguagePreferences.Select(x =>
            {
                return new
                {
                    Id = x.LanguageId,
                    ProficiencyLevel = x.ProficiencyLevel,
                    IsLearning = x.IsLearning
                };
            });

            foreach (var pref1 in user1Prefs)
            {
                foreach (var pref2 in user2Prefs)
                {
                    if (pref1.Id == pref2.Id && pref1.IsLearning != pref2.IsLearning)
                    {
                        // Complementary proficiency (e.g., native vs. learning)
                        double proficiencyScore = pref1.ProficiencyLevel == LanguageProficiencyLevel.Native && pref2.ProficiencyLevel != LanguageProficiencyLevel.Native ? 0.5 : 0.3;
                        score += proficiencyScore;
                    }
                }
            }

            // Shared interests (50% weight)
            var user1Interests = user1.UserInterests.Select(ui => ui.InterestId).ToList();
            var user2Interests = user2.UserInterests.Select(ui => ui.InterestId).ToList();
            var sharedInterests = user1Interests.Intersect(user2Interests).Count();
            score += (sharedInterests * 0.1); // 0.1 per shared interest

            return Math.Min(score, 1.0); // Normalize to 0-1
        }
    }
}
