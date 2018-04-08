using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;
using Microsoft.Win32;
using PadronApi.Reportes;

namespace Padron.Reportes
{
    /// <summary>
    /// Interaction logic for DistPorTipo.xaml
    /// </summary>
    public partial class DistPorTipo
    {

        ObservableCollection<TotalPorTipo> totales;
        string fileName;

        public DistPorTipo()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UDYear.Maximum = DateTime.Now.Year;

            totales = new ReportesModel().GetDistribucionPorTipo(2016);

            GReporte.ItemsSource = totales;
        }

        private void RBtnExport_Click(object sender, RoutedEventArgs e)
        {
            if (totales == null)
            {
                MessageBox.Show("Antes de exportar debes seleccionar el periodo de información", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            
                save.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";

            Nullable<bool> result = save.ShowDialog();
            if (result == true)
            {
                fileName = save.FileName;
            }

            ExcelReports report = new ExcelReports(totales, fileName,0);
            report.InformeDistribucionPortipo();
        }
    }
}
