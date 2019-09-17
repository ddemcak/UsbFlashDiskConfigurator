using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UsbFlashDiskConfigurator.Models;

namespace UsbFlashDiskConfigurator.Xml
{
    public class ConfigurationLoader
    {

        #region CONSTANTS

        private string _configFilePath = "config.xml";

        #endregion

        #region PROPERTIES

        private AppConfiguration config = null;
        public AppConfiguration Config
        {
            get { return config; }
        }

        public string Title
        {
            get { return config.Title; }
        }

        public string Information
        {
            get { return config.Information; }
        }

        public string ImagePath
        {
            get { return config.CompanyLogoPath; }
        }

        #endregion
        
        #region CONSTRUCTOR
        public ConfigurationLoader()
        {

        }
        #endregion

        #region CONSTRUCTOR
        public bool LoadConfiguration()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppConfiguration));

                StreamReader reader = new StreamReader(_configFilePath);
                config = (AppConfiguration)serializer.Deserialize(reader);
                reader.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
