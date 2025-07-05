using CORE.DTOs.UserMatch;

namespace CORE.Services.IServices
{
    public interface IMatchingService
    {
        Task<UserMatchResponse> FindMatchAsync(UserMatchRequest request);
        Task<UserMatchResponse> CreateMatchAsync(int userId1, int userId2, string matchType, double matchScore);
        Task<IEnumerable<UserMatchResponse>> GetMatchesAsync(int userId);
        Task<bool> EndMatchAsync(int matchId);
       
    }
}
