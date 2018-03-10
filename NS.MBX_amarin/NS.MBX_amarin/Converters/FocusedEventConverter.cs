using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace NS.MBX_amarin.Converters
{
    public class FocusedEventConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as FocusEventArgs;
            if (eventArgs == null)
            {
                throw new ArgumentException("Expected value to be of type ItemTappedEventArgs", nameof(value));
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
