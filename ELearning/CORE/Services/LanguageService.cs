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
                    StatusCode = 201,
                    Message = "Languages created successfully",
                    Data = createdLanguages
                };
            }
        }

        public async Task<ResponseDto<List<int>>> RemoveLanguagesAsync(HashSet<int> languagesIds)
        {
            var languages = await _unitOfWork.Languages.FindAsync(l => languagesIds.Contains(l.Id), 1, 5000);
            _unitOfWork.Languages.Delete(languages);
            var changes = await _unitOfWork.CommitAsync();
            if (changes == 0)
                return new ResponseDto<List<int>>()
                {
                    StatusCode = 500,
                    Message = "Error while deleting languages",
                };
            else
            {
                return new ResponseDto<List<int>>()
                {
                    StatusCode = 200,
                    Message = "Languages deleted successfully",
                    Data = languages.Select(l => l.Id).ToList()
                };
            }
        }
    }
}
