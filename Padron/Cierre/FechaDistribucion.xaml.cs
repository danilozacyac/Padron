using System;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for FechaDistribucion.xaml
    /// </summary>
    public partial class FechaDistribucion 
    {
        private PadronGenerado selectedPadron;

        public FechaDistribucion(PadronGenerado selectedPadron)
        {
            InitializeComponent();
            this.selectedPadron = selectedPadron;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DtpFechaDis.SelectableDateStart = selectedPadron.FechaGenerado;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (DtpFechaDis.SelectedDate == null)
            {
                MessageBox.Show("Antes de continuar debes seleccionar una fecha");
                return;
            }

            selectedPadron.FechaDistribucion = DtpFechaDis.SelectedDate;
            ObraModel model = new ObraModel();
            model.UpdateFechaDis(selectedPadron.IdObra, DtpFechaDis.SelectedDate);

            DialogResult = true;
            this.Close();

        }
    }
}
