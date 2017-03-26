using System;
using System.Collections.ObjectModel;
using System.Linq;
using PadronApi.Dto;
using PadronApi.Model;

namespace PadronApi.Singletons
{
    public class TituloSingleton
    {
        private static ObservableCollection<Titulo> titulos;

        private TituloSingleton()
        {
        }

        public static ObservableCollection<Titulo> Titulos
        {
            get
            {
                if (titulos == null)
                    titulos = new ElementalPropertiesModel().GetTitulos();

                return titulos;
            }
        }
    }
}
