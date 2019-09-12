using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CheckingSystem
{
    class ServiceLog
    {
        private const string DEFAULT_LOG_EXT = ".log";
        private string m_strLogFile = string.Empty;
        private StreamWriter m_sw = null;
        private StreamReader m_sr = null;
        private const int DEFAULT_MAX_SIZE = 5120000;
        private int m_intMaxSize = DEFAULT_MAX_SIZE;
        private string m_strFullLogPath = string.Empty;
        public ServiceLog() { }
        public string LogFile
        {
            set
            {
                m_strLogFile = value;

                // generate the log file based on time and date
                //AppendDateTimeToFileName();
            }
            get
            {
                return m_strLogFile;
            }
        }

        #region private
        private void AppendDateTimeToFileName()
        {
            if (string.Empty == m_strLogFile)
                GenerateDefaultLogFile();
            else
            {
                int inPos = m_strLogFile.LastIndexOf(".");
                string strPostFix = string.Empty;
                string strTemp = string.Empty;
                string strDirNm = string.Empty;
                string strArchieve = string.Empty;


                if (File.Exists(m_strLogFile))
                {
                    FileInfo fi = new FileInfo(m_strLogFile);

                    if (0 < inPos)
                    {
                        strTemp = m_strLogFile.Substring(0, inPos);
                        strPostFix = m_strLogFile.Substring(inPos);

                        strArchieve = strTemp + "_Archieve_" + DateTime.Now.ToString("yyyyMMddHHmmss") + strPostFix;
                    }
                    else
                    {
                        strArchieve = m_strLogFile + "_Archieve_" + DateTime.Now.ToString("yyyyMMddHHmmss") + DEFAULT_LOG_EXT;
                    }
                    File.Copy(m_strLogFile, strArchieve);
                    File.Delete(m_strLogFile);

                }
                try
                {


                    strDirNm = System.IO.Path.GetDirectoryName(m_strLogFile);
                    if (!System.IO.Directory.Exists(strDirNm))
                    {
                        System.IO.Directory.CreateDirectory(strDirNm);
                    }

                    m_sw = new StreamWriter(m_strLogFile);
                    m_sw.WriteLine("[Log started]");
                    m_sw.Flush();
                    m_sw.Close();
                }
                catch //(Exception e) 
                { }
            }
        }

        private void GenerateDefaultLogFile()
        {
            string strBaseDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();
            int nFirstSlashPos = strBaseDirectory.LastIndexOf("\\");
            string strTemp = string.Empty;
            string strProject = string.Empty;

            if (0 < nFirstSlashPos)
            {
                strTemp = strBaseDirectory.Substring(0, nFirstSlashPos);

                nFirstSlashPos = strTemp.LastIndexOf("\\");
                if (0 < nFirstSlashPos)
                    strProject = strTemp.Substring(nFirstSlashPos);

                m_strLogFile = strTemp + "\\" + strProject + DEFAULT_LOG_EXT;

                try
                {
                    m_sw = new StreamWriter(m_strLogFile);
                    m_sw.WriteLine("[Log started]");
                    m_sw.Flush();
                    m_sw.Close();
                }
                catch //(Exception e) 
                { }
            }
        }

        private void GenerateDefaultLogFileTemp(string strPath)
        {
            int nFirstSlashPos = strPath.LastIndexOf("\\");
            string strTemp = string.Empty;
            string strProject = string.Empty;
            if (0 < nFirstSlashPos)
            {
                strTemp = strPath.Substring(0, nFirstSlashPos);
                Directory.CreateDirectory(strTemp);
            }
            try
            {
                m_sw = new StreamWriter(strPath);
                m_sw.Flush();
                m_sw.Close();
            }
            catch //(Exception e) 
            { }
        }
        private void GenerateDefaultLogFile(string strPath)
        {
            int nFirstSlashPos = strPath.LastIndexOf("\\");
            string strTemp = string.Empty;
            string strProject = string.Empty;
            if (0 < nFirstSlashPos)
            {
                strTemp = strPath.Substring(0, nFirstSlashPos);
                Directory.CreateDirectory(strTemp);
            }
            try
            {
                m_sw = new StreamWriter(strPath);
                m_sw.WriteLine("[Log started]");
                m_sw.Flush();
                m_sw.Close();
            }
            catch //(Exception e) 
            { }
        }
        private void GenerateNewLogFile()
        {
            AppendDateTimeToFileName();
        }

        private void WriteLog(string strPathName, string strMessage)
        {
            try
            {
                m_sw = new StreamWriter(strPathName, true);

                m_sw.Write(strMessage);

                m_sw.Flush();
                m_sw.Close();
            }
            catch (Exception) { }
        }

        private void WriteLog(string strPathName, ref Exception objException)
        {
            try
            {
                m_sw = new StreamWriter(strPathName, true);

                m_sw.Write("[" + DateTime.Now.ToShortDateString() + " " +
                    DateTime.Now.ToLongTimeString() + "]");
                m_sw.Write("\t");
                m_sw.Write("@Exception : ");
                m_sw.Write(objException.Source.ToString().Trim() + "\t");
                m_sw.Write(objException.TargetSite.Name.ToString() + "\t");
                m_sw.Write(objException.Message.ToString().Trim() + " - " +
                    objException.StackTrace.ToString().Trim() + "\r\n");

                m_sw.Flush();
                m_sw.Close();
            }
            catch (Exception) { }
        }

       
        #endregion

        #region public
        public void Log(string strMessage)
        {
            try
            {
                // check if file name provided.
                // if not, will create an default file				
                if (string.Empty == m_strLogFile)
                    GenerateDefaultLogFile();

                // check if maximum log file size has reached.
                // if yes, will create a new file
                if (File.Exists(m_strLogFile))
                {
                    FileInfo fileInfo = new FileInfo(m_strLogFile);
                    if (fileInfo.Length >= m_intMaxSize)
                        GenerateNewLogFile();
                }
                else
                {

                    GenerateDefaultLogFile(m_strLogFile);

                    if (File.Exists(m_strLogFile))
                    {
                        FileInfo fileInfo = new FileInfo(m_strLogFile);
                        if (fileInfo.Length >= m_intMaxSize)
                            GenerateNewLogFile();
                    }

                }
                WriteLog(m_strLogFile, strMessage);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            };
        }

        public void Log(ref Exception objException)
        {
            try
            {

                // check if file name provided.
                // if not, will create an default file				
                if (string.Empty == m_strFullLogPath)
                    GenerateDefaultLogFile();

                // check if maximum log file size has reached.
                // if yes, will create a new file
                FileInfo fileInfo = new FileInfo(m_strFullLogPath);
                if (fileInfo.Length >= m_intMaxSize)
                    GenerateNewLogFile();


            }
            catch (Exception) { }
        }

        public void WriteTemp(string strMessage)
        {
            try
            {
                // check if file name provided.
                // if not, will create an default file				
                if (string.Empty == m_strLogFile)
                    GenerateDefaultLogFile();

                // check if maximum log file size has reached.
                // if yes, will create a new file
                if (File.Exists(m_strLogFile))
                {
                    File.Delete(m_strLogFile);
                }
                else
                {
                    GenerateDefaultLogFileTemp(m_strLogFile);
                }
                WriteLog(m_strLogFile, strMessage);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            };
        }

        public string ReadLog(string strPathName)
        {
            string strMessage = "";
            try
            {
                m_sr = new StreamReader(strPathName, Encoding.UTF8);

                strMessage = m_sr.ReadToEnd();

                m_sr.Dispose();
                m_sr.Close();

            }
            catch (Exception) { }
            return strMessage;
        }

        #endregion

        #region Get value from registry
        public static string GetValFrReg(string strRegSubKey, string strKey)
        {
            string strValue = "";
            // Get a reference to the desired registry hive.
            // This example uses the CurrentUser hive in order to store user preferences
            // for each user on this machine separately.
            //Microsoft.Win32.RegistryKey Reg = Microsoft.Win32.Registry.LocalMachine;
            RegistryKey localKey =
    RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,
        RegistryView.Registry64);

            RegistryKey localKey32 =
    RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine,
        RegistryView.Registry32);

            // The following line creates the desired sub key where the settings are be stored.
            // If the sub key already exists, a reference to the existing key is returned.
            Microsoft.Win32.RegistryKey regKey = localKey.OpenSubKey(strRegSubKey);

            Microsoft.Win32.RegistryKey regKey32 = localKey32.OpenSubKey(strRegSubKey);
            // The following lines retrieve the width and height settings from the sub key.
            // The second parameter indicates a default value if the setting
            // does not exist in the registry.
            if (regKey32 != null)
                strValue = (string)regKey32.GetValue(strKey, "");
            else
                strValue = (string)regKey.GetValue(strKey, "");
            return strValue;
        }
        #endregion
    }
}
