using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Models
{
    public class ConfigurationModel
    {

        #region MEMBERS

        private AppConfigurationConfiguration configuration;
                
        
        private List<ConfigurationStepModel> steps = new List<ConfigurationStepModel>();


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
        public ConfigurationModel(AppConfigurationConfiguration conf)
        {
            configuration = conf;

            foreach (AppConfigurationConfigurationSteps st in configuration.Steps)
            {
                steps.Add(new ConfigurationStepModel(st));
            }
        }

        

        #endregion

        #region METHODS
        private void ProcessConfiguration()
        {
            

        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Description);
        }

        #endregion
    }
}
