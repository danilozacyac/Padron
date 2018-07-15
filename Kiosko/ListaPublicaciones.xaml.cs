using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Kiosko.ReportesKiosko;
using Microsoft.Win32;
using PadronApi.Dto;
using PadronApi.Model;
using ScjnUtilities;

namespace Kiosko
{
    /// <summary>
    /// Interaction logic for ListaPublicaciones.xaml
    /// </summary>
    public partial class ListaPublicaciones : UserControl
    {
        ObservableCollection<Obra> catalogoObras;

        Obra selectedObra;

        int tipoProceso = 1;

        public ListaPublicaciones()
        {
            InitializeComponent();
            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tipoProceso = 1;
            if(catalogoObras == null)
                LaunchBusyIndicator();   
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper();

            if (!String.IsNullOrEmpty(tempString))
            {
                var resultado = (from n in catalogoObras
                                 where n.Titulo.ToUpper().Contains(tempString) ||
                                 n.TituloStr.ToUpper().Contains(tempString) || n.Sintesis.Contains(tempString) || 
                                 n.NumMaterial.Contains(tempString) || n.Isbn.Contains(tempString)
                                 select n).ToList();
                GObras.DataContext = resultado;
            }
            else
            {
                GObras.DataContext = catalogoObras;
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
        }


        public void VerInformacion()
        {
            if(selectedObra == null)
            {
                MessageBox.Show("Selecciona la obra que quieres visualizar");
                return;
            }

            ObrasKiosko kiosko = new ObrasKiosko(selectedObra, false) { Owner = this };
            kiosko.ShowDialog();
        }

        public void EditarInformacion()
        {
            if (selectedObra == null)
            {
                MessageBox.Show("Selecciona la obra que quieres editar");
                return;
            }

            ObrasKiosko kiosko = new ObrasKiosko(selectedObra, true) { Owner = this };
            kiosko.ShowDialog();
        }

        public void LimpiarSeleccion()
        {
            foreach (Obra removeObra in GObras.SelectedItems.Cast<Obra>().ToList())
                GObras.SelectedItems.Remove(removeObra);
        }

        List<Obra> obrasImprime;
        public void ImprimeInformacionObra()
        {
            tipoProceso = 2;
            obrasImprime = GObras.SelectedItems.Cast<Obra>().ToList();
            LaunchBusyIndicator();
        }

        public void ImprimeObrasADisposicion()
        {
            tipoProceso = 3;
            LaunchBusyIndicator();
        }

        public void AgregarImagen()
        {
            if (selectedObra == null)
            {
                MessageBox.Show("Selecciona la obra a la cual deseas agregar una imagen");
                return;
            }

            OpenFileDialog open = new OpenFileDialog()
            {
                Filter = "JPG (.jpg)|*.jpg|PNG (.png)|*.png|All Files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false
            };


            if (open.ShowDialog() == true)
            {
                

               bool exito = FilesUtilities.CopyToLocalResource(open.FileName, ConfigurationManager.AppSettings["Imagenes"] + selectedObra.IdObra + Path.GetExtension(open.FileName));

               if (exito)
               {
                   exito = new ObraModel().UpdateImagenObra(selectedObra.IdObra, selectedObra.IdObra + Path.GetExtension(open.FileName));

                   if (exito)
                       selectedObra.ImagePath = open.FileName;
                   else
                       MessageBox.Show("No se pudo completar la operación vuelve a intentarlo");
               }
            }
        }

        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoProceso == 1)
                catalogoObras = new ObraModel().GetObras();
            else if (tipoProceso == 2)
                new WordKiosko().PrintInfoPorObra(obrasImprime);
            else if (tipoProceso == 3)
                new WordKiosko().PrintObrasADisposicion((from n in catalogoObras
                                                         where n.ADisposicion == true
                                                         select n).ToList());
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tipoProceso == 1)
                GObras.DataContext = catalogoObras;
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
