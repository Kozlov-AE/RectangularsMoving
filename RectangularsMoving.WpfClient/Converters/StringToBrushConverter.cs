using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RectangularsMoving.WpfClient.Converters {
    public class StringToBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is string str) {
                return (SolidColorBrush)new BrushConverter().ConvertFrom(str);
            }

            return Brushes.DeepPink;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}