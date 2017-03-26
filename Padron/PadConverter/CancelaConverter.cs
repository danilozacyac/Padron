using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Padron.PadConverter
{
    public class CancelaConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int cancelado = 0;
            Int32.TryParse(value.ToString(), out cancelado);

            if (cancelado == 1)
            {
                return new SolidColorBrush(Colors.Orange);
            }
            else
                return new SolidColorBrush(Colors.White);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}