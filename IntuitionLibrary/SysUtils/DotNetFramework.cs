using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace QBits.Intuition.SysUtils
{
    /// <summary>
    /// Class that allows easy retrieval of .Net framework properties, such as version installed and service pack number.
    /// <para/>Implementation (incomplete) based on information on <seealso cref="http://stackoverflow.com/questions/199080/how-to-detect-what-net-framework-versions-and-service-packs-are-installed"/>.
    /// </summary>
    public class DotNetFramework
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DotNetFramework()
        {
        }
        /// <summary>
        /// Returns true when .Net Framework 2.0 is installed. False otherwise.
        /// </summary>
        public bool IsVersion20Installed
        {
            get
            {
                var frameworkKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\NET Framework Setup\NDP\v2.0.50727");
                if (frameworkKey != null)
                {
                    var valueName = "Install";
                    if (frameworkKey.GetValueKind(valueName) == RegistryValueKind.DWord)
                    {
                        var installedIndicator = (int)frameworkKey.GetValue(valueName, 0);
                        if (installedIndicator == 1) return true; //In all other cases return false.
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// Returns true when .Net Framework 3.0 is installed. False otherwise.
        /// </summary>
        public bool IsVersion30Installed
        {
            get
            {
                var frameworkKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\NET Framework Setup\NDP\v3.0\Setup");
                if (frameworkKey != null)
                {
                    var valueName = "InstallSuccess";
                    if (frameworkKey.GetValueKind(valueName) == RegistryValueKind.DWord)
                    {
                        var installedIndicator = (int)frameworkKey.GetValue(valueName, 0);
                        if (installedIndicator == 1) return true; //In all other cases return false.
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// Returns true when .Net Framework 3.5 is installed. False otherwise.
        /// </summary>
        public bool IsVersion35Installed
        {
            get
            {
                var frameworkKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\NET Framework Setup\NDP\v3.5");
                if (frameworkKey != null)
                {
                    var valueName = "Install";
                    if (frameworkKey.GetValueKind(valueName) == RegistryValueKind.DWord)
                    {
                        var installedIndicator = (int)frameworkKey.GetValue(valueName, 0);
                        if (installedIndicator == 1) return true; //In all other cases return false.
                    }
                }
                return false;
            }
        }
    }
}
