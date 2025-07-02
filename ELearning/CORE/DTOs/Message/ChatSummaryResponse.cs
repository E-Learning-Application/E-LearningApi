namespace CORE.DTOs.Message
{
    public class ChatSummaryResponse
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? ProfileImage { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
    }

}
