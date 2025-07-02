namespace CORE.DTOs.Language
{
    public class GetLanguagePreferenceDto
    {
        public GetLanguageDto Language { get; set; }
        public string proficiencyLevel { get; set; }
        public bool IsLearning { get; set; }
    }
}
