using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Organismos.OrgConverter
{
    public class AdscritosConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int tipo = 0;
            Int32.TryParse(value.ToString(), out tipo);

            if (tipo == 0)
            {
                return new SolidColorBrush(Colors.LightGreen);
            }
            else if (tipo == 10000)
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