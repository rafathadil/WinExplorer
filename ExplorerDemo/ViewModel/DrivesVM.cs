using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo.ViewModel
{
    public class DrivesVM : BaseClass, INavigation
    {

        #region Private Properties
        private ObservableCollection<ConnectedDrivesModel> LsConnectedDrives
        {
            get; set;
        }
        public ObservableCollection<ConnectedDrivesModel> ListofConnectedDrives
        {
            get
            {
                return LsConnectedDrives;
            }
            set
            {
                LsConnectedDrives = value;
            }
        }

        private DelegateCommand showNextPage;
        public DelegateCommand ShowNextPage
        {
            get { return showNextPage; }
            set { showNextPage = value; }
        }

        private DelegateCommand showPreviousPage;
        public DelegateCommand ShowPreviousPage
        {
            get { return showPreviousPage; }
            set { showPreviousPage = value; }
        }

        private DelegateCommand showHomePage;
        public DelegateCommand ShowHomePage
        {
            get { return showHomePage; }
            set { showHomePage = value; }
        }

        private DelegateCommand navigatebegin;
        public DelegateCommand Navigatebegin
        {
            get { return navigatebegin; }
            set { navigatebegin = value; }
        }

        #endregion

        #region Methods
        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        public void Getdrivedetails()
        {
            var LS = DriveInfo.GetDrives().Where(dr => dr.IsReady == true).ToList();

            ListofConnectedDrives = new ObservableCollection<ConnectedDrivesModel>();

            foreach (var DriveInfo in LS)
            {
                ConnectedDrivesModel ConnectedDrive = new ConnectedDrivesModel();
                ConnectedDrive.DriveLable = DriveInfo.VolumeLabel;
                ConnectedDrive.DriveLetter = DriveInfo.Name;
                ConnectedDrive.AvailableFreeSpace = "FreeSpace : " + FormatBytes(DriveInfo.AvailableFreeSpace);
                ConnectedDrive.TotalSize = "Total Size : " + FormatBytes(DriveInfo.TotalSize);
                ConnectedDrive.VloType = "Drive Type : " + DriveInfo.DriveType;
                ConnectedDrive.Click = new DelegateCommand(ExecuteClick, CanExcuteDrives);
                ListofConnectedDrives.Add(ConnectedDrive);
            }
        }

        private bool _CanExcute(object obj)
        {
            return false;
        }
        private bool CanExcuteDrives(object obj)
        {
            return true;
        }
        private void ExecuteClick(object obj)
        {
            GlobalVariable.objMainWindowVM.SavedData = ListofConnectedDrives.Where(i => i.IsSelected).FirstOrDefault();
            GlobalVariable.objMainWindowVM.HomePage = GlobalVariable.objMainWindowVM.CurrentPage;
            GlobalVariable.objMainWindowVM.CurrentPage = new ChoosenDrive();

        }
        #endregion

        public DrivesVM()
        {
            ShowNextPage = new DelegateCommand(null, _CanExcute);
            ShowPreviousPage = new DelegateCommand(null, _CanExcute);
            ShowHomePage = new DelegateCommand(null, _CanExcute);
            Getdrivedetails();

        }
    }
}
