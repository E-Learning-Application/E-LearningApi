namespace DATA.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderID { get; set; }
        public AppUser Sender { get; set; }
        public int ReceiverID { get; set; }
        public AppUser Receiver { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public bool IsDeletedBySender { get; set; } = false;
        public bool IsDeletedByReceiver { get; set; } = false;
    }
}
