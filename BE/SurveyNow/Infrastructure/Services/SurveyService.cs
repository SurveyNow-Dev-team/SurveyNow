using Application;
using Application.DTOs.Request.Survey;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
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
}