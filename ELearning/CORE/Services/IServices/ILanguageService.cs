using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.DTOs;
using CORE.DTOs.Language;

namespace CORE.Services.IServices
{
    public interface ILanguageService
    {
        Task<ResponseDto<List<GetLanguageDto>>> CreateLanguagesAsync(List<CreateLanguageDto> languagesDto);
        Task<ResponseDto<List<int>>> RemoveLanguagesAsync(HashSet<int> languagesIds);
        Task<ResponseDto<List<GetLanguageDto>>> UpdateLanguagesAsync(List<GetLanguageDto> languagesDto);

    }
}
