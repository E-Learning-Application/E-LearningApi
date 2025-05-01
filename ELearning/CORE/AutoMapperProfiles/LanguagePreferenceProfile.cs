using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.DTOs.Language;
using DATA.Models;

namespace CORE.AutoMapperProfiles
{
    public class LanguagePreferenceProfile : Profile
    {
        public LanguagePreferenceProfile()
        {
            CreateMap<CreateLanguagePreferenceDto, LanguagePreference>();
            CreateMap<LanguagePreference, GetLanguagePreferenceDto>()
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.proficiencyLevel, opt => opt.MapFrom(src => src.ProficiencyLevel.ToString()))
                .ForMember(dest => dest.IsLearning, opt => opt.MapFrom(src => src.IsLearning)); 
        }
    }
}
