using System;
using System.Linq;
using System.Windows.Data;
using PadronApi.Singletons;

namespace PadronApi.Converter
{
    public class TituloConverter : IValueConverter
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

                    return (from n in TituloSingleton.Titulos
                            where n.IdTitulo == number
                            select n.TituloDesc).ToList()[0];
                }

                return " ";
            }
            catch (IndexOutOfRangeException)
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}