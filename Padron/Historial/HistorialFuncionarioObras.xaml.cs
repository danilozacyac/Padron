using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using PadronApi.Converter;
using PadronApi.Dto;
using PadronApi.Reportes;
using Padron.Cierre;
using PadronApi.Model;

namespace Padron.Historial
{
    /// <summary>
    /// Interaction logic for HistorialFuncionarioObras.xaml
    /// </summary>
    public partial class HistorialFuncionarioObras 
    {
        private readonly Titular titular;
        private ObservableCollection<Devoluciones> recibio;
        private Devoluciones selectedPlantilla;

        public HistorialFuncionarioObras(Titular titular)
        {
            InitializeComponent();
            this.titular = titular;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtTitular.Text = new TituloConverter().Convert(titular.IdTitulo, null, null, null).ToString() + " " +
                titular.Nombre + " " + titular.Apellidos;

            recibio = new DevolucionModel().GetHistorialTitularObras(titular);
            GPlantilla.DataContext = recibio;
        }

        private void BtnReporteContraloria_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";

            Nullable<bool> result = save.ShowDialog();
            if (result == true)
            {
                string fileName = save.FileName;

                ObservableCollection<Devoluciones> obrasImprime = new ObservableCollection<Devoluciones>(from n in recibio
                                                                                                         where n.Oficina > 0 || n.Biblioteca > 0
                                                                                                         select n);
                if (obrasImprime.Count == 0)
                {
                    MessageBox.Show("Este titular no tiene ha recibido obras en propiedad de la oficina o biblioteca", "ATENCIÓN:", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                new PdfReports(fileName).ReporteFuncionarioContraloria(titular,obrasImprime);
            }
        }

        private void BtnDevolver_Click(object sender, RoutedEventArgs e)
        {
            Devolucion devs = new Devolucion(selectedPlantilla, 1);
            devs.Owner = this;
            devs.ShowDialog();
            selectedPlantilla.TotalDevoluciones = new DevolucionModel().GetTotalDevueltosFuncObra(selectedPlantilla.IdTitular, selectedPlantilla.IdObra);

        }

        private void GPlantilla_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedPlantilla = GPlantilla.SelectedItem as Devoluciones;
            if (selectedPlantilla.Oficina == 0 || selectedPlantilla.Oficina == selectedPlantilla.TotalDevoluciones)
            {
                BtnDetalle.IsEnabled = false;
                //BtnCancelacion.IsEnabled = false;
            }
            else
            {
                BtnDetalle.IsEnabled = true;
                //BtnCancelacion.IsEnabled = true;
            }
        }

        private void BtnCancelacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estas segur@ que esta entrega fue cancelada?", "ATENCIÓN:", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DevolucionModel dev = new DevolucionModel();
                bool completed = dev.SetCancelacion(selectedPlantilla);

                if (completed)
                    selectedPlantilla.TipoDevolucion = 1;
                else
                {
                    MessageBox.Show("No se pudo completar la devolución, intentalo de nuevo más tarde");
                }
            }

        }
    }
}
