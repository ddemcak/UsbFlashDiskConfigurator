using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UsbFlashDiskConfigurator.Helpers;
using UsbFlashDiskConfigurator.Models;
using UsbFlashDiskConfigurator.Services;
using UsbFlashDiskConfigurator.Xml;

namespace UsbFlashDiskConfigurator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region RELAY COMMANDS

        public RelayCommand RefreshDisksCommand { get; set; }
        public RelayCommand CreateDiskCommand { get; set; }
        public RelayCommand<Window> CancelCommand { get; set; }


        #endregion

        #region WORKERS
        private FileDownloader fd;
        private FileUnzipper fu;
        private FileExecuter fe;
        private TextEditor tr;



        #endregion

        #region PROPERTIES

        

        #endregion


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

        




        #region CONSTRUCTOR
        public MainWindowViewModel()
        {
            TitleMainWindow = "Test...";
            ImageMainWindow = "testIO.png";


            RefreshDisksCommand = new RelayCommand(RefreshDiskDrives, CanCreateDisk);
            CreateDiskCommand = new RelayCommand(CreateDisk, CanCreateDisk);
            CancelCommand = new RelayCommand<Window>(CancelApplication);

            DiskDrives = new ObservableCollection<DriveInfoCustom>();
            Configurations = new ObservableCollection<ConfigurationModel>();
            ConfigurationSteps = new ObservableCollection<ConfigurationStepModel>();

            //ConfigurationSteps.Add(new ConfigurationStepModel(1, "abc", "dasdasdasd"));
            //ConfigurationSteps.Add(new ConfigurationStepModel(2, "asd", "gdfg"));
            //ConfigurationSteps.Add(new ConfigurationStepModel(3, "fg", "dasdadfgdfgdfgdfgsdasd"));
            //ConfigurationSteps.Add(new ConfigurationStepModel(4, "fdfsd", "ftr"));
            //ConfigurationSteps.Add(new ConfigurationStepModel(5, "ferter", "wert"));
            //ConfigurationSteps.Add(new ConfigurationStepModel(6, "jhm", "fdfdfgtr"));

            RefreshDiskDrives(null);

            LoadConfiguration();

            /*
            fd = new FileDownloader(new Uri(cl.Config.Configurations[0].Steps[1].Parameters), "");
            fd.ProgressChanged += Fd_ProgressChanged;
            fd.RunWorkerCompleted += Fd_RunWorkerCompleted;
            */
            //fd.RunWorkerAsync();

            fu = new FileUnzipper("apt-flash-loader-1.8.0.tar", "E:\\");
            fu.ProgressChanged += Fu_ProgressChanged;
            fu.RunWorkerCompleted += Fu_RunWorkerCompleted;

            //fu.RunWorkerAsync();

            fe = new FileExecuter("E:\\slax\\boot\\bootinst.bat");
            fe.RunWorkerCompleted += Fe_RunWorkerCompleted;

            //fe.RunWorkerAsync();

            tr = new TextEditor("E:\\slax\\boot\\syslinux.cfg", "TIMEOUT 0", "TIMEOUT 60");
            tr.RunWorkerCompleted += Tr_RunWorkerCompleted;

            //tr.RunWorkerAsync();

        }

        private void Tr_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (!(bool)e.Result) StatusInformation = "Find and Replace failed!";
            else StatusInformation = "Text has been replaced.";
        }

        private void Fe_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (!(bool)e.Result) StatusInformation = "USB key boot failed!";
            else StatusInformation = "USB key is bootable.";
        }

        private void Fu_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (!(bool)e.Result) StatusInformation = "Unzipping failed!";
            else StatusInformation = "Unzipping done.";
        }

        private void Fu_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            StatusInformation = string.Format("{0} %", e.ProgressPercentage);
        }

        private void Fd_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (!(bool)e.Result) StatusInformation = string.Format("Downloading has been cancelled.");
            else StatusInformation = string.Format("Downloading has been sucessfully finished.");
        }

        private void Fd_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            StatusInformation = string.Format("{0} %", e.ProgressPercentage);
        }

        private void Bw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //int idxWorker = SelectedConfiguration.Workers[currentWorkerIdx];

            if (!(bool)e.Result)
            {
                if(currentWorkerIdx != -1) SelectedConfiguration.Steps[currentWorkerIdx].SetStatus("ERROR");

                //StatusInformation = string.Format("Current step has been cancelled.");
            }
            else
            {
                if (currentWorkerIdx != -1) SelectedConfiguration.Steps[currentWorkerIdx].SetStatus("DONE");

                //StatusInformation = string.Format("Current step has been sucessfully finished.");
            }

            CreateDisk(null);
            
        }

        private void Bw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //int idxWorker = SelectedConfiguration.Workers.IndexOf(currentWorker);
            if (currentWorkerIdx != -1)
            {
                if (SelectedConfiguration.Workers[currentWorkerIdx].WorkerReportsProgress) SelectedConfiguration.Steps[currentWorkerIdx].SetStatus(string.Format("{0}%", e.ProgressPercentage));
                else SelectedConfiguration.Steps[currentWorkerIdx].SetStatus("IN WORK");
            }

            //StatusInformation = string.Format("{0} %", e.ProgressPercentage);
            ProgressBarValue = e.ProgressPercentage;
        }

        #endregion

        #region METHODS


        private void LoadConfiguration()
        {
            ConfigurationLoader cl = new ConfigurationLoader();
            cl.LoadConfiguration();

            TitleMainWindow = cl.Title;
            ImageMainWindow = cl.ImagePath;


            Configurations.Clear();
            foreach (AppConfigurationConfiguration cfg in cl.Config.Configurations)
            {
                if (selectedDiskDrive != null) Configurations.Add(new ConfigurationModel(selectedDiskDrive.DriveInfo, cfg));
            }
            if (Configurations.Count != 0) SelectedConfiguration = Configurations.First();


            RefreshSteps();

            //StatusInformation = "...";


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
            }
        }



        private bool CanCreateDisk(object obj)
        {
            if (currentWorkerIdx != -1 && SelectedConfiguration.Workers[currentWorkerIdx].IsBusy) return false;
            else return true;
        }

        public void CreateDisk(object obj)
        {
            bool cnt = true;

            StatusInformation = "Configuration of a USB key is in progress...";

            if (currentWorkerIdx == -1)
            {
                SelectedConfiguration.ResetStepStatuses();
                currentWorkerIdx = 0;

                

                //currentWorker = SelectedConfiguration.Workers[0];
            }
            else
            {
                //int nextWorker = SelectedConfiguration.Workers.IndexOf(currentWorker) + 1;
                currentWorkerIdx++;
                if (currentWorkerIdx >= SelectedConfiguration.Workers.Count) cnt = false;
            }

            if (cnt)
            {
                SelectedConfiguration.Workers[currentWorkerIdx].ProgressChanged += Bw_ProgressChanged;
                SelectedConfiguration.Workers[currentWorkerIdx].RunWorkerCompleted += Bw_RunWorkerCompleted;

                
                SelectedConfiguration.Steps[currentWorkerIdx].SetStatus("IN WORK");
                //if (currentWorker.WorkerReportsProgress) ProgressBarIsInderetminate = false;
                //else ProgressBarIsInderetminate = true;

                SelectedConfiguration.Workers[currentWorkerIdx].RunWorkerAsync();
            }
            else
            {
                //SelectedConfiguration.Workers[currentWorkerIdx].ProgressChanged -= Bw_ProgressChanged;
                //SelectedConfiguration.Workers[currentWorkerIdx].RunWorkerCompleted -= Bw_RunWorkerCompleted;

                //ProgressBarIsInderetminate = false;
                //ProgressBarValue = 100;

                currentWorkerIdx = -1;

                StatusInformation = "Configuration of a USB key has finished.";
                CommandManager.InvalidateRequerySuggested();
            }

        }

        

        public void CancelApplication(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public void RefreshDiskDrives(object obj)
        {
            int selDrive = DiskDrives.IndexOf(SelectedDiskDrive);
            DiskDrives.Clear();

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable) DiskDrives.Add(new DriveInfoCustom(drive));
            }

            if (DiskDrives.Count != 0)
            {
                if (selDrive != -1 && selDrive < DiskDrives.Count) SelectedDiskDrive = DiskDrives[selDrive];
                else SelectedDiskDrive = DiskDrives.First();
                
            }

            //UpdateSelectedDiskDriveInformation();
            LoadConfiguration();

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

                //SelectedDiskInformation = string.Format("Size: {0:0.0} GB\nLabel: {1}\n\n{2}", SelectedDiskDrive.TotalSize / Math.Pow(10, 9), selectedDiskDrive.VolumeLabel, content);
                SelectedDiskInformation = string.Format("{0}", content);
            }
            else SelectedDiskInformation = string.Empty;

        }

        #endregion
    }

}
