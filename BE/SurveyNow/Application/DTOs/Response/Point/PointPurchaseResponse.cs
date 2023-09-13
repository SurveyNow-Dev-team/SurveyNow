using Domain.Enums;

namespace Application.DTOs.Response.Point
{
    public class PointPurchaseResponse
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public int Point { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
