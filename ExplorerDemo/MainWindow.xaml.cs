using ExplorerDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExplorerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindowVM MainVM { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainVM = new MainWindowVM();
            GlobalVariable.objMainWindowVM = MainVM;
            this.DataContext = GlobalVariable.objMainWindowVM;
        }
    }
}
