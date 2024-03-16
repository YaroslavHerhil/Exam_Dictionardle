using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Exam_Dictionardle
{
    public class MilisecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is int))
                return null;

            int milliseconds = (int)value;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            return timeSpan.ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //not needed
            throw new NotImplementedException();
        }
    }
}
