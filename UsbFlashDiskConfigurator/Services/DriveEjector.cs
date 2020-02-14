using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace UsbFlashDiskConfigurator.Services
{
       
    public class DriveEjector : BackgroundWorker
    {
        #region CONSTANTS

        
        #endregion

        private DriveInfo driveInfo;


        public DriveEjector(DriveInfo di)
        {
            WorkerReportsProgress = false;
            driveInfo = di;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            bool res = true;

            char letter = driveInfo.RootDirectory.Name[0];
            try
            {
                Thread.Sleep(2000);

                res = DriveManager.EjectDrive(letter);

                Thread.Sleep(2000);
                
            }
            catch
            {
                res = false;
            }


            e.Result = res;
        }

    }
}
