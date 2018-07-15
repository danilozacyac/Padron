using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadronApi.Dto;
using PadronApi.Model;
using System.ComponentModel;
using ScjnUtilities;
using PadronApi.Reportes;
using Microsoft.Win32;

namespace Organismos
{
    /// <summary>
    /// Interaction logic for ListaOrganismos.xaml
    /// </summary>
    public partial class ListaOrganismos : UserControl
    {
        ObservableCollection<Organismo> catalogoOrganismo;
        Organismo selectedOrganismo;

        private int tipoProceso;
        private string fileName;
        private ObservableCollection<Organismo> queImprime;

        public ListaOrganismos()
        {
            InitializeComponent();
            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tipoProceso = 1;
            BusyIndicator.BusyContent = "Cargando lista de organismos...";
            this.LaunchBusyIndicator();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LimpiaFiltros();

            String tempString = ((TextBox)sender).Text.ToUpper().Trim();

            long digitos = 0;
            Int64.TryParse(tempString, out digitos);

            if (!String.IsNullOrEmpty(tempString))
            {
                if(digitos == 0)
                tempString = StringUtilities.PrepareToAlphabeticalOrder(tempString);

                var temporal = (from n in catalogoOrganismo
                                where n.OrganismoStr.ToUpper().Contains(tempString)
                                select n).ToList();
                LblTotales.Content = temporal.Count + " registros";
                GOrganismos.DataContext = temporal;
            }
            else
            {
                GOrganismos.DataContext = catalogoOrganismo;
                LblTotales.Content = catalogoOrganismo.Count + " registros";
            }
        }

        private void GOrganismos_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedOrganismo = GOrganismos.SelectedItem as Organismo;

            LblTotales.Content = GOrganismos.Items.IndexOf(selectedOrganismo) + 1 + " de " + GOrganismos.Items.Count + " registros";
        }

        /// <summary>
        /// Permite visualizar la información del organismo seleccionado
        /// </summary>
        public void VerInformacion()
        {
            if (selectedOrganismo == null)
            {
                MessageBox.Show("Selecciona el organismo del cual requieres la información");
                return;
            }

            ActualizarVerOrganismo editOrg = new ActualizarVerOrganismo(selectedOrganismo, false) { Owner = this };
            editOrg.ShowDialog();
        }

        /// <summary>
        /// Permite agregar organismos de nueva creación o que no integraban el padrón al catálogo de Organismos
        /// </summary>
        public void Agregar()
        {
            AgregaOrganismo addOrg = new AgregaOrganismo(catalogoOrganismo) { Owner = this };
            addOrg.ShowDialog();
        }

        public void Modificar()
        {
            if (selectedOrganismo == null)
            {
                MessageBox.Show("Selecciona el organismo del cual quieres modificar su información");
                return;
            }
            ActualizarVerOrganismo editOrg = new ActualizarVerOrganismo(selectedOrganismo, true) { Owner = this };
            editOrg.ShowDialog();
        }

        public void VerActivos()
        {
            catalogoOrganismo = new OrganismoModel().GetOrganismos(true);
            GOrganismos.DataContext = catalogoOrganismo;
            LblTotales.Content = catalogoOrganismo.Count + " registros";

            this.LimpiaFiltros();
        }

        public void VerInactivos()
        {
            catalogoOrganismo = new OrganismoModel().GetOrganismos(false);
            GOrganismos.DataContext = catalogoOrganismo;
            LblTotales.Content = catalogoOrganismo.Count + " registros";

            this.LimpiaFiltros();
        }

        public Organismo VerHistorial()
        {
            if (selectedOrganismo == null)
            {
                MessageBox.Show("Selecciona el organismo del cual quieres modificar su información");
                return null;
            }
            else
                return selectedOrganismo;
        }

        public void Activar()
        {
            TxtBusqueda.Text = String.Empty;
            bool completed = new OrganismoModel().EstadoOrganismo(selectedOrganismo, 1);

            if (completed)
                catalogoOrganismo.Remove(selectedOrganismo);
            else
            {
                MessageBox.Show("No se pudo reactivar el organismos, intentelo más tarde");
            }
        }

        /// <summary>
        /// Cambia el estatus del organismo activo seleccionado por inactivo
        /// </summary>
        public void Desactivar()
        {
            //TxtBusqueda.Text = String.Empty;
            bool complete = false;

            selectedOrganismo.Adscripciones = new TitularModel().GetTitulares(selectedOrganismo);

            if (selectedOrganismo.Adscripciones != null && selectedOrganismo.Adscripciones.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Al desactivar este organismo sus integrantes quedarán sin adscripción y se modificarán las plantillas del padrón. ¿Deseas continuar?",
                    "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    complete = new OrganismoModel().DesactivaOrganismo(selectedOrganismo);

                    if (complete)
                        catalogoOrganismo.Remove(selectedOrganismo);
                    else
                    {
                        MessageBox.Show("No se pudo desactivar el organismos seleccionado. Favor de volver a intentar");
                    }
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(String.Format("¿Estas seguro de desactivar este organismo ({0})?", selectedOrganismo.OrganismoDesc),
                    "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    complete = new OrganismoModel().DesactivaOrganismo(selectedOrganismo);

                    if (complete)
                    {
                        catalogoOrganismo.Remove(selectedOrganismo);
                        GOrganismos.Items.Remove(selectedOrganismo);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo desactivar el organismos seleccionado. Favor de volver a intentar");
                    }
                }
            }
            LblTotales.Content = GOrganismos.Items.Count + " registros";
        }

