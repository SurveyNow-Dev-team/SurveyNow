using Application.DTOs.Request.Survey;
using Application.DTOs.Response;
using Application.DTOs.Response.Survey;
using Domain.Enums;

namespace Application.Interfaces.Services;

public interface ISurveyService
{
    Task<SurveyDetailResponse> CreateSurveyAsync(SurveyRequest request);
    Task<SurveyDetailResponse> GetByIdAsync(long id);
    Task<List<SurveyResponse>> GetAllAsync();

    Task<PagingResponse<SurveyResponse>> FilterSurveyAsync(
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
        int? size);

    Task<PagingResponse<CommonSurveyResponse>> FilterCommonSurveyAsync(
        string? status,
        string? title,
        string? sortTitle,
        string? sortTotalQuestion,
        string? sortPoint,
        string? sortStartDate,
        string? sortExpiredDate,
        int? page,
        int? size
    );

    Task<PagingResponse<SurveyResponse>> FilterAccountSurveyAsync(
        string? status,
        string? packType,
        string? title,
        string? sortTitle,
        string? sortCreatedDate,
        string? sortStartDate,
        string? sortExpiredDate,
        string? sortModifiedDate,
        int? page,
        int? size
    );

    Task DeleteSurveyAsync(long id);

    Task<SurveyDetailResponse> UpdateSurveyAsync(long id, SurveyRequest request);
    Task<SurveyDetailResponse> ChangeSurveyStatusAsync(long id);

    Task DoSurveyAsync(DoSurveyRequest request);

    Task<PagingResponse<CommonSurveyResponse>> FilterCompletedSurveyAsync(
        string? title,
        string? sortTitle,
        string? sortDate,
        int? page,
        int? size,
        bool disableTracking = true
    );

    Task<SurveyDetailResponse> GetAnswerAsync(long surveyId);

    Task<PagingResponse<UserSurveyResponse>> GetUserSurveyAsync(
        long surveyId,
        bool? isValid,
        int? page,
        int? size,
        bool disableTracking = true
    );

    Task<CommonSurveyResponse> PostSurveyAsync(long surveyId,
        DateTime? startDate, 
        DateTime expiredDate,
        CriterionRequest? criterionRequest);
}