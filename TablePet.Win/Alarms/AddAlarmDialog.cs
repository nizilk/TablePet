using System;
using System.Windows;
using TablePet.Services.Controllers;

namespace TablePet.Win.Alarms
{
    public class AddAlarmDialog : AlarmDialogBase
    {
        public AddAlarmDialog()
        {
            Title = "添加闹钟";
            ConfirmButton.Content = "添加";
            ConfirmButton.Click += ConfirmButton_Click;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // 将新闹钟数据存储到数据库，使用 SelectedTime、IsActive、RepeatMode 和 CustomDays 属性
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