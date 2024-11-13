using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePet.Services.Models
{
    public class FeedExt
    {
        public Feed feed { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public bool isFolder { get; set; }
        public FeedExt(Feed feed, string title, string url, bool isFolder=false)
        {
            this.feed = feed;
            this.title = title;
            this.url = url;
            this.isFolder = isFolder;
        }
    }
}
