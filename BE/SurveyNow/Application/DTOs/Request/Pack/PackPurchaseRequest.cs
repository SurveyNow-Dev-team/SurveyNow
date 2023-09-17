using Domain.Enums;

namespace Application.DTOs.Request.Pack
{
    public class PackPurchaseRequest
    {
        public required PackType PackType { get; set; }
        public required long SurveyId { get; set; }
        public required int TotalParticipants { get; set; }
    }
}
