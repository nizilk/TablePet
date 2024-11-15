using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TablePet.Win.Alarms
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                return isActive ? Brushes.Black : Brushes.Gray;
            }
            return Brushes.Gray; // 默认颜色
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}