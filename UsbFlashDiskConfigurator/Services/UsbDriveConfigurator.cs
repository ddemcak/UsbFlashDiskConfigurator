using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Services
{
    public class UsbDriveConfigurator : BackgroundWorker
    {
        private DriveInfo driveInfo;
        private string uicNumber;



        public UsbDriveConfigurator(DriveInfo di, string uic)
        {
            driveInfo = di;
            uicNumber = uic;


        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            bool res = true;

            char letter = driveInfo.RootDirectory.Name[0];
            try
            {
                res = DriveManager.FormatDrive(letter, driveInfo.VolumeLabel, "FAT32");
            }
            catch
            {
                res = false;
            }
            /*
            if (res)
            {
                TextFileCreator tfc = new TextFileCreator(driveInfo);
                res = tfc.CreateUicFile(uicNumber);
            }

            if (res)
            {
                TextFileValidator tfv = new TextFileValidator(driveInfo);
                res = tfv.ValidateUicFile(uicNumber);
            }
            */
            //Thread.Sleep(2000);

            e.Result = res;
        }
    }
}
