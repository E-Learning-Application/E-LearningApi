using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.Constants;
using CORE.DTOs;
using CORE.DTOs.Language;
using CORE.Services.IServices;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;

namespace CORE.Services
{
    public class LanguagePreferenceService : ILanguagePreferenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LanguagePreferenceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<GetLanguagePreferenceDto>>> GetUserLanguagePreferenceAsync(int userId)
        {
            var user = await _unitOfWork.AppUsers.GetAsync(userId);
            if (user == null)
                return new ResponseDto<List<GetLanguagePreferenceDto>>()
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "User not found"
                };

            var languagePrefreneces = await _unitOfWork.LanguagePreferences.GetAllAsync(x => x.UserId == userId, new string[] { "Language" });
            return new ResponseDto<List<GetLanguagePreferenceDto>>()
            {
                StatusCode = 200,
                Message = "User languages fetched successfully",
                Data = _mapper.Map<List<GetLanguagePreferenceDto>>(languagePrefreneces)
            };
        }

        public async Task<ResponseDto<List<GetLanguagePreferenceDto>>> UpdateUserLanguagePreferencesAsync(List<CreateLanguagePreferenceDto> dtos, int UserId)
        {
            var userLanguages = await _unitOfWork.LanguagePreferences.GetAllAsync(x => x.UserId == UserId);
            if (dtos == null || dtos.Count == 0)
            {
                _unitOfWork.LanguagePreferences.Delete(userLanguages);
                await _unitOfWork.CommitAsync();
                return new ResponseDto<List<GetLanguagePreferenceDto>>()
                {
                    StatusCode = 200,
                    Message = "User languages updated successfully",
                    Data = new List<GetLanguagePreferenceDto>()
                };
            }
            var languagesIds = dtos.Select(x => x.LanguageId).ToHashSet();
            var languages = await _unitOfWork.Languages.GetAllAsync(x => languagesIds.Contains(x.Id));
            if (languages.Count() != languagesIds.Count())
            {
                return new ResponseDto<List<GetLanguagePreferenceDto>>()
                {
                    StatusCode = 400,
                    Message = "Some of the languages are not valid"
                };
            }

            var list = _mapper.Map<List<LanguagePreference>>(dtos);
            foreach (var item in list)
            {
                item.UserId = UserId;
            }
            _unitOfWork.LanguagePreferences.Delete(userLanguages);
            await _unitOfWork.CommitAsync();
 
            await _unitOfWork.LanguagePreferences.AddRangeAsync(list);
            var changes = await _unitOfWork.CommitAsync();
            if (changes == 0)
                return new ResponseDto<List<GetLanguagePreferenceDto>>()
                {
                    StatusCode = 500,
                    Message = "Something went wrong while updating user languages"
                };
            var result = await _unitOfWork.LanguagePreferences.GetAllAsync(x => x.UserId == UserId, new string[] { "Language" });

            return new ResponseDto<List<GetLanguagePreferenceDto>>()
            {
                StatusCode = 200,
                Message = "User languages updated successfully",
                Data = _mapper.Map<List<GetLanguagePreferenceDto>>(result)
            };

        }
    }
}
