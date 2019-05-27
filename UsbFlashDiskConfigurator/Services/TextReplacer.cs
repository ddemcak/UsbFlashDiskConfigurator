using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Services
{
    public class TextReplacer : BackgroundWorker
    {
        #region PROPERTIES
        private string fileToEdit;
        public string FileToEdit
        {
            get { return fileToEdit; }
        }

        private string textToFind;
        public string TextToFind
        {
            get { return TextToFind; }
        }

        private string textToReplace;
        public string TextToReplace
        {
            get { return TextToReplace; }
        }



        #endregion


        #region CONSTRUCTOR
        public TextReplacer(string file, string findPattern, string replacePattern)
        {
            WorkerSupportsCancellation = true;

            fileToEdit = file;
            textToFind = findPattern;
            textToReplace = replacePattern;


        }

        #endregion

        #region METHODS

        protected override void OnDoWork(DoWorkEventArgs e)
        {

            try
            {
                string content = File.ReadAllText(fileToEdit);

                content = content.Replace(textToFind, textToReplace);
                File.WriteAllText(fileToEdit, content);

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
