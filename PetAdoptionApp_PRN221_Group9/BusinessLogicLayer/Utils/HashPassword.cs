using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Utils
{
    public class HashPassword
    {
        public static string HashWithSHA256(string input)
        {
            using SHA256 sHA256 = SHA256.Create();

            var inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] bytes = sHA256.ComputeHash(inputBytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
