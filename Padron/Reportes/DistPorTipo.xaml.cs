using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;
using Microsoft.Win32;
using PadronApi.Reportes;
using DevExpress.Xpf.Charts;

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
            UDYear.Value = 2016;
            //totales = new ReportesModel().GetDistribucionPorTipo(2016);
            //GReporte.ItemsSource = totales;
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

        private void UDYear_ValueChanged(object sender, Telerik.Windows.Controls.RadRangeBaseValueChangedEventArgs e)
        {
            int selectedYear = Convert.ToInt32(UDYear.Value);

            totales = new ReportesModel().GetDistribucionPorTipo(selectedYear);

            GReporte.ItemsSource = totales;

            if (ChartDist.Diagram.Series != null)
                while (ChartDist.Diagram.Series.Count() > 0)
                    ChartDist.Diagram.Series.RemoveAt(0);


                SetSeriesForDistribution2();
        }

        public BarSideBySideSeries2D SetSeriesForDistribution(TotalPorTipo estadoSerie)
        {
            BarSideBySideSeries2D serie = new BarSideBySideSeries2D() { DisplayName = estadoSerie.Estado };
            FlatGlassBar2DModel mode = new FlatGlassBar2DModel();
            serie.Model = mode;

            serie.Points.Add(new SeriesPoint("CD", estadoSerie.Cd));
            serie.Points.Add(new SeriesPoint("DVD", estadoSerie.Dvd));

            return serie;
        }

        public void SetSeriesForDistribution2()
        {
            BarSideBySideSeries2D serieCD = new BarSideBySideSeries2D() { DisplayName = "CD" };
            FlatGlassBar2DModel mode = new FlatGlassBar2DModel();
            serieCD.Model = mode;

            foreach (TotalPorTipo estadoSerie in totales)
                serieCD.Points.Add(new SeriesPoint(estadoSerie.Estado, estadoSerie.Cd));

            ChartDist.Diagram.Series.Add(serieCD);

            BarSideBySideSeries2D serieDVD = new BarSideBySideSeries2D() { DisplayName = "DVD" };
            FlatGlassBar2DModel modeDVD = new FlatGlassBar2DModel();
            serieDVD.Model = modeDVD;

            foreach (TotalPorTipo estadoSerie in totales)
                serieDVD.Points.Add(new SeriesPoint(estadoSerie.Estado, estadoSerie.Dvd));

            ChartDist.Diagram.Series.Add(serieDVD);

            BarSideBySideSeries2D serieLibro = new BarSideBySideSeries2D() { DisplayName = "Libros" };
            FlatGlassBar2DModel modeLibro = new FlatGlassBar2DModel();
            serieLibro.Model = modeLibro;

            foreach (TotalPorTipo estadoSerie in totales)
                serieLibro.Points.Add(new SeriesPoint(estadoSerie.Estado, estadoSerie.Libro));

            ChartDist.Diagram.Series.Add(serieLibro);


            BarSideBySideSeries2D serieEbook = new BarSideBySideSeries2D() { DisplayName = "eBooks" };
            FlatGlassBar2DModel modeEbook = new FlatGlassBar2DModel();
            serieEbook.Model = modeEbook;

            foreach (TotalPorTipo estadoSerie in totales)
                serieEbook.Points.Add(new SeriesPoint(estadoSerie.Estado, estadoSerie.Ebook));

            ChartDist.Diagram.Series.Add(serieEbook);

            BarSideBySideSeries2D serieAmbos = new BarSideBySideSeries2D() { DisplayName = "Libro y disco" };
            FlatGlassBar2DModel modeAmbos = new FlatGlassBar2DModel();
            serieAmbos.Model = modeAmbos;

            foreach (TotalPorTipo estadoSerie in totales)
                serieAmbos.Points.Add(new SeriesPoint(estadoSerie.Estado, estadoSerie.Ambos));

            ChartDist.Diagram.Series.Add(serieAmbos);

            BarSideBySideSeries2D serieAudio = new BarSideBySideSeries2D() { DisplayName = "Audiolibro" };
            FlatGlassBar2DModel modeAudio = new FlatGlassBar2DModel();
            serieAudio.Model = modeAudio;

            foreach (TotalPorTipo estadoSerie in totales)
                serieAudio.Points.Add(new SeriesPoint(estadoSerie.Estado, estadoSerie.AudioLibro));

            ChartDist.Diagram.Series.Add(serieAudio);

            ChartDist.Animate();
        }


    }
}
