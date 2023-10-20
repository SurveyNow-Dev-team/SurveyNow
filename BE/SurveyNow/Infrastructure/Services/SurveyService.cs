using System.Linq.Expressions;
using System.Reflection;
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

    public async Task<SurveyDetailResponse> CreateSurveyAsync(SurveyRequest request)
    {
        //begin transaction
        await _unitOfWork.BeginTransactionAsync();
        var surveyObject = _mapper.Map<Survey>(request);

        try
        {
            //Create Survey object
            var currentUser = await _userService.GetCurrentUserAsync();
            surveyObject.CreatedBy = currentUser ?? throw new UnauthorizedException("User does not log in.");
            surveyObject.CreatedUserId = currentUser.Id;
            //Update status to PackPurchase if user bought a pack
            surveyObject.Status = SurveyStatus.Draft;

            await _unitOfWork.SurveyRepository.AddAsync(surveyObject);
            //Create Survey object
            int totalQuestion = 0;

            foreach (var s in surveyObject.Sections)
            {
                //Add Section 
                s.SurveyId = surveyObject.Id;
                s.TotalQuestion = s.Questions.Count;
                totalQuestion += s.TotalQuestion;
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

            var surveyObj = await _unitOfWork.SurveyRepository.GetByIdAsync(surveyObject.Id);
            if (surveyObj == null)
            {
                _logger.LogError("Survey has not been created yet.");
                await _unitOfWork.RollbackAsync();
                throw new Exception("Survey has not been created yet.");
            }

            surveyObj.TotalQuestion = totalQuestion;
            _unitOfWork.SurveyRepository.Update(surveyObj);
            await _unitOfWork.SaveChangeAsync();

            await _unitOfWork.CommitAsync();

            surveyObj = await _unitOfWork.SurveyRepository.GetByIdWithoutTrackingAsync(surveyObject.Id);
            var result = _mapper.Map<SurveyDetailResponse>(surveyObj);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when creating survey: {}", e.Message);
            await _unitOfWork.RollbackAsync();
            throw new Exception(e.Message);
        }
        finally
        {
            await _unitOfWork.DisposeAsync();
        }
    }

    public async Task<SurveyDetailResponse> GetByIdAsync(long id)
    {
        var surveyObj = await _unitOfWork.SurveyRepository.GetByIdWithoutTrackingAsync(id);
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
                    Expression.Equal(Expression.Property(parameter, nameof(Survey.Status)),
                        Expression.Constant(statusEnum.Value)));
            }

            if (isDelete.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Survey.IsDelete)),
                        Expression.Constant(isDelete.Value)));
            }

            if (packTypeEnum.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Survey.PackType)),
                        Expression.Constant(packTypeEnum.Value)));
            }

            if (!string.IsNullOrEmpty(title))
            {
                var titleToLower = title.ToLower();
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Call(Expression.Property(parameter, nameof(Survey.Title)),
                            typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes) ??
                            throw new NotImplementException(
                                $"{nameof(string.ToLower)} method is deprecated or not supported.")),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }) ??
                        throw new NotImplementException(
                            $"{nameof(string.Contains)} method is deprecated or not supported."),
                        Expression.Constant(titleToLower)
                    )
                );
            }

            Func<IQueryable<Survey>, IOrderedQueryable<Survey>> orderBy = q => q.OrderBy(s => s.Id);

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
                Expression.Lambda<Func<Survey, bool>>(filter, parameter), orderBy, $"{nameof(Survey.CreatedBy)}", page,
                size);
            var result = _mapper.Map<PagingResponse<SurveyResponse>>(surveys);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Can not get data on FilterSurveyAsync method: {}.", e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PagingResponse<CommonSurveyResponse>> FilterCommonSurveyAsync(
        string? status,
        string? title,
        string? sortTitle,
        string? sortTotalQuestion,
        string? sortPoint,
        string? sortStartDate,
        string? sortExpiredDate,
        int? page,
        int? size
    )
    {
        var statusEnum = EnumUtil.ConvertStringToEnum<SurveyStatus>(status);

        var parameter = Expression.Parameter(typeof(Survey));
        Expression filter = Expression.Constant(true); // default is "where true"

        try
        {

            if (statusEnum.HasValue)
            {
                if (!(statusEnum.Value.Equals(SurveyStatus.Active) && statusEnum.Value.Equals(SurveyStatus.Expired)))
                {
                    throw new BadRequestException(
                        $"Invalid status. Only {SurveyStatus.Active.ToString()} or {SurveyStatus.Expired.ToString()} status are allowed.");
                }

                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Survey.Status)),
                        Expression.Constant(statusEnum.Value)));
            }
            else
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Survey.Status)),
                        Expression.Constant(SurveyStatus.Active)));

                filter = Expression.OrElse(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Survey.Status)),
                        Expression.Constant(SurveyStatus.Expired)));
            }

            //only survey that is not deleted
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Survey.IsDelete)), Expression.Constant(false)));

            #region survey which is not belong to the login user and user haven't done it yet
            // Get survey which is not belong to the login user
            var user = await _userService.GetCurrentUserAsync().ContinueWith(x => x.Result ?? throw new UnauthorizedException("User hasn't logged in yet"));
            filter = Expression.AndAlso(filter, Expression.NotEqual(Expression.Property(parameter, nameof(Survey.CreatedUserId)), Expression.Constant(user.Id)));

            // Get survey which user haven't done it yet
            // !survey.UserSurvey.Any(x => x.UserId == user.Id)
            filter = Expression.AndAlso(filter,
                Expression.Condition(
                    ExpressionUtils.Any<UserSurvey>(
                        ExpressionUtils.ToQueryable<UserSurvey>(Expression.Property(parameter, nameof(Survey.UserSurveys))), x => x.UserId == user.Id), Expression.Constant(false), Expression.Constant(true)));
            #endregion

            // Get survey on criterion
            #region by age
            // if min age | max age not null => max age > date now - user dob > min age 
            int? age = user.DateOfBirth != null ? (DateTime.Now.Year - user.DateOfBirth.Value.Year) : null;
            Expression minAge = Expression.Property(Expression.Property(parameter, nameof(Survey.Criteria)), nameof(Criterion.MinAge));
            Expression maxAge = Expression.Property(Expression.Property(parameter, nameof(Survey.Criteria)), nameof(Criterion.MaxAge));
            filter = Expression.AndAlso(filter, Expression.Condition(
                    Expression.NotEqual(
                        minAge,
                        Expression.Constant(null)),
                    Expression.GreaterThan(Expression.TypeAs(Expression.Constant(age), typeof(int?)), minAge),
                    Expression.Constant(true)));
            filter = Expression.AndAlso(filter, Expression.Condition(
                Expression.NotEqual(
                    maxAge,
                    Expression.Constant(null)),
                Expression.LessThan(Expression.TypeAs(Expression.Constant(age), typeof(int?)), maxAge),
                Expression.Constant(true)));
            #endregion
            #region by gender

            #endregion
            Expression gender = Expression.Property(Expression.Property(parameter, nameof(Survey.Criteria)), nameof(Criterion.GenderCriteria));
            filter = Expression.AndAlso(filter, Expression.Condition(
                ExpressionUtils.Any<GenderCriterion>(ExpressionUtils.ToQueryable<GenderCriterion>(gender)),
                ExpressionUtils.Any<GenderCriterion>(ExpressionUtils.ToQueryable<GenderCriterion>(gender), x => x.Gender.Equals(user.Gender)),
                Expression.Constant(true)));


            if (!string.IsNullOrEmpty(title))
            {
                var titleToLower = title.ToLower();
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Call(Expression.Property(parameter, nameof(Survey.Title)),
                            typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)
                            ?? throw new NotImplementException(
                                $"{nameof(string.ToLower)} method is deprecated or not supported.")),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })
                        ?? throw new NotImplementException(
                            $"{nameof(string.ToLower)} method is deprecated or not supported."),
                        Expression.Constant(titleToLower)
                    )
                );
            }

            Func<IQueryable<Survey>, IOrderedQueryable<Survey>> orderBy = q => q.OrderBy(s => s.Id);

            if (sortTitle != null && sortTitle.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Title);
            }
            else if (sortTitle != null && sortTitle.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Title);
            }
            else if (sortTotalQuestion != null && sortTotalQuestion.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.TotalQuestion);
            }
            else if (sortTotalQuestion != null && sortTotalQuestion.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.TotalQuestion);
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
            else if (sortPoint != null && sortPoint.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Point);
            }
            else if (sortPoint != null && sortPoint.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Point);
            }

            var surveys = await _unitOfWork.SurveyRepository.GetPaginateAsync(
                Expression.Lambda<Func<Survey, bool>>(filter, parameter), orderBy, nameof(Survey.CreatedBy), page,
                size);
            var result = _mapper.Map<PagingResponse<CommonSurveyResponse>>(surveys);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Can not get data on FilterCommonSurveyAsync method: {}.", e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<PagingResponse<SurveyResponse>> FilterAccountSurveyAsync(string? status, string? packType,
        string? title, string? sortTitle,
        string? sortCreatedDate, string? sortStartDate, string? sortExpiredDate, string? sortModifiedDate, int? page,
        int? size)
    {
        var currentUser = await _userService.GetCurrentUserAsync()
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("User has not logged in yet."));

        var statusEnum = EnumUtil.ConvertStringToEnum<SurveyStatus>(status);
        var packTypeEnum = EnumUtil.ConvertStringToEnum<PackType>(packType);

        var parameter = Expression.Parameter(typeof(Survey));
        Expression filter = Expression.Constant(true); // default is "where true"

        try
        {
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Survey.CreatedUserId)),
                    Expression.Constant(currentUser.Id)));

            if (statusEnum.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Survey.Status)),
                        Expression.Constant(statusEnum.Value)));
            }

            if (packTypeEnum.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(Survey.PackType)),
                        Expression.Constant(packTypeEnum.Value)));
            }

            if (!string.IsNullOrEmpty(title))
            {
                var titleToLower = title.ToLower();
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Call(Expression.Property(parameter, nameof(Survey.Title)),
                            typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes) ??
                            throw new NotImplementException(
                                $"{nameof(string.ToLower)} method is deprecated or not supported.")),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }) ??
                        throw new NotImplementException(
                            $"{nameof(string.Contains)} method is deprecated or not supported."),
                        Expression.Constant(titleToLower)
                    )
                );
            }

            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(Survey.IsDelete)),
                    Expression.Constant(false)));

            Func<IQueryable<Survey>, IOrderedQueryable<Survey>> orderBy = q => q.OrderBy(s => s.Id);

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
                Expression.Lambda<Func<Survey, bool>>(filter, parameter), orderBy, nameof(Survey.CreatedBy), page,
                size);
            var result = _mapper.Map<PagingResponse<SurveyResponse>>(surveys);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Can not get data on FilterAccountSurveyAsync method {}.", e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    //Need to check more validation
    public async Task DeleteSurveyAsync(long id)
    {
        var currentUser = await _userService.GetCurrentUserAsync().ContinueWith(
            t => t.Result ?? throw new UnauthorizedException("User has not logged in yet."));

        var surveyObj = await _unitOfWork.SurveyRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Survey {id} is not exist."));


        //Only owner can delete the survey
        //The survey is deleted can not be undone
        if (currentUser.Id != surveyObj.CreatedUserId)
            throw new ForbiddenException(
                "Only owner can delete the survey. If you are admin, try to change status instead.");

        if (surveyObj.Status != SurveyStatus.Draft)
            throw new BadRequestException($"Can only delete survey with status {SurveyStatus.Draft.ToString()}");

        //Need to check more before delete survey
        surveyObj.IsDelete = true;

        try
        {
            _unitOfWork.SurveyRepository.Update(surveyObj);
            await _unitOfWork.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error when save the updated survey {}.", e.Message);
            throw new Exception("Error when save the updated survey.");
        }
    }

    public async Task<SurveyDetailResponse> UpdateSurveyAsync(long id, SurveyRequest request)
    {
        var surveyObj = await _unitOfWork.SurveyRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Survey {id} does not exist."));

        var currentUser = await _userService.GetCurrentUserAsync()
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("User does not log in."));

        if (surveyObj.CreatedUserId != currentUser.Id)
        {
            throw new ForbiddenException("Only owner can update survey.");
        }

        if (!(surveyObj.Status.Equals(SurveyStatus.Draft) || surveyObj.Status.Equals(SurveyStatus.PackPurchased)))
        {
            throw new BadRequestException("Can only update survey that has not been published.");
        }

        //begin transaction
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var updateSurveyObj = _mapper.Map<Survey>(request);
            surveyObj.Title = updateSurveyObj.Title;
            surveyObj.Description = updateSurveyObj.Description;
            surveyObj.PackType = updateSurveyObj.PackType;
            surveyObj.Status = surveyObj.PackType != null ? SurveyStatus.PackPurchased : SurveyStatus.Draft;

            surveyObj.ModifiedDate = DateTime.UtcNow;

            //Need to add validation for start date and end date
            surveyObj.StartDate = updateSurveyObj.StartDate;
            surveyObj.ExpiredDate = updateSurveyObj.ExpiredDate;
            //Need to add validation for start date and end date

            int totalQuestion = 0;

            //Delete all old section and question
            foreach (var section in surveyObj.Sections)
            {
                //need to check here
                await _unitOfWork.SectionRepository.DeleteByIdAsync(section.Id);
            }

            //add new list section for survey
            foreach (var s in updateSurveyObj.Sections)
            {
                //Add Section 
                s.SurveyId = surveyObj.Id;
                s.TotalQuestion = s.Questions.Count;
                totalQuestion += s.TotalQuestion;
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

            surveyObj.TotalQuestion = totalQuestion;
            //Update survey Obj
            _unitOfWork.SurveyRepository.Update(surveyObj);
            //Update survey Obj

            await _unitOfWork.SaveChangeAsync();

            await _unitOfWork.CommitAsync();
            surveyObj = await _unitOfWork.SurveyRepository.GetByIdWithoutTrackingAsync(id);
            var result = _mapper.Map<SurveyDetailResponse>(surveyObj);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when updating survey: {}.", e.Message);
            await _unitOfWork.RollbackAsync();
            throw new Exception(e.Message);
        }
        finally
        {
            _logger.LogInformation("Call dispose method.");
            await _unitOfWork.DisposeAsync();
        }
    }

    public async Task<SurveyDetailResponse> ChangeSurveyStatusAsync(long id)
    {
        var currentUser = await _userService.GetCurrentUserAsync()
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("User does not log in."));

        var surveyObj = await _unitOfWork.SurveyRepository.GetByIdAsync(id)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Survey {id} does not exist."));

        if (!(currentUser.Role == Role.Admin || surveyObj.CreatedUserId.Equals(currentUser.Id)))
        {
            throw new ForbiddenException("You do not have authority to modify this resource.");
        }

        surveyObj.Status = surveyObj.Status switch
        {
            SurveyStatus.Active => SurveyStatus.InActive,
            SurveyStatus.InActive => SurveyStatus.Active,
            _ => throw new BadRequestException("Can not activate/deactivate survey: Survey status is not valid.")
        };

        //can check update result here
        await _unitOfWork.SaveChangeAsync();

        return _mapper.Map<SurveyDetailResponse>(surveyObj);
    }

    public async Task DoSurveyAsync(DoSurveyRequest request)
    {
        var currentUser = await _userService.GetCurrentUserAsync().ContinueWith(
            t => t.Result ?? throw new UnauthorizedException("Can not extract user from token."));

        var surveyObj = await _unitOfWork.SurveyRepository.GetByIdWithoutTrackingAsync(request.SurveyId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Survey {request.SurveyId} can not be found."));

        if (currentUser.Id == surveyObj.CreatedUserId)
        {
            throw new BadRequestException("You can not do your survey.");
        }

        // Check if user did the survey before
        if (await _unitOfWork.UserSurveyRepository.ExistBySurveyIdAndUserId(request.SurveyId, currentUser.Id))
        {
            throw new BadRequestException("You did the survey before.");
        }

        //Check if survey is active 
        if (surveyObj.Status != SurveyStatus.Active)
            throw new BadRequestException("Can not do this survey because it is not active.");

        var answerList = _mapper.Map<List<Answer>>(request.Answers).Select(a =>
        {
            a.UserId = currentUser.Id;
            return a;
        }).ToList();

        //Begin transaction
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var tasks = answerList.Select(a => _unitOfWork.AnswerRepository.AddAsync(a));
            await Task.WhenAll(tasks);
            var userSurvey = new UserSurvey
            {
                SurveyId = surveyObj.Id,
                UserId = currentUser.Id,
                Point = surveyObj.Point,
                IsValid = true,
                Date = DateTime.UtcNow
            };

            await _unitOfWork.UserSurveyRepository.AddAsync(userSurvey);
            await _unitOfWork.SaveChangeAsync();
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Error when saving answer: {}.", e.Message);
            await _unitOfWork.RollbackAsync();
            throw new Exception(e.Message);
        }
        finally
        {
            _logger.LogInformation("Call dispose method.");
            await _unitOfWork.DisposeAsync();
        }
    }

    public async Task<PagingResponse<CommonSurveyResponse>> FilterCompletedSurveyAsync(
        string? title,
        string? sortTitle,
        string? sortDate,
        int? page,
        int? size,
        bool disableTracking = true)
    {
        var currentUser = await _userService.GetCurrentUserAsync()
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException($"Can not extract current user from toke."));

        var parameter = Expression.Parameter(typeof(UserSurvey));
        Expression filter = Expression.Constant(true); // default is "where true"

        try
        {
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(UserSurvey.UserId)),
                    Expression.Constant(currentUser.Id)));

            if (!string.IsNullOrEmpty(title))
            {
                var titleToLower = title.ToLower();
                filter = Expression.AndAlso(filter,
                    Expression.Call(
                        Expression.Call(
                            Expression.Property(Expression.Property(parameter, nameof(UserSurvey.Survey)),
                                nameof(Survey.Title)),
                            typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes) ??
                            throw new NotImplementException(
                                $"{nameof(string.ToLower)} method is deprecated or not supported.")),
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }) ??
                        throw new NotImplementException(
                            $"{nameof(string.Contains)} method is deprecated or not supported."),
                        Expression.Constant(titleToLower)
                    )
                );
            }

            filter = Expression.AndAlso(filter,
                Expression.Equal(
                    Expression.Property(Expression.Property(parameter, nameof(UserSurvey.Survey)),
                        nameof(Survey.IsDelete)),
                    Expression.Constant(false)));

            Func<IQueryable<UserSurvey>, IOrderedQueryable<UserSurvey>> orderBy = q => q.OrderBy(s => s.Id);


            if (!string.IsNullOrEmpty(sortTitle) && sortTitle.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Survey.Title);
            }
            else if (!string.IsNullOrEmpty(sortTitle) && sortTitle.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Survey.Title);
            }
            else if (string.IsNullOrEmpty(sortDate) || sortDate.Trim().ToLower().Equals("asc"))
            {
                orderBy = q => q.OrderBy(s => s.Date);
            }
            else if (sortDate.Trim().ToLower().Equals("desc"))
            {
                orderBy = q => q.OrderByDescending(s => s.Date);
            }

            var userSurveys = await _unitOfWork.UserSurveyRepository.GetPaginateAsync(
                Expression.Lambda<Func<UserSurvey, bool>>(filter, parameter), orderBy,
                $"{nameof(UserSurvey.Survey)}",
                page,
                size,
                disableTracking
            );

            var surveys = userSurveys.Results.Select(us => _mapper.Map<CommonSurveyResponse>(us.Survey)).ToList();
            var result = new PagingResponse<CommonSurveyResponse>
            {
                CurrentPage = userSurveys.CurrentPage,
                RecordsPerPage = userSurveys.RecordsPerPage,
                TotalPages = userSurveys.TotalPages,
                TotalRecords = userSurveys.TotalRecords,
                Results = surveys
            };
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("Can not get data on FilterCompletedSurveyAsync method {}.", e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<SurveyDetailResponse> GetAnswerAsync(long surveyId)
    {
        var currentUser = await _userService.GetCurrentUserAsync()
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Can not extract current user from token."));

        if (!await _unitOfWork.UserSurveyRepository.ExistBySurveyIdAndUserId(surveyId, currentUser.Id))
        {
            throw new BadRequestException("You have not completed survey yet.");
        }

        var survey = await _unitOfWork.SurveyRepository.GetSurveyAnswerAsync(surveyId, currentUser.Id);

        return await _unitOfWork.SurveyRepository.GetSurveyAnswerAsync(surveyId, currentUser.Id)
            .ContinueWith(t =>
                _mapper.Map<SurveyDetailResponse>(t.Result ??
                                                  throw new NotFoundException($"Survey {surveyId} does not exist.")));
    }

    public async Task<PagingResponse<UserSurveyResponse>> GetUserSurveyAsync(
        long surveyId,
        bool? isValid,
        int? page,
        int? size,
        bool disableTracking = true
    )
    {
        var currentUser = await _userService.GetCurrentUserAsync()
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Can not extract current user from token."));

        var survey = await _unitOfWork.SurveyRepository.GetByIdAsync(surveyId)
            .ContinueWith(t => t.Result ?? throw new NotFoundException($"Survey {surveyId} does not exist"));

        if (currentUser.Role != Role.Admin && currentUser.Id != survey.Id)
        {
            throw new ForbiddenException(
                "Access denied. Only Admin or owner can view list user who completed the survey.");
        }

        var parameter = Expression.Parameter(typeof(UserSurvey));
        Expression filter = Expression.Constant(true); // default is "where true"

        IOrderedQueryable<UserSurvey> OrderBy(IQueryable<UserSurvey> q) => q.OrderByDescending(s => s.Date);
        try
        {
            filter = Expression.AndAlso(filter,
                Expression.Equal(Expression.Property(parameter, nameof(UserSurvey.SurveyId)),
                    Expression.Constant(surveyId)));

            if (isValid.HasValue)
            {
                filter = Expression.AndAlso(filter,
                    Expression.Equal(Expression.Property(parameter, nameof(UserSurvey.IsValid)),
                        Expression.Constant(isValid.Value)));
            }

            return await _unitOfWork.UserSurveyRepository.GetPaginateAsync(
                    Expression.Lambda<Func<UserSurvey, bool>>(filter, parameter), OrderBy,
                    $"{nameof(UserSurvey.User)}",
                    page,
                    size,
                    disableTracking
                )
                .ContinueWith(t => _mapper.Map<PagingResponse<UserSurveyResponse>>(t.Result));
        }
        catch (Exception e)
        {
            _logger.LogError("Can not get data on GetUserSurveyAsync method {}.", e.Message);
            throw new BadRequestException(e.Message);
        }
    }

    public async Task<CommonSurveyResponse> PostSurveyAsync(
        long surveyId,
        DateTime? startDate,
        DateTime expiredDate,
        CriterionRequest? criterionRequest
    )
    {
        var currentUser = await _userService.GetCurrentUserAsync()
            .ContinueWith(t => t.Result ?? throw new UnauthorizedException("Can not extract current user from token."));

        var survey = await _unitOfWork.SurveyRepository.Get(
                filter: s => s.Id == surveyId,
                orderBy: null,
                includeProperties: $"{nameof(Survey.CreatedBy)}",
                disableTracking: false
            )
            .ContinueWith(t =>
                t.Result.Any()
                    ? t.Result.First()
                    : throw new NotFoundException($"Survey {surveyId} does not exist."));

        if (currentUser.Id != survey.CreatedUserId)
        {
            throw new ForbiddenException("Access denied: Only owner can post this survey.");
        }

        if (survey.Status != SurveyStatus.PackPurchased)
        {
            throw new BadRequestException("This survey has been posted before or you have not bought a pack yet.");
        }

        if(currentUser.Point < survey.Point)
        {
            throw new BadRequestException("You don't have enough point to post this survey.");
        }

        var now = DateTime.UtcNow;

        if (startDate.HasValue)
        {
            if (startDate.Value.Date < now.Date)
            {
                throw new BadRequestException("Start date can not less than today.");
            }

            if (expiredDate.Date < startDate.Value.AddDays(14).Date)
            {
                throw new BadRequestException(
                    "The expiration date must be at least 14 days from the start date.");
            }
        }
        else
        {
            if (expiredDate.Date < now.AddDays(14).Date)
            {
                throw new BadRequestException(
                    "The expiration date must be at least 14 days from today.");
            }
        }

        if(survey.PackType == null)
        {
            throw new BadRequestException("You haven't bought pack yet");
        }

        survey.StartDate = startDate ?? now;
        survey.ExpiredDate = expiredDate;
        survey.Status = SurveyStatus.Active;
        _unitOfWork.SurveyRepository.Update(survey);
        await _unitOfWork.SaveChangeAsync();

        return _mapper.Map<CommonSurveyResponse>(survey);
    }
}