using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using DesktopBridge;

namespace WinDynamicDesktop
{
    class UwpHelper
    {
        public static bool IsRunningAsUwp()
        {
            Helpers helpers = new Helpers();

            return helpers.IsRunningAsUwp();
        }

        public static string GetCurrentDirectory()
        {
            if (!IsRunningAsUwp())
            {
                return Path.GetDirectoryName(Application.ExecutablePath);
            }
            else
            {
                return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            }
        }
    }

    class DesktopHelper
    {
        private static string registryStartupLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static bool IsStartOnBootEnabled()
        {
            bool startOnBoot;

            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(registryStartupLocation);
            startOnBoot = startupKey.GetValue("WinDynamicDesktop") != null;
            startupKey.Close();

            return startOnBoot;
        }

        public static void ToggleStartOnBoot(bool enable)
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(registryStartupLocation, true);

            if (enable)
            {
                string exePath = Path.Combine(Directory.GetCurrentDirectory(),
                    Environment.GetCommandLineArgs()[0]);
                startupKey.SetValue("WinDynamicDesktop", exePath);
            }
            else
            {
                startupKey.DeleteValue("WinDynamicDesktop");
            }
        }
    }
}
