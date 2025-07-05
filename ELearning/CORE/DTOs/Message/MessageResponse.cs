namespace CORE.DTOs.Message
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public int SenderID { get; set; }
        public string SenderUserName { get; set; }
        public string? SenderImagePath { get; set; }
        public int ReceiverID { get; set; }
        public string ReceiverUserName { get; set; }
        public string? ReceiverImagePath { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
