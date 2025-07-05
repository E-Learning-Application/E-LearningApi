namespace DATA.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<LanguagePreference>? LanguagePreferences { get; set; }
    }
}
