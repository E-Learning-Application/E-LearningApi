namespace DATA.Models
{
    public class Interest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
    }
}
