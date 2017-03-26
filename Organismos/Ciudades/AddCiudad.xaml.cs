using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Organismos.Ciudades
{
    /// <summary>
    /// Interaction logic for Ciudad.xaml
    /// </summary>
    public partial class AddCiudad 
    {
        Estado estado;
        Pais selectedPais;
        Ciudad ciudad;
        bool isUpdating;

        string ciudadOriginal;

        public AddCiudad()
        {
            InitializeComponent();
        }

        public AddCiudad(Estado estado)
        {
            InitializeComponent();
            this.estado = estado;
            this.isUpdating = false;
            ciudad = new Ciudad();
        }

        public AddCiudad(Estado estado, Ciudad ciudad)
        {
            InitializeComponent();
            this.estado = estado;
            this.ciudad = ciudad;
            isUpdating = true;
            ciudadOriginal = ciudad.CiudadDesc;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxPais.DataContext = PaisesSingleton.Paises;
            CbxPais.SelectedValue = estado.IdPais;

            this.DataContext = ciudad;

            TxtCiudad.Focus();
        }

        private void CbxPais_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedPais = CbxPais.SelectedItem as Pais;

            CbxEstado.DataContext = selectedPais.Estados;
            CbxEstado.SelectedItem = estado;
        }

        private void TxtCiudad_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;

            box.Text = VerificationUtilities.TextBoxStringValidation(box.Text);
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TxtCiudad.Text) || String.IsNullOrWhiteSpace(TxtCiudad.Text))
            {
                MessageBox.Show("Para continuar debes ingresar el nombre de la ciudad que deseas agregar, de lo contrario presiona Cancelar");
                return;
            }

            PaisEstadoModel model = new PaisEstadoModel();
            bool complete = false;

            if (isUpdating)
            {
                ciudad.CiudadDesc = TxtCiudad.Text;
                ciudad.CiudadStr = StringUtilities.PrepareToAlphabeticalOrder(TxtCiudad.Text);

                complete = model.UpdateCiudad(ciudad);

                if (complete)
                {
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo completar la operación, intentalo nuevamente");
                    return;
                }

            }
            else
            {
                ciudad.CiudadDesc = TxtCiudad.Text;
                ciudad.CiudadStr = StringUtilities.PrepareToAlphabeticalOrder(TxtCiudad.Text);
                ciudad.IdEstado = estado.IdEstado;

                complete = model.InsertaCiudad(ciudad);

                if (complete)
                {
                    estado.Ciudades.Insert(0, ciudad);
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo completar la operación, intentalo nuevamente");
                    return;
                }
            }
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void RadWindow_PreviewClosed(object sender, Telerik.Windows.Controls.WindowPreviewClosedEventArgs e)
        {
            if (isUpdating && DialogResult != true)
            {
                ciudad.CiudadDesc = ciudadOriginal;
            }
        }
    }
}
