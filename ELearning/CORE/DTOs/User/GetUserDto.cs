using CORE.DTOs.Language;

namespace CORE.DTOs.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? ImagePath { get; set; }
        public string? Bio { get; set; }
        public List<GetLanguagePreferenceDto>? LanguagePreferences { get; set; }
    }
}
