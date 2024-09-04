using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UsbFlashDiskConfigurator.Configurators;
using UsbFlashDiskConfigurator.Helpers;
using UsbFlashDiskConfigurator.Models;
using UsbFlashDiskConfigurator.Services;
using UsbFlashDiskConfigurator.Views;
using UsbFlashDiskConfigurator.Xml;

namespace UsbFlashDiskConfigurator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region CONSTANTS

        private const string _iniFile = "UsbFlashDiskConfiguratorConfig.ini";


        #endregion

        #region RELAY COMMANDS

        public RelayCommand RefreshDisksCommand { get; set; }
        public RelayCommand OpenFolderCommand { get; set; }
        public RelayCommand EjectDiskCommand { get; set; }
        public RelayCommand CreateDiskCommand { get; set; }
        public RelayCommand<Window> CancelCommand { get; set; }


        #endregion

        #region WORKERS

        #endregion

        #region PROPERTIES

        private MetroWindow mainWindow;

        #endregion


        private string titleBarMainWindow;
        public string TitleBarMainWindow
        {
            get { return titleBarMainWindow; }

            set
            {
                if (titleBarMainWindow != value)
                {
                    titleBarMainWindow = value;
                    RaisePropertyChanged("TitleBarMainWindow");
                }
            }
        }

        private string titleMainWindow;
        public string TitleMainWindow
        {
            get { return titleMainWindow; }

            set
            {
                if (titleMainWindow != value)
                {
                    titleMainWindow = value;
                    RaisePropertyChanged("TitleMainWindow");
                }
            }
        }

        private string titleInformation;
        public string TitleInformation
        {
            get { return titleInformation; }

            set
            {
                if (titleInformation != value)
                {
                    titleInformation = value;
                    RaisePropertyChanged("TitleInformation");
                }
            }
        }

        private string imageMainWindow;
        public string ImageMainWindow
        {
            get { return imageMainWindow; }

            set
            {
                if (imageMainWindow != value)
                {
                    imageMainWindow = value;
                    RaisePropertyChanged("ImageMainWindow");
                }
            }
        }


        public ObservableCollection<DriveInfoCustom> DiskDrives { get; private set; }
        public ObservableCollection<ConfigurationModel> Configurations { get; private set; }
        public ObservableCollection<ConfigurationStepModel> ConfigurationSteps { get; private set; }
        
        private DriveInfoCustom selectedDiskDrive;
        public DriveInfoCustom SelectedDiskDrive
        {
            get { return selectedDiskDrive; }

            set
            {
                if (selectedDiskDrive != value)
                {
                    selectedDiskDrive = value;
                    RaisePropertyChanged("SelectedDiskDrive");
                    //UpdateSelectedDiskDriveInformation();
                    LoadConfiguration();
                }
            }
        }

        private String selectedDiskInformation;
        public String SelectedDiskInformation
        {
            get { return selectedDiskInformation; }

            set
            {
                if (selectedDiskInformation != value)
                {
                    selectedDiskInformation = value;
                    RaisePropertyChanged("SelectedDiskInformation");
                }
            }
        }

        

        private ConfigurationModel selectedConfiguration;
        public ConfigurationModel SelectedConfiguration
        {
            get { return selectedConfiguration; }

            set
            {
                if (selectedConfiguration != value)
                {
                    selectedConfiguration = value;
                    RaisePropertyChanged("SelectedConfiguration");
                    RefreshSteps();
                }
            }
        }

        

        private String statusInformation;
        public String StatusInformation
        {
            get { return statusInformation; }

            set
            {
                if (statusInformation != value)
                {
                    statusInformation = value;
                    RaisePropertyChanged("StatusInformation");
                }
            }
        }
            
        //BackgroundWorker currentWorker;
        int currentWorkerIdx = -1;

        private double progressBarValue;
        public double ProgressBarValue
        {
            get { return progressBarValue; }

            set
            {
                if (progressBarValue != value)
                {
                    progressBarValue = value;
                    RaisePropertyChanged("ProgressBarValue");
                }
            }
        }

        private bool progressBarIsInderetminate;
        public bool ProgressBarIsInderetminate
        {
            get { return progressBarIsInderetminate; }

            set
            {
                if (progressBarIsInderetminate != value)
                {
                    progressBarIsInderetminate = value;
                    RaisePropertyChanged("ProgressBarIsInderetminate");
                }
            }
        }

        private ConfigurationStepModel selectedConfigurationStepModel;
        public ConfigurationStepModel SelectedConfigurationStepModel
        {
            get { return selectedConfigurationStepModel; }

            set
            {
                if (selectedConfigurationStepModel != value)
                {
                    selectedConfigurationStepModel = value;
                    RaisePropertyChanged("SelectedConfigurationStepModel");
                }
            }
        }

        private bool chooseDiskEnable;
        public bool ChooseDiskEnable
        {
            get { return chooseDiskEnable; }

            set
            {
                if (chooseDiskEnable != value)
                {
                    chooseDiskEnable = value;
                    RaisePropertyChanged("ChooseDiskEnable");
                }
            }
        }

        private bool selectConfigurationEnabled;
        public bool SelectConfigurationEnabled
        {
            get { return selectConfigurationEnabled; }

            set
            {
                if (selectConfigurationEnabled != value)
                {
                    selectConfigurationEnabled = value;
                    RaisePropertyChanged("SelectConfigurationEnabled");
                }
            }
        }

        

        private bool configurationStepsAreEnabled;
        public bool ConfigurationStepsAreEnabled
        {
            get { return configurationStepsAreEnabled; }

            set
            {
                if (configurationStepsAreEnabled != value)
                {
                    configurationStepsAreEnabled = value;
                    RaisePropertyChanged("ConfigurationStepsAreEnabled");
                }
            }
        }


        private Dictionary<string, string> userVariables;


        #region CONSTRUCTOR
        public MainWindowViewModel(MetroWindow window)
        {
            mainWindow = window;
            
            RefreshDisksCommand = new RelayCommand(RefreshDiskDrives, CanRefreshDiskDrives);
            OpenFolderCommand = new RelayCommand(OpenFolderDiskDrive, CanOpenFolderDiskDrive);
            EjectDiskCommand = new RelayCommand(EjectDiskDrive, CanEjectDiskDrive);
            CreateDiskCommand = new RelayCommand(CreateDisk, CanCreateDisk);
            CancelCommand = new RelayCommand<Window>(CancelApplication);

            DiskDrives = new ObservableCollection<DriveInfoCustom>();
            Configurations = new ObservableCollection<ConfigurationModel>();
            ConfigurationSteps = new ObservableCollection<ConfigurationStepModel>();

            Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            TitleBarMainWindow = string.Format("USB Flash Disk Configurator v{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build);

            RefreshDiskDrives(null);

            LoadConfiguration();

            ChooseDiskEnable = true;
            SelectConfigurationEnabled = true;

            ConfigurationStepsAreEnabled = true;

            userVariables = new Dictionary<string, string>();
        }

        

        private void Bw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (currentWorkerIdx != -1)
            {
                if (!(bool)e.Result)
                {
                    SelectedConfiguration.Steps[currentWorkerIdx].SetStatus("ERROR");
                    currentWorkerIdx = -2;
                }
                else
                {
                    // If USER INPUT step finished, save variable to dictionary.
                    if (SelectedConfiguration.Steps[currentWorkerIdx].Type == "userinput")
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                        string varName = SelectedConfiguration.Steps[currentWorkerIdx].ParametersArray[0];

                        UserInputWindow uiw = new UserInputWindow(SelectedConfiguration.Steps[currentWorkerIdx].Description);
                        uiw.ShowDialog();


                        string varValue = uiw.UserInput;
                        userVariables.Add(varName, varValue);

                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    }
                    

                    SelectedConfiguration.Steps[currentWorkerIdx].SetStatus("DONE");
                }
            }
            
            CreateDisk(null);

            
            
        }

        private void Bw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (currentWorkerIdx != -1)
            {
                if (SelectedConfiguration.Workers[currentWorkerIdx].WorkerReportsProgress) SelectedConfiguration.Steps[currentWorkerIdx].SetStatus(string.Format("{0}%", e.ProgressPercentage));
                else SelectedConfiguration.Steps[currentWorkerIdx].SetStatus("IN WORK");
            }

            ProgressBarValue = e.ProgressPercentage;
        }

        #endregion

        #region METHODS


        private void LoadConfiguration()
        {
            // Load INI Configuration
            ConfigurationFileManager cfm = new ConfigurationFileManager(_iniFile);
            
            
            ConfigurationLoader cl = new ConfigurationLoader(cfm.ProjectConfigurationFile);

            if (!cl.LoadConfiguration())
            {
                StatusInformation = String.Format("Loading configuration file \"{0}\" failed!", cfm.ProjectConfigurationFile);
                return;
            }
            else
            {
                TitleMainWindow = cl.Title;
                if (cl.Version != null && cl.Information != null) TitleInformation = string.Format("Version: {0}.{1}                {2}", cl.Version.Substring(0, 1), cl.Version.Substring(1, 2), cl.Information);
                ImageMainWindow = cl.ImagePath;

                Configurations.Clear();
                foreach (AppConfigurationConfiguration cfg in cl.Config.Configurations)
                {
                    if (selectedDiskDrive != null) Configurations.Add(new ConfigurationModel(selectedDiskDrive.DriveInfo, cfg));
                }
                if (Configurations.Count != 0) SelectedConfiguration = Configurations.First();

                RefreshSteps();
            }

            if (DiskDrives.Count != 0) StatusInformation = "Configuration file was successfully loaded. Select any and click on Create disk.";
            else StatusInformation = "Please insert empty USB disk and click on Refresh button.";
        }

        private void RefreshSteps()
        {
            if (SelectedConfiguration != null)
            {
                ConfigurationSteps.Clear();

                foreach (ConfigurationStepModel cfgs in SelectedConfiguration.Steps)
                {
                    ConfigurationSteps.Add(cfgs);
                }

                ConfigurationStepsAreEnabled = true;
            }
            else 
            {
                ConfigurationStepsAreEnabled = false;
            }
        }



        private bool CanCreateDisk(object obj)
        {
            if (SelectedConfiguration == null || (currentWorkerIdx != -1 && SelectedConfiguration.Workers[currentWorkerIdx].IsBusy)) return false;
            else return true;
        }

        public void CreateDisk(object obj)
        {
            bool cnt = true;

            StatusInformation = "Configuration of a USB disk is in progress...";

            ChooseDiskEnable = false;
            SelectConfigurationEnabled = false;

            if (currentWorkerIdx == -1)
            {

                var matches = SelectedConfiguration.Steps.Where(p => p.Type == "format");

                if (matches.ToList().Count > 0)
                {
                    WarningWindow ww = new WarningWindow(SelectedDiskDrive.ToString());
                    ww.Owner = mainWindow;
                    ww.ShowDialog();

                    if (!ww.UserConfirmed)
                    {
                        StatusInformation = "Configuration of a USB disk was cancelled!";

                        ChooseDiskEnable = true;
                        SelectConfigurationEnabled = true;

                        return;
                    }
                }
                                
                SelectedConfiguration.ResetStepStatuses();
                currentWorkerIdx = 0;
                
                
            }
            else if (currentWorkerIdx == -2)
            {

                foreach (BackgroundWorker bw in SelectedConfiguration.Workers)
                {
                    bw.ProgressChanged -= Bw_ProgressChanged;
                    bw.RunWorkerCompleted -= Bw_RunWorkerCompleted;
                }

                currentWorkerIdx = -1;
                SelectedConfigurationStepModel = null;

                StatusInformation = "Configuration of a USB disk finished with ERROR!";
                CommandManager.InvalidateRequerySuggested();

                ChooseDiskEnable = true;
                SelectConfigurationEnabled = true;

                //if (SelectedConfiguration.Steps.Last().Type == "eject") RefreshDiskDrives(null);

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                return;
            }
            else
            {
                currentWorkerIdx++;
                if (currentWorkerIdx >= SelectedConfiguration.Workers.Count) cnt = false;
            }

            if (cnt)
            {
                SelectedConfiguration.Workers[currentWorkerIdx].ProgressChanged += Bw_ProgressChanged;
                SelectedConfiguration.Workers[currentWorkerIdx].RunWorkerCompleted += Bw_RunWorkerCompleted;

                

                SelectedConfigurationStepModel = SelectedConfiguration.Steps[currentWorkerIdx];
                SelectedConfiguration.Steps[currentWorkerIdx].SetStatus("IN WORK");

                // Send User Variables dictionary to USER INPUT worker
                if (SelectedConfiguration.Steps[currentWorkerIdx].Type == "replacetext")
                {
                    SelectedConfiguration.Workers[currentWorkerIdx].RunWorkerAsync(userVariables);
                }
                else SelectedConfiguration.Workers[currentWorkerIdx].RunWorkerAsync();


                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            }
            else
            {
                foreach (BackgroundWorker bw in SelectedConfiguration.Workers)
                {
                    bw.ProgressChanged -= Bw_ProgressChanged;
                    bw.RunWorkerCompleted -= Bw_RunWorkerCompleted;
                }
                
                currentWorkerIdx = -1;
                SelectedConfigurationStepModel = null;

                if (SelectedConfiguration.Steps.Last().Type == "eject") StatusInformation = "USB disk was sucessfully created and can be removed now.";
                else StatusInformation = "USB disk was sucessfully created. Remove it safely by clicking on Eject button.";
                CommandManager.InvalidateRequerySuggested();

                ChooseDiskEnable = true;
                SelectConfigurationEnabled = true;

                if (SelectedConfiguration.Steps.Last().Type == "eject") RefreshDiskDrives(null);

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }

        }

        

        public void CancelApplication(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        private bool CanRefreshDiskDrives(object obj)
        {
            if (currentWorkerIdx != -1 && SelectedConfiguration.Workers[currentWorkerIdx].IsBusy) return false;
            else return true;
        }

        private bool CanOpenFolderDiskDrive(object obj)
        {
            if (currentWorkerIdx != -1 && SelectedConfiguration.Workers[currentWorkerIdx].IsBusy || SelectedConfiguration == null) return false;
            else return true;
        }

        private bool CanEjectDiskDrive(object obj)
        {
            if (currentWorkerIdx != -1 && SelectedConfiguration.Workers[currentWorkerIdx].IsBusy || SelectedConfiguration == null) return false;
            else return true;
        }

        public void RefreshDiskDrives(object obj)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            int selDrive = DiskDrives.IndexOf(SelectedDiskDrive);
            DiskDrives.Clear();

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.DriveType == DriveType.Removable) DiskDrives.Add(new DriveInfoCustom(drive));
            }

            if (DiskDrives.Count != 0)
            {
                if (selDrive != -1 && selDrive < DiskDrives.Count) SelectedDiskDrive = DiskDrives[selDrive];
                else SelectedDiskDrive = DiskDrives.First();
                
            }

            LoadConfiguration();

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;


        }

        public void OpenFolderDiskDrive(object obj)
        {
            if (SelectedDiskDrive != null)
            {
                Process.Start(SelectedDiskDrive.DriveInfo.RootDirectory.FullName);
            }
        }

        public void EjectDiskDrive(object obj)
        {
            if (SelectedDiskDrive != null)
            {
                DriveEjector de = new DriveEjector(SelectedDiskDrive.DriveInfo);
                de.RunWorkerCompleted += De_RunWorkerCompleted;

                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                de.RunWorkerAsync();
                
            }
        }

        private void De_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshDiskDrives(null);

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

            StatusInformation = "USB disk can be removed now.";
        }

        public void UpdateSelectedDiskDriveInformation()
        {
            if (SelectedDiskDrive != null && SelectedDiskDrive.DriveInfo != null)
            {
                string content = string.Empty;
                DirectoryInfo[] dis = SelectedDiskDrive.DriveInfo.RootDirectory.GetDirectories();
                FileInfo[] fis = SelectedDiskDrive.DriveInfo.RootDirectory.GetFiles();


                if (dis.Length == 0 && fis.Length == 0) content = "Drive is empty...";
                else content = "All content on the drive will be DELETED!";

                SelectedDiskInformation = string.Format("{0}", content);
            }
            else SelectedDiskInformation = string.Empty;

        }

        #endregion
    }

}
