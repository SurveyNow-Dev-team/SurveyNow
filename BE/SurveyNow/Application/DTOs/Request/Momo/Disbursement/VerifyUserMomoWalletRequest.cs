using Application.DTOs.Response.Momo;
using Application.Utils;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace Application.DTOs.Request.Momo.Disbursement
{
    public class VerifyUserMomoWalletRequest
    {
        public string partnerCode { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public string requestType { get; set; } = string.Empty;
        public string disbursementMethod { get; set; } = string.Empty;
        public string lang { get; set; } = string.Empty;
        public string signature { get; set; } = string.Empty;

        public VerifyUserMomoWalletRequest(string partnerCode, string orderId, string requestId, string requestType, string lang)
        {
            this.partnerCode = partnerCode;
            this.orderId = orderId;
            this.requestId = requestId;
            this.requestType = requestType;
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
                "&disbursementMethod=" + disbursementMethod +
                "&orderId=" + orderId +
                "&partnerCode=" + partnerCode +
                "&requestId=" + requestId +
                "&requestType=" + requestType;
            this.signature = HashUtil.HmacSHA256(rawHash, secretKey);
        }

        public (bool, string?) VerifyMomoWallet(string verifyUrl)
        {
            using HttpClient client = new HttpClient();
            var requestData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var verifyResponse = client.PostAsync(verifyUrl, requestContent).Result;

            if (verifyResponse.IsSuccessStatusCode)
            {
                var responseContent = verifyResponse.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<VerifyUserMomoWalletResponse>(responseContent);
                if (responseData.resultCode == 0)
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
                return (false, verifyResponse.ReasonPhrase);
            }
        }
    }
}
