using System.Windows;
using System.Windows.Controls;

namespace TablePet.Win.ProgressBar
{
    public partial class FeelingBar : UserControl
    {
        public FeelingBar()
        {
            InitializeComponent();
            Loaded += ProgressBar_Loaded;
        }
        
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress", typeof(double), typeof(FeelingBar),
                new PropertyMetadata(0.0, OnProgressChanged));

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FeelingBar progressBar)
            {
                progressBar.UpdateProgress();
                
            }
        }
        
        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProgress();
        }
        
        public void UpdateProgress()
        {
            double width = (Progress / 100) * Root.ActualWidth;
            ProgressFill.Width = width;
        }
    }
}