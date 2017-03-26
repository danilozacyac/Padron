using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Reportes;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using System.Configuration;
using ScjnUtilities;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for Acuses.xaml
    /// </summary>
    public partial class Acuses 
    {
        ObservableCollection<Acuse> entregados = new ObservableCollection<Acuse>();
        PadronGenerado padron;
        Acuse selectedOficio;

        private int tipoProceso = 1;
        private string fileName = String.Empty;

        private string selectedYear;

        List<RadRibbonButton> listadoBotones;

        public Acuses(PadronGenerado padron) 
        {
            InitializeComponent();

            this.padron = padron;

            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            listadoBotones = new List<RadRibbonButton>()
            {
                BtnDatosEnvio, BtnDatosEntrega, BtnDatosEnvioPart, BtnComprobantePaq, BtnDetalleAcuse, BtnFechaRecOfic, BtnAcuseOficina,BtnDfMetropol
            };

            if (padron == null)
                BtnDfMetropol.Visibility = Visibility.Collapsed;

            //El año de inicio es 2016 porque es cuando se libera a producción la plataforma
            int initialYear = 2016;
            List<string> years = new List<string>() { "Mostrar todos" };
            while (initialYear <= DateTime.Now.Year)
            {
                years.Add(initialYear.ToString());
                initialYear++;
            }
            CbxAnio.DataContext = years;

            
        }

        private void GPlantilla_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            selectedOficio = GPlantilla.SelectedItem as Acuse;
        }

        private void AddGroupDescriptor(string columna, string encabezado)
        {
            GroupDescriptor descriptor = new GroupDescriptor()
            {
                Member = columna,
                SortDirection = ListSortDirection.Ascending,
                DisplayContent = encabezado
            };

            this.GPlantilla.GroupDescriptors.Add(descriptor);
        }

        

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper().Trim();

            if (!String.IsNullOrEmpty(tempString))
            {
                var temporal = (from n in entregados
                                where n.Nombre.ToUpper().Contains(tempString) || n.Oficio.ToString().Contains(tempString) || n.Organismo.ToUpper().Contains(tempString) || n.NumGuia.Contains(tempString)
                                select n).ToList();
                GPlantilla.DataContext = temporal;
            }
            else
            {
                GPlantilla.DataContext = entregados;
            }
        }

        private void BtnDetalleAcuse_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOficio == null)
            {
                MessageBox.Show("Antes de continuar debes seleccionar el oficio al cual se añadirán datos de recepción de la oficina", "Atención:", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            AcuseOficina oficina = new AcuseOficina(padron, selectedOficio, (padron == null) ? true : false) { Owner = this };
            oficina.ShowDialog();
        }


        private void BtnAcuseOficina_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOficio == null)
            {
                MessageBox.Show("Selecciona el oficio del cual quieres ver el acuse");
                return;
            }

            if (!String.IsNullOrEmpty(selectedOficio.ArchivoAcuse) || !String.IsNullOrWhiteSpace(selectedOficio.ArchivoAcuse))
            {
                string basePath = ConfigurationManager.AppSettings["ArchivosSoporte"];
                Process.Start(basePath + selectedOficio.ArchivoAcuse);
            }
            else
                MessageBox.Show("Primero debes ingresar del acuse");
        }

        private void BtnFechaRecOfic_Click(object sender, RoutedEventArgs e)
        {
            if (GPlantilla.SelectedItems == null)
            {
                MessageBox.Show("Selecciona las obras a las cuales asignarás fecha de recepción");
                return;
            }
            else if (GPlantilla.SelectedItems.Count == 1)
            {
                MessageBox.Show("Para poder usar esta opción debes seleccionar la misma obra en almenos dos titulares");
                return;
            }

            ObservableCollection<Acuse> queRegistro = new ObservableCollection<Acuse>();
            foreach (Acuse acuse in GPlantilla.SelectedItems)
                queRegistro.Add(acuse);

            MultAcuseOficina fAcuse = new MultAcuseOficina(padron, queRegistro) { Owner = this };
            fAcuse.ShowDialog();

            if (fAcuse.DialogResult == true)
            {
                GPlantilla.SelectedItems.Clear();
            }
        }

        

        

        private void BtnDatosEntrega_Click(object sender, RoutedEventArgs e)
        {
            tipoProceso = 3;
            OpenFileDialog open = new OpenFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx" };

            Nullable<bool> result = open.ShowDialog();
            if (result == true)
            {
                fileName = open.FileName;

                BusyIndicator.BusyContent = "Cargando datos de entrega de las obras...";
                LaunchBusyIndicator();
            }


            
        }

        private void BtnDatosEnvio_Click(object sender, RoutedEventArgs e)
        {
            tipoProceso = 2;
            OpenFileDialog open = new OpenFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx" };

            Nullable<bool> result = open.ShowDialog();
            if (result == true)
            {
                fileName = open.FileName;

                BusyIndicator.BusyContent = "Cargando datos de envio de las obras...";
                LaunchBusyIndicator();
            }

            
        }

        private void BtnDatosEnvioPart_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOficio == null)
            {
                MessageBox.Show("Antes de continuar debes seleccionar el oficio al cual se añadirán datos de envio", "Atención:", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            AcusePaqueteria paq = new AcusePaqueteria(selectedOficio, padron) { Owner = this };
            paq.ShowDialog();
        }

        private void BtnDescargaComprobante_Click(object sender, RoutedEventArgs e)
        {
            //tipoProceso = 4;

            //BusyIndicator.BusyContent = "Descargando comprobantes de entrega...";
            //LaunchBusyIndicator();

            Acuse seleccion = GPlantilla.SelectedItem as Acuse;

            if (seleccion == null || String.IsNullOrEmpty(seleccion.NumGuia))
            {
                MessageBox.Show("Para obtener un comprobante de entrega primero tienes que capturar el número de guía");
                return;
            }

           
            string trackingQualifier = ConfigurationManager.AppSettings["trackingQualifier"];
            string cuentaCorte = ConfigurationManager.AppSettings["cuentaScjnFedex"];


            string urlPath = String.Format("https://www.fedex.com/trackingCal/retrievePDF.jsp?trackingNumber={0}&trackingQualifier={1}~{0}~FX&trackingCarrier=FDXE&shipDate=&destCountry=&locale=es_MX&accountNbr={2}&type=SPOD&appType=&anon=", seleccion.NumGuia, trackingQualifier, cuentaCorte);

            Process.Start("IExplore.exe", urlPath);
            Clipboard.SetText(selectedOficio.NumGuia);
        }


        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoProceso == 1)
            {
                if (padron == null)
                    entregados = new AcusesModel().GetDetalleRecepcion(Convert.ToInt32(selectedYear));
                else
                    entregados = new AcusesModel().GetDetalleRecepcion(padron);
            }

            if (tipoProceso == 2) // Listado Firmas Word SCJN
            {
                ExcelReports excel = new ExcelReports(null, fileName);
                excel.ObtenNumGuia();
            }

            if (tipoProceso == 3) //Listados excel
            {
                ExcelReports excel = new ExcelReports(null, fileName);
                excel.ObtenFechaEntregaPaqueteria();
            }

            
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tipoProceso == 1)
            {
                GPlantilla.DataContext = entregados;

                this.AddGroupDescriptor("TipoDistribucion", "Distribución");
                this.AddGroupDescriptor("EstadoStr", "Estado");
                this.AddGroupDescriptor("CiudadStr", "Ciudad");
                this.AddGroupDescriptor("Organismo", "Organismo");

                if (padron == null)
                {
                    this.AddGroupDescriptor("Nombre", "Nombre");
                    ColObra.IsVisible = true;
                    ColNombre.IsVisible = false;
                }
                else
                {
                    ColObra.IsVisible = false;
                    ColNombre.IsVisible = true;
                }


                foreach (RadRibbonButton boton in listadoBotones)
                {
                    int tag = Convert.ToInt32(boton.Tag);

                    if (!AccesoUsuario.Permisos.Contains(tag))
                        boton.IsEnabled = false;
                }
            }
            else if (tipoProceso == 2 || tipoProceso == 3)
            {
                tipoProceso = 1;
                fileName = String.Empty;
                Window_Loaded(null, null);

            }
            else if (tipoProceso == 4)
            {
                //AcusesModel model = new AcusesModel();
                //model.ObtenAcuseGuia(25);
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

        private void BtnDfMetropol_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<Acuse> queRegistro = (from n in entregados
                     where n.TipoDistribucion == 2 || n.TipoDistribucion == 3
                     select n).ToObservableCollection();


            AcuSanMetrop fAcuse = new AcuSanMetrop(padron, queRegistro) { Owner = this };
            fAcuse.ShowDialog();
        }

        private void CbxAnio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedYear = CbxAnio.SelectedItem as String;

            int year = 0;
            Int32.TryParse(selectedYear, out year);

            selectedYear = year.ToString();

            this.BusyIndicator.BusyContent = "Cargando información de acuses...";
            this.LaunchBusyIndicator();
        }

        



    }
}
