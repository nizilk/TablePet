using CodeHollow.FeedReader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TablePet.Services.Models
{
    public class FeedExt
    {
        public int ID { get; set; }

        public Feed Feed { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsFolder { get; set; }

        public int FolderID { get; set; }

        public ObservableCollection<FeedExt> Nodes { get; set; } = new ObservableCollection<FeedExt>();

        public FeedExt(int ID=-1, Feed Feed=null, string Title=null, string Url=null, int FolderID=0, bool IsFolder=false)
        {
            this.ID = ID;
            this.Feed = Feed;
            this.Title = Title;
            this.Url = Url;
            this.FolderID = FolderID;
            this.IsFolder = IsFolder;
            if (Feed != null && Title == null)
            {
                this.Title = Feed.Title;
            }
        }
    }
}
