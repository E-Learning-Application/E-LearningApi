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
        public async Task<IActionResult> AddLanguages([FromBody] List<CreateLanguageDto> languages)
        {
            var result = await _languageService.CreateLanguagesAsync(languages);
            return StatusCode(result.StatusCode, result);
        }
    }
}
