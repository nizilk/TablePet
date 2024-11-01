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

namespace TablePet.Win.FeedReader
{
    /// <summary>
    /// Temp.xaml 的交互逻辑
    /// </summary>
    public partial class Temp : Window
    {
        private Uri geoSource = new Uri("pack://application:,,,/Resources/IconDictionary.xaml");
        public Temp()
        {
            InitializeComponent();
            var appResourceDictionary = ((App)Application.Current).Resources;
            var mergedDictionaries = appResourceDictionary.MergedDictionaries;
            /*
            var specifiedDictionary = mergedDictionaries.FirstOrDefault(dictionary => dictionary.Source == sourceUri);
            if (specifiedDictionary != null)
            {
                var sortedList = ToSortedList(specifiedDictionary.Keys);
                foreach (string key in sortedList.Keys)
                {
                    if (specifiedDictionary[key] is Geometry geometry)
                    {
                        this.AllItems.Add(new CListItem<Geometry>(key, geometry));
                    }
                }
            }
            */
            //rtb_contentEntry.AppendText("测试文本：\r\nSummary：两个人都正在忙碌期但今天是情人节。\r\n\r\n摸鱼漫画，很潦草！");
        }

        private void bt_starEntry_Click(object sender, RoutedEventArgs e)
        {
            /*
            mergedDictionaries[""]
            bt_starEntry.Tag = Convert.ToInt32(bt_starEntry.Tag)^1;
            if (bt_starEntry.Tag.ToString() == "1")
            {
                bt_starEntry_Path.Data = new Geometry "{DynamicResource StarGeometry_Already}";
                bt_starEntry_Path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FC9D2F"));
            }
            else if (bt_starEntry.Tag.ToString() == "0")
            {
                bt_starEntry_Path.Data = "{DynamicResource StarGeometry}";
                bt_starEntry_Path.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999"));
            }
            */
        }
    }
}
