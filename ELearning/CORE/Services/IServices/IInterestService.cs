using CORE.DTOs.Interest;

namespace CORE.Services.IServices
{
    public interface IInterestService
    {
        Task<InterestResponse> AddInterestAsync(InterestAddRequest request);
        Task<UserInterestResponse> AddUserInterestAsync(UserInterestAddRequest request);
        Task<IEnumerable<InterestResponse>> GetUserInterestsAsync(int userId);
    }
}
