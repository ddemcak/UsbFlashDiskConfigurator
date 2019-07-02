using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbFlashDiskConfigurator.ViewModels;

namespace UsbFlashDiskConfigurator.Models
{
    public class ConfigurationStepModel : ViewModelBase
    {
        

        private int id;

        public int Id
        {
            get { return id; }
        }

        private string status;

        public string Status
        {
            get { return status; }
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
            status = "";
            type = step.Type;
            description = step.Description;
            parameters = step.Parameters;
        }

        public void SetStatus(string sts)
        {
            status = sts;
            RaisePropertyChanged("Status");
        }

        

    }
}
