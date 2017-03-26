using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using PadronApi.Dto;
using PadronApi.Model;
using ScjnUtilities;
using System.IO;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for MultAcuseOficina.xaml
    /// </summary>
    public partial class MultAcuseOficina
    {
        PadronGenerado padron;

        ObservableCollection<Acuse> plantillas;

       

        public MultAcuseOficina(PadronGenerado padron, ObservableCollection<Acuse> plantillas)
        {
            InitializeComponent();
            this.padron = padron;
            this.plantillas = plantillas;
        }

        


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnArchivoGuia_Click(object sender, RoutedEventArgs e)
        {
           TxtArchivoPaq.Text = this.GetFilePath();
        }

        private string GetFilePath()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                InitialDirectory = String.Format(@"C:\Users\{0}\Documents", Environment.UserName),
                Title = "Selecciona el archivo del proyecto"
            };
            dialog.ShowDialog();

            string ruta = dialog.FileName;


            return ruta;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            
            DialogResult = false;
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (DpAcuse.SelectedDate == null)
            {
                MessageBox.Show("Para continuar debes capturar la fecha de rececpción de la obra", "Fecha de recepción", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (String.IsNullOrEmpty(TxtArchivoPaq.Text))
            {
                MessageBox.Show("Para continuar debes indicar la ubicación del archivo de soporte", "Archivo soporte", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            AcusesModel model = new AcusesModel();

            int idenObra = (padron == null) ? plantillas[0].IdObra : padron.IdObra;


            foreach (Acuse acuse in plantillas)
            {
                string nuevaRuta = String.Format("{0}Acu{1}_{2}{3}", ConfigurationManager.AppSettings["ArchivosSoporte"], idenObra, "Parte" + acuse.TipoDistribucion, Path.GetExtension(TxtArchivoPaq.Text));

                bool finish = true;
                bool fileCopy = FilesUtilities.CopyToLocalResource(TxtArchivoPaq.Text, nuevaRuta);
                if (fileCopy)
                {
                    acuse.FechaRecAcuse = DpAcuse.SelectedDate;
                    acuse.ArchivoAcuse = String.Format("Acu{0}_{1}{2}", idenObra, "Parte" + acuse.TipoDistribucion, Path.GetExtension(TxtArchivoPaq.Text));
                    model.UpdateDetalleRecepcion(padron, acuse);

                }
                else
                {
                    MessageBox.Show("No se pudo copiar el archivo de soporte, vuelva a intentarlo", "ERROR:", MessageBoxButton.OK, MessageBoxImage.Error);
                    finish = false;
                    break;
                }
                if (finish)
                {
                    DialogResult = true;
                    this.Close();
                }
            }
        }


        private void RadWindow_PreviewClosed(object sender, Telerik.Windows.Controls.WindowPreviewClosedEventArgs e)
        {
            
        }
    }
}
