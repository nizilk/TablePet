using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using TablePet.Services.Controllers;
using System.Configuration;
using System.Linq;
using System.Windows.Threading;
using Notifications.Wpf;
using TablePet.Win.Alarms;
using TablePet.Services.Models;


namespace TablePet.Win.Alarm
{
    public partial class AlarmsWindow : Window
    {
        private AlarmService alarmService;

        public AlarmsWindow()
        {
            InitializeComponent();
            alarmService = new AlarmService();
            LoadAlarms();
        }
        
        public void LoadAlarms()
        {
            var alarms = alarmService.GetAllAlarms();
            AlarmList.ItemsSource = alarms;
        }


        private void AddAlarm_Click(object sender, RoutedEventArgs e)
        {
            var addDialog = new AddAlarmDialog();
            if (addDialog.ShowDialog() == true)
            {
                var newAlarm = new Services.Models.Alarm
                {
                    Time = addDialog.SelectedTime,
                    Status = addDialog.Status,
                    RepeatMode = addDialog.RepeatMode,
                    CustomDays = addDialog.CustomDays
                };
                
                alarmService.AddAlarm(newAlarm);
                LoadAlarms();
            }
        }

        private void EditAlarm_Click(object sender, RoutedEventArgs e)
        {
            var alarm = (sender as FrameworkElement).DataContext as Services.Models.Alarm;
            var editDialog = new EditAlarmDialog(alarm);
            if (editDialog.ShowDialog() == true)
            {
                alarm.Time = editDialog.SelectedTime;
                alarm.Status = editDialog.Status;
                alarm.CustomDays = editDialog.CustomDays;
                alarm.RepeatMode = editDialog.RepeatMode;
                
                alarmService.UpdateAlarm(alarm);
                LoadAlarms();
            }
        }

        private void DeleteAlarm_Click(object sender, RoutedEventArgs e)
        {
            var alarm = (sender as FrameworkElement).DataContext as Services.Models.Alarm;
            alarmService.DeleteAlarm(alarm.Id);
            LoadAlarms();
        }

        private void ToggleStatus(object sender, RoutedEventArgs e)
        {
            var alarm = (sender as FrameworkElement).DataContext as Services.Models.Alarm;
            alarm.Status = (sender as ToggleButton).IsChecked == true;
            alarmService.UpdateAlarm(alarm);
            LoadAlarms();
        }
    }
}