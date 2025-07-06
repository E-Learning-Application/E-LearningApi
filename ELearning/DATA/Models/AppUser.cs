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
        public bool IsOnline { get; set; } = false; // Added for online status
        public ICollection<LanguagePreference>? LanguagePreferences { get; set; } = new List<LanguagePreference>(); 
        public ICollection<Report> ReportsMade { get; set; } = new List<Report>();
        public ICollection<Report> ReportsReceived { get; set; } = new List<Report>();
        public ICollection<Feedback> GivenFeedbacks { get; set; } = new List<Feedback>();
        public ICollection<Feedback> ReceivedFeedbacks { get; set; } = new List<Feedback>();
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
        public ICollection<Message> MessagesReceived { get; set; } = new List<Message>();
        public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
        public ICollection<UserMatch> MatchesAsUser1 { get; set; } = new List<UserMatch>();
        public ICollection<UserMatch> MatchesAsUser2 { get; set; } = new List<UserMatch>();
    }
}
