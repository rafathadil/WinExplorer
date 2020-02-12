//using ExplorerDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExplorerDemo.ViewModel
{
    public class ChoosenDrive : BaseClass, INavigation
    {


        #region Private Properties
 
        private object previousPage;
        private object nextPage;

        #endregion

        #region Private Properties

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
            GlobalVariable.objMainWindowVM.CurrentPage = new DrivesVM();
        }

        private bool CanExcuteShowPreviousPage(object obj)
        {
            return false;
        }
        private bool CanExcuteShowHomePage(object obj)
        {
            return true;
        }


        private bool CanExcuteDrives(object obj)
        {
            return true;
        }

        ObservableCollection<ObservableCollection<RootTreeViewItem>> _MainNodeViewItem { get; set; }

        public ObservableCollection<ObservableCollection<RootTreeViewItem>> MainNodeViewItem
        {
            get { return _MainNodeViewItem; }
            set
            {
                if (_MainNodeViewItem == null)
                {
                    _MainNodeViewItem = new ObservableCollection<ObservableCollection<RootTreeViewItem>>();
                }
                _MainNodeViewItem = value;
                OnPropertyChanged("MainNodeViewItem");
            }
        }

        ObservableCollection<RootTreeViewItem> _RootTreeViewItem { get; set; }

        public ObservableCollection<RootTreeViewItem> LsRootTreeViewItem
        {
            get { return _RootTreeViewItem; }
            set
            {
                if (_RootTreeViewItem == null)
                {
                    _RootTreeViewItem = new ObservableCollection<RootTreeViewItem>();
                }
                _RootTreeViewItem = value;
                OnPropertyChanged("RootTreeViewItem");
            }
        }




        private DelegateCommand _ClickCopy;
        public DelegateCommand ClickCopy
        {
            get { return _ClickCopy; }
            set { _ClickCopy = value; }
        }

        private DelegateCommand _ClickCut;
        public DelegateCommand ClickCut
        {
            get { return _ClickCut; }
            set { _ClickCut = value; }
        }

        private DelegateCommand _ClickPaste;
        public DelegateCommand ClickPaste
        {
            get { return _ClickPaste; }
            set { _ClickPaste = value; }
        }

        //private bool _IsExpanded;
        //public bool IsExpanded
        //{
        //    get { return _IsExpanded; }
        //    set
        //    {
        //        if (_IsExpanded == value) return;
        //        _IsExpanded = value;
        //        OnPropertyChanged("IsExpanded");//or however you need to do it
        //        //CallSomeOtherFunc();//this is the code that you need to be called when changed.
        //    }
        //}

       

        private bool SetCopyingPath(object obj)
        {
            System.Windows.DependencyObject Obj = new System.Windows.DependencyObject();
            
            var test = TreeViewHelper.GetSelectedItem(Obj);
            return true;
        }

        private void _SetCopyingPath(object obj)
        {
            var test = TreeViewHelper.SelectedItemProperty;
          //  return null;
        }
        public ChoosenDrive()
        {


            ShowHomePage = new DelegateCommand(ExecuteShowNextPage, CanExcuteShowHomePage);
            ClickCopy = new DelegateCommand(_SetCopyingPath, SetCopyingPath);
            DirectoryInfo DIR = new DirectoryInfo(GlobalVariable.objMainWindowVM.SavedData.DriveLetter);
            LsRootTreeViewItem = new ObservableCollection<RootTreeViewItem>();
            MainNodeViewItem = new ObservableCollection<ObservableCollection<RootTreeViewItem>>();
            foreach (DirectoryInfo DR in DIR.GetDirectories())
            {
               
                if (!DR.Attributes.ToString().Contains("Hidden"))
                {
                    RootTreeViewItem ParenNode = new RootTreeViewItem();
                    ParenNode.Header = DR.Name;
                    ParenNode.Path = DR.FullName;
                    ParenNode.Type = DR.GetType().Name;
                    ParenNode.IsExpanded = false;
                    LsRootTreeViewItem.Add(ParenNode);

                    ParenNode.LsChildrenNode = new ObservableCollection<RootTreeViewItem>();
                   // ParenNode.LsFiles = new ObservableCollection<RootTreeViewItem>();
                    foreach (DirectoryInfo CDIR in DR.GetDirectories())
                    {
                        RootTreeViewItem ChildrenNode = new RootTreeViewItem();
                        ChildrenNode.IsExpanded = false;
                        ChildrenNode.Header = CDIR.Name;
                        ChildrenNode.Path = CDIR.FullName;
                        ChildrenNode.Type = CDIR.GetType().Name;
                        ParenNode.LsChildrenNode.Add(ChildrenNode);

                    }
                }


            }
            foreach (FileInfo DR in DIR.GetFiles())
            {
                if (!DR.Attributes.ToString().Contains("Hidden"))
                {
                    RootTreeViewItem ParenNode = new RootTreeViewItem();
                    ParenNode.Header = DR.Name;
                    ParenNode.Path = DR.FullName;
                    ParenNode.Type = DR.GetType().Name;
                    ParenNode.IsExpanded = false;
                    LsRootTreeViewItem.Add(ParenNode);
                }
            }
        }
    }


}
