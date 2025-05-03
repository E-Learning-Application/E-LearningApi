using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.DTOs;
using CORE.DTOs.Feedback;

namespace CORE.Services.IServices
{
    public interface IFeedbackService
    {
        Task<ResponseDto<GetFeedbackDto>> CreateFeedbackAsync(CreateFeedbackDto feedbackDto, int feedbackerId);
    }
}
