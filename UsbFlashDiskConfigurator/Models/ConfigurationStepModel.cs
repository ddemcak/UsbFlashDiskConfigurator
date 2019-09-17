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

        private string usbKeyLetter;


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

        private string[] parameters;
        
        public string[] ParametersArray
        {
            get { return parameters; }
        }

        public string Parameters
        {
            get
            {
                string prms = "";
                foreach (string s in parameters) prms += s + " | ";

                prms = prms.Substring(0, prms.Length - 3);
                return prms;
            }
        }

        



        public ConfigurationStepModel(int idn, AppConfigurationConfigurationStep step, string ukl)
        {
            id = idn;
            status = "";
            type = step.Type;
            description = step.Description;
            parameters = step.Parameter;

            usbKeyLetter = ukl;

            for (int i = 0; i < parameters.Length; i++) parameters[i] = parameters[i].Replace("[USBKEY]\\", usbKeyLetter);
        }

        public void SetStatus(string sts)
        {
            status = sts;
            RaisePropertyChanged("Status");
        }

        

    }
}
