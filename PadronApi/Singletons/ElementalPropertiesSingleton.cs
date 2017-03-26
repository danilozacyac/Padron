using System;
using System.Collections.ObjectModel;
using System.Linq;
using PadronApi.Dto;
using PadronApi.Model;

namespace PadronApi.Singletons
{
    public class ElementalPropertiesSingleton
    {

         private static ObservableCollection<ElementalProperties> estatus;

         private ElementalPropertiesSingleton()
        {
        }

         public static ObservableCollection<ElementalProperties> Estatus
        {
            get
            {
                if (estatus == null)
                    estatus = new ElementalPropertiesModel().GetEstatus();

                return estatus;
            }
        }



        #region Organismos 

         private static ObservableCollection<TipoOrganismo> tipoOrganismo;
         private static ObservableCollection<ElementalProperties> distribucion;
         private static ObservableCollection<Ordinales> ordinales;
         private static ObservableCollection<Ordinales> circuitos;

         public static ObservableCollection<TipoOrganismo> TipoOrganismo
         {
             get
             {
                 if (tipoOrganismo == null)
                     tipoOrganismo = new ElementalPropertiesModel().GetTipoOrganismo();

                 return tipoOrganismo;
             }
         }


         public static ObservableCollection<ElementalProperties> Distribucion
         {
             get
             {
                 if (distribucion == null)
                     distribucion = new ElementalPropertiesModel().GetTiposDistribucion();

                 return distribucion;
             }
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


         public static ObservableCollection<Ordinales> Circuitos
         {
             get
             {
                 if (circuitos == null)
                     circuitos = new ElementalPropertiesModel().GetCircuitos();

                 return circuitos;
             }
         }

        #endregion


         private static ObservableCollection<ElementalProperties> presentacion;
         private static ObservableCollection<ElementalProperties> tipoPublicacion;
         private static ObservableCollection<ElementalProperties> medioPublicacion;
         private static ObservableCollection<ElementalProperties> idioma;

        
         public static ObservableCollection<ElementalProperties> Presentacion
         {
             get
             {
                 if (presentacion == null)
                     presentacion = new ElementalPropertiesModel().GetPresentacion();

                 return presentacion;
             }
         }

         public static ObservableCollection<ElementalProperties> TipoPublicacion
         {
             get
             {
                 if (tipoPublicacion == null)
                     tipoPublicacion = new ElementalPropertiesModel().GetTipoPublicacion();

                 return tipoPublicacion;
             }
         }

         public static ObservableCollection<ElementalProperties> MedioPublicacion
         {
             get
             {
                 if (medioPublicacion == null)
                     medioPublicacion = new ElementalPropertiesModel().GetMedioPublicacion();

                 return medioPublicacion;
             }
         }

         public static ObservableCollection<ElementalProperties> Idioma
         {
             get
             {
                 if (idioma == null)
                     idioma = new ElementalPropertiesModel().GetIdioma();

                 return idioma;
             }
         }


         private static ObservableCollection<ElementalProperties> tipoAutor;


         public static ObservableCollection<ElementalProperties> TipoAutor
         {
             get
             {
                 if (tipoAutor == null)
                     tipoAutor = new ElementalPropertiesModel().GetTipoAutor();

                 return tipoAutor;
             }
         }

    }
}
