using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;

namespace Funcionarios
{
    /// <summary>
    /// Interaction logic for Trayectoria.xaml
    /// </summary>
    public partial class Trayectoria
    {
        Titular titular;

        public Trayectoria(Titular titular)
        {
            InitializeComponent();
            this.titular = titular;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Adscripcion> listaAdscripcion = new TitularModel().GetTrayectoria(titular);
            GTrayectoria.DataContext = listaAdscripcion;
        }
    }
}
