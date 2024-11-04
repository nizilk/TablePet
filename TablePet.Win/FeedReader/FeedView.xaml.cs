using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CodeHollow.FeedReader;
using TablePet.Services.Controllers;

namespace TablePet.Win.FeedReader
{
    /// <summary>
    /// FeedReader.xaml 的交互逻辑
    /// </summary>
    public partial class FeedView : Window
    {
        public FeedReaderService feedReaderService;


        public FeedView()
        {
            InitializeComponent();
        }

        public FeedView(FeedReaderService feedReaderService)
        {
            InitializeComponent();
            this.feedReaderService = feedReaderService;
            lb_Entries.ItemsSource = this.feedReaderService.Items;
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


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            for (int i = 0; i < this.lb_Entries.Items.Count; i++)
            {
                ListBoxItem it = this.lb_Entries.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                RichTextBox rtb = SearchVisualTree<RichTextBox>(it);
                rtb.Document.PageWidth = rtb.ActualWidth - 20;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.lb_Entries.Items.Count; i++)
            {
                ListBoxItem it = this.lb_Entries.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                RichTextBox rtb = SearchVisualTree<RichTextBox>(it);
                rtb.Document.PageWidth = rtb.ActualWidth - 20;
            }
        }
    }
}
