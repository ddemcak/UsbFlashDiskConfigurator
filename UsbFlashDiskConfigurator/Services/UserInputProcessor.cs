using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbFlashDiskConfigurator.Views;

namespace UsbFlashDiskConfigurator.Services
{
    public class UserInputProcessor : BackgroundWorker
    {

        #region CONSTANTS

        

        #endregion

        private string query;


        public UserInputProcessor(string qry)
        {
            WorkerReportsProgress = false;
            query = qry;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            UserInputWindow uiw = new UserInputWindow();
            uiw.ShowDialog();

            //uiw.

            e.Result = "";
        }

    }
}
