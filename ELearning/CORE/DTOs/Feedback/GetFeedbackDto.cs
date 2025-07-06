namespace CORE.DTOs.Feedback
{
    public class GetFeedbackDto
    {
        public int Id { get; set; }
        public int FeedbackerId { get; set; }
        public int FeedbackedId { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
