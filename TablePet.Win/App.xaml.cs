using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static System.Windows.Forms.LinkLabel;

namespace TablePet.Win
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void hyperlink_Title_Click(object sender, RoutedEventArgs e)
        {
            var hp = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(hp.NavigateUri.AbsoluteUri));
        }

        private void lb_num_Loaded(object sender, RoutedEventArgs e)
        {
            Label lb = sender as Label;
        }
    }
}
