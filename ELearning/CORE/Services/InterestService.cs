using AutoMapper;
using CORE.DTOs.Interest;
using CORE.Exceptions;
using CORE.Services.IServices;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;

namespace CORE.Services
{
    public class InterestService(IUnitOfWork _unitOfWork, IMapper _mapper) : IInterestService
    {
        public async Task<InterestResponse> AddInterestAsync(InterestAddRequest request)
        {
            if (request == null) throw new ArgumentNullException("Request can't be null");

            var existingInterest = await _unitOfWork.Interests.FindAsync(i => i.Name == request.Name);
            if (existingInterest != null)
                return _mapper.Map<InterestResponse>(existingInterest);

            var interest = _mapper.Map<Interest>(request);
            await _unitOfWork.Interests.AddOrUpdateAsync(interest);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<InterestResponse>(interest);
        }

        public async Task<UserInterestResponse> AddUserInterestAsync(UserInterestAddRequest request)
        {
            if (request == null) throw new ArgumentNullException("Request can't be null");

            var user = await _unitOfWork.AppUsers.FindAsync(u => u.Id == request.UserId)
                ?? throw new NotFoundException("User not found");
            var interest = await _unitOfWork.Interests.FindAsync(i => i.Id == request.InterestId)
                ?? throw new NotFoundException("Interest not found");

            var existingUserInterest = await _unitOfWork.UserInterests.FindAsync(ui => ui.UserId == request.UserId && ui.InterestId == request.InterestId);
            if (existingUserInterest != null)
                return _mapper.Map<UserInterestResponse>(existingUserInterest);

            var userInterest = _mapper.Map<UserInterest>(request);
            await _unitOfWork.UserInterests.AddOrUpdateAsync(userInterest);
            await _unitOfWork.CommitAsync();

            userInterest.User = user;
            userInterest.Interest = interest;
            return _mapper.Map<UserInterestResponse>(userInterest);
        }

        public async Task<IEnumerable<InterestResponse>> GetInterestsAsync()
        {
            var interests = await _unitOfWork.Interests.GetAllAsync(i=>true);
            return _mapper.Map<IEnumerable<InterestResponse>>(interests);
        }

        public async Task<IEnumerable<UserInterestResponse>> GetUserInterestsAsync(int userId)
        {
            var userInterests = await _unitOfWork.UserInterests.GetAllAsync(
                ui => ui.UserId == userId,
                includes: new[] { "Interest","User" });

            return _mapper.Map<IEnumerable<UserInterestResponse>>(userInterests);
        }
    }
}
