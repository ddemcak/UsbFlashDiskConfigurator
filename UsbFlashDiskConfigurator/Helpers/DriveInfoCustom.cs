using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbFlashDiskConfigurator.Helpers
{
    public class DriveInfoCustom
    {

        private DriveInfo di;

        public DriveInfo DriveInfo
        {
            get { return di; }
            set { di = value; }
        }

        public DriveInfoCustom(DriveInfo d)
        {
            di = d;
        }

        public override string ToString()
        {
            if (di.IsReady && di.VolumeLabel != string.Empty) return string.Format("{0}   [{1:0.0} GB, {2}]", di.RootDirectory.ToString(), di.TotalSize / Math.Pow(10, 9), di.VolumeLabel);
            else if (di.IsReady) return string.Format("{0}   [{1:0.0} GB]", di.RootDirectory.ToString(), di.TotalSize / Math.Pow(10, 9));
            else return string.Format("{0}", di.RootDirectory.ToString());
        }

    }
}
