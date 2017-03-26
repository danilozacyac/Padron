using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PadronApi.Model;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for FechaAcuses.xaml
    /// </summary>
    public partial class FechaAcuses
    {

        ObservableCollection<object> plantillas = new ObservableCollection<object>();
        
        /// <summary>
        /// 1. Paquetería 2. Oficina
        /// </summary>
        readonly int tipoAcuse;
        DateTime? fechaRecepcion;

        public FechaAcuses(ObservableCollection<object> plantillas, int tipoAcuse)
        {
            InitializeComponent();
            this.plantillas = plantillas;
            this.tipoAcuse = tipoAcuse;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void DtFechaRecepcion_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            fechaRecepcion = DtFechaRecepcion.SelectedDate;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (tipoAcuse == 1)
            {
                new AcusesModel().UpdateFechaRecepcion(plantillas, "FechaRecPaq", fechaRecepcion, tipoAcuse);
            }
            else
            {
                new AcusesModel().UpdateFechaRecepcion(plantillas, "FechaRecAcu", fechaRecepcion, tipoAcuse);
            }

            DialogResult = true;
            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        
    }
}
