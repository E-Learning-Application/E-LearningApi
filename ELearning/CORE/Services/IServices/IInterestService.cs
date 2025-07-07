using CORE.DTOs.Interest;

namespace CORE.Services.IServices
{
    public interface IInterestService
    {
        Task<InterestResponse> AddInterestAsync(InterestAddRequest request);
        Task<IEnumerable<InterestResponse>> GetInterestsAsync();
        Task<UserInterestResponse> AddUserInterestAsync(UserInterestAddRequest request);
        Task<IEnumerable<UserInterestResponse>> GetUserInterestsAsync(int userId);
    }
}
