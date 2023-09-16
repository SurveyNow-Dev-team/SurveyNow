using Application.DTOs.Response.Transaction;

namespace Application.DTOs.Response.Point.History
{
    public class PointPurchaseDetailResponse : BasePointHistoryResponse
    {
        public required TransactionResponse Transaction { get; set; }
    }
}
