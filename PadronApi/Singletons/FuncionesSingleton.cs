using System;
using System.Collections.ObjectModel;
using System.Linq;
using PadronApi.Dto;
using PadronApi.Model;

namespace PadronApi.Singletons
{
    public class FuncionesSingleton
    {
        private static ObservableCollection<ElementalProperties> funciones;

        private FuncionesSingleton()
        {
        }

        public static ObservableCollection<ElementalProperties> Funciones
        {
            get
            {
                if (funciones == null)
                    funciones = new ElementalPropertiesModel().GetFunciones();

                return funciones;
            }
        }
    }
}
