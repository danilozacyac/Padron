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

namespace Obras.Padron
{
    /// <summary>
    /// Interaction logic for CatalogoObrasPadron.xaml
    /// </summary>
    public partial class CatalogoObrasPadron : UserControl, INotifyPropertyChanged
    {
        ObservableCollection<Obra> catalogoObras;
        Obra selectedObra;
        private bool estadoObras = true;

        public CatalogoObrasPadron()
        {
            InitializeComponent();
        }

        public bool EstadoObras
        {
            get
            {
                return this.estadoObras;
            }
            set
            {
                this.estadoObras = value;
                this.OnPropertyChanged("EstadoObras");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BusyIndicator.IsBusy = true;

            catalogoObras = new ObraModel().GetObras(estadoObras);

            LblTotales.Content = catalogoObras.Count + " registros";
            GObras.DataContext = catalogoObras;

            BusyIndicator.IsBusy = false;
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper();

            if (!String.IsNullOrEmpty(tempString))
            {
                var resultado = (from n in catalogoObras
                                 where n.Titulo.ToUpper().Contains(tempString) ||
                                 n.TituloStr.ToUpper().Contains(tempString) 
                                 select n).ToList();
                LblTotales.Content = resultado.Count + " registros";
                GObras.DataContext = resultado;
            }
            else
            {
                GObras.DataContext = catalogoObras;
                LblTotales.Content = catalogoObras.Count + " registros";
            }

            this.GObras.FilterDescriptors.SuspendNotifications();
            foreach (Telerik.Windows.Controls.GridViewColumn column in this.GObras.Columns)
            {
                column.ClearFilters();
            }
            this.GObras.FilterDescriptors.ResumeNotifications();
        }

        private void GObras_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedObra = GObras.SelectedItem as Obra;

            LblTotales.Content = (GObras.Items.IndexOf(selectedObra) + 1) + " de " + GObras.Items.Count + " registros";
        }

        private void GObras_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ObrasWin view = new ObrasWin(selectedObra, false) { Owner = this };
            view.ShowDialog();
        }

        private void GObras_Filtered(object sender, Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs e)
        {
            LblTotales.Content = GObras.Items.Count + " registros";
        }

        int tipoReporte;
        string fileName;
        ObservableCollection<Obra> queImprimo;
        public void PrintReporteGeneral(int tipoReporte)
        {
            this.tipoReporte = tipoReporte;
            queImprimo = new ObservableCollection<Obra>(GObras.Items.Cast<Obra>());


            if (queImprimo.Count != 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                if (tipoReporte == 1)
                    save.Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*";
                else if (tipoReporte == 2)
                    save.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                else if (tipoReporte == 3)
                    save.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";

                Nullable<bool> result = save.ShowDialog();
                if (result == true)
                {
                    fileName = save.FileName;
                    DoBackgroundWork();
                }
            }
            else
            {
                MessageBox.Show("No Hay Datos Para Realizar Un Reporte", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Permite visualizar la información de la obra seleccionada
        /// </summary>
        public void VerInformacion()
        {
            if (selectedObra == null)
            {
                MessageBox.Show("Primero debes seleccionar la obra de la cual deseas ver la información", "Atención:", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            ObrasWin update = new ObrasWin(selectedObra, false) { Owner = this };
            update.ShowDialog();
        }

        /// <summary>
        /// Permite agregar las obras que se van a distribuir al catálogo de obras
        /// </summary>
        public void Agregar()
        {
            ObrasWin addObra = new ObrasWin(catalogoObras) { Owner = this };
            addObra.ShowDialog();
        }

        /// <summary>
        /// Permite modificar la información de una obra
        /// </summary>
        public void Modificar()
        {
            if (selectedObra == null)
            {
                MessageBox.Show("Selecciona la obra de la cual quieres modificar su información", "Atención:", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            ObrasWin update = new ObrasWin(selectedObra, true) { Owner = this };
            update.ShowDialog();
        }

        /// <summary>
        /// Cambia el estado de una obra entre activado y desactivado
        /// </summary>
        /// <param name="estadoObra">Nuevo estado de la obra</param>
        public void CambiaEstadoObra(int estadoObra)
        {
            string estado = (estadoObra == 0) ? "desactivar" : "activar";

            if (selectedObra == null)
            {
                MessageBox.Show("Primero debes seleccionar la obra que deseas " + estadoObra, "Atención:", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            MessageBoxResult result = MessageBox.Show("¿Estas seguro de " + estado +" la obra: " + selectedObra.Titulo + "?", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                new ObraModel().EstadoObra(selectedObra, estadoObra);
                catalogoObras.Remove(selectedObra);
            }
        }

       



        #region BackgroundWorker

        /// <summary>
        /// Creates a BackgroundWorker class to do work
        /// on a background thread.
        /// </summary>
        private void DoBackgroundWork()
        {
            BackgroundWorker worker = new BackgroundWorker();

            // Tell the worker to report progress.
            worker.WorkerReportsProgress = true;

            worker.ProgressChanged += ProgressChanged;
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// The work for the BackgroundWorker to perform.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        void DoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoReporte == 1)
            {
                WordReports word = new WordReports(queImprimo, fileName);
                word.InformeGeneralObras();
            }
            else if (tipoReporte == 2)
            {
                ExcelReports excel = new ExcelReports(queImprimo, fileName);
                excel.InformeGeneralObras();
            }
            else if (tipoReporte == 3)
            {
                PdfReports pdf = new PdfReports(queImprimo, fileName);
                pdf.InformeGenerlaObras();
            }
        }

        /// <summary>
        /// Occurs when the BackgroundWorker reports a progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //pbLoad.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Occurs when the BackgroundWorker has completed its work.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            queImprimo = null;
        }

        #endregion



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            UserControl_Loaded(null, null);
            
        }

        #endregion // INotifyPropertyChanged Members

        

    }
}
