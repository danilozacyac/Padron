using PadronApi.Dto;
using PadronApi.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Padron.ManttoCatalogos
{
    /// <summary>
    /// Interaction logic for TitulosControl.xaml
    /// </summary>
    public partial class TitulosControl : UserControl
    {
        private Titulo selectedTitulo;

        public TitulosControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GTitulos.DataContext = from n in TituloSingleton.Titulos
                                   where n.IdTitulo > 0
                                   orderby n.Orden
                                   select n;
        }

        private void GTitulos_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedTitulo = GTitulos.SelectedItem as Titulo;
        }
    }
}
