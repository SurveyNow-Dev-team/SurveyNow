using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs.Response.Point
{
    public class PointPurchaseDetailResponse
    {
        public long Id { get; set; }

        public string Date { get; set; }

        public string? Description { get; set; }

        public PointHistoryType Type { get; set; }

        public int Point { get; set; }

        public TransactionStatus Status { get; set; }

        public long UserId { get; set; }

        public ShortPointPurchaseResponse? PointPurchase { get; set; }
    }
}
