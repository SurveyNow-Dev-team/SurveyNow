using Application.DTOs.Response.Momo;
using Application.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Application.DTOs.Request.Momo
{
    public class MomoCreatePaymentRequest
    {
        public string partnerCode { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public long amount { get; set; }
        public string orderId { get; set; } = string.Empty;
        public string orderInfo { get; set; } = string.Empty;
        public string redirectUrl { get; set; } = string.Empty;
        public string ipnUrl { get; set; } = string.Empty;
        public string requestType { get; set; } = string.Empty;
        public string extraData { get; set; } = string.Empty;
        public string lang { get; set; } = string.Empty;
        public string signature { get; set; } = string.Empty;

        public MomoCreatePaymentRequest(string partnerCode, string requestId, long amount, string orderId, string orderInfo, string redirectUrl, string ipnUrl, string requestType, string extraData, string lang)
        {
            this.partnerCode = partnerCode;
            this.requestId = requestId;
            this.amount = amount;
            this.orderId = orderId;
            this.orderInfo = orderInfo;
            this.redirectUrl = redirectUrl;
            this.ipnUrl = ipnUrl;
            this.requestType = requestType;
            this.extraData = extraData;
            this.lang = lang;
        }

        public void MakeSignature(string accessKey, string secretKey)
        {
            var rawHash = "accessKey=" + accessKey +
                "&amount=" + this.amount +
                "&extraData=" + this.extraData +
                "&ipnUrl=" + this.ipnUrl +
                "&orderId=" + this.orderId +
                "&orderInfo=" + this.orderInfo +
                "&partnerCode=" + this.partnerCode +
                "&redirectUrl=" + this.redirectUrl +
                "&requestId=" + this.requestId +
                "&requestType=" + this.requestType;
            this.signature = HashUtil.HmacSHA256(rawHash, secretKey);
        }

        public (bool, string?) GetPaymentMethod(string paymentUrl)
        {
            using HttpClient client = new HttpClient();
            var requestData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var createPaymentResponse = client.PostAsync(paymentUrl, requestContent).Result;

            if (createPaymentResponse.IsSuccessStatusCode)
            {
                var responseContent = createPaymentResponse.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<MomoCreatePaymentResponse>(responseContent);
                if (responseData.resultCode == "0")
                {
                    return (true, responseContent);
                }
                else
                {
                    return (false, responseData.message);
                }
            }
            else
            {
                return (false, createPaymentResponse.ReasonPhrase);
            }
        }
    }
}
