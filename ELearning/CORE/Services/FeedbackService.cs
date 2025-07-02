using AutoMapper;
using CORE.Constants;
using CORE.DTOs;
using CORE.DTOs.Feedback;
using CORE.Services.IServices;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;

namespace CORE.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto<GetFeedbackDto>> CreateFeedbackAsync(CreateFeedbackDto feedbackDto, int feedbackerId)
        {
            if(feedbackDto.Rating < 1 || feedbackDto.Rating > 5)
                return new ResponseDto<GetFeedbackDto>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "Rating must be between 1 and 5."
                };

            var feedback = _mapper.Map<Feedback>(feedbackDto);

            feedback.UserId = feedbackerId;

            await _unitOfWork.Feedbacks.AddOrUpdateAsync(feedback);
            var changes = await _unitOfWork.CommitAsync();

            if(changes == 0)
                return new ResponseDto<GetFeedbackDto>
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An error occurred while creating feedback."
                };
            return new ResponseDto<GetFeedbackDto>
            {
                StatusCode = StatusCodes.Created,
                Message = "Feedback created successfully.",
                Data = _mapper.Map<GetFeedbackDto>(feedback)
            };
        }

        public async Task<ResponseDto<object>> DeleteFeedbadckAsync(int feedbackId, int userId)
        {
            var feedback = await _unitOfWork.Feedbacks.GetAsync(feedbackId);
            if (feedback == null)
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "Feedback not found."
                };
            if (feedback.UserId != userId)
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.Forbidden,
                    Message = "You do not have permission to delete this feedback."
                };
            _unitOfWork.Feedbacks.Delete(feedback);
            var changes = await _unitOfWork.CommitAsync();
            if (changes == 0)
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An error occurred while deleting feedback."
                };
            return new ResponseDto<object>
            {
                StatusCode = StatusCodes.OK,
                Message = "Feedback deleted successfully."
            };
        }

        public async Task<ResponseDto<List<GetFeedbackDto>>> GetAllFeedbacksAsync(int feedbackerId)
        {
            var feedbacks = await _unitOfWork.Feedbacks.GetAllAsync(f => f.UserId == feedbackerId);
            var feedbackDtos = _mapper.Map<List<GetFeedbackDto>>(feedbacks);
            return new ResponseDto<List<GetFeedbackDto>>
            {
                StatusCode = StatusCodes.OK,
                Message = "Feedbacks retrieved successfully.",
                Data = feedbackDtos
            };
        }

        public async Task<ResponseDto<GetFeedbackDto>> UpdateFeedbackAsync(UpdateFeedbackDto dto, int userId)
        {
            var feedback = await _unitOfWork.Feedbacks.GetAsync(dto.Id);
            if (feedback == null)
                return new ResponseDto<GetFeedbackDto>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "Feedback not found."
                };
            if (feedback.UserId != userId)
                return new ResponseDto<GetFeedbackDto>
                {
                    StatusCode = StatusCodes.Forbidden,
                    Message = "You do not have permission to update this feedback."
                };
            if (dto.Rating < 1 || dto.Rating > 5)
                return new ResponseDto<GetFeedbackDto>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "Rating must be between 1 and 5."
                };

            feedback.Rating = dto.Rating;
            feedback.Comment = dto.Comment;
            await _unitOfWork.Feedbacks.AddOrUpdateAsync(feedback);
            var changes = await _unitOfWork.CommitAsync();
            if (changes == 0)
                return new ResponseDto<GetFeedbackDto>
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "An error occurred while updating feedback."
                };
            return new ResponseDto<GetFeedbackDto>
            {
                StatusCode = StatusCodes.OK,
                Message = "Feedback updated successfully.",
                Data = _mapper.Map<GetFeedbackDto>(feedback)
            };
        }
    }
}
