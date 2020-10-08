using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using UsbFlashDiskConfigurator.ViewModels;
using UsbFlashDiskConfigurator.Views;

namespace UsbFlashDiskConfigurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel mwvm;

        public MainWindow()
        {
            InitializeComponent();

            mwvm = new MainWindowViewModel(this);
            DataContext = mwvm;
            
        }

        
    }
}
