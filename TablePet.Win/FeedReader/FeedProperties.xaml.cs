using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using TablePet.Services.Controllers;
using TablePet.Services.Models;

namespace TablePet.Win.FeedReader
{
    /// <summary>
    /// FeedProperties.xaml 的交互逻辑
    /// </summary>
    public partial class FeedProperties : Window
    {
        private FeedReaderService feedReaderService = new FeedReaderService();
        private FeedView feedView;
        private Feed feed;
        private FeedExt feedOriginal;
        private bool update = false;

        public FeedProperties()
        {
            InitializeComponent();
        }


        public FeedProperties(FeedView feedView)
        {
            InitializeComponent();
            this.feedView = feedView;
            update = false;
        }


        public FeedProperties(FeedView feedView, FeedExt feed)
        {
            InitializeComponent();
            this.feedView = feedView;
            this.feedOriginal = feed;
            this.feed = feed.feed;
            update = true;
            UpdateFeedText(feed);
        }


        private void UpdateUrl(List<string> urls)
        {
            cb_url.Dispatcher.Invoke(new Action(() =>
            {
                if (urls == null || urls.Count <= 0)
                {
                    cb_url.Text = string.Empty;
                }
                else if (urls.Count == 1)
                {
                    cb_url.Text = urls[0];
                }
                else
                {
                    foreach (string url in urls)
                    {
                        cb_url.Items.Add(url);
                        cb_url.IsDropDownOpen = true;
                    }
                }
            }));
        }


        private void UpdateFeedText(FeedExt f)
        {
            if (f.feed == null) return;
            Dispatcher.Invoke(new Action(() =>
            {
                if (update) cb_url.Text = f.url;
                feed = f.feed;
                tb_feedTitle.Text = feed.Title;
                lb_lastDate.Content = feedReaderService.GetTimeSpanTilNow((DateTime)feed.LastUpdatedDate);
                lb_state.Content = "OK";
            }));
        }


        private void bt_loadFeed_Click(object sender, RoutedEventArgs e)
        {
            string url = cb_url.Text;
            Task findTask = Task.Run(() =>
            {
                List<string> urls = feedReaderService.FindFeed(url);
                UpdateUrl(urls);
            });

            Task readTask = Task.Run(() =>
            {
                Feed f = feedReaderService.ReadFeed(url);
                UpdateFeedText(new FeedExt(f,url));
            });
        }


        private void cb_url_DropDownClosed(object sender, EventArgs e)
        {
            string url = cb_url.Text;
            Task readTask = Task.Run(() =>
            {
                Feed f = feedReaderService.ReadFeed(url);
                UpdateFeedText(new FeedExt(f, url));
            });
        }


        private void bt_feedSave_Click(object sender, RoutedEventArgs e)
        {
            feed.Title = tb_feedTitle.Text;
            if (update)
            {
                feedView.Feeds.Remove(feedOriginal);
            }
            feedView.Feeds.Add(new FeedExt(feed, cb_url.Text));
            this.Close();
        }


        private void bt_feedPreview_Click(object sender, RoutedEventArgs e)
        {

        }


        private void bt_feedCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
