using Application.DTOs.Response.Pack;

namespace Application.DTOs.Response.Point.History
{
    public class PointPackPurchaseDetailResponse : BasePointHistoryResponse
    {
        public required PackPurchaseResponse PackPurchaseResponse { get; set; }
    }
}
