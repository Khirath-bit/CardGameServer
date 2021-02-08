using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CardGame.Extensions
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Brushes.Red;

            return (bool)value
                ? Brushes.GreenYellow
                : Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();

            return null;
        }
    }
}
