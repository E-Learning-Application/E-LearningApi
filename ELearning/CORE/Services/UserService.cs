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

namespace CORE.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDto<List<GetLanguageDto>>> UpdateUserLanguagePreferences(HashSet<int> LanguagesIds, int UserId)
        {
            var user = await _unitOfWork.AppUsers.GetAsync(UserId, new string[] { "LanguagePreferences" });
            if (user == null)
            {
                return new ResponseDto<List<GetLanguageDto>>
                {
                    StatusCode = StatusCodes.Unauthorized,
                    Message = "User not found",
                };
            }
            var languages = (await _unitOfWork.Languages.FindAsync(l=>LanguagesIds.Contains(l.Id), 1, 5000)).ToList();

            if (user.LanguagePreferences != null)
                user.LanguagePreferences.Clear();

            user.LanguagePreferences = languages;

            await _unitOfWork.CommitAsync();

            return new ResponseDto<List<GetLanguageDto>>
            {
                Data = _mapper.Map<List<GetLanguageDto>>(languages),
                StatusCode = StatusCodes.OK,
                Message = "User language preferences updated successfully",
            };
        }
    }
}
