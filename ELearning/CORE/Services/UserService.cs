using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.Constants;
using CORE.DTOs;
using CORE.DTOs.Language;
using CORE.DTOs.User;
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

        public async Task<ResponseDto<object>> DeleteUserAsync(int userId, int authUserId, List<string> roles)
        {
            if(userId != authUserId && !roles.Contains(Roles.Admin))
            {
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.Forbidden,
                    Message = "You are not authorized to delete this user"
                };
            }
            var user = await _unitOfWork.AppUsers.GetAsync(userId);
            if (user == null)
            {
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "User not found"
                };
            }
            _unitOfWork.AppUsers.Delete(user);
            var changes = await _unitOfWork.CommitAsync();
            if (changes == 0)
            {
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "Error deleting user"
                };
            }
            return new ResponseDto<object>
            {
                StatusCode = StatusCodes.OK,
                Message = "User deleted successfully"
            };
        }

        public async Task<ResponseDto<GetUserDto>> GetUserAsync(int userId)
        {
            var user = await _unitOfWork.AppUsers.GetAsync(userId);
            if (user == null)
            {
                return new ResponseDto<GetUserDto>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "User not found"
                };
            }
            var userDto = _mapper.Map<GetUserDto>(user);
            var languagePreferences = await _unitOfWork.LanguagePreferences.GetAllAsync(x => x.UserId == userId, new string[] {"Language"});
            userDto.LanguagePreferences = _mapper.Map<List<GetLanguagePreferenceDto>>(languagePreferences);
            return new ResponseDto<GetUserDto>
            {
                StatusCode = StatusCodes.OK,
                Data = userDto
            };

        }
    }
}
