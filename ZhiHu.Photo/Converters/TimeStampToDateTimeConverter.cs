using System;
using System.Globalization;
using System.Windows.Data;

namespace ZhiHu.Photo.Converters
{
    public class TimeStampToDateTimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;
            if (value is int n)
            {
                var timeSpan = new TimeSpan(n*10000000L);
                var dateTime = new DateTime(1970,1,1);
                dateTime = dateTime.Add(timeSpan);
                result = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒");
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
