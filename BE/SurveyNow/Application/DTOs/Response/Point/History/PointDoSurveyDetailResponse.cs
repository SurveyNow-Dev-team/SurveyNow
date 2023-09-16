using Application.DTOs.Response.Survey;

namespace Application.DTOs.Response.Point.History
{
    public class PointDoSurveyDetailResponse : BasePointHistoryResponse
    {
        public required ShortSurveyResponse Survey { get; set; }
    }
}
