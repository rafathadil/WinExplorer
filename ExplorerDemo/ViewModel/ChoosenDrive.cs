//using ExplorerDemo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExplorerDemo.ViewModel
{
    public class ChoosenDrive : BaseClass, INavigation
    {

        #region properties  
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



        public string _ValidationError { get; set; }
        public string ValidationError
        {
            get
            {
                return _ValidationError;
            }
            set
            {
                _ValidationError = value;
                OnPropertyChanged("ValidationError");
            }
        }
        private DelegateCommand _ClickCopy;
        public DelegateCommand ClickCopy
        {
            get { return _ClickCopy; }
            set { _ClickCopy = value; OnPropertyChanged("ClickCopy"); }
        }

        private DelegateCommand _ClickCut;
        public DelegateCommand ClickCut
        {
            get { return _ClickCut; }
            set { _ClickCut = value; OnPropertyChanged("ClickCut"); }
        }

        private DelegateCommand _ClickPaste;
        public DelegateCommand ClickPaste
        {
            get { return _ClickPaste; }
            set
            {
                _ClickPaste = value;
                OnPropertyChanged("ClickPaste");
            }
        }

        #endregion

        #region Methods
        private bool IsAllowedToCopy(object obj)
        {
            if (ExplorerDemo.GlobalVariable.FileTransferModel == null || ExplorerDemo.GlobalVariable.FileTransferModel.CurrentAction == Model.FileTransferModel.EAction.none)
                return true;
            else if (ExplorerDemo.GlobalVariable.TreviewSelectedItem != null)
            {
                //ClickCut = new DelegateCommand(SetCutingPath, IsAllowedToCut);
                return true;
            }
            else
                return false;
        }

        private bool IsAllowedToCut(object obj)
        {
            if (ExplorerDemo.GlobalVariable.FileTransferModel == null || ExplorerDemo.GlobalVariable.FileTransferModel.CurrentAction == Model.FileTransferModel.EAction.none)
                return true;
            else if (ExplorerDemo.GlobalVariable.TreviewSelectedItem != null) //&& ExplorerDemo.GlobalVariable.FileTransferModel.CurrentAction == Model.FileTransferModel.EAction.cut)
            {
              //  ClickCopy = new DelegateCommand(SetCopyingPath, IsAllowedToCopy);
                return true;
            }
            else
                return false;
        }

        private bool IsAllowedToPaste(object obj)
        {

            if (ExplorerDemo.GlobalVariable.FileTransferModel == null)
                return false;
            else if (ExplorerDemo.GlobalVariable.TreviewSelectedItem != null)
            {
                ExplorerDemo.GlobalVariable.FileTransferModel.DestinationAdress = ExplorerDemo.GlobalVariable.TreviewSelectedItem;
                return true;
            }
            else
                return false;
        }

        private void SetCopyingPath(object obj)
        {
            ValidationError = string.Empty;
            if (ExplorerDemo.GlobalVariable.TreviewSelectedItem != null)
            {
                ExplorerDemo.GlobalVariable.FileTransferModel = new Model.FileTransferModel();
                ExplorerDemo.GlobalVariable.FileTransferModel.SourceFile = ExplorerDemo.GlobalVariable.TreviewSelectedItem;
                ExplorerDemo.GlobalVariable.FileTransferModel.CurrentAction = Model.FileTransferModel.EAction.none;
                ClickPaste = new DelegateCommand(Paste, IsAllowedToPaste);

                ValidationError = "File acquired to copy";
            }
        }
        private void SetCutingPath(object obj)
        {
            ValidationError = string.Empty;
            if (ExplorerDemo.GlobalVariable.TreviewSelectedItem != null)
            {
                ExplorerDemo.GlobalVariable.FileTransferModel = new Model.FileTransferModel();
                ExplorerDemo.GlobalVariable.FileTransferModel.SourceFile = ExplorerDemo.GlobalVariable.TreviewSelectedItem;
                ExplorerDemo.GlobalVariable.FileTransferModel.CurrentAction = Model.FileTransferModel.EAction.cut;
                ClickPaste = new DelegateCommand(Paste, IsAllowedToPaste);
                ValidationError = "File acquired to cut";
            }
        }
        private void Paste(object obj)
        {
            if (ExplorerDemo.GlobalVariable.FileTransferModel != null)
            {
                if (ExplorerDemo.ViewModel.ValidationVM.ValidatedPaste())
                {
                    ValidationError = string.Empty;
                    GlobalVariable.objMainWindowVM.CurrentPage = new ProcessVM();
                    Thread ProcessFiles = new Thread(delegate ()
                    {

                        if (!DataTransferVM.Pastefiles(ExplorerDemo.GlobalVariable.FileTransferModel))
                        {
                            ValidationError = DataTransferVM.LastErrorMsg;
                        }
                        else
                        {
                            ValidationError = "File Moved";
                            ExplorerDemo.GlobalVariable.FileTransferModel = null;
                        }


                        GlobalVariable.objMainWindowVM.CurrentPage = this;


                    });
                    ProcessFiles.Start();

                }
                else
                {
                    ExplorerDemo.GlobalVariable.FileTransferModel = null;
                    ValidationError = ExplorerDemo.ViewModel.ValidationVM.Validationerror;
                }
            }

        }

        private void ExecuteShowHomeNPage(object obj)
        {
            GlobalVariable.objMainWindowVM.CurrentPage = new DrivesVM();
        }

        private bool _CanExcute(object obj)
        {
            return false;
        }
        private bool CanExcuteShowHomePage(object obj)
        {
            return true;
        }

        public void BuildParentNode()
        {
            DirectoryInfo DIR = new DirectoryInfo(GlobalVariable.objMainWindowVM.SavedData.DriveLetter);
            LsRootTreeViewItem = new ObservableCollection<RootTreeViewItem>();

            RootTreeViewItem Parent = new RootTreeViewItem();
            Parent.Header = DIR.Name;
            Parent.Path = DIR.FullName;
            Parent.Type = DIR.GetType().Name;
            Parent.IsExpanded = false;
            LsRootTreeViewItem.Add(Parent);

            Parent.LsChildrenNode = new ObservableCollection<RootTreeViewItem>();
            foreach (DirectoryInfo DR in DIR.GetDirectories())
            {

                if (!DR.Attributes.ToString().Contains("Hidden"))
                {
                    RootTreeViewItem ParenNode = new RootTreeViewItem();
                    ParenNode.Header = DR.Name;
                    ParenNode.Path = DR.FullName;
                    ParenNode.Type = DR.GetType().Name;
                    ParenNode.IsExpanded = false;
                    Parent.LsChildrenNode.Add(ParenNode);

                    ParenNode.LsChildrenNode = new ObservableCollection<RootTreeViewItem>();
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
                    Parent.LsChildrenNode.Add(ParenNode);
                }
            }
        }

        #endregion

        public ChoosenDrive()
        {
            ShowHomePage = new DelegateCommand(ExecuteShowHomeNPage, CanExcuteShowHomePage);
            ShowNextPage = new DelegateCommand(null, _CanExcute);
            ShowPreviousPage = new DelegateCommand(null, _CanExcute);

            ClickCopy = new DelegateCommand(SetCopyingPath, IsAllowedToCopy);
            ClickCut = new DelegateCommand(SetCutingPath, IsAllowedToCut);
            ClickPaste = new DelegateCommand(Paste, IsAllowedToPaste);

            BuildParentNode();

        }
    }


}
