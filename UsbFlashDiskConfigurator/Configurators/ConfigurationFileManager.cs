using Microsoft.Extensions.Configuration.Ini;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Configurators
{
    class ConfigurationFileManager
    {

        #region PROPERTIES

        public string RenameUsbDriveLabel { get; set; }

        public string ProjectPath { get; set; }
        public string ProjectConfigurationFile { get; set; }
        


        #endregion

        #region CONSTRUCTOR

        public ConfigurationFileManager(string configFilename)
        {
            LoadConfigFile(configFilename);
        }

        #endregion

        #region METHODS


        private void LoadConfigFile(string filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    IniConfigurationSource iniSrouce = new IniConfigurationSource();
                    iniSrouce.Path = filename;

                    IniConfigurationProvider iniFile = new IniConfigurationProvider(iniSrouce);
                    iniFile.Load(sr.BaseStream);

                    string value;
                    iniFile.TryGet("General:RenameUsbDriveLabel", out value);
                    RenameUsbDriveLabel = value;

                    iniFile.TryGet("Project:ProjectPath", out value);
                    ProjectPath = value;

                    iniFile.TryGet("Project:ProjectConfigurationFile", out value);
                    ProjectConfigurationFile = value;

                    //iniFile.TryGet("Database:Password", out value);
                    //DbPass = value;
                    //
                    //iniFile.TryGet("Database:Name", out value);
                    //DbName = value;
                }
            }
            catch (Exception exp)
            {
                
            }

        }

        #endregion

    }
}
