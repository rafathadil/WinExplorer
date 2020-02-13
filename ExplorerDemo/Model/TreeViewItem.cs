using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ExplorerDemo
{

    public class TreeViewHelper 
    {
        private static Dictionary<DependencyObject, TreeViewSelectedItemBehavior> behaviors = new Dictionary<DependencyObject, TreeViewSelectedItemBehavior>();

        public static object GetSelectedItem(DependencyObject obj)
        {
            return (object)obj.GetValue(SelectedItemProperty);
        }

       
        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new UIPropertyMetadata(null, SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is TreeView))
                return;

            if (!behaviors.ContainsKey(obj))
                behaviors.Add(obj, new TreeViewSelectedItemBehavior(obj as TreeView));

            TreeViewSelectedItemBehavior view = behaviors[obj];
            view.ChangeSelectedItem(e.NewValue);
        }

        private class TreeViewSelectedItemBehavior
        {
            TreeView view;
            public TreeViewSelectedItemBehavior(TreeView view)
            {
                this.view = view;
                view.SelectedItemChanged += (sender, e) => SetSelectedItem(view, e.NewValue);
            }

            internal void ChangeSelectedItem(object p)
            {
                TreeViewItem item = (TreeViewItem)view.ItemContainerGenerator.ContainerFromItem(p);
                if (item != null)
                    item.IsSelected = true;
            }
        }
    }
    public class RootTreeViewItem : BaseClass 
    {


        public string Path { get; set; }
        public string Type { get; set; }
        public string Header { get; set; }

        private bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                _IsExpanded = value;
                OnPropertyChanged("IsExpanded");
                GetChiledNodes();
            }
        }

        private void GetChiledNodes()
        {
            if (LsChildrenNode != null)
            {
                foreach (var CTVI in LsChildrenNode)
                {
                    if (CTVI.Type == "DirectoryInfo")
                    {
                        DirectoryInfo DIR = new DirectoryInfo(CTVI.Path);
                        CTVI.LsChildrenNode = new ObservableCollection<RootTreeViewItem>();
                        foreach (DirectoryInfo CDIR in DIR.GetDirectories())
                        {
                            if (!CDIR.Attributes.ToString().Contains("Hidden"))
                            {
                                RootTreeViewItem CTVI1 = new RootTreeViewItem();
                                CTVI1.LsChildrenNode = new ObservableCollection<RootTreeViewItem>();
                                CTVI1.Header = CDIR.Name;
                                CTVI1.Path = CDIR.FullName;
                                CTVI1.Type = CDIR.GetType().Name;
                                CTVI1.IsExpanded = false;
                                CTVI.LsChildrenNode.Add(CTVI1);
                            }
                        }


                        foreach (FileInfo FL in DIR.GetFiles())
                        {
                            if (!FL.Attributes.ToString().Contains("Hidden"))
                            {
                                RootTreeViewItem CTVI1 = new RootTreeViewItem();
                                //CTVI1. = new ObservableCollection<RootTreeViewItem>();
                                CTVI1.IsExpanded = false;
                                CTVI1.Header = FL.Name;
                                CTVI1.Path = FL.FullName;
                                CTVI1.Type = FL.GetType().Name;
                                CTVI.LsChildrenNode.Add(CTVI1);
                            }
                        }

                    }
                }
            }
        }

        bool _IsSelected { get; set; }
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected == value) return;
                _IsSelected = value;
                OnPropertyChanged("IsSelected");

                ExplorerDemo.GlobalVariable.TreviewSelectedItem = this;
                
            }
        }
        ObservableCollection<RootTreeViewItem> _LsChildrenNode { get; set; }

        public ObservableCollection<RootTreeViewItem> LsChildrenNode
        {
            get { return _LsChildrenNode; }
            set
            {
                if (_LsChildrenNode == null)
                {
                    _LsChildrenNode = new ObservableCollection<RootTreeViewItem>();
                }
                _LsChildrenNode = value;
                OnPropertyChanged("LsChildrenNode");
            }
        }

    }
}
