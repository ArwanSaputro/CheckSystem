using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;

namespace CheckingSystem
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        protected System.Timers.Timer timer1 = new System.Timers.Timer();
        protected string connectionString = CheckingSystem.Properties.Settings.Default.ConnectionString;
        protected double CPUAlert = CheckingSystem.Properties.Settings.Default.CPUAlert;
        protected double DiskSpaceAlert = CheckingSystem.Properties.Settings.Default.DiskSpaceAlert;
        protected double VirtualMemoryAlert = CheckingSystem.Properties.Settings.Default.VirtualMemoryAlert;
        protected double PhysicalMemoryAlert = CheckingSystem.Properties.Settings.Default.PhysicalMemoryAlert;
        protected int timeInterval = CheckingSystem.Properties.Settings.Default.TimeInterval;
        protected string DEFAULT_PARAMETER = CheckingSystem.Properties.Settings.Default.AppId;
        protected string TEMP_PARAMETER = CheckingSystem.Properties.Settings.Default.TempAppId;
        protected string Subject = CheckingSystem.Properties.Settings.Default.SubjectMessage;
        protected string EmailTo = CheckingSystem.Properties.Settings.Default.EmailTo;

        SystemData _SysData = new SystemData();
        bool SendNotif = true;

        protected override void OnStart(string[] args)
        {
            
           BuilderString builder = new BuilderString();
            string strTraceToLog= builder.StartAction();
            if (strTraceToLog != null)
            {
                CheckingSystemLogger oLogger = new CheckingSystemLogger(DEFAULT_PARAMETER);
                oLogger.WriteAsych(strTraceToLog.ToString());
            }
            timer1.Elapsed +=(sender,e)=> timer1_Elapsed(sender,e, SendNotif);
            timer1.Interval = timeInterval;
            timer1.Enabled = true;
        }

        private void timer1_Elapsed(object sender, ElapsedEventArgs e,bool sendNotif)
        {
            BuilderString builder = new BuilderString();
            string strTraceToLog = null;
            string strEmailBody = null;
            double pPhysMem = 0, pVirMem = 0,  pCPU=0;
            List<double> pDisk = new List<double>() ;
            double textPhysMem = 0, textVirMem = 0, textCPU = 0;
            bool textBolean;
            List<double> textDisk = new List<double>();
            string strRead = "";
            try
            {
                CheckingSystemLogger tempLogger = new CheckingSystemLogger(TEMP_PARAMETER);
                 tempLogger.ReadTempAsynch(out strRead);

                var Ip = _SysData.GetIpAddress();
                var Cpu = _SysData.GetProcessorData();
                var PhysMem = _SysData.GetPhysicalMemory();
                var VirMem = _SysData.GetVirtualMemory();
                var Disk = _SysData.GetDiskSpaces();

                pCPU = Math.Round(Cpu,2);
                pPhysMem =  GeneralCommon.ConvertToPercent(PhysMem);
                pVirMem = GeneralCommon.ConvertToPercent(VirMem);
                foreach(var item in Disk)
                {
                    var tempItem = GeneralCommon.ConvertToPercent(item);
                    pDisk.Add(tempItem);
                }
                if (strRead == string.Empty)
                {
                    if (Cpu >= CPUAlert)
                    {
                        strTraceToLog= builder.RunningAction(Ip, Cpu, PhysMem, VirMem, Disk);
                        if(sendNotif)
                        {
                            var strEmail=EmailTo.Split('|');
                            strEmailBody = builder.EmailBody(Ip, Cpu, PhysMem, VirMem, Disk);
                            foreach (var item in strEmail)
                                Notification.SendNotificationGlobal(item, Subject, strEmailBody, connectionString);
                            sendNotif = false;
                           var strTemp= builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, sendNotif);
                            tempLogger.WriteTempAsych(strTemp);
                        }
                    }

                    if (pPhysMem >= PhysicalMemoryAlert)
                    {
                        strTraceToLog = builder.RunningAction(Ip, Cpu, PhysMem, VirMem, Disk);
                        if (sendNotif)
                        {
                            var strEmail = EmailTo.Split('|');
                            strEmailBody = builder.EmailBody(Ip, Cpu, PhysMem, VirMem, Disk);
                            foreach (var item in strEmail)
                                Notification.SendNotificationGlobal(item, Subject, strEmailBody, connectionString);
                            sendNotif = false;
                            var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, sendNotif);
                            tempLogger.WriteTempAsych(strTemp);
                        }
                    }

                    if (pVirMem >= VirtualMemoryAlert)
                    {
                        strTraceToLog = builder.RunningAction(Ip, Cpu, PhysMem, VirMem, Disk);
                        if (sendNotif)
                        {
                            var strEmail = EmailTo.Split('|');
                            strEmailBody = builder.EmailBody(Ip, Cpu, PhysMem, VirMem, Disk);
                            foreach (var item in strEmail)
                                Notification.SendNotificationGlobal(item, Subject, strEmailBody, connectionString);
                            sendNotif = false;
                            var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, sendNotif);
                            tempLogger.WriteTempAsych(strTemp);
                        }
                    }

                    foreach (var item in Disk)
                    {
                        var tempPdisk = GeneralCommon.ConvertToPercent(item);
                        pDisk.Add( GeneralCommon.ConvertToPercent(item));
                        if (tempPdisk >= DiskSpaceAlert)
                        {
                            strTraceToLog = builder.RunningAction(Ip, Cpu, PhysMem, VirMem, Disk);
                            if (sendNotif)
                            {
                                var strEmail = EmailTo.Split('|');
                                strEmailBody = builder.EmailBody(Ip, Cpu, PhysMem, VirMem, Disk);
                                foreach (var itemEmail in strEmail)
                                    Notification.SendNotificationGlobal(itemEmail, Subject, strEmailBody, connectionString);
                                sendNotif = false;
                                var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, sendNotif);
                                tempLogger.WriteTempAsych(strTemp);
                            }
                        }
                    }
                }
                else
                {
                    builder.tempSplit(strRead,out textCPU,out textPhysMem,out textVirMem,out textDisk,out textBolean);
                    SendNotif = textBolean;
                    if (pCPU >= CPUAlert)
                    {
                        if (pCPU >= textCPU +10 )
                        {
                            strTraceToLog = builder.RunningAction(Ip, Cpu, PhysMem, VirMem, Disk);
                            if (sendNotif)
                            {
                                var strEmail = EmailTo.Split('|');
                                strEmailBody = builder.EmailBody(Ip, Cpu, PhysMem, VirMem, Disk);
                                foreach (var item in strEmail)
                                    Notification.SendNotificationGlobal(item, Subject, strEmailBody, connectionString);
                                sendNotif = false;
                                var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, SendNotif);
                                tempLogger.WriteTempAsych(strTemp);
                            }
                        }
                    }
                    else if (pPhysMem >= PhysicalMemoryAlert)
                    {
                        if (pPhysMem >= textPhysMem+10 )
                        {
                            strTraceToLog = builder.RunningAction(Ip, Cpu, PhysMem, VirMem, Disk);
                            if (sendNotif)
                            {
                                var strEmail = EmailTo.Split('|');
                                strEmailBody = builder.EmailBody(Ip, Cpu, PhysMem, VirMem, Disk);
                                foreach (var item in strEmail)
                                    Notification.SendNotificationGlobal(item, Subject, strEmailBody, connectionString);
                                sendNotif = false;
                                var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, SendNotif);
                                tempLogger.WriteTempAsych(strTemp);
                            }
                        }
                    }
                    else if (pVirMem >= VirtualMemoryAlert)
                    {
                        if (pVirMem >= textVirMem+10)
                        {
                            strTraceToLog = builder.RunningAction(Ip, Cpu, PhysMem, VirMem, Disk);
                            if (sendNotif)
                            {
                                var strEmail = EmailTo.Split('|');
                                strEmailBody = builder.EmailBody(Ip, Cpu, PhysMem, VirMem, Disk);
                                foreach (var item in strEmail)
                                    Notification.SendNotificationGlobal(item, Subject, strEmailBody, connectionString);
                                sendNotif = false;
                                var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, SendNotif);
                                tempLogger.WriteTempAsych(strTemp);
                            }
                        }
                    }
                    else
                    {
                        SendNotif = true;
                        var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, SendNotif);
                        tempLogger.WriteTempAsych(strTemp);
                    }

                    for (int i = 0; i < pDisk.Count();i++)
                    {
                        //var tempPdisk = GeneralCommon.ConvertToPercent(item);
                        //pDisk.Add(GeneralCommon.ConvertToPercent(item));
                        if (pDisk[i] >= DiskSpaceAlert)
                        {
                            if (pDisk[i] >= textDisk[i]+10)
                            {
                                strTraceToLog = builder.RunningAction(Ip, Cpu, PhysMem, VirMem, Disk);
                                if (sendNotif)
                                {
                                    var strEmail = EmailTo.Split('|');
                                    strEmailBody = builder.EmailBody(Ip, Cpu, PhysMem, VirMem, Disk);
                                    foreach (var item in strEmail)
                                        Notification.SendNotificationGlobal(item, Subject, strEmailBody, connectionString);
                                    sendNotif = false;
                                    var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, sendNotif);
                                    tempLogger.WriteTempAsych(strTemp);
                                }
                            }
                        }
                        else
                        {
                            SendNotif = true;
                            var strTemp = builder.tempUnsplit(Cpu, PhysMem, VirMem, Disk, sendNotif);
                            tempLogger.WriteTempAsych(strTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strTraceToLog= builder.ErrorAction(ex.Source.ToString(), ex.Message);
            }
            finally
            {
                if (strTraceToLog != null)
                {
                    CheckingSystemLogger oLogger = new CheckingSystemLogger(DEFAULT_PARAMETER);
                    oLogger.WriteAsych(strTraceToLog.ToString());
                }
            }
        }

        protected override void OnStop()
        {
            BuilderString builder = new BuilderString();
            string strTraceToLog = builder.StopAction();
            if (strTraceToLog != null)
            {
                CheckingSystemLogger oLogger = new CheckingSystemLogger(DEFAULT_PARAMETER);
                oLogger.WriteAsych(strTraceToLog.ToString());
            }
        }
        

    }
}
