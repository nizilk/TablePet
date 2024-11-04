using CodeHollow.FeedReader;
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
using System.Windows.Shapes;
using TablePet.Services.Controllers;

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

        public FeedProperties()
        {
            InitializeComponent();
        }


        public FeedProperties(FeedView feedView)
        {
            InitializeComponent();
            this.feedView = feedView;
        }


        private void bt_loadFeed_Click(object sender, RoutedEventArgs e)
        {
            List<string> urls = feedReaderService.FindFeed(cb_url.Text);

            if (urls.Count <= 0)
            {
                cb_url.Text = string.Empty;
            }
            else if (urls.Count == 1)
            {
                cb_url.Text = urls[0];
                feed = feedReaderService.ReadFeed(cb_url.Text);
                tb_feedTitle.Text = feed.Title;
                lb_lastDate.Content = feedReaderService.GetTimeSpanTilNow((DateTime)feed.LastUpdatedDate);
                lb_state.Content = "OK";
            }
            else
            {
                foreach (string url in urls)
                {
                    cb_url.Items.Add(url);
                    cb_url.IsDropDownOpen = true;
                }
            }
        }

        private void cb_url_DropDownClosed(object sender, EventArgs e)
        {
            feed = feedReaderService.ReadFeed(cb_url.Text);
            if (feed == null) return;
            tb_feedTitle.Text = feed.Title;
            lb_lastDate.Content = feedReaderService.GetTimeSpanTilNow((DateTime)feed.LastUpdatedDate);
            lb_state.Content = "OK";
        }


        private void bt_feedSave_Click(object sender, RoutedEventArgs e)
        {
            feed.Title = tb_feedTitle.Text;
            feedReaderService.ParseFeedItems(feed);
            feedView.Feeds.Add(feed);
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
