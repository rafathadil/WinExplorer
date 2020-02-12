using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo.Model
{
    public class FileTransferModel : BaseClass
    {

        string _CopyFrom { get; set; }
        public string CopyFrom
        {
            get { return _CopyFrom; }
            set
            {
                _CopyFrom = value;
                OnPropertyChanged("CopyFrom");
            }
        }

        public string _PastTo { get; set; }
        public string PastTo
        {
            get { return _PastTo; }
            set
            {
                _PastTo = value;
                OnPropertyChanged("PastTo");
            }
        }

        string _CutFrom { get; set; }
        public string CutFrom
        {
            get { return _CutFrom; }
            set
            {
                _CutFrom = value;
                OnPropertyChanged("CutFrom");
            }
        }

        private DelegateCommand _click;
        public DelegateCommand Click
        {
            get { return _click; }
            set { _click = value; }
        }
    }
}
