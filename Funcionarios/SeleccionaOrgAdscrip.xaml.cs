using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;

namespace Funcionarios
{
    /// <summary>
    /// Interaction logic for SeleccionaOrgAdscrip.xaml
    /// </summary>
    public partial class SeleccionaOrgAdscrip 
    {
        Adscripcion adscripcion;
       // Organismo organismo;
        ObservableCollection<Organismo> catalogoOrganismo;
        //ObservableCollection<Adscripcion> adscripciones;
        bool isUpdating = false, actualizaTiraje = false;

        Titular titular;
        ObservableCollection<ElementalProperties> tipoObras;
        ObservableCollection<TirajePersonal> listaTirajes;

        public SeleccionaOrgAdscrip(Titular titular,bool isUpdating)
        {
            InitializeComponent();
            this.titular = titular;
            this.adscripcion = new Adscripcion();
            this.isUpdating = isUpdating;
        }


        public SeleccionaOrgAdscrip(Adscripcion adscripcion)
        {
            InitializeComponent();
            this.adscripcion = adscripcion;
            this.isUpdating = true;
            this.actualizaTiraje = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            catalogoOrganismo = new OrganismoModel().GetOrganismoForAdscripcion();
            GOrganismos.DataContext = catalogoOrganismo;
            CbxFunciones.DataContext = FuncionesSingleton.Funciones;

            //ElementalPropertiesModel model = new ElementalPropertiesModel();

            //Cargamos los tipos de obra      //Descomentar si se hace uso del panel izquierdo
            //tipoObras = model.GetTipoObra();
            //LstTipoObra.DataContext = tipoObras;

            //Cargamos los tirajes
            listaTirajes = new AcuerdosModel().GetAcuerdos();
            GTirajes.DataContext = listaTirajes;

            if (isUpdating && actualizaTiraje)
            {
                PanelOrganismo.Visibility = Visibility.Collapsed;
                SearchBox.Visibility = Visibility.Collapsed;

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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (adscripcion.Organismo == null || (GOrganismos.SelectedItem == null && isUpdating == false))
            {
                MessageBox.Show("Debes seleccionar el organismo al cual esta adscrito el titular, de los contrario presiona cancelar");
                return;
            }

            ObservableCollection<TirajePersonal> nuevoTiraje = new ObservableCollection<TirajePersonal>(GTirajes.Items.Cast<TirajePersonal>());

            var cuantosOk = (from n in nuevoTiraje
                             where n.IsChecked == true
                             select n);

            if (cuantosOk.Count() == 0)
            {
                MessageBox.Show("Para continuar debes incluir al titular en al menos un tiraje");
                return;
            }

            //Descomentar si se hace uso del panel izquierdo
            //adscripcion.ObrasRecibe = String.Empty;
            
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

            adscripcion.Funcion = Convert.ToInt32(CbxFunciones.SelectedValue);
            adscripcion.Tirajes = nuevoTiraje;


            if (isUpdating && actualizaTiraje)
            {
                TitularModel model = new TitularModel();
                model.EliminaAdscripcion(adscripcion, false);
                model.EstableceAdscripcion(adscripcion, false);
            }
            else if (isUpdating)
            {
                TitularModel model = new TitularModel();
                adscripcion.Titular = titular;
                model.EstableceAdscripcion(adscripcion, true);

                if (titular.Adscripciones == null)
                    titular.Adscripciones = new ObservableCollection<Adscripcion>();

                titular.Adscripciones.Add(adscripcion);
            }
            else
            {
                titular.Adscripciones.Add(adscripcion);
            }


            DialogResult = true;
            this.Close();
        }

        private void GOrganismos_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            adscripcion.Organismo = GOrganismos.SelectedItem as Organismo;
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper().Trim();

            long digitos = 0;
            Int64.TryParse(tempString, out digitos);

            if (!String.IsNullOrEmpty(tempString))
            {
                if (digitos == 0)
                    tempString = StringUtilities.PrepareToAlphabeticalOrder(tempString);

                GOrganismos.DataContext = (from n in catalogoOrganismo
                                           where n.OrganismoStr.Contains(tempString)
                                           select n).ToList();
            }
            else
                GOrganismos.DataContext = catalogoOrganismo;
        }

       
    }
}
