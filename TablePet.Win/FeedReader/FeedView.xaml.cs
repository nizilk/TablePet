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
        public ObservableCollection<FeedExt> Feeds { get; set; } = new ObservableCollection<FeedExt>();
        public ObservableCollection<FeedItemExt> Items { get; set; } = new ObservableCollection<FeedItemExt>();


        public FeedView()
        {
            InitializeComponent();
        }


        public FeedView(ObservableCollection<FeedExt> Feeds)
        {
            InitializeComponent();
            this.Feeds = Feeds;
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


        private T SearchVisualTree<T>(DependencyObject tarElem) where T : DependencyObject
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
                        return (T)child;
                    }
                    else
                    {
                        var res = SearchVisualTree<T>(child);
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
                RichTextBox rtb = SearchVisualTree<RichTextBox>(it);
                rtb.Document.PageWidth = rtb.ActualWidth - 20;
            }
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeDocumentWidth();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tv_Feeds.ItemsSource = Feeds;
            lb_Entries.ItemsSource = Items;
            ChangeDocumentWidth();
        }


        private void bt_addFeed_Click(object sender, RoutedEventArgs e)
        {
            FeedProperties feedProperties = new FeedProperties(this);
            feedProperties.Show();
        }


        private void FeedUpdate_Click(object sender, RoutedEventArgs e)
        {
            var obj = (FeedExt)tv_Feeds.SelectedItem;
            Task readTask = Task.Run(() =>
            {
                obj.feed = feedReaderService.ReadFeed(obj.url);
                Dispatcher.Invoke(new Action(() =>
                {
                    lb_Entries.ItemsSource = obj.feed.Items;
                    ChangeDocumentWidth();
                }));
            });
        }


        private void FeedDelete_Click(object sender, RoutedEventArgs e)
        {
            var obj = (FeedExt)tv_Feeds.SelectedItem;
            Feeds.Remove(obj);
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

            if (feed.feed == null) return;

            Items.Clear();
            foreach (FeedItem it in feed.feed.Items)
            {
                Items.Add(new FeedItemExt(it, feed.feed.Title));
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
    }
}
