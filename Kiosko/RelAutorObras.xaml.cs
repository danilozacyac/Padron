using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Kiosko
{
    /// <summary>
    /// Interaction logic for RelAutorObras.xaml
    /// </summary>
    public partial class RelAutorObras 
    {
        ObservableCollection<Autor> listaAutores;
        ObservableCollection<Autor> listaInstituciones;

        Obra selectedObra;


        public RelAutorObras() { }

        public RelAutorObras(Obra selectedObra)
        {
            InitializeComponent();
            this.selectedObra = selectedObra;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listaAutores = new AutorModel().GetAutores();
            listaInstituciones = new AutorModel().GetInstituciones();

            this.SetAutoresInfo();
            this.SetInstitucionesInfo();
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper().Trim();

            if (!String.IsNullOrEmpty(tempString))
            {
                var temporal = (from n in listaAutores
                                where n.Nombre.ToUpper().Contains(tempString) || n.Apellidos.ToUpper().Contains(tempString) ||
                                n.NombreStr.ToUpper().Contains(tempString)
                                select n).ToList();
                ObraAutor.DataContext = temporal;
            }
            else
            {
                ObraAutor.DataContext = listaAutores;
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            List<Autor> autSelect = (from n in listaAutores
                                     where n.IsAutor == true || n.IsCompilador == true || n.IsTraductor == true ||
                                     n.IsCoordinador == true || n.IsComentarista == true || n.IsCoedicion == true ||
                                     n.IsEstudio == true || n.IsPrologo == true
                                     select n).ToList();

            List<Autor> instSelect = (from n in listaInstituciones
                                     where n.IsAutor == true || n.IsCompilador == true || n.IsTraductor == true ||
                                     n.IsCoordinador == true || n.IsComentarista == true || n.IsCoedicion == true ||
                                     n.IsEstudio == true || n.IsPrologo == true
                                     select n).ToList();

            AutorModel model = new AutorModel();
            model.DeleteAutorObra(selectedObra.IdObra);

            if (autSelect.Count > 0 || instSelect.Count > 0)
            {
                foreach (Autor autor in autSelect)
                    this.InsertaRelacionObraAutor(autor, 1);

                foreach (Autor autor in instSelect)
                    this.InsertaRelacionObraAutor(autor, 2);
            }

            DialogResult = true;
            this.Close();
        }


        private void SetAutoresInfo()
        {
            ObservableCollection<Autor> autores = new AutorModel().GetAutores(selectedObra);

            foreach (Autor autor in autores)
            {
                Autor autLista = listaAutores.SingleOrDefault(n => n.IdTitular == autor.IdTitular);

                if (autor.IsAutor)
                    autLista.IsAutor = true;
                if (autor.IsCompilador)
                    autLista.IsCompilador = true;
                if (autor.IsTraductor)
                    autLista.IsTraductor = true;
                if (autor.IsCoordinador)
                    autLista.IsCoordinador = true;
                if (autor.IsComentarista)
                    autLista.IsComentarista = true;
                if (autor.IsCoedicion)
                    autLista.IsCompilador = true;
                if (autor.IsEstudio)
                    autLista.IsEstudio = true;
                if (autor.IsPrologo)
                    autor.IsPrologo = true;
            }

            listaAutores = new ObservableCollection<Autor>(from n in listaAutores
                                                           orderby n.IsAutor descending, n.IsCompilador descending, n.IsTraductor descending, 
                                                           n.IsCoordinador descending, n.IsComentarista descending, n.IsCoedicion descending,
                                                           n.IsEstudio descending, n.IsPrologo descending, n.NombreCompleto ascending
                                                           select n);

            ObraAutor.DataContext = listaAutores;
        }

        private void SetInstitucionesInfo()
        {
            ObservableCollection<Autor> instituciones = new AutorModel().GetInstituciones(selectedObra);

            foreach (Autor institucion in instituciones)
            {
                Autor autLista = listaInstituciones.SingleOrDefault(n => n.IdTitular == institucion.IdTitular);

                if (institucion.IsAutor)
                    autLista.IsAutor = true;
                if (institucion.IsCompilador)
                    autLista.IsCompilador = true;
                if (institucion.IsTraductor)
                    autLista.IsTraductor = true;
                if (institucion.IsCoordinador)
                    autLista.IsCoordinador = true;
                if (institucion.IsComentarista)
                    autLista.IsComentarista = true;
                if (institucion.IsCoedicion)
                    autLista.IsCompilador = true;
                if (institucion.IsEstudio)
                    autLista.IsEstudio = true;
                if (institucion.IsPrologo)
                    institucion.IsPrologo = true;
            }

            listaInstituciones = new ObservableCollection<Autor>(from n in listaInstituciones
                                                           orderby n.IsAutor descending, n.IsCompilador descending, n.IsTraductor descending,
                                                           n.IsCoordinador descending, n.IsComentarista descending, n.IsCoedicion descending,
                                                           n.IsEstudio descending, n.IsPrologo descending, n.NombreCompleto ascending
                                                           select n);

            ObraInstitucion.DataContext = listaInstituciones;
        }


        private void InsertaRelacionObraAutor(Autor autor, int tipoAutor)
        {
            int idTitular, idOrganismo;
            

            if(tipoAutor == 1)
            {
                idTitular = autor.IdTitular;
                idOrganismo = 0;
            }
            else
            {
                idTitular = 0;
                idOrganismo = autor.IdTitular;
            }


            if (autor.IsAutor)
                new AutorModel().SetAutorObra(selectedObra.IdObra, idTitular, idOrganismo, 1);
            if (autor.IsCompilador)
                new AutorModel().SetAutorObra(selectedObra.IdObra, idTitular, idOrganismo, 2);
            if (autor.IsTraductor)
                new AutorModel().SetAutorObra(selectedObra.IdObra, idTitular, idOrganismo, 3);
            if (autor.IsCoordinador)
                new AutorModel().SetAutorObra(selectedObra.IdObra, idTitular, idOrganismo, 4);
            if (autor.IsComentarista)
                new AutorModel().SetAutorObra(selectedObra.IdObra, idTitular, idOrganismo, 5);
            if (autor.IsCoedicion)
                new AutorModel().SetAutorObra(selectedObra.IdObra, idTitular, idOrganismo, 6);
            if (autor.IsEstudio)
                new AutorModel().SetAutorObra(selectedObra.IdObra, idTitular, idOrganismo, 7);
            if (autor.IsPrologo)
                new AutorModel().SetAutorObra(selectedObra.IdObra, idTitular, idOrganismo, 8);


        }
    }
}
