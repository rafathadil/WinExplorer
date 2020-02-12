using ExplorerDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo
{
    public static class GlobalVariable
    {
        public static ViewModel.MainWindowVM objMainWindowVM { get; set; }

        public static FileTransferModel FileTransferModel { get; set; }

        public static RootTreeViewItem TreviewSelectedItem { get; set; }
    }
}
