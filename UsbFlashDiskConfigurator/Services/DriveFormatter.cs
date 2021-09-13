using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Services
{
    public class DriveFormatter : BackgroundWorker
    {

        #region CONSTANTS

        private static string[] supportedFilesystems = { "FAT", "FAT32", "EXFAT", "NTFS", "UDF" };

        #endregion

        private DriveInfo driveInfo;
        private string filesystem;
        private string volumeLabel;


        public DriveFormatter(DriveInfo di, string fs, string vl)
        {
            WorkerReportsProgress = false;
            driveInfo = di;
            filesystem = fs;
            volumeLabel = vl;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            bool res = true;

            char letter = driveInfo.RootDirectory.Name[0];
            try
            {
                // Check supported file system
                if (!supportedFilesystems.Contains(filesystem))
                {
                    res = false;
                }
                else
                {
                    // Obsolete
                    //res = DriveManager.FormatDrive(letter, driveInfo.VolumeLabel, filesystem);



                    // We will call system standard FORMAT command.
                    Process process = new Process();
                    
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.FileName = "cmd.exe";
                    psi.Arguments = string.Format("/C format {0}: /fs:{1} /q /v:{2} /y", letter, filesystem, volumeLabel);
                    psi.Verb = "runas";

                    process.StartInfo = psi;
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0) res = true;

                    Thread.Sleep(2000);
                    
                    // Testing simulated ERROR
                    //res = false;
                }
            }
            catch
            {
                res = false;
            }
            

            e.Result = res;
        }
    }
}
