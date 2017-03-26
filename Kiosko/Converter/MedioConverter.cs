using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Kiosko.Converter
{
    public class MedioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int medio = value as int? ?? 0;

            switch (medio)
            {
                case 1: return new SolidColorBrush(Colors.LightBlue);  // CD
                case 2: return new SolidColorBrush(Colors.LightCyan);  // DVD
                case 3: return new SolidColorBrush(Colors.LightGreen); //Libro
                case 4: return new SolidColorBrush(Colors.LightPink);  //Ebook
                case 5: return new SolidColorBrush(Colors.Yellow);     //VHS
                case 6: return new SolidColorBrush(Colors.Beige);      // USB
                case 7: return new SolidColorBrush(Colors.Coral);      //Libro y disco
                case 8: return new SolidColorBrush(Colors.Gainsboro);  //Audiolibro
                default: return new SolidColorBrush(Colors.White);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
