
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace TablePet.Win.Start
{
    public partial class EditWindow : Window
    {
        private ObservableCollection<MainWindow.LaunchItem> LaunchItems;
        private readonly Action SaveLaunchItems;

        public EditWindow(ObservableCollection<MainWindow.LaunchItem> launchItems, Action saveLaunchItems)
        {
            InitializeComponent();
            LaunchItems = launchItems;
            SaveLaunchItems = saveLaunchItems;
            LaunchItemList.ItemsSource = LaunchItems;
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                LaunchItems.Add(new MainWindow.LaunchItem { Name = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName), Path = dialog.FileName });
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (LaunchItemList.SelectedItem is MainWindow.LaunchItem selectedItem)
            {
                LaunchItems.Remove(selectedItem);
            }
        }

        private void SaveItems_Click(object sender, RoutedEventArgs e)
        {
            SaveLaunchItems();
            MessageBox.Show("启动项已保存！");
            this.Close();
        }
    }
}