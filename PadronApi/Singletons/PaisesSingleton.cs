using System;
using System.Collections.ObjectModel;
using System.Linq;
using PadronApi.Dto;
using PadronApi.Model;

namespace PadronApi.Singletons
{
    public class PaisesSingleton
    {
         private static ObservableCollection<Pais> paises;
         private static ObservableCollection<Estado> estados;
         private static ObservableCollection<Ciudad> ciudades;

         private PaisesSingleton()
        {
        }

         public static ObservableCollection<Pais> Paises
        {
            get
            {
                if (paises == null)
                    paises = new PaisEstadoModel().GetPaises();

                return paises;
            }
        }

         public static ObservableCollection<Estado> Estados
         {
             get
             {
                 if (estados == null)
                     estados = new PaisEstadoModel().GetEstados();

                 return estados;
             }
         }

         public static ObservableCollection<Ciudad> Ciudades
         {
             get
             {
                 if (ciudades == null)
                     ciudades = new PaisEstadoModel().GetCiudades();

                 return ciudades;
             }
         }
    }
}
