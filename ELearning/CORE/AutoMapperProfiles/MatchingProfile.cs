using AutoMapper;
using CORE.DTOs.Interest;
using CORE.DTOs.UserMatch;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.AutoMapperProfiles
{
    public class MatchingProfile :Profile
    {
        public MatchingProfile()
        {
            CreateMap<InterestAddRequest, Interest>()
                .ReverseMap();

            CreateMap<Interest, InterestResponse>()
                .ReverseMap();

            CreateMap<UserInterestAddRequest, UserInterest>()
                .ReverseMap();

            CreateMap<UserInterest, UserInterestResponse>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.InterestName, opt => opt.MapFrom(src => src.Interest.Name))
                .ReverseMap();

            CreateMap<UserMatch, UserMatchResponse>()
                .ForMember(dest => dest.UserName1, opt => opt.MapFrom(src => src.User1.UserName))
                .ForMember(dest => dest.ImagePath1, opt => opt.MapFrom(src => src.User1.ImagePath))
                .ForMember(dest => dest.UserName2, opt => opt.MapFrom(src => src.User2.UserName))
                .ForMember(dest => dest.ImagePath2, opt => opt.MapFrom(src => src.User2.ImagePath))
                .ReverseMap();
        }
    }
}
