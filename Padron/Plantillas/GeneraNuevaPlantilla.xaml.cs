using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadronApi.Dto;
using PadronApi.Model;
using Telerik.Windows.Controls;

namespace Padron.Plantillas
{
    /// <summary>
    /// Interaction logic for GeneraNuevaPlantilla.xaml
    /// </summary>
    public partial class GeneraNuevaPlantilla
    {
        private ObservableCollection<TirajePersonal> acuerdos;
        TirajePersonal selectedTiraje;
        AcuerdosModel model;
        private int distribucion, reserva, sede, zaragoza, ventas, total;

        public GeneraNuevaPlantilla()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            model = new AcuerdosModel();
            acuerdos = model.GetAcuerdos();
            CbxAcuerdos.DataContext = acuerdos;
            
        }

        private void CbxAcuerdos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTiraje = CbxAcuerdos.SelectedItem as TirajePersonal;

            NudNuevoTiraje.Value = Convert.ToInt32(selectedTiraje.Acuerdo) + 100;

            distribucion = model.GetTotalDistribucionAcuerdo(selectedTiraje);
            reserva = model.GetTotalResguardoAcuerdo(selectedTiraje, 32630);
            sede = model.GetTotalResguardoAcuerdo(selectedTiraje, 6000);
            zaragoza = model.GetTotalResguardoAcuerdo(selectedTiraje, 6001);
            ventas = model.GetTotalResguardoAcuerdo(selectedTiraje, 6002);

            total = distribucion + reserva + sede + zaragoza + ventas;

            TxtDistrBase.Text = distribucion.ToString();
            TxtReservaBase.Text = reserva.ToString();
            TxtSedeBase.Text = sede.ToString();
            TxtZaragozaBase.Text = zaragoza.ToString();
            TxtVentasBase.Text = ventas.ToString();

            TxtDistrNueva.Text = distribucion.ToString();
            TxtReservaNueva.Text = reserva.ToString();
            TxtSedeNueva.Text = sede.ToString();
            NudZaragoza.Value = zaragoza;
            NudVentas.Value = ventas;

            LblTotales.Content = total.ToString();
        }

        private void NudZaragoza_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            zaragoza = Convert.ToInt32(NudZaragoza.Value);

            total = distribucion + reserva + sede + zaragoza + ventas;
            LblTotales.Content = total.ToString();

            BtnGenera.IsEnabled = (total == Convert.ToInt32(NudNuevoTiraje.Value)) ? true : false;
        }

        private void NudVentas_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            ventas = Convert.ToInt32(NudVentas.Value);

            total = distribucion + reserva + sede + zaragoza + ventas;
            LblTotales.Content = total.ToString();

            BtnGenera.IsEnabled = (total == Convert.ToInt32(NudNuevoTiraje.Value)) ? true : false;
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGenera_Click(object sender, RoutedEventArgs e)
        {
            foreach (TirajePersonal tiraje in acuerdos)
            {
                if ((Convert.ToInt32(tiraje.Acuerdo) == Convert.ToInt32(NudNuevoTiraje.Value)) && (String.IsNullOrEmpty(TxtDescripcion.Text) || String.IsNullOrWhiteSpace(TxtDescripcion.Text)))
                {
                    MessageBox.Show("Ya existe una plantilla para este tiraje, si aun así deseas generarla agrega una breve descripción (1 o 2 palabras)");
                    return;
                }
            }

            TirajePersonal nuevoAcuerdo = new TirajePersonal() { Acuerdo = NudNuevoTiraje.Value.ToString() };

            model.GeneraAcuerdo(nuevoAcuerdo, TxtDescripcion.Text);

            ObservableCollection<PlantillaDto> plantilla = model.GetPlantillaBase(selectedTiraje.IdAcuerdo);

            PlantillaDto planZaragoza = (from n in plantilla
                                         where n.IdOrganismo == 6001
                                         select n).ToList()[0];

            planZaragoza.Resguardo = Convert.ToInt32(NudZaragoza.Value);

            PlantillaDto planVentas;

            try
            {
                planVentas = (from n in plantilla
                                           where n.IdOrganismo == 6002
                                           select n).ToList()[0];

                planVentas.Resguardo = Convert.ToInt32(NudVentas.Value);
            }
            catch (Exception)
            {
                planVentas = new PlantillaDto();
            }

            bool exito = model.GeneraPlantilla(plantilla, nuevoAcuerdo);

            if (exito)
            {
                MessageBox.Show("Plantilla generada correctamente");
                this.Close();
            }
            else
            {
                MessageBox.Show("No se pudo generar la plantilla, favor de contactar al administrador del sistema");
                this.Close();
            }
        }
    }
}
