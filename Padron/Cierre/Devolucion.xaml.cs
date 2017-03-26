using System;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;
using ScjnUtilities;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for Devolucion.xaml
    /// </summary>
    public partial class Devolucion
    {
        PlantillaDto plantilla;
        DevolucionModel model;
        int devPart, devOfic, devBibl, devResg;

        /// <summary>
        /// Indica el tipo de devolución que se hace 1. Para devoluciones voluntarias 2. Cancelaciones
        /// </summary>
        readonly int tipoDevolucion;

        public Devolucion(PlantillaDto plantilla, int tipoDevolucion)
        {
            InitializeComponent();
            this.plantilla = plantilla;
            this.tipoDevolucion = tipoDevolucion;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = plantilla;

            model = new DevolucionModel();

            devOfic = model.GetDevueltosTituProp(plantilla.IdTitular, plantilla.IdObra, 2);
            
            TxtDevolvioOficina.Text = devOfic.ToString();

            NumDevOficina.Maximum = plantilla.Oficina - devOfic;

            if (tipoDevolucion == 2)
            {
                TxtOficio.Text = "Sin Oficio";
                RdpFechaDevuelve.SelectedDate = DateTime.Now;
                TxtObservaciones.Text = "CANCELACIÓN";
                NumDevOficina.Value = plantilla.Oficina;
                NumDevOficina.IsEnabled = false;
            }

        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Devoluciones devolucion = new Devoluciones()
            {
                IdTitular = plantilla.IdTitular,
                IdObra = plantilla.IdObra,
                Propiedad = 2,
                FechaDevolucion = RdpFechaDevuelve.SelectedDate,
                Observaciones = VerificationUtilities.TextBoxStringValidation(TxtObservaciones.Text),
                Cantidad = Convert.ToInt16(NumDevOficina.Value),
                OficioDevolucion = VerificationUtilities.TextBoxStringValidation(TxtOficio.Text),
                TipoDevolucion = tipoDevolucion
            };

            bool complete = false;

            if (RdpFechaDevuelve.SelectedDate == null)
            {
                MessageBox.Show("Para realizar una devolución debes ingresar la fecha en que se llevó a cabo");
                return;
            }

            if (NumDevOficina.Value > 0)
                complete = model.InsertaDevolucion(devolucion);

            if (!complete)
                MessageBox.Show("No se realizaron todas las devoluciones correctamente, favor de volver a intentar");

            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}