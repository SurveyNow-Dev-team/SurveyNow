using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Point;
using Application.DTOs.Response.Momo;
using Application.DTOs.Response.Point;

namespace Application.Interfaces.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponse> CreateMomoPaymentAsync(PointPurchaseRequest purchaseRequest);
        (bool, string) ValidateMomoPaymentResult(MomoCreatePaymentResultRequest resultRequest);
        Task<CheckMerchantBalanceResponse> CheckMerchantBalanceAsync();
        Task<MomoPointRedeemResponse> ProcessMomoRedeemGiftAsync(PointRedeemRequest redeemRequest);
    }
}
