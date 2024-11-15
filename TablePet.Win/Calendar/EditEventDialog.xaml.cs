using System;
using System.Windows;
using TablePet.Services.Models;

namespace TablePet.Win.Calendar
{
    public partial class EditEventDialog : Window
    {
        public string EventTitle { get; private set; }
        public DateTime EventStartTime { get; private set; }

        public EditEventDialog(CalendarEvent calendarEvent)
        {
            InitializeComponent();

            // 设置初始事件数据
            EventTitle = calendarEvent.Description;
            EventStartTime = calendarEvent.StartTime;

            // 初始化控件数据
            EventTitleTextBox.Text = EventTitle;
            InitializeTimePickers(EventStartTime);
        }

        private void InitializeTimePickers(DateTime eventTime)
        {
            // 填充小时和分钟选择框
            for (int hour = 0; hour < 24; hour++)
                HourComboBox.Items.Add(hour.ToString("D2"));

            for (int minute = 0; minute < 60; minute++)
                MinuteComboBox.Items.Add(minute.ToString("D2"));

            // 设置初始值
            HourComboBox.SelectedItem = eventTime.Hour.ToString("D2");
            MinuteComboBox.SelectedItem = eventTime.Minute.ToString("D2");
        }

        private void SaveEvent_Click(object sender, RoutedEventArgs e)
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(EventTitleTextBox.Text))
            {
                MessageBox.Show("事件描述不能为空！");
                return;
            }

            if (HourComboBox.SelectedItem == null || MinuteComboBox.SelectedItem == null)
            {
                MessageBox.Show("请选择完整的时间！");
                return;
            }

            // 获取修改后的描述和时间
            EventTitle = EventTitleTextBox.Text;
            int selectedHour = int.Parse(HourComboBox.SelectedItem.ToString());
            int selectedMinute = int.Parse(MinuteComboBox.SelectedItem.ToString());
            EventStartTime = new DateTime(EventStartTime.Year, EventStartTime.Month, EventStartTime.Day, selectedHour, selectedMinute, 0);

            DialogResult = true;
            Close();
        }
    }
}
