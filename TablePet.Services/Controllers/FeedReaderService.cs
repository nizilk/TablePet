using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TablePet.Services.Controllers
{
    public class FeedReaderService
    {
        public FeedReaderService() { }

        public ObservableCollection<Feed> Feeds { get; set; } = new ObservableCollection<Feed>();

        public ObservableCollection<FeedItem> Items { get; set; } = new ObservableCollection<FeedItem>();


        public List<string> FindFeed(string url = "https://youkilee.top/")
        {
            List<string> feedUrl = new List<string>();
            try
            {
                var urlsTask = FeedReader.GetFeedUrlsFromUrlAsync(url);
                var urls = urlsTask.Result;

                if (urls == null || urls.Count() < 1)
                    feedUrl.Add(url);
                else if (urls.Count() == 1 || urls.Count() == 2)    // if 2 urls, then its usually a feed and a comments feed, so take the first per default
                    feedUrl.Add(urls.First().Url);
                else
                {
                    foreach (var u in urls)
                    {
                        feedUrl.Add(u.Url);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.InnerException.Message}{Environment.NewLine}{ex.InnerException}");
            }

            return feedUrl;
        }


        public Feed UpdateFeed(string url = "https://youkilee.top/index.php/feed/")
        {
            var readerTask = FeedReader.ReadAsync(url);
            readerTask.ConfigureAwait(false);
            Feed feed = readerTask.Result;
            Feeds.Add(feed);
            return feed;
        }


        public void ParseFeedItems(Feed feed)
        {
            foreach (var item in feed.Items)
            {
                
                DateTime pd = (DateTime)item.PublishingDate;
                item.PublishingDateString = pd.ToString("ddd MMM dd yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-us"));
                item.PublishingDateString += " " + GetTimeSpanTilNow(pd);   // Sat Aug 10 2024 23:59:00 (2 months)
                item.Content = FiltContent(item.Content);
                Items.Add(item);
            }
        }


        public string GetTimeSpanTilNow(DateTime dt)
        {
            DateTime now = DateTime.Now;
            if (now.Year != dt.Year)
                return "(" + (now.Year - dt.Year).ToString() + " years)";
            else if (now.Month != dt.Month)
                return "(" + (now.Month - dt.Month).ToString() + " months)";
            else if (now.Day != dt.Day)
                return "(" + (now.Day - dt.Day).ToString() + " days)";
            else if (now.Hour != dt.Hour)
                return "(" + (now.Hour - dt.Hour).ToString() + " hours)";
            else if (now.Minute != dt.Minute)
                return "(" + (now.Minute - dt.Minute).ToString() + " minutes)";
            else
                return "(" + (now.Second - dt.Second).ToString() + " seconds)";
        }

        public string FiltContent(string content)
        {
            content = "<FlowDocument LineHeight=\"10\" FontSize=\"14\" " +
                "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
                "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">" + 
                content + 
                "</FlowDocument>";

            content = content.Replace("<p>", "<Paragraph>");
            content = content.Replace("</p>", "</Paragraph>");

            content = content.Replace("&", "&amp;");
            // content = content.Replace(">", "&gt;");
            // content = content.Replace("<", "&lt;");
            // content = content.Replace("\"", "&quot;");
            // content = content.Replace("\'", "&apos;");

            content = content.Replace("</a>", "</Hyperlink>");
            string pattern_href = @"<a href=""([^"">]*)""[^>]*>";
            content = Regex.Replace(content, pattern_href, "<Hyperlink Foreground=\"Blue\" NavigateUri=\"${1}\">");

            string pattern_head = @"<h2[^>]*>(.*)</h2>";
            content = Regex.Replace(content, pattern_head, "<Paragraph LineHeight=\"20\" FontSize=\"18\"><Bold>${1}</Bold></Paragraph>");

            string pattern_hr = @"<hr[^>]*>";
            content = Regex.Replace(content, pattern_hr, "<Paragraph><Separator Width=\"500\" BorderBrush=\"Gray\" BorderThickness=\"1\" SnapsToDevicePixels=\"True\" HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Center\"/></Paragraph>");

            string pattern_ent = @"(<br>|</br>|<br/>|<br />)";
            content = Regex.Replace(content, pattern_ent, "\r\n");

            string pattern_blk = @"(<div[^>]*>(.|\n)*</div>|<p>|</p>)";
            content = Regex.Replace(content, pattern_blk, "");
            return content;
        }
    }
}
