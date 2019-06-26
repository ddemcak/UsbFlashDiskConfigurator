using System;
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


        #endregion

        #region CONSTRUCTOR
        public ConfigurationModel(DriveInfo di, AppConfigurationConfiguration conf)
        {
            driveInfo = di;
            configuration = conf;

            int i = 1;

            foreach (AppConfigurationConfigurationSteps st in configuration.Steps)
            {
                steps.Add(new ConfigurationStepModel(i++, st));
            }
        }

        

        #endregion

        #region METHODS
        private void ProcessConfiguration()
        {
            foreach (ConfigurationStepModel csm in steps)
            {
                switch (csm.Type)
                {
                    case "format":
                        

                        break;
                    default:
                        break;
                        
                            
                    

                }
            }

        }

        private void ProcessDriveFormatter(ConfigurationStepModel csm)
        {
            string fs = csm.ParametersArray[0];

            DriveFormatter df = new DriveFormatter(driveInfo, fs);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Description);
        }

        #endregion
    }
}
