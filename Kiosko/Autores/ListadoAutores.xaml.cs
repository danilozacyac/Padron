using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Kiosko.ReportesKiosko;
using PadronApi.Dto;
using PadronApi.Model;

namespace Kiosko.Autores
{
    /// <summary>
    /// Interaction logic for ListadoAutores.xaml
    /// </summary>
    public partial class ListadoAutores : UserControl
    {
        ObservableCollection<Autor> catalogoTitulares;
        ObservableCollection<Autor> catalogoInstituciones;
        //ObservableCollection<Obra> obrasInstitucionSelect;
        Autor selectedTitular;
        Autor selectedOrganismo;



        int tipoProceso = 1;

        int tipoAutor = 1;

        public ListadoAutores()
        {
            InitializeComponent();
            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tipoProceso = 1;
            this.BusyIndicator.BusyContent = "Cargando titulares...";
            this.LaunchBusyIndicator();


        }


        private void RlstAutores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTitular = RlstAutores.SelectedItem as Autor;

            if (selectedTitular != null)
            {
                tipoProceso = 2;
                this.BusyIndicator.BusyContent = "Cargando obras...";
                this.LaunchBusyIndicator();
            }
            else
                GObras.DataContext = null;

            tipoAutor = 1;
        }

        private void RlstInstituciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedOrganismo = RlstInstituciones.SelectedItem as Autor;

            if (selectedOrganismo != null)
            {
                tipoProceso = 3;
                this.BusyIndicator.BusyContent = "Cargando obras...";
                this.LaunchBusyIndicator();
            }
            else
                GObras.DataContext = null;

            tipoAutor = 2;
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper().Trim();

            if (!String.IsNullOrEmpty(tempString))
            {
                var temporal = (from n in catalogoTitulares
                                where n.Nombre.ToUpper().Contains(tempString) || n.Apellidos.ToUpper().Contains(tempString) ||
                                n.NombreStr.ToUpper().Contains(tempString)
                                select n).ToList();
                RlstAutores.DataContext = temporal;
            }
            else
            {
                RlstAutores.DataContext = catalogoTitulares;
            }
        }

        /// <summary>
        /// Permite visualizar la información del titular seleccionado
        /// </summary>
        public void VerInformacion()
        {
            if (selectedTitular == null)
            {
                MessageBox.Show("Selecciona el titular del cual quieres visualizar la información");
                return;
            }

            ManageAutor view = new ManageAutor(selectedTitular, false);
            view.ShowDialog();
        }

        /// <summary>
        /// Permite agregar funcionarios al catálogo de titulares
        /// </summary>
        public void Agregar()
        {
            ManageAutor addFuncionario = new ManageAutor(catalogoTitulares) { Owner = this };
            addFuncionario.ShowDialog();
        }

        /// <summary>
        /// Permite modificar la información del titular seleccionado
        /// </summary>
        public void Modificar()
        {
            if (selectedTitular == null)
            {
                MessageBox.Show("Selecciona el titular que quieres modificar");
                return;
            }

            ManageAutor update = new ManageAutor(selectedTitular, true) { Owner = this };
            update.ShowDialog();
        }

        bool includeSintesis;
        List<Autor> listaSeleccion = null;
        /// <summary>
        /// Permite el listado de obras en las que ha participado el autor seleccionado
        /// </summary>
        /// <param name="conSintesis">Indica si se incluye la síntesis de las obras o solamente el título</param>
        public void ImprimeInformacion(bool conSintesis)
        {
           if (tipoAutor == 1)
               listaSeleccion = RlstAutores.SelectedItems.Cast<Autor>().ToList();
           else
               listaSeleccion = RlstInstituciones.SelectedItems.Cast<Autor>().ToList();

           if (listaSeleccion != null && listaSeleccion.Count > 0)
           {
               tipoProceso = 4;
               this.includeSintesis = conSintesis;
               this.BusyIndicator.BusyContent = "Exportando información...";
               this.LaunchBusyIndicator();
           }
           else
               MessageBox.Show("Selecciona la información que deseas imprimir");

        }

        int tipoSeleccion = 1;
        public void SeleccionMultiple()
        {
            RlstAutores.SelectionMode = SelectionMode.Multiple;
            RlstInstituciones.SelectionMode = SelectionMode.Multiple;
            tipoSeleccion = 2;
        }

        public void SeleccionSencilla()
        {
            RlstAutores.SelectionMode = SelectionMode.Single;
            RlstInstituciones.SelectionMode = SelectionMode.Single;
            tipoSeleccion = 1;
        }




        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();
        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoProceso == 1)
            {
                AutorModel model = new AutorModel();
                catalogoTitulares = model.GetAutores();
                catalogoInstituciones = model.GetInstituciones();
            }
            else if (tipoProceso == 2)
            {
                if (selectedTitular.Obras == null)
                    selectedTitular.Obras = new ObraModel().GetObras(selectedTitular.IdTitular,"IdTitular");
            }
            else if (tipoProceso == 3)
            {
                if(selectedOrganismo.Obras == null)
                    selectedOrganismo.Obras = new ObraModel().GetObras(selectedOrganismo.IdTitular, "IdOrg");
            }
            else if (tipoProceso == 4)
            {
                new WordKiosko().PrintObrasPorAutor(listaSeleccion, includeSintesis, tipoAutor);
             
            }
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tipoProceso == 1)
            {
                RlstAutores.DataContext = catalogoTitulares;
                RlstInstituciones.DataContext = catalogoInstituciones;
            }
            else if (tipoProceso == 2)
                GObras.DataContext = selectedTitular.Obras;
            else if (tipoProceso == 3)
                GObras.DataContext = selectedOrganismo.Obras;
            else if(tipoProceso == 4)
            {
                this.SeleccionSencilla();
                listaSeleccion = null;
            }

            this.BusyIndicator.IsBusy = false;
        }

        private void LaunchBusyIndicator()
        {
            if (!worker.IsBusy)
            {
                this.BusyIndicator.IsBusy = true;
                worker.RunWorkerAsync();

            }
        }

        #endregion

        

       

        

    }
}
