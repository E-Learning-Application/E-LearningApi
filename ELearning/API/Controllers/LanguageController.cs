using CORE.Constants;
using CORE.DTOs.Language;
using CORE.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }
        [HttpPost("add-languages")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddLanguagesAsync([FromBody] List<CreateLanguageDto> languages)
        {
            var result = await _languageService.CreateLanguagesAsync(languages);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("remove-languages")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> RemoveLanguagesAsync(HashSet<int> languagesIds)
        {
            var result = await _languageService.RemoveLanguagesAsync(languagesIds);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("update-languages")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateLanguagesAsync([FromBody] List<GetLanguageDto> languages)
        {
            var result = await _languageService.UpdateLanguagesAsync(languages);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("languages")]
        [Authorize]
        public async Task<IActionResult> GetAllLanguagesAsync()
        {
            var result = await _languageService.GetAllLanguagesAsync();
            return StatusCode(result.StatusCode, result);
        }
    }
}
