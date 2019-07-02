using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Services
{
    public class FileMover : BackgroundWorker
    {
        #region PROPERTIES
        private string fileFromMove;
        public string FileFromMove
        {
            get { return fileFromMove; }
        }

        private string fileToMove;
        public string FileToMove
        {
            get { return fileToMove; }
        }




        #endregion


        #region CONSTRUCTOR
        public FileMover(string fiFrom, string fiTo)
        {
            WorkerReportsProgress = false;

            fileFromMove = fiFrom;
            fileToMove = fiTo;


        }

        #endregion

        #region METHODS

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            try
            {
                File.Move(fileFromMove, fileToMove);
                                

                e.Result = true;
            }
            catch
            {
                e.Result = false;
            }
        }



        #endregion

    }
}
