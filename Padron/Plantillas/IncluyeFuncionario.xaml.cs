using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Padron.Model;
using PadronApi.Converter;
using PadronApi.Dto;
using PadronApi.Model;

namespace Padron.Plantillas
{
    /// <summary>
    /// Interaction logic for IncluyeFuncionario.xaml
    /// </summary>
    public partial class IncluyeFuncionario
    {

        private ObservableCollection<PlantillaDto> incluidosPlantilla;
        private ObservableCollection<Titular> listaTitulares;
        private Titular selectedTitular;

        public IncluyeFuncionario(ObservableCollection<PlantillaDto> incluidosPlantilla)
        {
            InitializeComponent();
            this.incluidosPlantilla = incluidosPlantilla;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<int> incluidos = new List<int>();

            foreach(PlantillaDto item in incluidosPlantilla)
                incluidos.Add(item.IdTitular);

            listaTitulares = new PadronModel().GetTitularesNoincluidos(incluidos);

            GTitulares.DataContext = listaTitulares;
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper();

            if (!String.IsNullOrEmpty(tempString))
                GTitulares.DataContext = (from n in listaTitulares
                                          where n.NombreStr.ToUpper().Contains(tempString)
                                          select n).ToList();
            else
                GTitulares.DataContext = listaTitulares;
        }

        private void GTitulares_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedTitular = GTitulares.SelectedItem as Titular;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTitular == null)
            {
                MessageBox.Show("Antes de continuar debes seleccionar al titular que deseas incluir en esta distribución");
                return;
            }
            else
            {
                ObservableCollection<Organismo> listaOrgs = null;

                if (selectedTitular.TotalAdscripciones == 1)
                    listaOrgs = new OrganismoModel().GetOrganismos(selectedTitular);
                else
                {
                    listaOrgs = new ObservableCollection<Organismo>();
                    listaOrgs.Add(new OrganismoModel().GetOrganismos(49470));
                }

                foreach (Organismo organismo in listaOrgs)
                {

                    PlantillaDto nuevo = new PlantillaDto()
                    {
                        IdTitular = selectedTitular.IdTitular,
                        Nombre = String.Format("{0} {1}", selectedTitular.Nombre, selectedTitular.Apellidos),
                        IdOrganismo = organismo.IdOrganismo,
                        Organismo = organismo.OrganismoDesc,
                        CiudadStr = new CiudadesConverter().Convert(organismo.Ciudad, null, null, null).ToString(),
                        EstadoStr = new EstadoConverter().Convert(organismo.Estado, null, null, null).ToString(),
                        TipoDistribucion = organismo.TipoDistr,
                        Particular = Convert.ToInt16(NudParticular.Value),
                        Personal = Convert.ToInt16(NudPersonal.Value),
                        Oficina = Convert.ToInt16(NudOficina.Value),
                        Biblioteca = Convert.ToInt16(NudBiblioteca.Value),
                        Resguardo = Convert.ToInt16(NudResguardo.Value),
                        Autor = Convert.ToInt16(NudAutor.Value)
                    };

                    incluidosPlantilla.Add(nuevo);
                }
                DialogResult = true;
                this.Close();
            }

        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
