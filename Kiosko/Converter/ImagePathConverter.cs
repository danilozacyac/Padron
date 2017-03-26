using System;
using System.Configuration;
using System.Linq;
using System.Windows.Data;

namespace Kiosko.Converter
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {


            if (String.IsNullOrEmpty( value.ToString())  || String.IsNullOrWhiteSpace(value.ToString()))
            {
                //return String.Empty;
                return String.Format("{0}{1}", ConfigurationManager.AppSettings["Imagenes"], "picture.png");
            }
            else
            {
                return String.Format("{0}{1}",ConfigurationManager.AppSettings["Imagenes"], value.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
