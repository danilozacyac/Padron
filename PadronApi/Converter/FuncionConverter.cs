using System;
using System.Linq;
using System.Windows.Data;
using PadronApi.Singletons;

namespace PadronApi.Converter
{
    public class FuncionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    int number = 0;
                    int.TryParse(value.ToString(), out number);

                    if (number == 0)
                        return " ";

                    return (from n in FuncionesSingleton.Funciones
                            where n.IdElemento == number
                            select n.Descripcion).ToList()[0];
                }

                return " ";
            }
            catch (ArgumentOutOfRangeException) { return " "; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}