using CodeHollow.FeedReader;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TablePet.Services.Controllers;

namespace TablePet.Services.Models
{
    public class FeedItemExt: INotifyPropertyChanged
    {
        public FeedItem FeedItem { get; set; }

        public string FeedTitle { get; set; }

        private bool isstarred;
        public bool IsStarred
        {
            get
            {
                return isstarred;
            }
            set
            {
                isstarred = value;
                OnPropertyChanged(nameof(IsStarred));
            }
        }

        public FeedExt Parent { get; set; }

        public FeedReaderService feedReaderService { get; set; }

        public ICommand StarCommand
        {
            get
            {
                return new RelayCommand(StarClick, (o) => true);
            }
        }

        private void StarClick(object parameter)
        {
            IsStarred = !IsStarred;
            if (IsStarred)
                feedReaderService.StarItems.Add(this);
            else
                feedReaderService.StarItems.Remove(this);
        }

        public ICommand DelCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    Parent.Items.Remove(this);
                });
            }
        }

        public ICommand ShareCommand
        {
            get
            {
                return new RelayCommand((o) =>
                {
                    var idx = (Int32)o;
                    string url = "";
                    string cnt = "我的分享：\r\n" + FeedItem.Title + "\r\n" + FeedItem.Link;
                    switch (idx)
                    {
                        case 0:
                            url = "http://service.weibo.com/share/share.php?url=" + cnt;
                            System.Diagnostics.Process.Start(url);
                            break;
                        case 1:
                            url = "https://twitter.com/intent/tweet?text=" + cnt;
                            System.Diagnostics.Process.Start(url);
                            break;
                        default:
                            break;
                    }
                });
            }
        }

        public FeedItemExt(FeedItem feedItem, string feedTitle, FeedExt parent=null, FeedReaderService service=null)
        {            
            FeedItem = feedItem;
            FeedTitle = feedTitle;
            IsStarred= false;
            Parent = parent;
            feedReaderService = service;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
