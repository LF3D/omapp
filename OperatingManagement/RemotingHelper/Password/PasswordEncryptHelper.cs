using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// Provides method to encrypt password.
    /// </summary>
    public class PasswordEncryptHelper
    {
        private static string GenerateSalt()
        {
            byte[] buffer = Encoding.Unicode.GetBytes(
                ConfigurationManager.AppSettings["encryptSalt"].ToString());
            return Convert.ToBase64String(buffer);
        }

        private static string GenerateSalt(string salt)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(salt);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Encrypt the password with salt and encryptType.
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static string EncryptPassword(string password)
        {
            string hashAlgorithmType = "SHA1";
            return EncryptPassword(password, hashAlgorithmType);
        }

        /// <summary>
        /// Encrypt the password with salt and encryptType.
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="salt">salt</param>
        /// <returns></returns>
        public static string EncryptPasswordBySalt(string password, string salt)
        {
            string hashAlgorithmType = "SHA1";
            return EncryptPasswordBySalt(password, hashAlgorithmType, salt);
        }
        
        /// <summary>
        /// Encrypt the password.
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="hashAlgorithmType">md5/sha1/clear</param>
        /// <returns></returns>
        private static string EncryptPassword(string password, string hashAlgorithmType)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;
            string salt = GenerateSalt();
            byte[] bIn = Encoding.Unicode.GetBytes(password);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
            HashAlgorithm s = HashAlgorithm.Create(hashAlgorithmType);

            if (s == null)
                throw new Exception("Could not create a hash algorithm");

            bRet = s.ComputeHash(bAll);
            return Convert.ToBase64String(bRet);
        }

        /// <summary>
        /// Encrypt the password with passed salt
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="hashAlgorithmType">md5/sha1/clear</param>
        /// <param name="salt">salt</param>
        /// <returns></returns>
        private static string EncryptPasswordBySalt(string password, string hashAlgorithmType, string salt)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;
            string strSalt = GenerateSalt(salt);
            byte[] bIn = Encoding.Unicode.GetBytes(password);
            byte[] bSalt = Convert.FromBase64String(strSalt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
            HashAlgorithm s = HashAlgorithm.Create(hashAlgorithmType);

            if (s == null)
                throw new Exception("Could not create a hash algorithm");

            bRet = s.ComputeHash(bAll);
            return Convert.ToBase64String(bRet);
        }
    }
}
