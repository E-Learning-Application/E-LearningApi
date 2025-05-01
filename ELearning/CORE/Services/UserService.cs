using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.Constants;
using CORE.DTOs;
using CORE.DTOs.Language;
using CORE.DTOs.Paths;
using CORE.DTOs.User;
using CORE.Helpers;
using CORE.Services.IServices;
using DATA.Constants;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StatusCodes = CORE.Constants.StatusCodes;

namespace CORE.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IOptions<Paths> _paths;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, IFileService fileService, IOptions<Paths> paths)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _fileService = fileService;
            _paths = paths;
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

        public async Task<ResponseDto<GetUserDto>> UpdateUserAsync(UpdateUserDto dto, int userId)
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
            user.Bio = dto.Bio;
            user.UserName = dto.Username;

            if (dto.Image != null)
            {
                user.ImagePath = await _fileService.UploadFileAsync(_paths.Value?.UserImages, user.ImagePath, dto.Image, AllowedExtensions.ImageExtensions);
                if (user.ImagePath == null)
                    return new ResponseDto<GetUserDto>
                    {
                        StatusCode = StatusCodes.InternalServerError,
                        Message = "Failed to upload main image"
                    };
            }
            var changes = await _unitOfWork.CommitAsync();
            if (changes == 0)
            {
                return new ResponseDto<GetUserDto>
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "Error updating user"
                };
            }
            return new ResponseDto<GetUserDto>
            {
                StatusCode = StatusCodes.OK,
                Message = "User updated successfully",
                Data = _mapper.Map<GetUserDto>(user)
            };
        }

        public async Task<ResponseDto<object>> UpdateUserPasswordAsync(UpdatePasswordDto dto, int userId)
        {
            var user = await _unitOfWork.AppUsers.GetAsync(userId);

            if (user == null)
            {
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "User not found"
                };
            }
            var identityResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (identityResult.Succeeded == false)
            {
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = UserHelpers.GetErrors(identityResult)
                };
            }
            return new ResponseDto<object>
            {
                StatusCode = StatusCodes.OK,
                Message = "Password updated successfully"
            };
        }
    }
}
