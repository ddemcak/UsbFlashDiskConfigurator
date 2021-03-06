﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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


        public DriveFormatter(DriveInfo di, string fs)
        {
            WorkerReportsProgress = false;
            driveInfo = di;
            filesystem = fs;
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
                    res = DriveManager.FormatDrive(letter, driveInfo.VolumeLabel, filesystem);

                    Thread.Sleep(2000);
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
