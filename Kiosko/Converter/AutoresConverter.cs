using PadronApi.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace Kiosko.Converter
{
    public class AutoresConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                ObservableCollection<Autor> autores = value as ObservableCollection<Autor>;

                List<string> nombres = new List<string>();

                foreach (Autor autor in autores)
                    nombres.Add(autor.NombreCompleto);

                return string.Join(", ",nombres);
            }
            else
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
