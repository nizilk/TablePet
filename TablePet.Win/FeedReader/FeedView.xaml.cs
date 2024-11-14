using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using CodeHollow.FeedReader;
using TablePet.Services.Controllers;
using TablePet.Services.Models;

namespace TablePet.Win.FeedReader
{
    /// <summary>
    /// FeedReader.xaml 的交互逻辑
    /// </summary>
    public partial class FeedView : Window
    {
        public FeedReaderService feedReaderService = new FeedReaderService();

        public ObservableCollection<FeedExt> Nodes { get; set; }

        public ObservableCollection<FeedItemExt> Items { get; set; } = new ObservableCollection<FeedItemExt>();
        public List<string> Folders { get; set; } = new List<string>();


        public FeedView()
        {
            InitializeComponent();
        }


        public FeedView(ObservableCollection<FeedExt> Feeds, List<string> Folders)
        {
            InitializeComponent();
            this.Nodes = Feeds;
            this.Folders = Folders;
        }


        private void HandlePreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }


        private DependencyObject VisualDownwardSearch<T>(DependencyObject tarElem)
        {
            if (tarElem != null)
            {
                var count = VisualTreeHelper.GetChildrenCount(tarElem);
                if (count == 0)
                    return null;
                for (int i = 0; i < count; ++i)
                {
                    var child = VisualTreeHelper.GetChild(tarElem, i);
                    if (child != null && child is T)
                    {
                        return child;
                    }
                    else
                    {
                        var res = VisualDownwardSearch<T>(child);
                        if (res != null)
                        {
                            return res;
                        }
                    }
                }
            }
            return null;
        }


        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }


        private void ChangeDocumentWidth()
        {
            lb_Entries.UpdateLayout();
            for (int i = 0; i < this.lb_Entries.Items.Count; i++)
            {
                ListBoxItem it = this.lb_Entries.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                var rtb = VisualDownwardSearch<RichTextBox>(it) as RichTextBox;
                rtb.Document.PageWidth = rtb.ActualWidth - 20;
            }
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeDocumentWidth();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tv_Feeds.ItemsSource = Nodes;
            lb_Entries.ItemsSource = Items;
            ChangeDocumentWidth();
        }


        private void bt_addFeed_Click(object sender, RoutedEventArgs e)
        {
            FeedProperties feedProperties = new FeedProperties(this);
            feedProperties.Show();
        }


        public void AddFeed(FeedExt feed)
        {
            if (feed == null) return;
            if (feed.IsFolder) return;
            if (feed.FolderID == 0) Nodes.Add(feed);
            else
            {
                foreach(var node in Nodes)
                {
                    if (!node.IsFolder) continue;
                    if (node.ID == feed.FolderID)
                    {
                        Nodes.Remove(node);
                        node.Nodes.Add(feed);
                        Nodes.Add(node);
                        break;
                    }
                }
            }
        }


        public void UpdateFeed(FeedExt feed, FeedExt oldFeed)
        {
            if (feed == null) return;
            if (oldFeed.FolderID == 0)  Nodes.Remove(oldFeed);
            else
            {
                foreach(var node in Nodes)
                {
                    if (!node.IsFolder) continue;
                    if (node.ID == oldFeed.FolderID)    node.Nodes.Remove(oldFeed);
                }
            }
            AddFeed(feed);
        }


        private void FeedUpdate_Click(object sender, RoutedEventArgs e)
        {
            var obj = (FeedExt)tv_Feeds.SelectedItem;
            Task readTask = Task.Run(() =>
            {
                obj.Feed = feedReaderService.ReadFeed(obj.Url);
                Dispatcher.Invoke(new Action(() =>
                {
                    lb_Entries.ItemsSource = obj.Feed.Items;
                    ChangeDocumentWidth();
                }));
            });
        }


        private void FeedDelete_Click(object sender, RoutedEventArgs e)
        {
            var obj = (FeedExt)tv_Feeds.SelectedItem;
            Nodes.Remove(obj);
            lb_Entries.ItemsSource = null;
        }


        private void FeedSetting_Click(object sender, RoutedEventArgs e)
        {
            var obj = (FeedExt)tv_Feeds.SelectedItem;
            FeedProperties feedProperties = new FeedProperties(this, obj);
            feedProperties.Show();
        }


        private void tv_Feeds_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem == null) return;
            FeedExt feed = (FeedExt)treeViewItem.DataContext;

            if (feed.Feed == null) return;
            Items.Clear();
            foreach (FeedItem it in feed.Feed.Items)
            {
                Items.Add(new FeedItemExt(it, feed.Feed.Title));
            }

            ChangeDocumentWidth();
            //treeViewItem.
        }


        private void tv_Feeds_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        private void bt_addFolder_Click(object sender, RoutedEventArgs e)
        {
            AddFolder addFolder = new AddFolder(this);
            addFolder.Show();
        }

        public void AddFolder(string name)
        {
            FeedExt folder = new FeedExt(ID: Folders.Count, Title: name, IsFolder: true);
            Nodes.Add(folder);
            Folders.Add(name);
        }
    }
}
