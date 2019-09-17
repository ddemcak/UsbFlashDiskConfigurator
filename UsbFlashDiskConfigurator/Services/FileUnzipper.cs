using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Services
{
    public class FileUnzipper : BackgroundWorker
    {

        #region PROPERTIES
        private string sourceFile;
        public string SourceFile
        {
            get { return sourceFile; }
        }

        private string destinationPath;
        public string DestinationPath
        {
            get { return destinationPath; }
        }

        private IReader reader;

        #endregion


        #region CONSTRUCTOR
        public FileUnzipper(string src, string dest)
        {
            WorkerReportsProgress = true;
            
            sourceFile = src;
            destinationPath = dest;
            
        }

        #endregion

        #region METHODS

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            try
            {

                if (!File.Exists(sourceFile))
                {
                    e.Result = false;
                    return;
                }

                FileInfo fi = new FileInfo(sourceFile);
                if (fi.Length == 0)
                {
                    e.Result = false;
                    return;
                }

                using (Stream stream = File.OpenRead(sourceFile))
                {
                    reader = ReaderFactory.Open(stream);


                    while (reader.MoveToNextEntry())
                    {

                        if (!reader.Entry.IsDirectory)
                        {
                            //Console.WriteLine(reader.Entry.Key);
                            reader.WriteEntryToDirectory(destinationPath,
                                new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                        }
                        else
                        {
                            Directory.CreateDirectory(string.Format("{0}\\{1}", destinationPath, reader.Entry.Key));
                        }



                        if (CancellationPending)
                        {
                            reader.Dispose();
                            e.Result = false;
                            break;
                        }
                        else e.Result = true;
                    }
                }
            }
            catch
            {
                e.Result = false;
            }
        }

        

        #endregion
    }
}
