using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace UsbFlashDiskConfigurator.Services
{
       
    public class DriveEjector : BackgroundWorker
    {
        #region CONSTANTS

        private const string _tempScriptName = "ufd_config_eject.ps1";

        #endregion

        private DriveInfo driveInfo;


        public DriveEjector(DriveInfo di)
        {
            WorkerReportsProgress = false;
            driveInfo = di;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            bool res = false;

            char letter = driveInfo.RootDirectory.Name[0];
            try
            {

                // Obsole and unreliable class.
                //res = DriveManager.EjectDrive(letter);
                //TODO: Try this - https://stackoverflow.com/questions/58735900/how-to-eject-usb-drive-on-windows-10-ioctl-storage-eject-media-no-longer-enough

                //TODO: Implement this - https://www.py4u.net/discuss/1296468

                // Create script file
                using (StreamWriter sw = new StreamWriter(_tempScriptName))
                {
                    sw.WriteLine("$driveEject = New-Object -comObject Shell.Application");
                    sw.WriteLine(string.Format("$driveEject.Namespace(17).ParseName(\"{0}:\").InvokeVerb(\"Eject\")", letter));
                }

                Thread.Sleep(1000);

                // We will call crated script.
                Process process = new Process();
                    
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.FileName = "powershell.exe";
                psi.Arguments = string.Format("-NoProfile -ExecutionPolicy unrestricted -file \"{0}\"", _tempScriptName);
                psi.Verb = "runas";
                    
                process.StartInfo = psi;
                process.Start();
                process.WaitForExit();
                    
                if (process.ExitCode == 0) res = true;

                Thread.Sleep(1000);

                // Delete created script file.
                File.Delete(_tempScriptName);

            }
            catch
            {
                res = false;
            }


            e.Result = res;
        }

    }
}
