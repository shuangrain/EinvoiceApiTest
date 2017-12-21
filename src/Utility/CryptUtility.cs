using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EinvoiceApiTest.Utility
{
    public class CryptUtility
    {
        public static string HMACSHA1(string APIKey, string data)
        {
            byte[] key = Encoding.UTF8.GetBytes(APIKey);
            HMACSHA1 sha1 = new HMACSHA1(key);
            byte[] source = Encoding.UTF8.GetBytes(data);
            byte[] crypto = sha1.ComputeHash(source);
            return Convert.ToBase64String(crypto);
        }
    }
}
