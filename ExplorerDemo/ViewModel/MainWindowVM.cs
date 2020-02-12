using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo.ViewModel
{
    public class MainWindowVM : BaseClass
    {
        #region Private Properties
        private object homePage;
        private object currentPage;
        private ConnectedDrivesModel savedData;

        #endregion

        #region Private Properties
        public object HomePage
        {
            get
            {
                return homePage;
            }
            set
            {
                homePage = value;
                OnPropertyChanged("HomePage");
            }
        }
        public object CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public ConnectedDrivesModel SavedData
        {
            get
            {
                return savedData;
            }
            set
            {
                savedData = value;
                OnPropertyChanged("SavedData");
            }
        }
        #endregion

        public MainWindowVM()
        {
            //objMainWindowVM = this;
            HomePage = new DrivesVM();
            CurrentPage = HomePage;
        }

    }
}
