using System.Text;
using System.Security.Cryptography;
using System;

namespace Genesis.GameClient
{
    public class AESMgrTool
    {

        //Key 要16位
        public static string Encrypt(string toEncrypt, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            return ToHexStr(resultArray);
        }

        public static string Decrypt(string toDecrypt, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            //       byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            byte[] toEncryptArray = HexToByte(toDecrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);

        }
        /**
      * hex转成byte[]和上面的方法配合使用
      * 
      * @param srt
      * @return
      */
        private static byte[] HexToByte(String str)
        {
            byte[] ab = new byte[str.Length / 2];
            for (int i = 0; i < ab.Length; i++)
            {
                ab[i] = (byte)(Convert.ToInt32(str.Substring(i * 2, 2), 16));
            }
            return ab;
        }

        /**
        * byte数组转换成hex字符串
        * 
        * @param ba
        * @return
*/
        private static String ToHexStr(byte[] ba)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0, c = ba.Length; i < c; i++)
            {
                sb.Append(String.Format("{0:X2}", ba[i] & 0xFF).ToUpper());
            }
            return sb.ToString();
        }

    }
}