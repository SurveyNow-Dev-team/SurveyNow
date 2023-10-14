using Application.Utils;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace Application.DTOs.Request.Momo.Disbursement
{
    public class PayUserRequest
    {
        public string partnerCode { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public long amount { get; set; }
        public string requestId { get; set; } = string.Empty;
        public string requestType { get; set; } = string.Empty;
        public string disbursementMethod { get; set; } = string.Empty;
        public string extraData { get; set; } = string.Empty;
        public string orderInfo { get; set; } = string.Empty;
        public string lang { get; set; } = string.Empty;
        public string signature { get; set; } = string.Empty;

        public PayUserRequest(string partnerCode, string orderId, long amount, string requestId, string requestType, string extraData, string orderInfo, string lang)
        {
            this.partnerCode = partnerCode;
            this.orderId = orderId;
            this.amount = amount;
            this.requestId = requestId;
            this.requestType = requestType;
            this.extraData = extraData;
            this.orderInfo = orderInfo;
            this.lang = lang;
        }

        public void SetDisbursementMethod(string jsonData, string modulus, string exponent)
        {
            RSAEncryption rsa = new RSAEncryption(modulus, exponent);
            disbursementMethod = rsa.Encrypt(jsonData);
        }

        public void MakeSignature(string accessKey, string secretKey)
        {
            var rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&disbursementMethod=" + disbursementMethod +
                "&extraData=" + extraData +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&requestId=" + requestId +
                "&requestType=" + requestType;
            this.signature = HashUtil.HmacSHA256(rawHash, secretKey);
        }

        public (bool, string?) PayUser(string payUrl)
        {
            using HttpClient client = new HttpClient();
            var requestData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var payResponse = client.PostAsync(payUrl, requestContent).Result;

            if (payResponse.IsSuccessStatusCode)
            {
                var responseContent = payResponse.Content.ReadAsStringAsync().Result;
                return (true, responseContent);
            }
            else
            {
                return (false, payResponse.ReasonPhrase);
            }
        }
    }
}
