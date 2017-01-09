using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Dimmer_Labels_Wizard_WPF
{
    class BooleanFlipValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(bool))
            {
                throw new NotSupportedException("value must be a Boolean.");
            }

            return !(bool)value;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(bool))
            {
                throw new NotSupportedException("value must be a Boolean.");
            }

            return !(bool)value;
        }
    }

    class HideZeroValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(int))
            {
                throw new NotSupportedException("This Value Converter only supports Converting to a from an integer.");
            }

            int number = (int)value;

            if (number == 0)
            {
                return string.Empty;
            }

            else
            {
                return number;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;

            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            else
            {
                return str;
            }
        }
    }

    class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = value as SolidColorBrush;

            return brush.Color;
        }
    }

    class BooleanToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return new SolidColorBrush(Colors.Green);
            }

            else
            {
                return new SolidColorBrush(Colors.Red);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityInvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return Visibility.Collapsed;
            }

            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Collapsed)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }

    public class PixelsToMillimetresConverter : IValueConverter
    {
        private const double _UnitConversionRatio = 96d / 25.4d;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var pixelValue = (double)value;

            // Don't Divide by Zero.
            if (pixelValue == 0)
            {
                return 0;
            }

            else
            {
                return pixelValue / _UnitConversionRatio;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = (string)value;
            double numericValue = double.Parse(stringValue);

            return numericValue * _UnitConversionRatio;
        }
    }
}
