using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CORE.Constants;
using CORE.DTOs;
using CORE.DTOs.Report;
using CORE.Services.IServices;
using DATA.DataAccess.Repositories.UnitOfWork;
using DATA.Models;

namespace CORE.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto<object>> CreateReportAsync(CreateReportDto dto, int reporterId)
        {
            if(await _unitOfWork.AppUsers.CheckAnyAsync(r=>r.Id == reporterId, null) == false)
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.Unauthorized,
                    Message = "Reporter not found"
                };
            if (await _unitOfWork.AppUsers.CheckAnyAsync(r => r.Id == dto.ReportedId, null) == false)
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.BadRequest,
                    Message = "Reported user not found"
                };
            var report = _mapper.Map<Report>(dto);
            report.ReporterId = reporterId;

            await _unitOfWork.Reports.AddOrUpdateAsync(report);
        
            var changes = await _unitOfWork.CommitAsync();
            if (changes == 0)
                return new ResponseDto<object>
                {
                    StatusCode = StatusCodes.InternalServerError,
                    Message = "Failed to create report"
                };

            return new ResponseDto<object>
            {
                    StatusCode = StatusCodes.OK,
                    Message = "Report created successfully"
            };
        }
    }
}
