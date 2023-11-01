using Domain.Enums;

namespace Application.DTOs.Request.Transaction
{
    /// <summary>
    /// Request DTO dùng để gửi yêu cầu nạp điểm vào tài khoản của người dùng
    /// </summary>
    public class PointPurchaseTransactionCreateRequest
    {
        public decimal PointAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
