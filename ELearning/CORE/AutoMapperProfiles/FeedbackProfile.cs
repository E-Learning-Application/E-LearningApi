using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.DTOs.Feedback;
using DATA.Models;

namespace CORE.AutoMapperProfiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<CreateFeedbackDto, Feedback>();
            CreateMap<Feedback, GetFeedbackDto>();
        }
    }
}
