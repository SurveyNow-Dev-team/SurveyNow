using Application.DTOs.Response.Momo;
using Application.Utils;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace Application.DTOs.Request.Momo.Disbursement
{
    public class CheckMerchantBalanceRequest
    {
        public string partnerCode { get; set; } = string.Empty;
        public string orderId { get; set; } = string.Empty;
        public string requestId { get; set; } = string.Empty;
        public string lang { get; set; } = string.Empty;
        public string signature { get; set; } = string.Empty;

        public CheckMerchantBalanceRequest(string partnerCode, string orderId, string requestId, string lang)
        {
            this.partnerCode = partnerCode;
            this.orderId = orderId;
            this.requestId = requestId;
            this.lang = lang;
        }

        public void MakeSignature(string accessKey, string secretKey)
        {
            var rawHash = "accessKey=" + accessKey +
                "&orderId=" + orderId +
                "&partnerCode=" + partnerCode +
                "&requestId=" + requestId;
            signature = HashUtil.HmacSHA256(rawHash, secretKey);
        }

        public (bool, string?) CheckBalance(string checkBalanceUrl)
        {
            using HttpClient client = new HttpClient();
            var requestData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var checkResponse = client.PostAsync(checkBalanceUrl, requestContent).Result;

            if (checkResponse.IsSuccessStatusCode)
            {
                var responseContent = checkResponse.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<CheckMerchantBalanceResponse>(responseContent);
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
                return (false, checkResponse.ReasonPhrase);
            }
        }
    }
}
