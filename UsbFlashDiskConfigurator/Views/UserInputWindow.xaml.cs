using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
    /// Interaction logic for UserInputWindow.xaml
    /// </summary>
    public partial class UserInputWindow : MetroWindow
    {
        public string UserInput;
        
        public UserInputWindow(string UserMessage)
        {
            InitializeComponent();

            LabelUserInputLabel.Content = UserMessage;

            TextBoxUserInput.Focus();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            UserInput = TextBoxUserInput.Text;
            this.Close();
        }

        private void TextBoxUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                UserInput = TextBoxUserInput.Text;
                this.Close();
            }
        }
    }
}
