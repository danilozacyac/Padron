using System;
using System.Linq;
using System.Windows.Data;
using PadronApi.Singletons;

namespace PadronApi.Converter
{
    public class CiudadesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    int number = 0;
                    int.TryParse(value.ToString(), out number);



                    return (from n in PaisesSingleton.Ciudades
                            where n.IdCiudad == number
                            select n.CiudadDesc).ToList()[0];
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
