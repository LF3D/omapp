using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// Implements method to decrypt specific string.
    /// </summary>
    public class EncryptHelper
    {
        static readonly string plusReplacer = "______OM_________________";
        /// <summary>
        /// Decrypt the specific string with MD5+TRIPLE DES.
        /// </summary>
        /// <param name="planText">The specific string</param>
        public static string Decrypt(string planText)
        {
            try
            {
                planText = planText.Replace(plusReplacer, "+");
                byte[] inputs = Convert.FromBase64String(planText);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] buffer = md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(ConfigurationManager.AppSettings["encryptKey"]));
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.Key = buffer;
                des.Mode = CipherMode.ECB;
                return ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(inputs, 0, inputs.Length));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
