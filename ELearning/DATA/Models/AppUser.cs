using Microsoft.AspNetCore.Identity;
using DATA.Models.Contract;

namespace DATA.Models
{
    public class AppUser : IdentityUser<int>, ISoftDeleteable
    {

        public string? ImagePath { get; set; }
        public string? Bio { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DateDeleted { get; set; }
        public ICollection<Language>? LanguagePreferences { get; set; }
        public ICollection<Report> ReportsMade { get; set; } = new List<Report>();
        public ICollection<Report> ReportsReceived { get; set; } = new List<Report>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
        public ICollection<Message> MessagesReceived { get; set; } = new List<Message>();
    }
}
