using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.DTOs;
using CORE.DTOs.Language;
using CORE.Services.IServices;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;

namespace CORE.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LanguageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<GetLanguageDto>>> CreateLanguagesAsync(List<CreateLanguageDto> languagesDto)
        {
            var languages = _mapper.Map<List<Language>>(languagesDto);

            await _unitOfWork.Languages.AddRangeAsync(languages);
            var changes = await _unitOfWork.CommitAsync();
            if (changes == 0)
                return new ResponseDto<List<GetLanguageDto>>()
                {
                    StatusCode = 500,
                    Message = "Error while creating languages",
                };
            else
            {
                var createdLanguages = _mapper.Map<List<GetLanguageDto>>(languages);
                return new ResponseDto<List<GetLanguageDto>>()
                {
                    StatusCode = 200,
                    Message = "Languages created successfully",
                    Data = createdLanguages
                };
            }
        }
    }
}
