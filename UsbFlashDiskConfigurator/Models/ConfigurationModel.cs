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
        
        #region PROPERTIES

        private AppConfiguration appconfig;

        private List<BackgroundWorker> workers = new List<BackgroundWorker>();

        #endregion

        #region CONSTRUCTOR
        public ConfigurationModel(AppConfiguration cfg)
        {
            appconfig = cfg;
            
        }

        #endregion

        #region METHODS
        private void ProcessConfiguration()
        {


        }

        #endregion
    }
}
