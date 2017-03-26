using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;

namespace Organismos
{
    /// <summary>
    /// Interaction logic for SelecccionaFuncionarios.xaml
    /// </summary>
    public partial class SelecccionaFuncionarios 
    {
        private Organismo currentOrganismo;
        private Titular selectedTitular;
        private ObservableCollection<Titular> listaTitulares;
        bool isUpdatingOrganismo;

        ObservableCollection<ElementalProperties> tipoObras;
        ObservableCollection<TirajePersonal> listaTirajes;

        Adscripcion adscripcion;

        bool isUpdatingTiraje = false;

        public SelecccionaFuncionarios(Organismo currentOrganismo, bool isUpdatingOrganismo)
        {
            InitializeComponent();
            this.currentOrganismo = currentOrganismo;
            this.isUpdatingOrganismo = isUpdatingOrganismo;
            this.adscripcion = new Adscripcion();
        }

        public SelecccionaFuncionarios(Adscripcion adscripcion)
        {
            InitializeComponent();
            this.adscripcion = adscripcion;
            this.isUpdatingTiraje = true;
            this.currentOrganismo = adscripcion.Organismo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ElementalPropertiesModel model = new ElementalPropertiesModel();

            //Cargamos los tipos de obra    //Descomentar si se hace uso del panel izquierdo
            //tipoObras = model.GetTipoObra();
            //LstTipoObra.DataContext = tipoObras;

            //Cargamos los tirajes
            listaTirajes = new AcuerdosModel().GetAcuerdos();
            GTirajes.DataContext = listaTirajes;

            CbxFunciones.DataContext = FuncionesSingleton.Funciones;

            if (isUpdatingTiraje)
            {
                GridBuscar.Visibility = Visibility.Collapsed;
                GTitulares.Visibility = Visibility.Collapsed;

                CbxFunciones.SelectedValue = adscripcion.Funcion;

                adscripcion.Tirajes = new TitularModel().GetAcuerdosPorTitular(adscripcion.Titular.IdTitular, adscripcion.Organismo.IdOrganismo);

                foreach (TirajePersonal tiraje in adscripcion.Tirajes)
                {
                    TirajePersonal queTiraje = (from n in listaTirajes
                                                where n.IdAcuerdo == tiraje.IdAcuerdo
                                                select n).ToList()[0];

                    queTiraje.IsChecked = true;
                    queTiraje.Particular = tiraje.Particular;
                    queTiraje.Oficina = tiraje.Oficina;
                    queTiraje.Biblioteca = tiraje.Biblioteca;
                    queTiraje.Resguardo = tiraje.Resguardo;
                    queTiraje.Personal = tiraje.Personal;
                    queTiraje.Autor = tiraje.Autor;

                }
            }
            else
            {
                listaTitulares = new TitularModel().GetTitulares(true);
                GTitulares.DataContext = listaTitulares;

                adscripcion.Organismo = currentOrganismo;

                //Descomentar si se hace uso del panel izquierdo
                //foreach (ElementalProperties item in tipoObras)
                //    adscripcion.ObrasRecibe += "1";

                adscripcion.Tirajes = new ObservableCollection<TirajePersonal>();
            }
            
            //Descomentar si se hace uso del panel izquierdo
            //char[] queObras = adscripcion.ObrasRecibe.ToCharArray();
            //int total = queObras.Count();

            //for (int index = 0; index < total; index++)
            //{
            //    if (queObras[index].Equals('1'))
            //    {
            //        tipoObras[index].IsChecked = true;
            //    }
            //}
            
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper();

            if (!String.IsNullOrEmpty(tempString))
            {
                tempString = StringUtilities.PrepareToAlphabeticalOrder(tempString);

                GTitulares.DataContext = (from n in listaTitulares
                                          where n.NombreStr.ToUpper().Contains(tempString)
                                          select n).ToList();
            }
            else
                GTitulares.DataContext = listaTitulares;
        }

        private void GTitulares_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedTitular = GTitulares.SelectedItem as Titular;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTitular == null && !isUpdatingTiraje )
            {
                MessageBox.Show("Para poder esablecer una relación Organismo-Titular debes seleccionar un funcionario");
                return;
            }
            else
            {

                ObservableCollection<TirajePersonal> nuevoTiraje = new ObservableCollection<TirajePersonal>(GTirajes.Items.Cast<TirajePersonal>());

                var cuantosOk = (from n in nuevoTiraje
                                 where n.IsChecked == true
                                 select n);

                if (cuantosOk.Count() == 0)
                {
                    MessageBox.Show("Para continuar debes incluir al titular en al menos un tiraje");
                    return;
                }

                //adscripcion.ObrasRecibe = String.Empty;
                //Quitar comentarios si en algún momento se hace uso del panel izquierdo
                //foreach (ElementalProperties property in tipoObras)
                //{
                //    if (property.IsChecked)
                //        adscripcion.ObrasRecibe += "1";
                //    else
                //        adscripcion.ObrasRecibe += "0";
                //}

                //Comentar si se hace uso del panel izquierdo
                adscripcion.ObrasRecibe = "1111111";

                foreach (TirajePersonal tiraje in nuevoTiraje)
                {
                    if (tiraje.IsChecked && tiraje.Particular == 0 && tiraje.Oficina == 0 && tiraje.Biblioteca == 0
                        && tiraje.Resguardo == 0 && tiraje.Autor == 0 && tiraje.Personal == 0)
                    {
                        MessageBox.Show("Para incluir al titular en una plantilla debes asignarle al menos un ejemplar");
                        return;
                    }
                }


                if (currentOrganismo.Adscripciones == null)
                    currentOrganismo.Adscripciones = new ObservableCollection<Adscripcion>();


                adscripcion.Funcion = Convert.ToInt32(CbxFunciones.SelectedValue);
                adscripcion.Tirajes = nuevoTiraje;

                if (isUpdatingTiraje)
                {
                    TitularModel model = new TitularModel();
                    model.EliminaAdscripcion(adscripcion, false);
                    model.EstableceAdscripcion(adscripcion, false);
                }
                else
                {
                    adscripcion.Titular = selectedTitular;
                    int yaExiste = 0;

                    foreach (Adscripcion ad in currentOrganismo.Adscripciones)
                    {
                        if (adscripcion.Titular.IdTitular == ad.Titular.IdTitular)
                            yaExiste++;
                    }

                    if (yaExiste == 0)
                    {
                        ObservableCollection<Adscripcion> currentAds = new TitularModel().GetAdscripcionesTitular(selectedTitular);
                        if (isUpdatingOrganismo)
                        {
                            TitularModel model = new TitularModel();
                            model.EstableceAdscripcion(adscripcion, true);
                        }

                        currentOrganismo.Adscripciones.Add(adscripcion);

                        if (currentAds.Count > 0)
                            MessageBox.Show("El titular que acabas de adscribir a este organismo ya se encontraba adscrito a otro organismo");
                    }
                }

                this.Close();
            }

        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
