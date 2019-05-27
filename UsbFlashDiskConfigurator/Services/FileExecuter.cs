using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Services
{
    public class FileExecuter : BackgroundWorker
    {
        #region PROPERTIES
        private string fileToExecute;
        public string FileToExecute
        {
            get { return fileToExecute; }
        }

        

        #endregion


        #region CONSTRUCTOR
        public FileExecuter(string exeFile)
        {
            fileToExecute = exeFile;
            

        }

        #endregion

        #region METHODS

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = fileToExecute;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "auto";
            //startInfo.Verb = "runas";


            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();

                    if (exeProcess.ExitCode != 0) e.Result = false;
                    else e.Result = true;
                }
            }
            catch (Exception exp)
            {
                e.Result = false;
            }
        }



        #endregion

    }
}
