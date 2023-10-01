using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Point;
using Application.DTOs.Response.Momo;
using Application.Interfaces.Services;
using Application.Utils;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoConfig> options;

        public MomoService(IOptions<MomoConfig> options)
        {
            this.options = options;
        }

        public async Task<MomoCreatePaymentResponse> CreateMomoPaymentAsync(PointPurchaseRequest purchaseRequest)
        {
            string orderId = Guid.NewGuid().ToString();
            decimal moneyAmount = purchaseRequest.PointAmount * BusinessData.BasePointVNDPrice;
            string orderInfo = EnumUtil.GeneratePointHistoryDescription(PointHistoryType.PurchasePoint, purchaseRequest.UserId, purchaseRequest.PointAmount, paymentMethod: purchaseRequest.PaymentMethod);

            string extraData = "";

            MomoCreatePaymentRequest paymentRequest = new MomoCreatePaymentRequest(options.Value.PartnerCode,
                orderId,
                (long)moneyAmount,
                orderId,
                orderInfo,
                options.Value.ReturnUrl,
                options.Value.IpnUrl,
                options.Value.RequestType,
                extraData,
                options.Value.Language);
            paymentRequest.MakeSignature(options.Value.AccessKey, options.Value.SecretKey);

            (bool createPaymentResult, string? resultString) = paymentRequest.GetPaymentMethod(options.Value.PaymentUrl);

            if (!createPaymentResult)
            {
                return null;
                
            }
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponse>(resultString);
        }
    }
}
