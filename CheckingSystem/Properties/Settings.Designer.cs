﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CheckingSystem.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public double CPUAlert {
            get {
                return ((double)(this["CPUAlert"]));
            }
            set {
                this["CPUAlert"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public double DiskSpaceAlert {
            get {
                return ((double)(this["DiskSpaceAlert"]));
            }
            set {
                this["DiskSpaceAlert"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public double PhysicalMemoryAlert {
            get {
                return ((double)(this["PhysicalMemoryAlert"]));
            }
            set {
                this["PhysicalMemoryAlert"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public double VirtualMemoryAlert {
            get {
                return ((double)(this["VirtualMemoryAlert"]));
            }
            set {
                this["VirtualMemoryAlert"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("smtp.office365.com|587|donotreply@harigajian.com|donotreply@harigajian.com|j8a42q" +
            "/gXjVHZN3pUHnb2Mp4M2+vNlaCTm+bfQv5YNs=|TLS")]
        public string ConnectionString {
            get {
                return ((string)(this["ConnectionString"]));
            }
            set {
                this["ConnectionString"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public int TimeInterval {
            get {
                return ((int)(this["TimeInterval"]));
            }
            set {
                this["TimeInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CheckSystem")]
        public string AppId {
            get {
                return ((string)(this["AppId"]));
            }
            set {
                this["AppId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Y")]
        public string LogServices {
            get {
                return ((string)(this["LogServices"]));
            }
            set {
                this["LogServices"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Temp")]
        public string TempAppId {
            get {
                return ((string)(this["TempAppId"]));
            }
            set {
                this["TempAppId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Bigkodok@gmail.com")]
        public string EmailTo {
            get {
                return ((string)(this["EmailTo"]));
            }
            set {
                this["EmailTo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("System Notification")]
        public string SubjectMessage {
            get {
                return ((string)(this["SubjectMessage"]));
            }
            set {
                this["SubjectMessage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public double AlertAdditional {
            get {
                return ((double)(this["AlertAdditional"]));
            }
            set {
                this["AlertAdditional"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ind0P@yrO11-A5a")]
        public string PKey {
            get {
                return ((string)(this["PKey"]));
            }
            set {
                this["PKey"] = value;
            }
        }
    }
}
