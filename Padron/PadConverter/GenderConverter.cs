using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Padron.PadConverter
{
    public class GenderConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int genero = 0;
            Int32.TryParse(value.ToString(), out genero);

            if (genero == 1)
            {
                return "pack://application:,,,/Padron;component/Resources/titular.png";
            }
            else
                return "pack://application:,,,/Padron;component/Resources/female_128.png";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}