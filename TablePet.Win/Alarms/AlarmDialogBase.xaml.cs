using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TablePet.Win.Alarms
{
    public partial class AlarmDialogBase : Window
    {
        public TimeSpan SelectedTime { get; set; }
        public bool Status { get; set; }
        public string RepeatMode { get; set; } 
        public List<DayOfWeek> CustomDays { get; set; } 

        public AlarmDialogBase()
        {
            InitializeComponent();
            CustomDays = new List<DayOfWeek>();

            // Initialize the RepeatModeComboBox with options
            RepeatModeComboBox.ItemsSource = new List<string> { "仅一次", "每天", "自定义" };
            RepeatModeComboBox.SelectionChanged += RepeatModeComboBox_SelectionChanged;
            PopulateTimeComboBoxes();
        }
        
        
        
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

        private void RepeatModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RepeatModeComboBox.SelectedItem as string == "自定义")
            {
                OpenCustomDaysSelection();
            }
        }

        private void OpenCustomDaysSelection()
        {
            /*// Open the custom days selection dialog
            var customDaysDialog = new CustomDaysDialog(CustomDays);
            if (customDaysDialog.ShowDialog() == true)
            {
                CustomDays = customDaysDialog.SelectedDays;
            }*/
        }

        

        protected void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}