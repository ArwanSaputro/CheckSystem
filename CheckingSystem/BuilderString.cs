using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckingSystem
{
    class BuilderString
    {
        public string  StartAction()
        {
            string strTraceToLog = null;
            StringBuilder sbTrace = new StringBuilder();

            sbTrace.Append("=========================================================");
            sbTrace.Append("\r\n" + GeneralCommon.ConvertDateToString(DateTime.Now));
            sbTrace.Append("\r\n Start Checking System Notification");
            sbTrace.Append("\r\n");
            strTraceToLog = sbTrace.ToString();
            return strTraceToLog;
        }

        public string StopAction()
        {
            string strTraceToLog = null;
            StringBuilder sbTrace = new StringBuilder();

            sbTrace.Append("\r\n-----------------------------------------------------");
            sbTrace.Append("\r\n" + GeneralCommon.ConvertDateToString(DateTime.Now));
            sbTrace.Append("\r\n Stop Checking System Notification");
            sbTrace.Append("\r\n");
            strTraceToLog = sbTrace.ToString();
            return strTraceToLog;
        }
        public string RunningAction(string Ip,double Cpu,SysValues PhysMem, SysValues VirMem,List<SysValues> Disk)
        {
            string strTraceToLog = null;
            StringBuilder sbTrace = new StringBuilder();

            //var Ip= _SysData.GetIpAddress();
            //var Cpu=_SysData.GetProcessorData();
            //var PhysMem=_SysData.GetPhysicalMemory();
            //var VirMem = _SysData.GetVirtualMemory();
            //var Disk=_SysData.GetDiskSpaces();

            sbTrace.Append("\r\n-----------------------------------------------------");
            sbTrace.Append("\r\n" + GeneralCommon.ConvertDateToString(DateTime.Now));
            sbTrace.Append("\r\n Checking Ip Address :");
            sbTrace.Append("\r\n" + Ip );
            sbTrace.Append("\r\n----------  Checking System -----------");
            sbTrace.Append("\r\n CPU Used " + Cpu.ToString("F") + " % ");
            sbTrace.Append("\r\n"+LogSysValueWithUsage(PhysMem));
            sbTrace.Append("\r\n" + LogSysValueWithUsage(VirMem));
            foreach (SysValues v in Disk)
            {
                sbTrace.Append("\r\n" + LogSysValueWithUsage(v));
            }
            sbTrace.Append("\r\n");
            strTraceToLog = sbTrace.ToString();
            return strTraceToLog;
        }

        public string EmailBody(string Ip, double Cpu, SysValues PhysMem, SysValues VirMem, List<SysValues> Disk)
        {
            string strTraceToLog = null;
            StringBuilder sbTrace = new StringBuilder();

            //var Ip= _SysData.GetIpAddress();
            //var Cpu=_SysData.GetProcessorData();
            //var PhysMem=_SysData.GetPhysicalMemory();
            //var VirMem = _SysData.GetVirtualMemory();
            //var Disk=_SysData.GetDiskSpaces();
            sbTrace.Append("<p><span >Dear Sir/Mom,</span><br /></p>");
            sbTrace.Append("<p> &nbsp; </p>");
            sbTrace.Append("<p>-----------------------------------------------------</p>");
            sbTrace.Append("<p>" + GeneralCommon.ConvertDateToString(DateTime.Now)+"</p>");
            sbTrace.Append("<p> Checking Ip Address : </p>");
            sbTrace.Append("<p>" + Ip);
            sbTrace.Append("<p>----------  Checking System -----------</p>");
            sbTrace.Append("<p> CPU Used " + Cpu.ToString("F") + " % "+ "</p>");
            sbTrace.Append("<p>" + LogSysValueWithUsage(PhysMem)+ "</p>");
            sbTrace.Append("<p>" + LogSysValueWithUsage(VirMem)+ "</p>");
            sbTrace.Append("<p> Disk Drive :</p>");
            foreach (SysValues v in Disk)
            {
                sbTrace.Append("<p> " + LogSysValueWithUsage(v)+ "</p>");
            }
            sbTrace.Append("<p>&nbsp;</p>");
            sbTrace.Append("<p>&nbsp;</p>");
            sbTrace.Append("<p>Regards,</p>");
            sbTrace.Append("<p>&nbsp;</p>");
            sbTrace.Append("<p>Harigajian.com</p>");
            strTraceToLog = sbTrace.ToString();
            return strTraceToLog;
        }


        public string ErrorAction(string strSource,string strEx)
        {
            string strTraceToLog = null;
            StringBuilder sbTrace = new StringBuilder();
            sbTrace.Append("\r\n-------------------  Exception  ---------------------");
            sbTrace.Append("\r\n" + GeneralCommon.ConvertDateToString(DateTime.Now));
            sbTrace.Append("\r\n"+ strSource);
            sbTrace.Append("\r\n" + strEx);
            sbTrace.Append("\r\n");
            strTraceToLog = sbTrace.ToString();
            return strTraceToLog;
        }

        private string LogSysValueWithUsage(SysValues val)
        {
            StringBuilder sbTrace = new StringBuilder();
            double d = 100 * val.Used / val.Total;
            //string s = (d >= usage.Threshold ? " Over Threshold(" + usage.Threshold + ")" : "");
            sbTrace.Append(val.DeviceID + " " + d.ToString("F") + "% ("
                + FormatBytes(double.Parse(val.Used.ToString())) + "/"
                + FormatBytes(double.Parse(val.Total.ToString())) + ")" );
            return sbTrace.ToString();

        }

        enum Unit { B, KB, MB, GB, TB, ER }

        string FormatBytes(double bytes)
        {
            int unit = 0;
            while (bytes > 1024)
            {
                bytes /= 1024;
                ++unit;
            }

            return bytes.ToString("F") + " " + ((Unit)unit).ToString();
        }

        public string tempUnsplit(double Cpu, SysValues PhysMem, SysValues VirMem, List<SysValues> Disk,bool strBolean)
        {
            string strTraceToLog = null;
            string tempDisk = "";

            var tempCpu = Math.Round(Cpu,2).ToString();
            var tempPhys = GeneralCommon.ConvertToPercent(PhysMem).ToString();
            var tempVir = GeneralCommon.ConvertToPercent(VirMem).ToString();
            foreach(var item in Disk)
            {
                tempDisk = tempDisk + GeneralCommon.ConvertToPercent(item).ToString() + "#";
            }
            tempDisk= tempDisk.Remove(tempDisk.Length - 1);
            strTraceToLog = tempCpu + "|" + tempPhys + "|" + tempVir + "|" + tempDisk+ "|" + strBolean;
            return strTraceToLog;
        }
        public void tempSplit(string strMessage,out double CPU, out double PHYSMEM,out double VIRMEM,out List<double> DISK,out bool strBolean)
        {
            DISK = new List<double>();
            var splitStr = strMessage.Split('|');
            CPU = Convert.ToDouble(splitStr[0]);
            PHYSMEM = Convert.ToDouble(splitStr[1]);
            VIRMEM = Convert.ToDouble(splitStr[2]);
            var splitdisk= splitStr[3].Split('#');
            foreach (var item in splitdisk)
            {
                DISK.Add(Convert.ToDouble(item));
            }
            strBolean =Convert.ToBoolean( splitStr[4]);
        }
        SystemData _SysData = new SystemData();

    }
}
