using System;
using System.Globalization;
using System.Windows.Data;

namespace RxSandbox
{
    public class MethodCallCommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value == null) || (parameter == null))
                return null;
            return new MethodCallCommand(value, parameter.ToString());
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}