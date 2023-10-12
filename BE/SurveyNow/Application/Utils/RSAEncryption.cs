using System.Security.Cryptography;
using System.Text;

namespace Application.Utils
{
    public class RSAEncryption
    {
        private static RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
        private RSAParameters _publicKey;

        public RSAEncryption(string modulusHex, string publicExponentHex)
        {
            _publicKey = new RSAParameters
            {
                Modulus = Convert.FromHexString(modulusHex),
                Exponent = Convert.FromHexString(publicExponentHex),
            };
            provider.ImportParameters(_publicKey);
        }

        public string Encrypt(string plainText)
        {
            var data = Encoding.UTF8.GetBytes(plainText);
            var encrypted = provider.Encrypt(data, false);
            return Convert.ToBase64String(encrypted);
        }
    }
}
