using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadronApi.Dto;
using PadronApi.Model;
using System.ComponentModel;
using Microsoft.Win32;
using PadronApi.Reportes;

namespace Funcionarios
{
    /// <summary>
    /// Interaction logic for ListaFuncionarios.xaml
    /// </summary>
    public partial class ListaFuncionarios : UserControl
    {

        ObservableCollection<Titular> catalogoTitulares;
        Titular selectedTitular;
        bool titularesMostradosActivos = true;


        private int tipoProceso = 1;
        private string fileName = String.Empty;

        public ListaFuncionarios()
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

        private void GTitulares_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedTitular = GTitulares.SelectedItem as Titular;
            LblTotales.Content = String.Format("{0} de {1} registros", (GTitulares.Items.IndexOf(selectedTitular) + 1), catalogoTitulares.Count);
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            this.GTitulares.FilterDescriptors.SuspendNotifications();
            foreach (Telerik.Windows.Controls.GridViewColumn column in this.GTitulares.Columns)
            {
                column.ClearFilters();
            }
            this.GTitulares.FilterDescriptors.ResumeNotifications();

            String tempString = ((TextBox)sender).Text.ToUpper().Trim();

            if (!String.IsNullOrEmpty(tempString))
            {
                var temporal = (from n in catalogoTitulares
                                where n.Nombre.ToUpper().Contains(tempString) || n.Apellidos.ToUpper().Contains(tempString) ||
                                n.NombreStr.ToUpper().Contains(tempString)
                                select n).ToList();
                LblTotales.Content = temporal.Count + " registros";
                GTitulares.DataContext = temporal;
            }
            else
            {
                GTitulares.DataContext = catalogoTitulares;
                LblTotales.Content = catalogoTitulares.Count + " registros";
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

            AgregaFuncionario view = new AgregaFuncionario(selectedTitular, false) { Owner = this };
            view.ShowDialog();
        }

        /// <summary>
        /// Permite agregar funcionarios al catálogo de titulares
        /// </summary>
        public void Agregar()
        {
            AgregaFuncionario addFuncionario = new AgregaFuncionario(catalogoTitulares) { Owner = this };
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

            AgregaFuncionario update = new AgregaFuncionario(selectedTitular, true) { Owner = this };
            update.ShowDialog();
        }

        public void VerTrayectoria()
        {
            Trayectoria trayectoria = new Trayectoria(selectedTitular) { Owner = this };
            trayectoria.ShowDialog();
        }

        public Titular VerHistorialObras()
        {
            if (selectedTitular == null)
            {
                MessageBox.Show("Selecciona el titular del cual quieres visualizar su historial");
                return null;
            }
            else
                return selectedTitular;
        }

        public void NoDistribucion()
        {
            if (selectedTitular == null)
            {
                MessageBox.Show("Selecciona el titular que ya no quiere recibir ninguna obra");
                return;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de que este titular ya no quiere recibir ninguna obra?","Atención",MessageBoxButton.YesNo,MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    TitularModel model = new TitularModel();

                    bool exito = model.UpdateTitular(selectedTitular, false);

                    if (exito)
                    {
                        selectedTitular.QuiereDistribucion = -1;
                        selectedTitular.TotalAdscripciones = -1;
                    }
                    else
                        MessageBox.Show("No se pudo completar el proceso, favor de volver a intentarlo");
                }

            }
        }

        public void MuestraTitularesActivos()
        {
            titularesMostradosActivos = true;
            LaunchBusyIndicator();
        }

        public void MuestraTitularesJubilados()
        {
            titularesMostradosActivos = false;
            LaunchBusyIndicator();
        }


        public void ReporteIncluidosTodos()
        {
            tipoProceso = 2;

            SaveFileDialog save = new SaveFileDialog() { Filter = "PDF Files (*.pdf)|*.pdf" };

            Nullable<bool> result = save.ShowDialog();

            if (result == true)
            {
                fileName = save.FileName;
                BusyIndicator.BusyContent = "Generando reporte de inclusión en distribución...";
                LaunchBusyIndicator();
            }

        }


        public void MarcarComoAutor()
        {
            if (selectedTitular == null)
            {
                MessageBox.Show("Selecciona el titular que quieres marcar como autor");
                return;
            }
            else
            {
                new AutorModel().SetAsAutor(selectedTitular);
                selectedTitular.HaPublicado = true;
            }
        }

        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();
        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if(tipoProceso == 1)
                catalogoTitulares = new TitularModel().GetTitulares(titularesMostradosActivos);
            else if (tipoProceso == 2)
            {
                ObservableCollection<Adscripcion> incluidosTodo = new TitularModel().GetTitularesIncluidosEnTodo();

                PdfReports report = new PdfReports(fileName);
                report.ReporteIncluidosTodosTirajes(incluidosTodo);
            }
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tipoProceso == 1)
            {
                GTitulares.DataContext = catalogoTitulares;
                LblTotales.Content = catalogoTitulares.Count + " registros";
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

        private void GTitulares_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
            LblTotales.Content = GTitulares.Items.Count + " registros";
        }

    }
}
