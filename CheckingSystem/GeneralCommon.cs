using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CheckingSystem
{
    class GeneralCommon
    {
        public static string ErrorMessage = "";
        public static string ConvertDateToString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
            //return dt.Year.ToString()+ "-" + str2(dt.Month) + "-" + str2(dt.Day) + " " + str2(dt.Hour) + ":" + str2(dt.Minute) + ":" + str2(dt.Second) + "." + str3(dt.Millisecond);
        }
        public static double ConvertToPercent(SysValues param)
        {
            double d = 100 * param.Used / param.Total;

            return Math.Round(d,2);
        }
        #region Encrypt
        /// <summary>
        /// Created By : Herry Sutedja
        /// Created Date : 3 November 2016
        /// Purpose : For encrypt string
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            try
            {
                if (!string.IsNullOrEmpty(clearText))
                {
                    var SaltKey = "";
                    var SaltKeyPaarameter = CheckingSystem.Properties.Settings.Default.PKey;
                    if (SaltKeyPaarameter != null)
                        SaltKey = SaltKeyPaarameter;
                    else
                        SaltKey = "Ind0P@yrO11-A5a";
                    string strEncryptionKey = SaltKey;

                    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(strEncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(clearBytes, 0, clearBytes.Length);
                                cs.Close();
                            }
                            clearText = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
                return clearText;
            }
            
            catch (Exception E)
            {

                ErrorMessage = E.ToString();

                if (E.InnerException != null)
                {
                    ErrorMessage = ErrorMessage + E.InnerException.ToString();
                }
                //UIException.LogException(ErrorMessage, E.StackTrace);
            }
            return clearText;
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// Created By : Herry Sutedja
        /// Created Date : 3 November 2016
        /// Purpose : For decrypt string
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            try
            {
                if (!string.IsNullOrEmpty(cipherText))
                {
                    var SaltKey = "";
                    var SaltKeyPaarameter = CheckingSystem.Properties.Settings.Default.PKey;
                    if (SaltKeyPaarameter != null)
                        SaltKey = SaltKeyPaarameter;
                    else
                        SaltKey = "Ind0P@yrO11-A5a";
                    string strEncryptionKey = SaltKey;

                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(strEncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            cipherText = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                }
                return cipherText;
            }
            
            catch (Exception E)
            {

                ErrorMessage = E.ToString();

                if (E.InnerException != null)
                {
                    ErrorMessage = ErrorMessage + E.InnerException.ToString();
                }
                //UIException.LogException(ErrorMessage, E.StackTrace);
            }
            return cipherText;
        }
        #endregion
    }
}
