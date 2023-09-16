using Microsoft.EntityFrameworkCore;

namespace Application.DTOs.Response.Point.History
{
    public class ShortPointHistoryResponse
    {
        public long Id { get; set; }

        // dd/mm/yyyy HH:mm:ss
        public required string Date { get; set; }

        public required string PointHistoryType { get; set; }

        // max 999,999,999.9
        [Precision(10, 1)]
        public decimal Point { get; set; }

        public required string Status { get; set; }
    }
}
