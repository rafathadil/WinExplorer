using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo.Model
{
    public class FileTransferModel : BaseClass
    {
        public enum EAction
        {
            none,  cut, copy, 
        }
        RootTreeViewItem _SourceFile { get; set; }
        public RootTreeViewItem SourceFile
        {
            get { return _SourceFile; }
            set
            {
                _SourceFile = value;
                OnPropertyChanged("SourceFile");
            }
        }

        public RootTreeViewItem _DestinationAdress { get; set; }
        public RootTreeViewItem DestinationAdress
        {
            get { return _DestinationAdress; }
            set
            {
                _DestinationAdress = value;
                OnPropertyChanged("DestinationAdress");
            }
        }

       
        public EAction CurrentAction { get; set; } 
        

        private DelegateCommand _click;
        public DelegateCommand Click
        {
            get { return _click; }
            set { _click = value; }
        }
    }
}
