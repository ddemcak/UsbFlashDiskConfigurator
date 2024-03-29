﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbFlashDiskConfigurator.Services;

namespace UsbFlashDiskConfigurator.Models
{
    public class ConfigurationModel
    {

        #region MEMBERS

        private DriveInfo driveInfo;
        private AppConfigurationConfiguration configuration;
                
        
        private List<ConfigurationStepModel> steps = new List<ConfigurationStepModel>();

        private List<BackgroundWorker> workers = new List<BackgroundWorker>();


        public string Name
        {
            get { return configuration.Name; }
        }

        public string Description
        {
            get { return configuration.Description; }
        }

        public List<ConfigurationStepModel> Steps
        {
            get { return steps; }
        }

        public List<BackgroundWorker> Workers
        {
            get { return workers; }
        }


        #endregion

        #region CONSTRUCTOR
        public ConfigurationModel(DriveInfo di, AppConfigurationConfiguration conf)
        {
            driveInfo = di;
            configuration = conf;

            int i = 1;

            foreach (AppConfigurationConfigurationStep st in configuration.Step)
            {
                steps.Add(new ConfigurationStepModel(i++, st, driveInfo.Name));
            }

            ProcessConfiguration();
        }



        

        #endregion

        #region METHODS
        private void ProcessConfiguration()
        {
            workers.Clear();

            foreach (ConfigurationStepModel csm in steps)
            {
                switch (csm.Type)
                {
                    case "format":
                        ProcessDriveFormatter(csm);
                        break;

                    case "replacetext":
                        ProcessTextEditor(csm);
                        break;

                    case "download":
                        ProcessFileDownloader(csm);
                        break;

                    case "unpack":
                        ProcessFileUnzipper(csm);
                        break;

                    case "execute":
                        ProcessFileExecuter(csm);
                        break;

                    case "move":
                        ProcessFileMover(csm);
                        break;

                    case "eject":
                        ProcessDriveEjecter(csm);
                        break;

                    case "userinput":
                        ProcessUserInput(csm);
                        break;

                    default:
                        break;
                        
                            
                    

                }
            }

        }

        private void ProcessDriveFormatter(ConfigurationStepModel csm)
        {
            string filesystem = csm.ParametersArray[0].ToUpper();
            string volumeLabel = RemoveSpecialCharactersAndLimit(Name);

            DriveFormatter df = new DriveFormatter(driveInfo, filesystem, false, volumeLabel);
            workers.Add(df);
        }

        private void ProcessTextEditor(ConfigurationStepModel csm)
        {
            if (csm.ParametersArray.Length == 3)
            {
                string file = csm.ParametersArray[0];
                string findText = csm.ParametersArray[1];
                string replaceText = csm.ParametersArray[2];

                TextEditor te = new TextEditor(file, findText, replaceText);
                workers.Add(te);
            }
        }

        private void ProcessFileDownloader(ConfigurationStepModel csm)
        {
            if (csm.ParametersArray.Length == 2)
            {
                string remotefile = csm.ParametersArray[0];
                string localfile = csm.ParametersArray[1];

                FileDownloader fd = new FileDownloader(new Uri(remotefile), localfile);
                workers.Add(fd);
            }
        }

        private void ProcessFileUnzipper(ConfigurationStepModel csm)
        {
            if (csm.ParametersArray.Length == 2)
            {
                string localfile = csm.ParametersArray[0];
                string driveSubDir = csm.ParametersArray[1];

                FileUnzipper fu = new FileUnzipper(localfile, driveSubDir);
                workers.Add(fu);
            }
        }

        private void ProcessFileExecuter(ConfigurationStepModel csm)
        {
            if (csm.ParametersArray.Length == 1)
            {
                string localfile = csm.ParametersArray[0];

                FileExecuter fe = new FileExecuter(localfile);
                workers.Add(fe);
            }
        }

        private void ProcessFileMover(ConfigurationStepModel csm)
        {
            if (csm.ParametersArray.Length == 2)
            {
                string fileFromMove = csm.ParametersArray[0];
                string fileToMove = csm.ParametersArray[1];

                FileMover fm = new FileMover(fileFromMove, fileToMove);
                workers.Add(fm);
            }
        }

        private void ProcessDriveEjecter(ConfigurationStepModel csm)
        {
            DriveEjector de = new DriveEjector(driveInfo);
            workers.Add(de);
        }
        
        private void ProcessUserInput(ConfigurationStepModel csm)
        {
            if (csm.ParametersArray.Length == 1)
            {
                string userInputVariable = csm.ParametersArray[0];

                UserInputProcessor uip = new UserInputProcessor(csm.Description);
                workers.Add(uip);
            }
        }


        public void ResetStepStatuses()
        {
            foreach (ConfigurationStepModel csm in Steps) csm.SetStatus("");
        }

        /// <summary>
        /// Remove all characters except 0-9, a-z, A-Z, _ and -
        /// Also cuts off more than 11 characters.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string RemoveSpecialCharactersAndLimit(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-')
                {
                    sb.Append(c);
                }
            }
            string noSpecChars = sb.ToString();
            
            // 11 characters are MAX allowed.
            if (noSpecChars.Length > 11) noSpecChars = noSpecChars.Substring(0, 11);
            return noSpecChars;        
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Description);
        }

        #endregion
    }
}
