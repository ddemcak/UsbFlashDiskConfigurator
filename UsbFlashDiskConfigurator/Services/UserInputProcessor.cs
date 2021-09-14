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

        public string UserInputVariableValue;


        public UserInputProcessor(string qry)
        {
            WorkerReportsProgress = false;
            query = qry;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //UserInputWindow uiw = new UserInputWindow(query);
            //uiw.ShowDialog();
            //
            //UserInputVariableValue = uiw.UserInput;

            e.Result = true;
        }

    }
}
