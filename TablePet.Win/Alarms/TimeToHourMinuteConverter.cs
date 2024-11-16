using System;
using System.Globalization;
using System.Windows.Data;

namespace TablePet.Win.Alarms
{
    public class TimeToHourMinuteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan time)
            {
                return $"{time.Hours:D2}:{time.Minutes:D2}"; // 格式化为小时:分钟
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}