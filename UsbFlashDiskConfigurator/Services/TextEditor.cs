using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Services
{
    public class TextEditor : BackgroundWorker
    {
        #region PROPERTIES
        private string fileToEdit;
        
        private string textToFind;
        
        private string textToReplace;
        
       

        private Dictionary<string, string> userVariables;

        #endregion


        #region CONSTRUCTOR
        public TextEditor(string file, string findPattern, string replacePattern)
        {
            WorkerReportsProgress = false;

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
                userVariables = e.Argument as Dictionary<string,string>;

                // Change parameters if they are defined in User Variables dictionary
                foreach (KeyValuePair<string,string> kvp in userVariables)
                {
                    if (textToFind.Contains(kvp.Key)) textToFind = textToFind.Replace(kvp.Key, kvp.Value);
                    if (textToReplace.Contains(kvp.Key)) textToReplace = textToReplace.Replace(kvp.Key, kvp.Value);
                }

                string content = File.ReadAllText(fileToEdit);

                content = content.Replace(textToFind, textToReplace);
                File.WriteAllText(fileToEdit, content);

                e.Result = true;

                // Testing simulated ERROR
                //e.Result = false;
            }
            catch
            {
                e.Result = false;
            }
        }



        #endregion

    }
}
