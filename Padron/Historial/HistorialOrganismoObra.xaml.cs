using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using Padron.Model;

namespace Padron.Historial
{
    /// <summary>
    /// Interaction logic for HistorialOrganismoObra.xaml
    /// </summary>
    public partial class HistorialOrganismoObra 
    {

        ObservableCollection<PlantillaDto> obrasRecibidas;
        PlantillaDto selectedObra;
        Organismo organismo;

        public HistorialOrganismoObra(Organismo organismo)
        {
            InitializeComponent();
            this.organismo = organismo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            obrasRecibidas = new PadronModel().GetHistorialOrganismoObras(organismo.IdOrganismo);
            GObrasRecibidas.DataContext = obrasRecibidas;
        }
    }
}
