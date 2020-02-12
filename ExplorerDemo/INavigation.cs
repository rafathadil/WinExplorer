using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerDemo
{
    interface INavigation
    {
      //  Int16 Id { get; set; }
        object PreviousPage { get; set; }
        object NextPage { get; set; }

        DelegateCommand ShowNextPage { get; set; }
        DelegateCommand ShowPreviousPage { get; set; }
        DelegateCommand ShowHomePage { get; set; }

    }
}
