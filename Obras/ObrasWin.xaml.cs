using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PadronApi.Dto;
using PadronApi.Model;
using ScjnUtilities;
using System.Windows.Controls;

namespace Obras
{
    /// <summary>
    /// Interaction logic for ObrasWin.xaml
    /// </summary>
    public partial class ObrasWin 
    {
        private Obra obraOrigen;
        private Obra obra;
        private bool isUpdating;
        private ObservableCollection<Obra> catalogoObras;
        private bool llenarCombos;

        public ObrasWin(ObservableCollection<Obra> catalogoObras)
        {
            InitializeComponent();
            this.obra = new Obra();
            this.isUpdating = false;
            this.catalogoObras = catalogoObras;
        }

        public ObrasWin(Obra obra, bool isUpdating)
        {
            InitializeComponent();
            this.obra = obra;
            this.obraOrigen = new ObraModel().GetObras(obra.IdObra);
            this.isUpdating = isUpdating;
            this.llenarCombos = !isUpdating;

            if (!isUpdating)
            {
                PanelCentral.IsEnabled = false;
                BtnGuardar.Visibility = Visibility.Hidden;
                BtnSalir.Content = "Salir";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = obra;

            if (!isUpdating)
            {
                NudYear.Minimum = DateTime.Now.Year - 1;
                NudYear.Maximum = DateTime.Now.Year + 1;
                NudYear.Value = DateTime.Now.Year;
            }
            else
            {
                NudYear.Minimum = 1800;
                NudYear.Maximum = 2100;
            }

            CbxPresentacion.DataContext = new ElementalPropertiesModel().GetPresentacion();
            CbxTipoObra.DataContext = new ElementalPropertiesModel().GetTipoObra();

            CbxPresentacion.SelectedValue = obra.Presentacion;
            CbxTipoObra.SelectedValue = obra.TipoObra;
            
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TxtTitulo.Text) || String.IsNullOrWhiteSpace(TxtTitulo.Text))
            {
                MessageBox.Show("Para dar de alta una obra debes ingresar su título");
                return;
            }

            if (String.IsNullOrEmpty(TxtNumMaterial.Text) || String.IsNullOrWhiteSpace(TxtNumMaterial.Text))
            {
                MessageBox.Show("Para dar de alta una obra debes ingresar el número de material");
                return;
            }
            else if (TxtNumMaterial.Text.Length < 8)
            {
                MessageBox.Show("El número de material debe contener al menos 8 cifras");
                return;
            }

            if ((TxtIsbn.Text.Length > 0) && !VerificationUtilities.IsbnValidation(TxtIsbn.Text))
            {
                MessageBoxResult result = MessageBox.Show("El número de ISBN que ingresaste es incorrecto sino cuentas con el y deseas continuar presiona SI y el campo se vaciará, para revisar el ISBN presona NO", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    TxtIsbn.Text = String.Empty;
                else
                    return;
            }

            if (CbxPresentacion.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar la presentación de la obra");
                return;
            }

            if (CbxTipoObra.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar el tipo de obra");
                return;
            }

           


            bool exito = false;

            ElementalProperties pres = CbxPresentacion.SelectedItem as ElementalProperties;
            ElementalProperties tipo = CbxTipoObra.SelectedItem as ElementalProperties;

            obra.Presentacion = pres.IdElemento;
            obra.TipoObra = tipo.IdElemento;

             ObraModel model = new ObraModel();

             if ((obra.NumMaterial != null && !String.IsNullOrEmpty(obra.NumMaterial.Trim())) && !isUpdating)
             {
                 if (model.CheckIfExist("C_Obra", "NumeroMaterial", obra.NumMaterial))
                 {
                     MessageBox.Show("El número de material que estas ingresando ya fue asignado a otra obra. Favor de verificar");
                     return;
                 }
             }

             if ((obra.Isbn != null && !String.IsNullOrEmpty(obra.Isbn.Trim())) && !isUpdating)
             {
                 if (model.CheckIfExist("C_Obra", "ISBN", obra.Isbn))
                 {
                     MessageBox.Show("El número de ISBN que estas ingresando ya fue asignado a otra obra. Favor de verificar");
                     return;
                 }
             }

            obra.Titulo = VerificationUtilities.TextBoxStringValidation(obra.Titulo);
            obra.NumMaterial = VerificationUtilities.TextBoxStringValidation(obra.NumMaterial);



            if (isUpdating)
            {
                exito = model.UpdateObra(obra);

                if (!exito)
                {
                    MessageBox.Show("Algo salio mal al intentar actualizar, por favor vuelve a intentarlo");
                    return;
                }
            }
            else
            {
                exito = model.InsertaObra(obra);

                if (!exito)
                {
                    MessageBox.Show("Algo salio mal al intentar agregar la obra al catálogo, por favor vuelve a intentarlo");
                    return;
                }

                catalogoObras.Insert(0, obra);
            }

            DialogResult = true;

            this.Close();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = StringUtilities.IsTextAllowed(e.Text);
        }

        private void TxtIsbn_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TxtIsbn.Text) || !String.IsNullOrWhiteSpace(TxtIsbn.Text))
            {
                if (!VerificationUtilities.IsbnValidation(TxtIsbn.Text))
                {
                    MessageBox.Show("El número de ISBN que ingresaste es incorrecto, sino cuentas con el deja el campo en blanco", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
           
        }

        private void TxtNumMaterial_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (TxtNumMaterial.Text.Contains("-") && e.Text.Equals("-"))
            {
                e.Handled = true;
                return;
            }

            if (TxtNumMaterial.Text.Length > 20)
            {
                e.Handled = true;
                return;
            }

            e.Handled = VerificationUtilities.IsNumberOrGuion(e.Text);
            TxtNumMaterial.Text = TxtNumMaterial.Text.Trim();
        }

        private void TxtsLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;

            box.Text = VerificationUtilities.TextBoxStringValidation(box.Text);
        }

        private void TxtIsbn_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = VerificationUtilities.IsNumberOrGuion(e.Text);
            
        }

        private void TxtsPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void CbxPresentacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ElementalProperties presentacion = CbxPresentacion.SelectedItem as ElementalProperties;

            if (presentacion.IdElemento != 4)
                CbxTipoObra.DataContext = new ElementalPropertiesModel().GetTipoObra(0);
            else
                CbxTipoObra.DataContext = new ElementalPropertiesModel().GetTipoObra(4);
        }

        private void RadWindow_PreviewClosed(object sender, Telerik.Windows.Controls.WindowPreviewClosedEventArgs e)
        {
            if (DialogResult != true && obraOrigen != null && PanelCentral.IsEnabled == true)
            {
                obra.Titulo = obraOrigen.Titulo;
                obra.NumMaterial = obraOrigen.NumMaterial;
                obra.AnioPublicacion = obraOrigen.AnioPublicacion;
                obra.Isbn = obraOrigen.Isbn;
                obra.Presentacion = obraOrigen.Presentacion;
                obra.TipoObra = obraOrigen.TipoObra;
                obra.Tiraje = obraOrigen.Tiraje;
            }
        }



       
    }
}