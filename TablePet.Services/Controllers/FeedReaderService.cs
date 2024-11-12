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

        // public ObservableCollection<Feed> Feeds { get; set; } = new ObservableCollection<Feed>();

        // public ObservableCollection<FeedItem> Items { get; set; } = new ObservableCollection<FeedItem>();


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


        public Feed ReadFeed(string url = "https://youkilee.top/index.php/feed/")
        {
            if (url == "" || url is null)   return null;
            var readerTask = FeedReader.ReadAsync(url);
            readerTask.ConfigureAwait(false);
            Feed feed = readerTask.Result;

            foreach (var item in feed.Items)
            {
                if (item.Author == null)
                {
                    item.Author = GetCreator(feed);
                }
                if (item.PublishingDate != null)
                {
                    DateTime pd = (DateTime)item.PublishingDate;
                    item.PublishingDateString = pd.ToString("ddd MMM dd yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-us"));
                    item.PublishingDateString += " (" + GetTimeSpanTilNow(pd) + ")";   // Sat Aug 10 2024 23:59:00 (2 months)
                }
                if (item.Content == null)
                {
                    item.Content = item.Description;
                }
                item.Content = FiltContent(item.Content);
            }
            return feed;
        }


        public string GetCreator(Feed feed)
        {
            string origDoc = feed.OriginalDocument;
            string pattern = @"<dc:creator><!\[CDATA\[([^\]]+)\]\]></dc:creator>";
            string creator = Regex.Match(origDoc, pattern).Groups[1].Value;
            return creator;
        }


        public string GetTimeSpanTilNow(DateTime dt)
        {
            DateTime now = DateTime.Now;
            if (now.Year != dt.Year)
                return (now.Year - dt.Year).ToString() + " years";
            else if (now.Month != dt.Month)
                return (now.Month - dt.Month).ToString() + " months";
            else if (now.Day != dt.Day)
                return (now.Day - dt.Day).ToString() + " days";
            else if (now.Hour != dt.Hour)
                return (now.Hour - dt.Hour).ToString() + " hours";
            else if (now.Minute != dt.Minute)
                return (now.Minute - dt.Minute).ToString() + " minutes";
            else
                return (now.Second - dt.Second).ToString() + " seconds";
        }

        public string FiltContent(string content)
        {
            content = "<FlowDocument LineHeight=\"10\" FontSize=\"14\" " +
                "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
                "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"> " +
                "<Paragraph> " +
                content +
                "</Paragraph> " +
                "</FlowDocument>";

            content = content.Replace("<p>", "<Paragraph>");
            content = content.Replace("</p>", "</Paragraph>");

            content = content.Replace("&", "&amp;");
            // content = content.Replace(">", "&gt;");
            // content = content.Replace("<", "&lt;");
            // content = content.Replace("\"", "&quot;");
            // content = content.Replace("\'", "&apos;");

            content = content.Replace("</a>", "</Hyperlink>");
            string pattern_href = @"<a[^>]*href=""([^"">]*)""[^>]*>";
            content = Regex.Replace(content, pattern_href, "<Hyperlink Foreground=\"Blue\" NavigateUri=\"${1}\">");

            string pattern_head = @"<h2[^>]*>(.*)</h2>";
            content = Regex.Replace(content, pattern_head, "<Paragraph FontSize=\"18\"><Bold>${1}</Bold></Paragraph>");     // LineHeight=\"20\"

            string pattern_hr = @"<hr[^>]*>";
            content = Regex.Replace(content, pattern_hr, "<Paragraph><Separator Width=\"500\" BorderBrush=\"Gray\" BorderThickness=\"1\" SnapsToDevicePixels=\"True\" HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Center\"/></Paragraph>");

            string pattern_ent = @"(<br>|</br>|<br/>|<br />)";
            content = Regex.Replace(content, pattern_ent, "</Paragraph> <Paragraph>");

            string pattern_blk = @"<div[^>]*>((.|\n)*?)</div>";
            content = Regex.Replace(content, pattern_blk, "<Section><Paragraph>${1}</Paragraph></Section>");

            /*
            string pattern_divc = @"<div class=""([^""]+)"">(.|\n)*</div>";
            string cls;
            MatchCollection collection = Regex.Matches(content, pattern_divc);
            foreach (Match match in collection)
            {
                cls = match.Groups[1].Value;
            }
            */

            string pattern_fgr = @"<figure[^>]*>(.*?)</figure>";
            content = Regex.Replace(content, pattern_fgr, "${1}");

            string pattern_img = @"<img[^>]*src=""([^""]+)""[^>]*>";
            content = Regex.Replace(content, pattern_img, "<Image Source=\"${1}\"/>");

            string pattern_multi_p = @"<Paragraph>(\s*<Paragraph>)+";
            content = Regex.Replace(content, pattern_multi_p, "<Paragraph>");

            string pattern_multi_pend = @"</Paragraph>(\s*</Paragraph>)+";
            content = Regex.Replace(content, pattern_multi_pend, "</Paragraph>");

            return content;
        }
    }
}
