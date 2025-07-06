namespace CORE.DTOs.Feedback
{
    public class CreateFeedbackDto
    {
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public int FeedbackedId { get; set; } // the user who received the feedback
    }
}
