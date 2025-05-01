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
        Task<ResponseDto<List<GetLanguagePreferenceDto>>> UpdateUserLanguagePreferences(List<CreateLanguagePreferenceDto> dtos, int UserId);
    }
}
