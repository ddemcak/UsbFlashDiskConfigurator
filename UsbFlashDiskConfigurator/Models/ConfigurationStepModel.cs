using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Models
{
    public class ConfigurationStepModel
    {
        private static int _ID = 1;



        private int id;

        public int Id
        {
            get { return id; }
        }

        private string type;

        public string Type
        {
            get { return type; }
        }

        private string description;

        public string Description
        {
            get { return description; }
        }

        private string parameters;
        public string Parameters
        {
            get { return parameters; }
        }

        public string[] ParametersArray
        {
            get { return parameters.Split(','); }
        }



        public ConfigurationStepModel(int idn, AppConfigurationConfigurationSteps step)
        {
            id = idn;
            type = step.Type;
            description = step.Description;
            parameters = step.Parameters;
        }

        

    }
}
