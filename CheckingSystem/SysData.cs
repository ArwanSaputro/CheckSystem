using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;

namespace CheckingSystem
{
    public class SystemData
    {
        #region "Public Methods"
        public double GetProcessorData()
        {
            return _GetCounterValue(_cpuCounter, "Processor", "% Processor Time", "_Total");
        }

        public SysValues GetVirtualMemory()
        {
            double d = _GetCounterValue(_memoryCounter, "Memory", "Committed Bytes", null);
            double totalphysicalmemory = _GetCounterValue(_memoryCounter, "Memory", "Commit Limit", null);
            return new SysValues { DeviceID = "Virtual Memory", Total = totalphysicalmemory, Used = d };
        }

        public SysValues GetPhysicalMemory()
        {
            string s = _QueryComputerSystem("totalphysicalmemory");
            double totalphysicalmemory = Convert.ToDouble(s);
            double d = _GetCounterValue(_memoryCounter, "Memory", "Available Bytes", null);
            return new SysValues { DeviceID = "Physical Memory", Total = totalphysicalmemory, Used = totalphysicalmemory - d };
        }

        public List<SysValues> GetDiskSpaces()
        {
            List<SysValues> disks = new List<SysValues>();
            object device, size, free;
            double totle;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                device = objMgmt["DeviceID"];		// C:
                if (null != device)
                {
                    size = objMgmt["Size"];
                    free = objMgmt["FreeSpace"];	// C:10.32 GB, D:5.87GB
                    if (null != free && null != size)
                    {
                        totle = double.Parse(size.ToString());
                        disks.Add(new SysValues { DeviceID = device.ToString(), Total = totle, Used = totle - double.Parse(free.ToString()) });
                    }
                }
            }

            return disks;
        }
        public  string GetIpAddress()  // Get IP Address
        {
            string ip = "";
            IPHostEntry ipEntry = Dns.GetHostEntry(GetCompCode());
            //IPAddress[] addr = ipEntry.AddressList;

            foreach (IPAddress addr in ipEntry.AddressList)
                ip += "(" + addr.ToString() + ") ";
            //ip = addr[2].ToString();
            return ip;
        }

        public  string GetCompCode()  // Get Computer Name
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();
            return strHostName;
        }
        #endregion

        #region "Private Helpers"
        double _GetCounterValue(PerformanceCounter pc, string categoryName, string counterName, string instanceName)
        {
            pc.CategoryName = categoryName;
            pc.CounterName = counterName;
            pc.InstanceName = instanceName;
            return pc.NextValue();
        }

        string _QueryComputerSystem(string type)
        {
            string str = null;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                str = objMgmt[type].ToString();
            }
            return str;
        }
        #endregion

        #region "Members"
        PerformanceCounter _memoryCounter = new PerformanceCounter();
        PerformanceCounter _cpuCounter = new PerformanceCounter();
        #endregion
    }
    public class SysValues
    {
        public string DeviceID { get; set; }
        public double Total { get; set; }
        public double Used { get; set; }
    }
}
