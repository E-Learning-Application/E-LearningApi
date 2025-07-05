namespace CORE.DTOs.Message
{
    public class MessageAddRequest
    {
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string Message { get; set; }

    }
}
