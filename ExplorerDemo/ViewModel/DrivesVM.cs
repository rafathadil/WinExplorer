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
        private ObservableCollection<ConnectedDrivesModel> LsConnectedDrives
        {
            get; set;
        }

        #region Private Properties

        private object previousPage;
        private object nextPage;

        #endregion

        #region Private Properties
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
        public object PreviousPage
        {
            get
            {
                return previousPage;
            }
            set
            {
                previousPage = value;
                OnPropertyChanged("PreviousPage");
            }
        }
        public object NextPage
        {
            get
            {
                return nextPage;
            }
            set
            {
                nextPage = value;
                OnPropertyChanged("NextPage");
            }
        }
        #endregion


        //The command property
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

        public DrivesVM()
        {
            var LS = DriveInfo.GetDrives().Where(dr => dr.IsReady == true).ToList();

            ListofConnectedDrives = new ObservableCollection<ConnectedDrivesModel>();

            foreach (var DriveInfo in LS)
            {
                ConnectedDrivesModel ConnectedDrive = new ConnectedDrivesModel();
                ConnectedDrive.DriveLetter = DriveInfo.Name;
                ConnectedDrive.Click = new DelegateCommand(ExecuteClick, CanExcuteDrives);
                ListofConnectedDrives.Add(ConnectedDrive);
            }

            PreviousPage = this;

            ShowNextPage = new DelegateCommand(null, CanExcuteShowNextPage);
            ShowPreviousPage = new DelegateCommand(null, CanExcuteShowPreviousPage);
            ShowHomePage = new DelegateCommand(null, CanExcuteShowPreviousPage);
            Navigatebegin = new DelegateCommand(ExecuteShowNextPage, _CanExcuteShowNextPage);


        }

        private bool _CanExcuteShowNextPage(object obj)
        {
            return true;
        }
        //The execute and can execute methods.
        private bool CanExcuteShowNextPage(object obj)
        {
            return false;
        }
        private void ExecuteShowNextPage(object obj)
        {
            //  NextPage = new LanguageVM();
            GlobalVariable.objMainWindowVM.CurrentPage = NextPage;
        }

        private bool CanExcuteShowPreviousPage(object obj)
        {
            return false;
        }
        private bool CanExcuteShowHomePage(object obj)
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

            NextPage = new ChoosenDrive();
            GlobalVariable.objMainWindowVM.HomePage = GlobalVariable.objMainWindowVM.CurrentPage;
            GlobalVariable.objMainWindowVM.CurrentPage = NextPage;
            
        }

    }
}
