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
        public string url { get; set; }
        public FeedExt(Feed feed, string url)
        {
            this.feed = feed;
            this.url = url;
        }
    }
}
