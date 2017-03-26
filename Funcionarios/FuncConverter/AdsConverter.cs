using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Funcionarios.FuncConverter
{
    public class AdsConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int tipo = 0;
            Int32.TryParse(value.ToString(), out tipo);

            if (tipo > 1) //Indica que el titular esta adscrito a más de un organismo
            {
                return new SolidColorBrush(Colors.LightBlue);
            }
            else if (tipo == -1) //Indica que ya no quiere recibir ninguna obra
            {
                return new SolidColorBrush(Colors.LightCoral);
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