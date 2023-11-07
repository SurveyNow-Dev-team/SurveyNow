using Application;
using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System.Linq.Expressions;

namespace Infrastructure.Services
{
    public class UserReportService: IUserReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserReportService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<UserReportResponse> ChangeReportStatus(long id, UserReportStatusRequest status)
        {
            var report = await _unitOfWork.UserReportRepository.GetByIdAsync(id);
            if (report == null)
            {
                throw new NotFoundException();
            }
            report = _mapper.Map(status, report);
            _unitOfWork.UserReportRepository.Update(report);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<UserReportResponse>(report);
        }

        public async Task Create(UserReportRequest request)
        {
            var report = _mapper.Map<UserReport>(request);
            var user = await _userService.GetCurrentUserAsync();
            if(user == null)
            {
                throw new NotFoundException("User not login");
            }
            report.CreatedUserId = user.Id;
            await _unitOfWork.UserReportRepository.AddAsync(report);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<UserReportResponse>> Get(UserReportFilter? filter)
        {
            ParameterExpression parameters = Expression.Parameter(typeof(UserReport));
            Expression expression = Expression.Constant(true);
            if(filter != null)
            {
                if(filter.CreatedDate != null)
                {
                    expression = Expression.AndAlso(expression, Expression.Equal(Expression.Constant(filter.CreatedDate), Expression.Property(parameters, nameof(UserReport.CreatedDate))));
                }
                if(filter.Status != null)
                {
                    expression = Expression.AndAlso(expression, Expression.Equal(Expression.Constant(filter.Status), Expression.Property(parameters, nameof(UserReport.Status))));
                }
                if (filter.Type != null)
                {
                    expression = Expression.AndAlso(expression, Expression.Equal(Expression.Constant(filter.Type), Expression.Property(parameters, nameof(UserReport.Type))));
                }
                if (filter.Reason != null)
                {
                    expression = Expression.AndAlso(expression, Expression.Equal(Expression.Constant(filter.Reason), Expression.Property(parameters, nameof(UserReport.Reason))));
                }
                if (filter.UserId != null)
                {
                    expression = Expression.AndAlso(expression, Expression.Equal(Expression.Constant(filter.UserId), Expression.Property(parameters, nameof(UserReport.UserId))));
                }
                if (filter.CreatedUserId != null)
                {
                    expression = Expression.AndAlso(expression, Expression.Equal(Expression.Constant(filter.CreatedUserId), Expression.Property(parameters, nameof(UserReport.CreatedUserId))));
                }
                if (filter.Result != null)
                {
                    expression = Expression.AndAlso(expression, Expression.Equal(Expression.Constant(filter.Result), Expression.Property(parameters, nameof(UserReport.Result))));
                }
                if (filter.SurveyId != null)
                {
                    expression = Expression.AndAlso(expression, Expression.Equal(Expression.Constant(filter.SurveyId), Expression.Property(parameters, nameof(UserReport.SurveyId))));
                }
            }
            var reports = await _unitOfWork.UserReportRepository.GetAllAsync(filter: Expression.Lambda<Func<UserReport, bool>>(expression, parameters));
            return _mapper.Map<IEnumerable<UserReportResponse>>(reports);
        }

        public async Task<UserReportResponse> Get(long id)
        {
            var report = await _unitOfWork.UserReportRepository.GetByIdAsync(id);
            if (report == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<UserReportResponse>(report);
        }
    }
}
