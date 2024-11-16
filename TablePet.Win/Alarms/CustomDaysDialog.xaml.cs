using System;
using System.Collections.Generic;
using System.Windows;

namespace TablePet.Win.Alarms
{
    public partial class CustomDaysDialog : Window
    {
        public List<DayOfWeek> SelectedDays { get; private set; } = new List<DayOfWeek>();

        public CustomDaysDialog(List<DayOfWeek> _SelectedDays)
        {
            InitializeComponent();
            InitializeFields(_SelectedDays);
        }
        
        private void InitializeFields(List<DayOfWeek> _SelectedDays)
        {
            if (_SelectedDays.Contains(DayOfWeek.Monday)) MondayCheckbox.IsChecked = true;
            if (_SelectedDays.Contains(DayOfWeek.Tuesday)) TuesdayCheckbox.IsChecked = true;
            if (_SelectedDays.Contains(DayOfWeek.Wednesday)) WednesdayCheckbox.IsChecked = true;
            if (_SelectedDays.Contains(DayOfWeek.Thursday)) ThursdayCheckbox.IsChecked = true;
            if (_SelectedDays.Contains(DayOfWeek.Friday)) FridayCheckbox.IsChecked = true;
            if (_SelectedDays.Contains(DayOfWeek.Saturday)) SaturdayCheckbox.IsChecked = true;
            if (_SelectedDays.Contains(DayOfWeek.Sunday)) SundayCheckbox.IsChecked = true;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (MondayCheckbox.IsChecked == true) SelectedDays.Add(DayOfWeek.Monday);
            if (TuesdayCheckbox.IsChecked == true) SelectedDays.Add(DayOfWeek.Tuesday);
            if (WednesdayCheckbox.IsChecked == true) SelectedDays.Add(DayOfWeek.Wednesday);
            if (ThursdayCheckbox.IsChecked == true) SelectedDays.Add(DayOfWeek.Thursday);
            if (FridayCheckbox.IsChecked == true) SelectedDays.Add(DayOfWeek.Friday);
            if (SaturdayCheckbox.IsChecked == true) SelectedDays.Add(DayOfWeek.Saturday);
            if (SundayCheckbox.IsChecked == true) SelectedDays.Add(DayOfWeek.Sunday);

            DialogResult = true;
            Close();
        }
    }
}