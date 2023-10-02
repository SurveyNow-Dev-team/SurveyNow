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
                options.Value.WebReturnUrl,
                options.Value.IpnUrl,
                options.Value.RequestType,
                extraData,
                options.Value.Language);
            if (purchaseRequest.Platform == Platform.Mobile)
            {
                paymentRequest.redirectUrl = options.Value.MobilebReturnUrl;
            }

            paymentRequest.MakeSignature(options.Value.AccessKey, options.Value.SecretKey);

            (bool createPaymentResult, string? resultString) = paymentRequest.GetPaymentMethod(options.Value.PaymentUrl);

            if (!createPaymentResult)
            {
                return null;

            }
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponse>(resultString);
        }

        public (bool, string) ValidateMomoPaymentResult(MomoCreatePaymentResultRequest resultRequest)
        {
            // Check result signature (Always return invalid, fix later)
            //if(!resultRequest.IsValidSignature(options.Value.AccessKey, options.Value.SecretKey))
            //{
            //    return (false, "Invalid momo transaction signature");
            //}

            // Check result code
            switch (resultRequest.resultCode)
            {
                case 0:
                    return (true, "Successful momo transaction");
                case 1001:
                    return (false, "Insufficient user momo balance");
                case 1003:
                    return (false, "Transaction has been cancelled by Momo or SurveyNow");
                case 1005:
                    return (false, "Expired transaction's pay url or QR code");
                case 1006:
                    return (false, "Transaction has been cancelled by user");
                default:
                    return (false, "Failed momo transaction");
            }
        }
    }
}
