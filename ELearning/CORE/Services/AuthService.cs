﻿using AutoMapper;
using CORE.DTOs.Auth;
using CORE.Services.IServices;
using DATA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CORE.Constants;
using CORE.DTOs;
using DATA.DataAccess.Repositories.UnitOfWork;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace CORE.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOptions<JwtConfig> _jwt;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public AuthService(IOptions<JwtConfig> jwt,
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IMemoryCache memoryCache)
        {
            _jwt = jwt;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }
        private async Task<string?> ValidateRegistrationAsync(RegisterDto registerDto)
        {
            if (await _userManager.FindByNameAsync(registerDto.Username) != null)
                return "Username already exists.";
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                return "Email already exists.";
            return null;
        }
        private async Task<IdentityResult> UpdateUserRolesByUserAsync(AppUser user, List<string> roles)
        {
            return await _userManager.AddToRolesAsync(user, roles);
        }
        private async Task<string> CreateAccessTokenAsync(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleClaims = userRoles.Select(role => new Claim("roles", role)).ToList();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.Id}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.UserName),
                new Claim("uid", user.Id.ToString()),
            }
            .Union(roleClaims);

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwt.Value.Issuer,
                    audience: _jwt.Value.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(_jwt.Value.DurationInHours),
                    signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = RandomNumberGenerator.GetBytes(32);
            var rawToken = Convert.ToBase64String(randomNumber);

            using (var sha256 = SHA256.Create())
            {
                var hashedToken = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(rawToken)));

                return new RefreshToken
                {
                    Token = hashedToken, // Store hashed token in the database
                    RawToken = rawToken, // Send raw token back to the client
                    ExpiresOn = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("RefreshTokenDurationInDays")),
                    CreatedOn = DateTime.UtcNow,
                };
            }
        }
        private async Task AddRefreshToken(AppUser user, RefreshToken refreshToken)
        {
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
        }
        private string HashRefreshToken(string rawToken)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedToken = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(rawToken)));
                return hashedToken;
            }
        }
        private RefreshToken? ValidateRefreshToken(string providedToken, AppUser user)
        {
            var refreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
            if (refreshToken == null) return null;

            return HashRefreshToken(providedToken) == refreshToken.Token ? refreshToken : null;
        }
        private async Task<RefreshToken> ProcessUserRefreshToken(AppUser user)
        {
            var activeRefreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
            if (activeRefreshToken != null)
                activeRefreshToken.RevokedOn = DateTime.UtcNow;
            var refreshToken = GenerateRefreshToken();
            await AddRefreshToken(user, refreshToken);
            return refreshToken;
        }
        public async Task<ResponseDto<AuthDto>> RegisterAsync(RegisterDto registerDto, List<string> roles)
        {

            if (await ValidateRegistrationAsync(registerDto) is string validationMessage)
            {
                return new ResponseDto<AuthDto> {  StatusCode = StatusCodes.BadRequest, Message = validationMessage };
            }

            AppUser user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded == false)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResponseDto<AuthDto> {StatusCode = StatusCodes.InternalServerError, Message = errors };
            }


            if (roles != null && roles.Any() == true)
            {
                var roleResult = await UpdateUserRolesByUserAsync(user, roles);
                if (roleResult.Succeeded == false)
                {
                    var errors = "";
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    errors += $"Failed to assign roles: {roleErrors}.";

                    var deleteResult = await _userManager.DeleteAsync(user);
                    if (deleteResult.Succeeded == false)
                    {
                        var deleteErrors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
                        errors += $"\nFailed to cleanup user: {deleteErrors}.";
                    }
                    return new ResponseDto<AuthDto> { StatusCode = StatusCodes.InternalServerError, Message = errors };
                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return new ResponseDto<AuthDto>
            {
                Data = new AuthDto
                {
                    UserId = user.Id,
                    IsAuthenticated = true,
                    Roles = userRoles.ToList(),
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                },
                StatusCode = StatusCodes.Created,
                Message = "Registered Successfully.",
            };
        }
        public async Task<ResponseDto<AuthDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail); 
            }

            if (user == null || await _userManager.CheckPasswordAsync(user, loginDto.Password) == false)
            {
                return new ResponseDto<AuthDto> { StatusCode = StatusCodes.BadRequest, Message = "Username or password is incorrect." };
            }

            var roles = await _userManager.GetRolesAsync(user);
            if(roles == null || roles.Count == 0)
            {
                return new ResponseDto<AuthDto> { StatusCode = StatusCodes.InternalServerError, Message = "User has no roles, Try logging in again." };
            }


            var result = new ResponseDto<AuthDto>
            {
                Data = new AuthDto
                {
                    UserId = user.Id,
                    IsAuthenticated = true,
                    Username = user?.UserName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                },
                StatusCode = StatusCodes.OK,
                Message = "Logged in successfully."
            };

            result.Data.AccessToken = await CreateAccessTokenAsync(user);

            var refreshToken = await ProcessUserRefreshToken(user);
            result.Data.RefreshToken = refreshToken.RawToken;
            result.Data.RefreshTokenExpiration = refreshToken.ExpiresOn;

            return result;
        }
        public async Task<ResponseDto<AuthDto>> RefreshAccessTokenAsync(string refreshToken)
        {

            var hashedRefreshToken = HashRefreshToken(refreshToken);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == hashedRefreshToken));
            if (user == null)
            {
                return new ResponseDto<AuthDto> { Message = "Invalid token.", StatusCode = StatusCodes.BadRequest };
            }


            if (ValidateRefreshToken(refreshToken, user) == null)
            {
                return new ResponseDto<AuthDto> { Message = "Invalid token.", StatusCode = StatusCodes.BadRequest };
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Any() == false)
            {
                return new ResponseDto<AuthDto> { Message = "User has no roles assigned.", StatusCode = StatusCodes.InternalServerError };
            }

            var newRefreshToken = await ProcessUserRefreshToken(user);

            var accessToken = await CreateAccessTokenAsync(user);

            return new ResponseDto<AuthDto>
            {
                Data = new AuthDto
                {
                    IsAuthenticated = true,
                    AccessToken = accessToken,
                    Username = user.UserName,
                    Email = user.Email,
                    RefreshToken = newRefreshToken.RawToken,
                    RefreshTokenExpiration = newRefreshToken.ExpiresOn,
                    Roles = roles.ToList()
                },
                StatusCode = StatusCodes.OK,
            };
        }
        public async Task<ResponseDto<object>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var hashedRefreshToken = HashRefreshToken(refreshToken);

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == hashedRefreshToken));

            if (user == null)
            {
                return new ResponseDto<object> { Message = "Invalid token.", StatusCode = StatusCodes.BadRequest };
            }

            var token = ValidateRefreshToken(refreshToken, user);

            if (token == null)
            {
                return new ResponseDto<object> { Message = "Invalid token.", StatusCode = StatusCodes.BadRequest };
            }

            token.RevokedOn = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded == false)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResponseDto<object> { Message = $"Failed to revoke token: {errors}.", StatusCode = StatusCodes.InternalServerError };
            }

            return new ResponseDto<object> { StatusCode = StatusCodes.OK, Message = "You have been logged out. Please delete your access token and refresh token." };
        }
        public async Task<ResponseDto<object>> UpdateUserRolesAsync(int userId, List<string> roles)
        {
            var user = await _unitOfWork.AppUsers.GetAsync(userId);
            if (user == null)
            {
                return new ResponseDto<object> { Message = "User not found.", StatusCode = StatusCodes.NotFound };
            }

            var result = await UpdateUserRolesByUserAsync(user, roles);
            if (result.Succeeded == false)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResponseDto<object> { Message = $"Failed to update roles: {errors}.", StatusCode = StatusCodes.InternalServerError };
            }
            
            return new ResponseDto<object> { StatusCode = StatusCodes.OK, Message = "Roles updated successfully." };
        }
    }
}
