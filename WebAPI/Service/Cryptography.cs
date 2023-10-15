using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebAPI.Service
{
    public static class Cryptography
    {
        private const string ENCRYPTION_KEY = "A60934D8C1A2AC3A69642A3902198";
        private readonly static byte[] SALT = new byte[] { 99, 52, 2, 24, 51, 67, 22, 88 };
        public static string EncryptUrl(string originalUrl, int maxLength)
        {
            byte[] plainText = Encoding.Unicode.GetBytes(originalUrl);
            var secretKey = new Rfc2898DeriveBytes(ENCRYPTION_KEY, SALT);

            using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
            {
                using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainText, 0, plainText.Length);
                    cryptoStream.FlushFinalBlock();
                    string encryptedUrl = Convert.ToBase64String(memoryStream.ToArray());
                    encryptedUrl = HttpUtility.UrlEncode(encryptedUrl);
                    
                    if (encryptedUrl.Length > maxLength)
                    {
                        encryptedUrl = encryptedUrl.Substring(0, maxLength);

                    }
                    return encryptedUrl;
                }
            }
        }

    }
}
//
/*
 * {
 * "ShrinkUrlSettings": {
    "BaseUrl": "https://example.co/",
    "MaxLength": 6
    }
    "Logging": {
        "LogLevel":{
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
     }
    },

    "AllowedHosts": "*"
   }
*/

