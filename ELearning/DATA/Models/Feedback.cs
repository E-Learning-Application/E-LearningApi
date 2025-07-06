using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int FeedbackerId { get; set; }//the user who gave the feedback
        public AppUser Feedbacker { get; set; }
        public int FeedbackedId { get; set; }//the user who received the feedback
        public AppUser Feedbacked { get; set; }
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
