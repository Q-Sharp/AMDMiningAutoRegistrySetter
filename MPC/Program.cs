using System;
using System.Security;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AMD_Compute_Switcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            if(MessageBox.Show("Do you want to auto set: compute mode ON, ULPS OFF and CrossfireAutoSync OFF for all your AMD cards?",
                "AMD Mining Auto registry setter", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                var sk = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\{4d36e968-e325-11ce-bfc1-08002be10318}");
                var cfs = sk.GetSubKeyNames();
                var i = 0;

                foreach(var cf in cfs)
                {
                    try
                    {
                        int.Parse(cf);
                        var cr = sk.OpenSubKey(cf, true);

                        cr.SetValue("KMD_EnableInternalLargePage", "2", RegistryValueKind.DWord);
                        cr.SetValue("EnableCrossFireAutoLink", "0", RegistryValueKind.DWord);
                        cr.SetValue("EnableUlps", "0", RegistryValueKind.DWord);

                        i++;
                    }
                    catch(SecurityException)
                    {
                        MessageBox.Show("This tool needs admin rights!");
                        break;
                    }
                    catch(Exception)
                    {
                        continue;
                    }
                }

                MessageBox.Show($"Successfully set reg entries for {i} cards (igfx included))");
            }
        }
    }
}