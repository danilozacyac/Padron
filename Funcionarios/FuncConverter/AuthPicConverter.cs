using System;
using System.Configuration;
using System.Linq;
using System.Windows.Data;

namespace Funcionarios.FuncConverter
{
    public class AuthPicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isAutor = (bool)value;

            if (isAutor)
            {
                //return String.Empty;
                return String.Format("{0}{1}", ConfigurationManager.AppSettings["Imagenes"], "colorBooks_128.png");
            }
            else
            {
                return String.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
