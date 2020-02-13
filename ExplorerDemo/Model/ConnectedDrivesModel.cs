using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo
{
    public class ConnectedDrivesModel
    {
        public bool IsSelected { get; set; }
        public string DriveLetter { get; set; }

        public string DriveLable { get; set; }

        private DelegateCommand click;
        public DelegateCommand Click
        {
            get { return click; }
            set { click = value; }
        }

        public string AvailableFreeSpace { get;  set; }
        public string TotalSize { get;  set; }
        public string VloType { get;  set; }
    }
}
