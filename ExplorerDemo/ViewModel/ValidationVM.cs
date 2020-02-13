using ExplorerDemo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo.ViewModel
{
    public static class ValidationVM
    {

        public static string Validationerror { get; set; }
        public static bool ValidatedPaste()
        {
            if (ExplorerDemo.GlobalVariable.FileTransferModel.DestinationAdress.Equals(ExplorerDemo.GlobalVariable.FileTransferModel.SourceFile))
            {
                Validationerror = "source address and destination address is same";
                return false;
            }
            else if (!IsDirectory && !File.Exists(ExplorerDemo.GlobalVariable.FileTransferModel.SourceFile.Path))
            {
                Validationerror = "Can't find source File";
                return false;
            }
            else
            {
                return true;
            }

        }


        public static bool IsDirectory
        {

            get
            {
                FileAttributes attr = File.GetAttributes(ExplorerDemo.GlobalVariable.FileTransferModel.SourceFile.Path);

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    return true;
                else
                    return false;
            }
        }
    }
}
