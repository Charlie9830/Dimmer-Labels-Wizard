using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

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
}
