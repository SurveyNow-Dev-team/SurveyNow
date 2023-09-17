using Application;
using Application.DTOs.Request.Survey;
using Application.Interfaces.Services;

namespace Infrastructure.Services;

public class SurveyService : ISurveyService
{
    private readonly IUnitOfWork _unitOfWork;

    public SurveyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}