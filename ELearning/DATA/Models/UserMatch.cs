namespace DATA.Models
{
    public class UserMatch
    {
        public int Id { get; set; }
        public int UserId1  { get; set; }
        public AppUser User1 { get; set; }
        public int UserId2 { get; set; }
        public AppUser User2 { get; set; }
        public string MatchType { get; set; } = string.Empty; 
        public double MatchScore { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
