using System;
using System.Collections.ObjectModel;
using System.Linq;
using PadronApi.Dto;
using PadronApi.Model;

namespace PadronApi.Singletons
{
    public class OrdinalSingleton
    {
        private static ObservableCollection<Ordinales> ordinales;

        private OrdinalSingleton()
        {
        }

        public static ObservableCollection<Ordinales> Ordinales
        {
            get
            {
                if (ordinales == null)
                    ordinales = new ElementalPropertiesModel().GetOrdinales();

                return ordinales;
            }
        }
    }
}