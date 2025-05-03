using AutoMapper;
using CORE.DTOs.Auth;
using CORE.DTOs.User;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.AutoMapperProfiles
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUser, RegisterDto>()
                .ForMember(dest=> dest.ConfirmPassword, opt=>opt.Ignore())
                .ForMember(dest=> dest.Password, opt=>opt.Ignore())
                .ReverseMap();

            CreateMap<AppUser, GetUserDto>()
                .ForMember(dest => dest.LanguagePreferences, opt => opt.Ignore());
            
            CreateMap<AppUser, GetUserSummeryDto>();
        }
    }
}
