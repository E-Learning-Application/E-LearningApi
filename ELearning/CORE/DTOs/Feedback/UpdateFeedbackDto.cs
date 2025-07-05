namespace CORE.DTOs.Feedback
{
    public class UpdateFeedbackDto
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }
    }
}
