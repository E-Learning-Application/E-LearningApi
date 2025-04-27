using AutoMapper;
using CORE.DTOs.Auth;
using CORE.DTOs.Language;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.AutoMapperProfiles
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<CreateLanguageDto, Language>();
            CreateMap<Language, GetLanguageDto>()
                .ReverseMap();
        }
    }
}
