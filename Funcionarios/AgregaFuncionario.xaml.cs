using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;
using System.Windows.Media.Imaging;

namespace Funcionarios
{
    /// <summary>
    /// Interaction logic for AgregaFuncionario.xaml
    /// </summary>
    public partial class AgregaFuncionario 
    {
        private bool isUpdating;
        private bool mostrarEnCombo;
        private ObservableCollection<Titular> catalogoTitulares;
        private Titular titular;
        string qCambio = String.Empty;

        private Adscripcion selectedAdscripcion;

        public AgregaFuncionario(ObservableCollection<Titular> catalogoTitulares)
        {
            InitializeComponent();
            this.catalogoTitulares = catalogoTitulares;
            this.titular = new Titular() { Adscripciones = new ObservableCollection<Adscripcion>() };
            this.isUpdating = false;
            titular.Estado = 1;
            BtnModificaAdscripcion.Visibility = Visibility.Collapsed;
        }

        public AgregaFuncionario(Titular titular, bool isUpdating)
        {
            InitializeComponent();
            this.titular = titular;
            this.isUpdating = isUpdating;

            if (!isUpdating)
            {
                PanelPrincipal.IsEnabled = false;
                BtnGuardar.Visibility = Visibility.Hidden;
                this.Header = "Ver información";
                BtnSalir.Content = "Salir";
               
            }
            else
            {
                this.Header = "Actualizar titular";
                CbxEstado.Visibility = Visibility.Visible;
                
            }
            mostrarEnCombo = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (titular.IdTitular != 0)
                titular.Adscripciones = new TitularModel().GetAdscripcionesTitular(titular);
            GOrganismos.DataContext = titular.Adscripciones;

            if (titular.Genero == 2)
                TbGenero_Checked(null, null);
            else
                TbGenero.IsChecked = false;

            CbxEstado.DataContext = ElementalPropertiesSingleton.Estatus;

            CbxEstado.IsEnabled = (isUpdating) ? true : false;
            CbxEstado.SelectedValue = titular.Estado;

            this.DataContext = titular;

            if (mostrarEnCombo)
            {


                CbxGrado.SelectedValue = titular.IdTitulo;
                CbxEstado.SelectedValue = titular.Estado;
            }
            else
                CbxEstado.SelectedIndex = 0;

            qCambio = String.Empty;
            BtnModificaAdscripcion.IsEnabled = (titular.QuiereDistribucion == -1) ? false : true;
        }

        private void BtnEliminaAdscripcion_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAdscripcion == null)
            {
                MessageBox.Show("Selecciona el organismo del cual deseas eliminar la adscripción");

            }
            else
            {
                MessageBoxResult result = MessageBox.Show("¿Estas seguro de eliminar la adscripción de este titular?", "Atención",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (titular.IdTitular != 0)
                    {
                        TitularModel model = new TitularModel();
                        model.EliminaAdscripcion(selectedAdscripcion,true);
                    }
                    titular.Adscripciones.Remove(selectedAdscripcion);
                }
            }
        }

        private void BtnAgregaAdscripcion_Click(object sender, RoutedEventArgs e)
        {
            if (titular.Adscripciones == null)
                titular.Adscripciones = new ObservableCollection<Adscripcion>();

                //Mostrar la ventana de seleccion de organismo
            SeleccionaOrgAdscrip org = new SeleccionaOrgAdscrip(titular, isUpdating) { Owner = this };
            org.ShowDialog();
           
        }

        private void BtnModificaAdscripcion_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAdscripcion == null)
            {
                MessageBox.Show("Selecciona la adscripción que deseas modificar");
                return;
            }

            SeleccionaOrgAdscrip org = new SeleccionaOrgAdscrip(selectedAdscripcion) { Owner = this };
            org.ShowDialog();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (CbxGrado.SelectedIndex == -1)
            {
                MessageBox.Show("Selecciona el título con el cual se deben dirigir los oficios al titular", "Agregar Titular", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(TxtNombre.Text) || String.IsNullOrEmpty(TxtApellidos.Text))
            {
                MessageBox.Show("Ingresa el nombre y los apellidos del titular", "Agregar Titular", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!isUpdating && titular.Adscripciones.Count == 0)
            {
                MessageBox.Show("Para continuar debes asignar este titular a un organismo", "Titulares", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
            }

            titular.IdTitulo = Convert.ToInt32(CbxGrado.SelectedValue);
            titular.Estado = Convert.ToInt32(CbxEstado.SelectedValue);
            titular.NombreStr = String.Format("{0} {1}", StringUtilities.PrepareToAlphabeticalOrder(titular.Nombre), StringUtilities.PrepareToAlphabeticalOrder(titular.Apellidos));

            if ((!String.IsNullOrWhiteSpace(titular.Correo) && !String.IsNullOrEmpty(titular.Correo) && !VerificationUtilities.IsMailAddress(titular.Correo)))
            {
                MessageBox.Show("El correo electrónico ingresado no es válido, si no cuentas con uno deja el campo en blanco", "Titulares", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            TitularModel model = new TitularModel();
            bool exito = false;

            if (!isUpdating)
            {
                if (model.DoTitularExist(titular.NombreStr))
                {
                    MessageBox.Show("El titular que deseas agregar ya existe", "Titulares", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            titular.Nombre = VerificationUtilities.TextBoxStringValidation(titular.Nombre);
            titular.Apellidos = VerificationUtilities.TextBoxStringValidation(titular.Apellidos);

            if (isUpdating)
            {
                exito = model.UpdateTitular(titular);

                if (!exito)
                {
                    MessageBox.Show("Hubo un problema con la actualización intentelo nuevamente");
                    return;
                }

                titular.TotalAdscripciones = titular.Adscripciones.Count;

                this.Close();

            }
            else
            {
                exito = model.InsertaTitular(titular);

                if (!exito)
                {
                    MessageBox.Show("Hubo un problema al ingresar el titular intentelo nuevamente");
                    return;
                }
                else
                {
                    catalogoTitulares.Insert(0, titular);

                    if(titular.Adscripciones != null)
                        titular.TotalAdscripciones = titular.Adscripciones.Count;
                    this.Close();
                }
            }
        }

        private void TxtPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = VerificationUtilities.ContieneCaractNoPermitidos(e.Text);
        }


        private void GOrganismos_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedAdscripcion = GOrganismos.SelectedItem as Adscripcion;
        }

        private void TbGenero_Checked(object sender, RoutedEventArgs e)
        {
            ImGenero.Source = new BitmapImage(new Uri("pack://application:,,,/Funcionarios;component/Resources/female_128.png", UriKind.Absolute));
            titular.Genero = 2;
            this.CargaTitulos(2);
            this.CbxGrado.SelectedValue = titular.IdTitulo;
        }

        private void TbGenero_Unchecked(object sender, RoutedEventArgs e)
        {
            ImGenero.Source = new BitmapImage(new Uri("pack://application:,,,/Funcionarios;component/Resources/male_128.png", UriKind.Absolute));
            titular.Genero = 1;
            this.CargaTitulos(1);
            this.CbxGrado.SelectedValue = titular.IdTitulo;
        }

        private void CargaTitulos(int genero)
        {
            CbxGrado.DataContext = (from n in TituloSingleton.Titulos
                                    where n.IdGenero == genero || n.IdGenero == 3
                                    select n);
        }

       
    }
}