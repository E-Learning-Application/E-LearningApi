using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.DTOs;
using CORE.DTOs.Language;

namespace CORE.Services.IServices
{
    public interface ILanguagePreferenceService
    {
        Task<ResponseDto<List<GetLanguagePreferenceDto>>> UpdateUserLanguagePreferencesAsync(List<CreateLanguagePreferenceDto> dtos, int UserId);
        Task<ResponseDto<List<GetLanguagePreferenceDto>>> GetUserLanguagePreferenceAsync(int userId);
    }
}
