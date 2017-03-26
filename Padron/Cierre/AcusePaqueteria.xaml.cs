using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using PadronApi.Dto;
using ScjnUtilities;
using PadronApi.Model;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for AcusePaqueteria.xaml
    /// </summary>
    public partial class AcusePaqueteria
    {
        Acuse plantilla;
        PadronGenerado padronGenerado;

        public AcusePaqueteria(Acuse plantilla, PadronGenerado padronGenerado)
        {
            InitializeComponent();
            this.plantilla = plantilla;
            this.padronGenerado = padronGenerado;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = plantilla;
        }

        private void BtnArchivoGuia_Click(object sender, RoutedEventArgs e)
        {
            plantilla.ArchivoGuia = this.GetFilePath();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (plantilla.FechaEnvio == null)
            {
                MessageBox.Show("Para continuar debes capturar la fecha de envio", "Fecha de envio", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (String.IsNullOrEmpty(plantilla.NumGuia))
            {
                MessageBox.Show("Para continuar debes capturar el número de guía del envio", "Número de guía", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            if (!String.IsNullOrEmpty(plantilla.ArchivoGuia))
            {
                string nuevaRuta = ConfigurationManager.AppSettings["ArchivosSoporte"].ToString() + "Paq" + plantilla.IdObra
               + "_" + plantilla.IdOrganismo + "_" + plantilla.IdTitular + Path.GetExtension(TxtArchivoPaq.Text);

                bool fileCopy = FilesUtilities.CopyToLocalResource(TxtArchivoPaq.Text, nuevaRuta);

                if (fileCopy)
                {
                    plantilla.ArchivoGuia = nuevaRuta;
                    AcusesModel model = new AcusesModel();
                    bool complete = model.UpdateDetalleRecepcion(padronGenerado, plantilla);

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
            else
            {
                AcusesModel model = new AcusesModel();
                bool complete = model.UpdateDetalleRecepcion(padronGenerado, plantilla);

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

        private string GetFilePath()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.InitialDirectory = String.Format(@"C:\Users\{0}\Documents", Environment.UserName);
            dialog.Title = "Selecciona el archivo del proyecto";
            dialog.ShowDialog();

            return dialog.FileName;
        }

        private void TxtGuia_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = VerificationUtilities.IsNumber(e.Text);
        }

        

        private void RadWindow_PreviewClosed(object sender, Telerik.Windows.Controls.WindowPreviewClosedEventArgs e)
        {
            if (DialogResult != true)
            {
                DpPaqueteria.SelectedValue = null;
                plantilla.NumGuia = String.Empty;
                TxtArchivoPaq.Text = String.Empty;
            }
        }
    }
}
