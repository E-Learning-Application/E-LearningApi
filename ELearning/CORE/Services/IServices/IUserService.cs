using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.DTOs;
using CORE.DTOs.Language;
using CORE.DTOs.User;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace CORE.Services.IServices
{
    public interface IUserService
    {
        Task<ResponseDto<GetUserDto>> GetUserAsync(int userId);
        Task<ResponseDto<object>> DeleteUserAsync(int userId, int authUserId, List<string> roles);
        Task<ResponseDto<object>> UpdateUserPasswordAsync(UpdatePasswordDto dto, int userId);
    }
}
