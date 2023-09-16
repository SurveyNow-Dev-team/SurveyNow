using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Response.Point.History
{
    public class BasePointHistoryResponse
    {
        // 1
        public long Id { get; set; }

        // dd/mm/yyyy HH:mm:ss
        public required string Date { get; set; }

        public string? Description { get; set; }

        public PointHistoryType PointHistoryType { get; set; }

        // max 999,999,999.9
        [Precision(10, 1)]
        public decimal Point { get; set; }

        public TransactionStatus Status { get; set; }

        public long UserId { get; set; }
    }
}
