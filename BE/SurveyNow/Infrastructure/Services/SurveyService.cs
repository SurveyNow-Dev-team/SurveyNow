using System.Linq.Expressions;
using Application;
using Application.DTOs.Request.Survey;
using Application.DTOs.Response;
using Application.DTOs.Response.Survey;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class SurveyService : ISurveyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SurveyService> _logger;
    private readonly IUserService _userService;

    public SurveyService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SurveyService> logger,
        IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _userService = userService;
    }

    public async Task<long> CreateSurveyAsync(SurveyRequest request)
    {
        //begin transaction
        await _unitOfWork.BeginTransactionAsync();
        var surveyObject = _mapper.Map<Survey>(request);

        try
        {
            //Create Survey object
            var currentUser = await _userService.GetCurrentUserAsync();
            surveyObject.CreatedBy = currentUser ?? throw new UnAuthorizedException("User does not log in.");
            surveyObject.CreatedUserId = currentUser.Id;
            await _unitOfWork.SurveyRepository.AddAsync(surveyObject);
            //Create Survey object

            foreach (var s in surveyObject.Sections)
            {
                //Add Section 
                s.SurveyId = surveyObject.Id;
                s.TotalQuestion = s.Questions.Count;
                await _unitOfWork.SectionRepository.AddAsync(s);
                //Add Section 

                foreach (var q in s.Questions)
                {
                    q.SectionId = s.Id;
                    await _unitOfWork.QuestionRepository.AddAsync(q);

                    var rowOptionTask = Task.Run(async () =>
                    {
                        foreach (var ro in q.RowOptions)
                        {
                            ro.QuestionId = q.Id;
                            await _unitOfWork.RowOptionRepository.AddAsync(ro);
                        }
                    });

                    var columnOptionTask = Task.Run(async () =>
                    {
                        foreach (var co in q.ColumnOptions)
                        {
                            co.QuestionId = q.Id;
                            await _unitOfWork.ColumnOptionRepository.AddAsync(co);
                        }
                    });

                    await Task.WhenAll(rowOptionTask, columnOptionTask);
                }
            }

            await _unitOfWork.SaveChangeAsync();

            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception("Error when create survey");
        }
        finally
        {
            await _unitOfWork.DisposeAsync();
        }

        return surveyObject.Id;
    }

    public async Task<SurveyDetailResponse> GetByIdAsync(long id)
    {
        var surveyObj = await _unitOfWork.SurveyRepository.GetByIdAsync(id);
        if (surveyObj == null)
            throw new NotFoundException($"Survey {id} does not exist.");
        var result = _mapper.Map<SurveyDetailResponse>(surveyObj);
        return result;
    }

    public async Task<List<SurveyResponse>> GetAllAsync()
    {
        var surveys = await _unitOfWork.SurveyRepository.GetAllAsync();
        return _mapper.Map<List<SurveyResponse>>(surveys);
    }

    public async Task<PagingResponse<SurveyResponse>> FilterSurveyAsync(
        string? status,
        bool? isDelete,
        string? packType,
        string? title,
        string? sortTitle,
        string? sortCreatedDate,
        string? sortStartDate,
        string? sortExpiredDate,
        string? sortModifiedDate,
        int? page,
        int? size)
    {
        var statusEnum = EnumUtil.ConvertStringToEnum<SurveyStatus>(status);
        var packTypeEnum = EnumUtil.ConvertStringToEnum<PackType>(packType);

        var parameter = Expression.Parameter(typeof(Survey));
        Expression filter = Expression.Constant(true); // default is "where true"

        try
        {
            if (statusEnum.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, "Status"), Expression.Constant(statusEnum.Value)));
            }

            if (isDelete.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, "IsDelete"), Expression.Constant(isDelete.Value)));
            }

            if (packTypeEnum.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, "PackType"),
                        Expression.Constant(packTypeEnum.Value)));
            }

            if (!string.IsNullOrEmpty(title))
            {
                var titleToLower = title.ToLower();
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Call(Expression.Property(parameter, "Title"),
                            typeof(string).GetMethod("ToLower", Type.EmptyTypes)),
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        Expression.Constant(titleToLower)
                    )
                );
            }

            Func<IQueryable<Survey>, IOrderedQueryable<Survey>> orderBy = q => q.OrderByDescending(s => s.Id);

            if (sortTitle != null && sortTitle.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Title);
            }
            else if (sortTitle != null && sortTitle.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Title);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.CreatedDate);
            }
            else if (sortCreatedDate != null && sortCreatedDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.CreatedDate);
            }
            else if (sortStartDate != null && sortStartDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.StartDate);
            }
            else if (sortStartDate != null && sortStartDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.StartDate);
            }
            else if (sortExpiredDate != null && sortExpiredDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.ExpiredDate);
            }
            else if (sortExpiredDate != null && sortExpiredDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.ExpiredDate);
            }
            else if (sortModifiedDate != null && sortModifiedDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.ModifiedDate);
            }
            else if (sortModifiedDate != null && sortModifiedDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.ModifiedDate);
            }

            var surveys = await _unitOfWork.SurveyRepository.GetPaginateAsync(
                Expression.Lambda<Func<Survey, bool>>(filter, parameter), orderBy, "CreatedBy", page, size);
            var result = _mapper.Map<PagingResponse<SurveyResponse>>(surveys);

            return result;
        }
        catch (Exception e)
        {
            throw new BadRequestException(e.Message);
        }
    }
}