using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo
{
    public class ConnectedDrivesModel
    {
        public bool IsSelected { get; set; }
        public string DriveLetter { get; set; }

        private DelegateCommand click;
        public DelegateCommand Click
        {
            get { return click; }
            set { click = value; }
        }
    }
}
