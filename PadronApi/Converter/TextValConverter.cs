using System;
using System.Linq;
using System.Windows.Data;
using ScjnUtilities;

namespace PadronApi.Converter
{
    public class TextValConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string
                theString = (string)value;
                theString = theString.Trim();

                return VerificationUtilities.TextBoxStringValidation(theString);
            }
            return String.Empty;

        }

        public object
        ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}