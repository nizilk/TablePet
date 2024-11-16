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
        private FeedReaderService feedReaderService;
        

        public FeedView()
        {
            InitializeComponent();
        }


        public FeedView(FeedReaderService service)
        {
            InitializeComponent();
            this.feedReaderService = service;
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
            tv_Feeds.ItemsSource = feedReaderService.Feeds;
            ChangeDocumentWidth();
        }


        private void bt_addFeed_Click(object sender, RoutedEventArgs e)
        {
            FeedProperties feedProperties = new FeedProperties(feedReaderService);
            feedProperties.Show();
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
            feedReaderService.DelFeed(obj);
            lb_Entries.ItemsSource = null;
        }


        private void FeedSetting_Click(object sender, RoutedEventArgs e)
        {
            var obj = (FeedExt)tv_Feeds.SelectedItem;
            FeedProperties feedProperties = new FeedProperties(feedReaderService, obj);
            feedProperties.Show();
        }


        private void tv_Feeds_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem == null) return;
            FeedExt feed = (FeedExt)treeViewItem.DataContext;

            if (feed.Feed == null) return;

            lb_Entries.ItemsSource = null;
            lb_Entries.Items.Clear();
            lb_Entries.ItemsSource = feed.Items;

            ChangeDocumentWidth();
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
            AddFolder addFolder = new AddFolder(feedReaderService);
            addFolder.Show();
        }

        private void lbi_star_Selected(object sender, RoutedEventArgs e)
        {

            ChangeDocumentWidth();
        }
    }
}
