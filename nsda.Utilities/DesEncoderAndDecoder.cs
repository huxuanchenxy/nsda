using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace nsda.Utilities
{
    public class DesEncoderAndDecoder
    {
        public static string Decrypt(string message)
        {
            string key = "!qax@魑魅wsnsdaz耄耋#rfc魍魉c$edv";
            byte[] inputByteArray = Convert.FromBase64String(message);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                des.Key = Encoding.ASCII.GetBytes(key.Md5().Substring(0, 8));
                des.IV = Encoding.ASCII.GetBytes(key.Md5().Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        public static string Encrypt(string message)
        {
            string key = "!qax@魑魅wsnsdaz耄耋#rfc魍魉c$edv";
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(message);
                des.Key = ASCIIEncoding.ASCII.GetBytes(key.Md5().Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(key.Md5().Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }
    }
}
