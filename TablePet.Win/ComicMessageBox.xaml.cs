using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TablePet.Win.Messagebox
{
    public partial class ComicMessageBox : UserControl
    {
         private DispatcherTimer timer;

        public ComicMessageBox()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3); 
            timer.Tick += Timer_Tick;
        }
        
        public void ShowMessage(string message)
        {
            MessageText.Text = message;
            this.Visibility = Visibility.Visible;
            timer.Start(); 
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            timer.Stop(); 
        }
    }
}