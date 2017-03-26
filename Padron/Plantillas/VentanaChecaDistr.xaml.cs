using System;
using System.Linq;
using System.Windows;
using PadronApi.Dto;

namespace Padron.Plantillas
{
    /// <summary>
    /// Interaction logic for VentanaChecaDistr.xaml
    /// </summary>
    public partial class VentanaChecaDistr
    {
        Obra obra;
        TirajePersonal tiraje;
        PadronGenerado padronAMostrar;

        public VentanaChecaDistr(Obra obra, TirajePersonal tiraje, PadronGenerado padronAMostrar)
        {
            InitializeComponent();

            this.obra = obra;
            this.tiraje = tiraje;
            this.padronAMostrar = padronAMostrar;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Plantilla plantillacontrol = new Plantilla(obra, tiraje, padronAMostrar, true);
            CentralPanel.Children.Add(plantillacontrol);
        }
    }
}
