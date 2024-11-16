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
    /// Temp.xaml 的交互逻辑
    /// </summary>
    public partial class AddFolder : Window
    {
        private FeedReaderService feedReaderService;

        public AddFolder()
        {
            InitializeComponent();
        }

        public AddFolder(FeedReaderService service)
        {
            InitializeComponent();
            this.feedReaderService = service;
        }

        private void bt_folderOK_Click(object sender, RoutedEventArgs e)
        {
            string name = tb_FolderName.Text;
            if (name is null || name == "") return;
            feedReaderService.AddFolder(name);
            this.Close();
        }

        private void bt_folderCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