        public void LimpiaFiltros()
        {
            this.GOrganismos.FilterDescriptors.SuspendNotifications();
            foreach (Telerik.Windows.Controls.GridViewColumn column in this.GOrganismos.Columns)
            {
                column.ClearFilters();
            }
            this.GOrganismos.FilterDescriptors.ResumeNotifications();

            LblTotales.Content = catalogoOrganismo.Count + " registros";
        }

        public void DetalleSeleccion()
        {
            tipoProceso = 3;
            BusyIndicator.BusyContent = "Obteniendo detalle del organismo...";
            queImprime = new ObservableCollection<Organismo>() { selectedOrganismo };

            LaunchBusyIndicator();
        }

        public void DetalleLista()
        {
            tipoProceso = 3;
            BusyIndicator.BusyContent = "Obteniendo detalle de los organismos...";
            queImprime = new ObservableCollection<Organismo>(GOrganismos.Items.Cast<Organismo>());

            LaunchBusyIndicator();
        }

        public void DetalleTodo()
        {
            tipoProceso = 3;
            BusyIndicator.BusyContent = "Obteniendo detalle de los organismos...";
            queImprime = catalogoOrganismo;

            LaunchBusyIndicator();
        }

        public void TotalSecretarios()
        {
            SaveFileDialog save = new SaveFileDialog() { Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*" };

            Nullable<bool> result = save.ShowDialog();

            if (result == true)
            {
                tipoProceso = 2;
                fileName = save.FileName;
                BusyIndicator.BusyContent = "Generando reporte de secretarios...";
                LaunchBusyIndicator();
            }
        }

        public void TotalOrganismos()
        {
            SaveFileDialog save = new SaveFileDialog() { Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*" };

            Nullable<bool> result = save.ShowDialog();

            if (result == true)
            {
                tipoProceso = 4;
                fileName = save.FileName;
                BusyIndicator.BusyContent = "Generando reporte de organismos...";
                LaunchBusyIndicator();
            }
        }

        public void EtiquetaSeleccion()
        {
            SaveFileDialog save = new SaveFileDialog() { Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*" };

            Nullable<bool> result = save.ShowDialog();

            if (result == true)
            {
                fileName = save.FileName;

                WordReports etiquetas = new WordReports(fileName, String.Empty);
                etiquetas.GeneraEtiquetas(new ObservableCollection<Organismo>() { selectedOrganismo });
            }

            
        }

        public void EtiquetaListaGrid()
        {
            SaveFileDialog save = new SaveFileDialog() { Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*" };

            Nullable<bool> result = save.ShowDialog();

            if (result == true)
            {
                fileName = save.FileName;
                WordReports etiquetas = new WordReports(fileName, String.Empty);
                etiquetas.GeneraEtiquetas(new ObservableCollection<Organismo>(GOrganismos.Items.Cast<Organismo>()));
            }

            

            
        }

        public void TotalTitulares()
        {
            SaveFileDialog save = new SaveFileDialog() { Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*" };

            Nullable<bool> result = save.ShowDialog();

            if (result == true)
            {
                fileName = save.FileName;

                PdfReports report = new PdfReports(fileName);
                report.ReporteGeneroTitulares();
            }
        }

        private void GOrganismos_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
            LblTotales.Content = GOrganismos.Items.Count + " registros";
        }

        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoProceso == 1)
                catalogoOrganismo = new OrganismoModel().GetOrganismos(true);

            if (tipoProceso == 2)
            {
                PdfReports report = new PdfReports(fileName);
                report.ReporteSecretarios();
            }

            if (tipoProceso == 3)
            {
                WordReports detalle = new WordReports(queImprime);
                detalle.ImprimeDetalleOrganismo();
            }

            if (tipoProceso == 4)
            {
                PdfReports report = new PdfReports(fileName);
                report.ReporteOrganismosxCircuito();
            }
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tipoProceso == 1)
            {
                GOrganismos.DataContext = catalogoOrganismo;
                LblTotales.Content = catalogoOrganismo.Count + " registros";
            }

            if (tipoProceso == 3)
                queImprime = null;

            fileName = String.Empty;
            //Dispatcher.BeginInvoke(new Action<ObservableCollection<Organismos>>(this.UpdateGridDataSource), e.Result);
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