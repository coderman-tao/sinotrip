using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SinoTrip.FrameWork.Security
{
    public class Rijndael
    {
        private SymmetricAlgorithm mobjCryptoService;
        private string Key;
        ///<summary> 
        ///对称加密类的构造函数
        ///</summary> 
        public Rijndael()
        {
            mobjCryptoService = new RijndaelManaged();
            Key = "Quz(m&hr7x89H$yuBIc456Ftmag5&fvHUFCyd6*h%(HilJ$lhj!e6&(*jkP87jH7";
        }
        ///<summary>
        ///获得密钥
        ///</summary> 
        ///<returns>密钥</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength, ' ');
            }
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        ///<summary>
        ///获得初始向量IV
        ///</summary> 
        ///<returns>初试向量IV</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = "C4ghj*Ghg7!rNcfb&95GpY86GfRhAb#er67HB9(u%g6HJ($chWkT&!hg4ui%$hji";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength, ' ');
            }
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        ///<summary>
        ///加密方法
        ///<param name="Source">待加密的串</param>
        ///</summary> 
        ///<returns>经过加密的串</returns>
        public string Encrypto(string Source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length); cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }
        ///<summary>
        ///解密方法
        ///<param name="Source">待解密的串</param>
        ///</summary> 
        ///<returns>经过解密的串</returns>
        public string Decrypto(string Source)
        {
            if (string.IsNullOrEmpty(Source))
            {
                return string.Empty;
            }
            byte[] bytIn = Convert.FromBase64String(Source);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
