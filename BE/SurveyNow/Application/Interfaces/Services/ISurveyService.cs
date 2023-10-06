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
}