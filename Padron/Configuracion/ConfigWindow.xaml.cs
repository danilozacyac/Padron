using System;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;

namespace Padron.Configuracion
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow
    {
        PadConfigModel model;

        public ConfigWindow()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            model = new PadConfigModel();
            model.GetConfiguraciones();

            this.TxtTitular.Text = PadConfiguracion.Titular;
            this.TxtRubricas.Text = PadConfiguracion.Rubricas;
            this.TxtLeyendaOficio.Text = PadConfiguracion.LeyendaOficio;
            this.TxtNumOficio.Text = PadConfiguracion.NumOficio;
            this.TxtAclaraciones.Text = PadConfiguracion.TxtAclaraciones;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            PadConfiguracion.Titular = this.TxtTitular.Text;
            PadConfiguracion.Rubricas = this.TxtRubricas.Text;
            PadConfiguracion.LeyendaOficio = this.TxtLeyendaOficio.Text;
            PadConfiguracion.NumOficio = this.TxtNumOficio.Text;
            PadConfiguracion.TxtAclaraciones = this.TxtAclaraciones.Text;

            model.UpdateConfiguraciones();
            this.Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Los cambios no guardados no serán aplicados. ¿Deseas continuar?", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if(result == MessageBoxResult.Yes)
                this.Close();
        }
    }
}
