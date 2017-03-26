using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using PadronApi.Dto;
using ScjnUtilities;
using PadronApi.Model;
using System.Collections.ObjectModel;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for AcuseOficina.xaml
    /// </summary>
    public partial class AcuseOficina 
    {
        PadronGenerado padron;
        Acuse plantilla;
        bool todosAcuses;


        /// <summary>
        /// Actualiza la fecha de recepción de la ofician y adjunta su comprobante
        /// </summary>
        /// <param name="padron">Obra de la cual se actualiza la info</param>
        /// <param name="plantilla"></param>
        /// <param name="todosAcuses">Indica si la vista mostrará todos los acuses o solo de una obra en particular</param>
        public AcuseOficina(PadronGenerado padron, Acuse plantilla,bool todosAcuses )
        {
            InitializeComponent();
            this.padron = padron;
            this.plantilla = plantilla;
            this.todosAcuses = todosAcuses;

        }

       


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = plantilla;
        }

        private void BtnArchivoGuia_Click(object sender, RoutedEventArgs e)
        {
                plantilla.ArchivoAcuse = this.GetFilePath();
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
            if (plantilla.FechaRecAcuse == null)
            {
                MessageBox.Show("Para continuar debes capturar la fecha del acuse", "Fecha de acuse", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (String.IsNullOrEmpty(plantilla.ArchivoAcuse))
            {
                MessageBox.Show("Para continuar debes indicar la ubicación del archivo de soporte", "Archivo soporte", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            int idenObra = (todosAcuses) ? plantilla.IdObra : padron.IdObra;

            string nuevaRuta = String.Format("{0}Acu{1}_{2}_{3}_{4}{5}", ConfigurationManager.AppSettings["ArchivosSoporte"], idenObra, plantilla.IdOrganismo, plantilla.IdTitular, plantilla.Oficio, Path.GetExtension(TxtArchivoPaq.Text));

            bool fileCopy = FilesUtilities.CopyToLocalResource(TxtArchivoPaq.Text, nuevaRuta);

            if (fileCopy)
            {
                plantilla.ArchivoAcuse = String.Format("Acu{0}_{1}_{2}_{3}{4}", idenObra, plantilla.IdOrganismo, plantilla.IdTitular, plantilla.Oficio, Path.GetExtension(TxtArchivoPaq.Text));

                AcusesModel model = new AcusesModel();
                bool complete = model.UpdateDetalleRecepcion(padron, plantilla);

                if (!complete)
                {
                    MessageBox.Show("No se pudo completar la operación, favor de volver a intentarlo", "Error de actualización", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    DialogResult = true;
                    this.Close();
                }
            }

        }


        private void RadWindow_PreviewClosed(object sender, Telerik.Windows.Controls.WindowPreviewClosedEventArgs e)
        {
            if (DialogResult != true)
            {
                DpAcuse.SelectedValue = null;
                TxtArchivoPaq.Text = String.Empty;
            }
        }
    }
}
