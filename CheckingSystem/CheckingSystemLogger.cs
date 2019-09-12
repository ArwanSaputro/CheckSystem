using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CheckingSystem
{
    class CheckingSystemLogger
    {
        private const string const_LOGFILE_KEY = "LogFile";
        private const string const_REGISTRY_KEY = "SOFTWARE\\BPO7\\System";
        private const string const_DEFAULT_REG_KEY = "default";
        ServiceLog m_oLogger = null;
        public CheckingSystemLogger(string strHostName)
        {
            try
            {

                string strRegKey;
                string strAppId = CheckingSystem.Properties.Settings.Default.AppId;
                string strTempAppId= CheckingSystem.Properties.Settings.Default.TempAppId;
                if (strAppId == null || strAppId == string.Empty)
                {
                    strRegKey = const_REGISTRY_KEY + "\\" + const_DEFAULT_REG_KEY;
                }
                else
                {
                    if (strTempAppId == strHostName)
                        strRegKey = const_REGISTRY_KEY + "\\" ;
                    else
                        strRegKey = const_REGISTRY_KEY + "\\" ;
                }

                string strLogFileKey = const_LOGFILE_KEY;
                if (strHostName != null && strHostName != string.Empty)
                {
                    strLogFileKey = strLogFileKey + "_" + strHostName;
                }
                string strLogFile = ServiceLog.GetValFrReg(strRegKey, strLogFileKey);
                m_oLogger = new ServiceLog();
                m_oLogger.LogFile = strLogFile;
            }
            catch { }

        }
        private static object objLock = new object();
        #region 
        public void WriteAsych(string strTrace)
        {
            try
            {
                string strMessageForwarderLog = CheckingSystem.Properties.Settings.Default.LogServices;
                if (strMessageForwarderLog == null || strMessageForwarderLog == "Y")
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.workerProcess), strTrace);
            }
            catch //(Exception ex)
            {
                //supress exception
            }
        }
        private void workerProcess(object stateInfo)
        {
            try
            {
                string strTrace = (string)stateInfo;
                lock (objLock)
                {
                    m_oLogger.Log(strTrace);
                }
            }
            catch //(Exception ex)
            {
                //supress exception
            }
        }
        #endregion
        #region temp
        public void WriteTempAsych(string strTrace)
        {
            try
            {
                string strMessageForwarderLog = CheckingSystem.Properties.Settings.Default.LogServices;
                if (strMessageForwarderLog == null || strMessageForwarderLog == "Y")
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.workerProcessTemp), strTrace);
            }
            catch //(Exception ex)
            {
                //supress exception
            }
        }
        private void workerProcessTemp(object stateInfo)
        {
            try
            {
                string strTrace = (string)stateInfo;
                lock (objLock)
                {
                    m_oLogger.WriteTemp(strTrace);
                }
            }
            catch //(Exception ex)
            {
                //supress exception
            }
        }
        public void ReadTempAsynch(out string str)
        {
            str = "";
            try
            {
                string strMessageForwarderLog = CheckingSystem.Properties.Settings.Default.LogServices;
                if (strMessageForwarderLog == null || strMessageForwarderLog == "Y")
                    str= m_oLogger.ReadLog(m_oLogger.LogFile);


            }
            catch //(Exception ex)
            {
                //supress exception
            }
        }
        
        #endregion
    }
}
