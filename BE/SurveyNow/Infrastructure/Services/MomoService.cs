using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Momo.Disbursement;
using Application.DTOs.Request.Point;
using Application.DTOs.Response.Momo;
using Application.DTOs.Response.Point;
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

        public async Task<CheckMerchantBalanceResponse?> CheckMerchantBalanceAsync()
        {
            string orderId = Guid.NewGuid().ToString();
            CheckMerchantBalanceRequest request = new CheckMerchantBalanceRequest(options.Value.PartnerCode,
                orderId,
                orderId,
                options.Value.Language);
            request.MakeSignature(options.Value.AccessKey, options.Value.SecretKey);
            (bool result, string? resultData) = request.CheckBalance(options.Value.DisbursementBalanceUrl);
            if (!result)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<CheckMerchantBalanceResponse>(resultData);
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
                paymentRequest.redirectUrl = options.Value.MobileReturnUrl;
            }

            paymentRequest.MakeSignature(options.Value.AccessKey, options.Value.SecretKey);

            (bool createPaymentResult, string? resultString) = paymentRequest.GetPaymentMethod(options.Value.PaymentUrl);

            if (!createPaymentResult)
            {
                return null;

            }
            return JsonConvert.DeserializeObject<MomoCreatePaymentResponse>(resultString);
        }

        public async Task<MomoPointRedeemResponse> ProcessMomoRedeemGiftAsync(PointRedeemRequest redeemRequest)
        {
            // Validate user momo wallet
            (bool verifyResult, string? verifyData) = VerifyUserMomoWallet(redeemRequest);
            if (!verifyResult)
            {
                return new MomoPointRedeemResponse()
                {
                    Status = TransactionStatus.Cancel.ToString(),
                    Message = verifyData,
                };
            }

            // Check merchant momo wallet balance

            // Pay user
            (bool payResult, string? payData) = PayUserMomoWallet(redeemRequest);
            if (!payResult)
            {
                return new MomoPointRedeemResponse()
                {
                    Status = TransactionStatus.Fail.ToString(),
                    Message = payData,
                };
            }

            // Save transaction in app db

            // return result
            return new MomoPointRedeemResponse()
            {
                Status = "",
                Message = "",
                PointAmount = redeemRequest.PointAmount,
                MoneyAmount = redeemRequest.PointAmount * BusinessData.BasePointVNDPrice,
                TransactionId = "",
                PaymentMethod = PaymentMethod.Momo.ToString(),
            };
        }

        private (bool, string?) VerifyUserMomoWallet(PointRedeemRequest redeemRequest)
        {
            string verifyId = Guid.NewGuid().ToString();
            VerifyUserMomoWalletRequest verifyRequest = new VerifyUserMomoWalletRequest(options.Value.PartnerCode,
                verifyId,
                verifyId,
                "checkWallet",
                options.Value.Language);
            MomoWallet userWallet = new MomoWallet() { walletId = redeemRequest.MomoAccount };
            string userWalletJson = JsonConvert.SerializeObject(userWallet);
            verifyRequest.SetDisbursementMethod(userWalletJson, options.Value.PublicKeyModulus, options.Value.PublicKeyExponent);
            verifyRequest.MakeSignature(options.Value.AccessKey, options.Value.SecretKey);
            (bool verifyResult, string? verifyData) = verifyRequest.VerifyMomoWallet(options.Value.DisbursementVerifyUrl);
            return (verifyResult, verifyData);
        }

        private (bool, string?) PayUserMomoWallet(PointRedeemRequest redeemRequest)
        {
            string payId = Guid.NewGuid().ToString();
            decimal moneyAmount = redeemRequest.PointAmount * BusinessData.BasePointVNDPrice;
            string orderInfo = EnumUtil.GeneratePointHistoryDescription(PointHistoryType.RedeemPoint, redeemRequest.UserId, redeemRequest.PointAmount, paymentMethod: PaymentMethod.Momo);
            PayUserRequest payRequest = new PayUserRequest(options.Value.PartnerCode,
                payId,
                (long)moneyAmount,
                payId,
                "disburseToWallet",
                "",
                orderInfo,
                options.Value.Language);
            MomoWallet userWallet = new MomoWallet() { walletId = redeemRequest.MomoAccount };
            string userWalletJson = JsonConvert.SerializeObject(userWallet);
            payRequest.SetDisbursementMethod(userWalletJson, options.Value.PublicKeyModulus, options.Value.PublicKeyExponent);
            payRequest.MakeSignature(options.Value.AccessKey, options.Value.SecretKey);
            (bool payResult, string? payData) = payRequest.PayUser(options.Value.DisbursementPayUrl);
            return (payResult, payData);
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
