using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo.ViewModel
{
    public class ProcessVM : BaseClass,INavigation
    {

        #region Private Properties

        
        

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

        #endregion

        private bool CanExcute(object obj)
        {
            return false;
        }
        public ProcessVM()
        {
            ShowNextPage = new DelegateCommand(null, CanExcute);
            ShowPreviousPage = new DelegateCommand(null, CanExcute);
            ShowHomePage = new DelegateCommand(null, CanExcute);

        }

    }
}
