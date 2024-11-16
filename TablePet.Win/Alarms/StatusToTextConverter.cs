using System;
using System.Globalization;
using System.Windows.Data;

namespace TablePet.Win.Alarms
{
    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool status)
            {
                return status ? "关闭" : "开启"; // 开启时显示“关闭”，关闭时显示“开启”
            }
            return "开启";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == "关闭"; // 将“关闭”转换回 true
        }
    }
}