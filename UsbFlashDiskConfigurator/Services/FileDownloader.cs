﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Services
{
    public class FileDownloader : BackgroundWorker
    {

        #region PROPERTIES
        private Uri source;
        public Uri Source
        {
            get { return source; }
        }


        private string fileSize;
        public string FileSize
        {
            get { return fileSize; }
        }

        private string downloadLocation;
        public string DownloadLocation
        {
            get { return downloadLocation; }
        }

        private WebClient webClient;
        

        #endregion


        #region CONSTRUCTOR
        public FileDownloader(Uri src, string location)
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;

            source = src;
            downloadLocation = location;
            //fileSize = CalculateFileSize(source.AbsoluteUri);

            webClient = new WebClient();
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
        }

        #endregion

        #region METHODS
        private string CalculateFileSize(string uri)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = "HEAD";
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            return string.Format("{0} MB", Math.Round((double)resp.ContentLength / 1048576, 2));
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            webClient.DownloadFileAsync(source, string.Format("{0}\\{1}", Directory.GetCurrentDirectory(), source.Segments.Last()));


            while (webClient.IsBusy)
            {
                if (CancellationPending)
                {
                    webClient.CancelAsync();
                    e.Result = false;
                    break;
                }
                Thread.Sleep(100);
                e.Result = true;
            }
        }
                
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (webClient.IsBusy) ReportProgress(e.ProgressPercentage);
        }

        #endregion


    }
}
