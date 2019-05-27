using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Models
{
    public class ConfigurationStep
    {

        private int id;

        public int Id
        {
            get { return id; }
        }

        private string name;

        public string Name
        {
            get { return name; }
        }

        private string description;

        public string Description
        {
            get { return description; }
        }


        public ConfigurationStep(int idfn, string stepname, string desc)
        {
            id = idfn;
            name = stepname;
            description = desc;
        }

    }
}
