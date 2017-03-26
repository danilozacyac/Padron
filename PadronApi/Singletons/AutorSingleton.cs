using PadronApi.Dto;
using PadronApi.Model;
using System.Collections.ObjectModel;

namespace PadronApi.Singletons
{
    public class AutorSingleton
    {

        private static ObservableCollection<Autor> autores;
        private static ObservableCollection<Autor> instituciones;


        public static ObservableCollection<Autor> Presentacion
        {
            get
            {
                if (autores == null)
                    autores = new AutorModel().GetAutores();

                CleanAutorSelection();

                return autores;
            }
        }

        public static ObservableCollection<Autor> TipoPublicacion
        {
            get
            {
                if (instituciones == null)
                    instituciones = new AutorModel().GetInstituciones();

                CleanInstitucionesSelection();

                return instituciones;
            }
        }

        private static void CleanAutorSelection(){

            foreach (Autor autor in autores)
            {
                autor.IsAutor = false;
                autor.IsCompilador = false;
                autor.IsTraductor = false;
                autor.IsCoordinador = false;
                autor.IsComentarista = false;
                autor.IsCoedicion = false;
                autor.IsEstudio = false;
                autor.IsPrologo = false;
            }
        }

        private static void CleanInstitucionesSelection()
        {

            foreach (Autor autor in instituciones)
            {
                autor.IsAutor = false;
                autor.IsCompilador = false;
                autor.IsTraductor = false;
                autor.IsCoordinador = false;
                autor.IsComentarista = false;
                autor.IsCoedicion = false;
                autor.IsEstudio = false;
                autor.IsPrologo = false;
            }
        }

    }
}
