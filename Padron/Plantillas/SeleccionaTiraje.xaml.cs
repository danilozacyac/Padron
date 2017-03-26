using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Padron.Model;
using PadronApi.Dto;
using PadronApi.Model;

namespace Padron.Plantillas
{
    /// <summary>
    /// Interaction logic for SeleccionaTiraje.xaml
    /// </summary>
    public partial class SeleccionaTiraje 
    {
        public Obra SelectedObra;
        public TirajePersonal Tiraje;
        PadronGenerado padronClonado;

        public SeleccionaTiraje()
        {
            InitializeComponent();
        }


        public SeleccionaTiraje(PadronGenerado padronClonado)
        {
            InitializeComponent();
            this.padronClonado = padronClonado;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxObra.DataContext = new PadronModel().GetObrasSinPadron();

            CbxTiraje.DataContext = new AcuerdosModel().GetAcuerdos();

            if (padronClonado != null)
            {
                CbxTiraje.SelectedValue = padronClonado.IdAcuerdo;
                CbxTiraje.IsEnabled = false;
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            Tiraje = CbxTiraje.SelectedItem as TirajePersonal;
            

            DialogResult = true;
            this.Close();

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void CbxObra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedObra = CbxObra.SelectedItem as Obra;
        }
    }
}
