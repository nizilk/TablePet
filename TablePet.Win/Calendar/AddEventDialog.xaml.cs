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

namespace TablePet.Win.Calendar
{
    /// <summary>
    /// AddEventDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddEventDialog : Window
    {
        public string EventTitle { get; private set; }
        public DateTime EventStartTime { get; private set; }
        public DateTime SelectedDate { get; private set; }

        public AddEventDialog(DateTime selectedDate)
        {
            InitializeComponent();
            SelectedDate = selectedDate;
            PopulateTimeComboBoxes();
        }

        //填充小时和分钟的下拉框
        private void PopulateTimeComboBoxes()
        {
            for (int i = 0; i < 24; i++)
            {
                HourComboBox.Items.Add(i.ToString("D2"));
            }

            for (int i = 0; i < 60; i++)
            {
                MinuteComboBox.Items.Add(i.ToString("D2"));
            }
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            if (HourComboBox.SelectedItem != null && MinuteComboBox.SelectedItem != null && !string.IsNullOrEmpty(EventTitleTextBox.Text))
            {
                int hour = int.Parse(HourComboBox.SelectedItem.ToString());
                int minute = int.Parse(MinuteComboBox.SelectedItem.ToString());

                EventStartTime = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, hour, minute, 0);
                EventTitle = EventTitleTextBox.Text;

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }
    }
}
