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

                for (int tries = 0; tries < 5; tries++)
                {
                    res = DriveManager.EjectDrive(letter);
                    //TODO: Try this - https://stackoverflow.com/questions/58735900/how-to-eject-usb-drive-on-windows-10-ioctl-storage-eject-media-no-longer-enough
                    
                    Thread.Sleep(5000);

                    if (res) break;
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
