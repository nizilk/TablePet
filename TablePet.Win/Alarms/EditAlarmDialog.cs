using System;
using System.Windows;
using TablePet.Services.Controllers;

namespace TablePet.Win.Alarms
{
    public class EditAlarmDialog : AlarmDialogBase
    {
        public EditAlarmDialog(Services.Models.Alarm alarm)
        {
            Title = "修改闹钟";
            ConfirmButton.Content = "保存";
            ConfirmButton.Click += ConfirmButton_Click;

            // 初始化属性和控件
            SelectedTime = alarm.Time;
            Status = alarm.Status;
            RepeatMode = alarm.RepeatMode;
            CustomDays = alarm.CustomDays;
            InitializeFields();
        }

        private void InitializeFields()
        {
            HourComboBox.SelectedItem = SelectedTime.Hours.ToString("D2");
            MinuteComboBox.SelectedItem = SelectedTime.Minutes.ToString("D2");
            RepeatModeComboBox.SelectedItem = RepeatMode;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (HourComboBox.SelectedItem != null && MinuteComboBox.SelectedItem != null && !string.IsNullOrEmpty((string)RepeatModeComboBox.SelectedItem))
            {
                int hour = int.Parse(HourComboBox.SelectedItem.ToString());
                int minute = int.Parse(MinuteComboBox.SelectedItem.ToString());

                SelectedTime = new TimeSpan(hour, minute, 0);
                RepeatMode = (string)RepeatModeComboBox.SelectedItem;
                Status = true;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("请填写所有信息");
            }
        }
    }
}