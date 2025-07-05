using CORE.DTOs.Message;

namespace CORE.Services.IServices
{
    public interface IMessageService
    {
        Task<MessageResponse> CreateAsync(MessageAddRequest? request);
        //like whatsapp main page
        Task<IEnumerable<ChatSummaryResponse>> GetChatListAsync(int userId);

        Task<IEnumerable<MessageResponse>> GetAllMessageAsync(int userID);

        Task<IEnumerable<MessageResponse>> GetAllChatAsync(int userID, int receiverID);

        Task<MessageResponse> GetByAsync(int messageID);

        Task<bool> DeleteMessageAsync(int messageID, int currentUserId);

        Task<bool> MarkMessageAsReadAsync(int messageID);

        Task<int> GetUnreadCountAsync(int userId);

        Task<MessageResponse?> GetLastMessageWithUserAsync(int userId, int otherUserId);
    }
}
