using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace UsbFlashDiskConfigurator.Views
{
    /// <summary>
    /// Interaction logic for WarningWindow.xaml
    /// </summary>
    public partial class WarningWindow : MetroWindow
    {
        public bool UserConfirmed = false;


        public WarningWindow(string diskInformation)
        {
            InitializeComponent();

            LabelUserAreYouSure.Text = string.Format("All data on\n{0}\nwill be deleted!", diskInformation);
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            UserConfirmed = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
