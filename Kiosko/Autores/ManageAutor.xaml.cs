using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Kiosko.Autores
{
    /// <summary>
    /// Interaction logic for ManageAutor.xaml
    /// </summary>
    public partial class ManageAutor
    {
       private bool isUpdating;
        private bool mostrarEnCombo;
        private ObservableCollection<Autor> catalogoTitulares;
        private Autor titular;
        string qCambio = String.Empty;


        public ManageAutor(ObservableCollection<Autor> catalogoTitulares)
        {
            InitializeComponent();
            this.catalogoTitulares = catalogoTitulares;
            this.titular = new Autor();
            this.isUpdating = false;
            titular.Estado = 1;
        }

        public ManageAutor(Autor titular, bool isUpdating)
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
                this.Header = "Actualizar autor";
            }
            mostrarEnCombo = true;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (titular.Genero == 2)
                TbGenero_Checked(null, null);
            else
                TbGenero.IsChecked = false;

            this.DataContext = titular;

            if (mostrarEnCombo)
            {
                CbxGrado.SelectedValue = titular.IdTitulo;
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {

            var oVldResults = new List<ValidationResult>();
            var oVldContext = new ValidationContext(titular, null, null);

            titular.IdTitulo = Convert.ToInt32(CbxGrado.SelectedValue);
           


            if (Validator.TryValidateObject(titular, oVldContext, oVldResults, true))
            {
                titular.Nombre = VerificationUtilities.TextBoxStringValidation(titular.Nombre);
                titular.Apellidos = VerificationUtilities.TextBoxStringValidation(titular.Apellidos);
                titular.NombreStr = StringUtilities.PrepareToAlphabeticalOrder(titular.Nombre) + " " + StringUtilities.PrepareToAlphabeticalOrder(titular.Apellidos);

                if ((!String.IsNullOrWhiteSpace(titular.Correo) && !String.IsNullOrEmpty(titular.Correo) && !VerificationUtilities.IsMailAddress(titular.Correo)))
                {
                    MessageBox.Show("El correo electrónico ingresado no es válido, si no cuentas con uno deja el campo en blanco", "Titulares", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                AutorModel model = new AutorModel();
                bool exito = false;

                if (!isUpdating)
                {
                    if (new TitularModel().DoTitularExist(titular.NombreStr))
                    {
                        MessageBox.Show("El titular que deseas agregar ya existe", "Titulares", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                if (isUpdating)
                {
                    exito = model.UpdateAutor(titular);

                    if (!exito)
                    {
                        MessageBox.Show("Hubo un problema con la actualización intentelo nuevamente");
                        return;
                    }

                    this.Close();

                }
                else
                {
                    exito = model.InsertaAutor(titular);

                    if (!exito)
                    {
                        MessageBox.Show("Hubo un problema al ingresar el autor intentelo nuevamente");
                        return;
                    }
                    else
                    {
                        catalogoTitulares.Insert(0, titular);

                        this.Close();
                    }
                }
            }
            else
            {
                var sVldErrors = String.Join("\n", oVldResults.Select(t => String.Format("- {0}", t.ErrorMessage)));
                MessageBox.Show(sVldErrors, "Error de validación", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TxtPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = VerificationUtilities.ContieneCaractNoPermitidos(e.Text);
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