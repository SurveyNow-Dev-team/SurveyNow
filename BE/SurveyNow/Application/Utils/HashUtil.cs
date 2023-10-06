using System.Security.Cryptography;
using System.Text;

namespace Application.Utils
{
    public static class HashUtil
    {
        public static string HmacSHA256(string inputData, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageByte = Encoding.UTF8.GetBytes(inputData);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageByte);
                string hex = BitConverter.ToString(hashMessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;
            }
        }

        public static string EncodeStringToBase64(string inputData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(inputData);

            return Convert.ToBase64String(bytes);
        }

        public static string DecodeStringToBase64(string inputData)
        {
            byte[] bytes = Convert.FromBase64String(inputData);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
