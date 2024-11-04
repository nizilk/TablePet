using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TablePet.Win.Messagebox
{
    public partial class ComicMessageBox : UserControl
    {
        private DispatcherTimer timer;
        private string fullMessage; 
        private int displayIndex = 0; 
        private int maxCharsPerDisplay = 65; // 最大字符数
        private double baseHeight = 80; 

        public ComicMessageBox()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
        }
        
        public void ShowMessage(string message)
        {
            fullMessage = message;
            displayIndex = 0;
            SetDisplayText();
            AdjustHeight();
            SetFontSize();
            this.Visibility = Visibility.Visible;
            timer.Start();
        }
        
        private void SetDisplayText()
        {
            int start = displayIndex * maxCharsPerDisplay;
            int length = Math.Min(maxCharsPerDisplay, fullMessage.Length - start);
            MessageText.Text = fullMessage.Substring(start, length);
        }

        private void AdjustHeight()
        {
            if (fullMessage.Length <= maxCharsPerDisplay)
            {
                this.Height = baseHeight + fullMessage.Length * 0.5; 
            }
            else
            {
                this.Height = MaxHeight; 
            }
        }

        private void SetFontSize()
        {
            MessageText.FontSize = fullMessage.Length > maxCharsPerDisplay ? 14 : 16;
        }
        
        private void Timer_Tick(object sender, EventArgs e)
        {
            displayIndex++;
            if (displayIndex * maxCharsPerDisplay < fullMessage.Length)
            {
                SetDisplayText();
                AdjustHeight();
                timer.Start(); 
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                timer.Stop();
            }
        }
    }
}
