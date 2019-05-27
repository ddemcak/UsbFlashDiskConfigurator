using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private TextReplacer tr;



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
        public ObservableCollection<ConfigurationStep> ConfigurationSteps { get; private set; }

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

        public ObservableCollection<ConfigurationModel> Configurations { get; private set; }

        private DriveInfoCustom selectedConfiguration;
        public DriveInfoCustom SelectedConfiguration
        {
            get { return selectedConfiguration; }

            set
            {
                if (selectedConfiguration != value)
                {
                    selectedConfiguration = value;
                    RaisePropertyChanged("SelectedConfiguration");
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


        #region CONSTRUCTOR
        public MainWindowViewModel()
        {
            TitleMainWindow = "Test...";
            ImageMainWindow = "testIO.png";


            RefreshDisksCommand = new RelayCommand(RefreshDiskDrives);
            CreateDiskCommand = new RelayCommand(CreateDisk);
            CancelCommand = new RelayCommand<Window>(CancelApplication);

            DiskDrives = new ObservableCollection<DriveInfoCustom>();
            ConfigurationSteps = new ObservableCollection<ConfigurationStep>();

            ConfigurationSteps.Add(new ConfigurationStep(1, "abc", "dasdasdasd"));
            ConfigurationSteps.Add(new ConfigurationStep(2, "asd", "gdfg"));
            ConfigurationSteps.Add(new ConfigurationStep(3, "fg", "dasdadfgdfgdfgdfgsdasd"));
            ConfigurationSteps.Add(new ConfigurationStep(4, "fdfsd", "ftr"));

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

            tr = new TextReplacer("E:\\slax\\boot\\syslinux.cfg", "TIMEOUT 0", "TIMEOUT 60");
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

        #endregion

        #region METHODS


        private void LoadConfiguration()
        {
            ConfigurationLoader cl = new ConfigurationLoader();
            cl.LoadConfiguration();

            TitleMainWindow = cl.Title;
            ImageMainWindow = cl.ImagePath;
        }


        public void CreateDisk(object obj)
        {
            if (fd.IsBusy) fd.CancelAsync();
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
