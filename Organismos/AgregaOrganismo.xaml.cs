using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;

namespace Organismos
{
    /// <summary>
    /// Interaction logic for AgregaOrganismo.xaml
    /// </summary>
    public partial class AgregaOrganismo 
    {
        private Pais selectedPais;
        private Estado selectedEstado;
        private Ciudad selectedCiudad;

        private Adscripcion selectedAdscripcion;
        private TipoOrganismo selectedTipoOrg;

        private ObservableCollection<Organismo> listaOrganismos;
        private ObservableCollection<Materia> listaMaterias;

        private Organismo organismo;

        /// <summary>
        /// Permite generar un nuevo registro de Organismo y al finalizar agregar dicho organismo al listado previo
        /// </summary>
        /// <param name="listaOrganismos"></param>
        public AgregaOrganismo(ObservableCollection<Organismo> listaOrganismos)
        {
            InitializeComponent();
            this.listaOrganismos = listaOrganismos;
            this.organismo = new Organismo();
            this.organismo.Adscripciones = new ObservableCollection<Adscripcion>();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CbxTipoOrg.DataContext = ElementalPropertiesSingleton.TipoOrganismo;
            CbxDistribucion.DataContext = ElementalPropertiesSingleton.Distribucion;
            CbxCircuito.DataContext = new ElementalPropertiesModel().GetCircuitos();
            CbxOrdinal.DataContext = new ElementalPropertiesModel().GetOrdinales();

            CbxPais.DataContext = PaisesSingleton.Paises;
            CbxPais.SelectedValue = 39;

            listaMaterias = new Materia().GetMaterias();
            CbxMateria1.DataContext = listaMaterias;
            //CbxMateria2.DataContext = listaMaterias;
            //CbxMateria3.DataContext = listaMaterias;

            this.DataContext = organismo;
            GridIntegrantes.DataContext = organismo.Adscripciones;

        }


        private void CbxTipoOrg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TxtOrganismo.Text = String.Empty;
            selectedTipoOrg = CbxTipoOrg.SelectedItem as TipoOrganismo;

            if (selectedTipoOrg.IdGrupo == 0)
            {
                GpxMaterias.IsEnabled = false;
                CbxOrdinal.IsEnabled = false;
                CbxCircuito.IsEnabled = false;
                LblDescripcion.IsEnabled = false;
                //TxtOrganismo.IsEnabled = true;

                CbxCircuito.SelectedIndex = -1;
                CbxOrdinal.SelectedIndex = -1;

                CbxMateria1.SelectedValue = null;
                CbxMateria2.SelectedValue = null;
                CbxMateria3.SelectedValue = null;

            }
            else
            {
                GpxMaterias.IsEnabled = true;
                CbxOrdinal.IsEnabled = true;
                CbxCircuito.IsEnabled = true;
                LblDescripcion.IsEnabled = true;
                //TxtOrganismo.IsEnabled = false;
            }
        }

        private void RbtnAgregaFuncionario_Click(object sender, RoutedEventArgs e)
        {
            SelecccionaFuncionarios select = new SelecccionaFuncionarios(organismo,false);
            select.Owner = this;
            select.ShowDialog();
        }

        private void RbtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAdscripcion == null)
            {
                MessageBox.Show("Antes de continuar debes seleccionar al titular que ya no pertenece a esta integración");
                return;
            }

            MessageBoxResult result = MessageBox.Show("¿Estas seguro de que este funcionario ya no es integrante de este tribunal?", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                organismo.Adscripciones.Remove(selectedAdscripcion);
            }
        }

        #region Valida informacion

        private void TxtPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (TxtCp.Text.Length >= 5)
            {
                e.Handled = true;
                return;
            }

            e.Handled = VerificationUtilities.IsNumber(e.Text);
            TxtCp.Text.Trim();
        }

        private void TxtTelValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = VerificationUtilities.IsNumberOrGuion(e.Text);
        }

        private void TxtsLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;

            if (!String.IsNullOrEmpty(box.Text) || !String.IsNullOrWhiteSpace(box.Text))
                box.Text = VerificationUtilities.TextBoxStringValidation(box.Text);

        }

        
        #endregion


        #region Direccion

        private void CbxPais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPais = CbxPais.SelectedItem as Pais;


            if (selectedPais.Estados == null)
                selectedPais.Estados = new PaisEstadoModel().GetEstados(selectedPais.IdPais);

            CbxEstado.DataContext = selectedPais.Estados;
            CbxEstado.IsEnabled = true;
            CbxCiudad.DataContext = null;
        }

        private void CbxEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedEstado = CbxEstado.SelectedItem as Estado;

                if (selectedEstado.Ciudades == null)
                    selectedEstado.Ciudades = new PaisEstadoModel().GetCiudades(selectedEstado.IdEstado);

                if (selectedPais.IdPais == 39 && CbxCircuito.IsEnabled)
                {
                    Ordinales selec = (from n in ElementalPropertiesSingleton.Circuitos
                                       where n.IdEstado == selectedEstado.IdEstado
                                       select n).ToList()[0];

                    CbxCircuito.SelectedValue = selec.IdOrdinal;
                }

                CbxCiudad.DataContext = selectedEstado.Ciudades;
                CbxCiudad.IsEnabled = true;
            }
            catch (NullReferenceException)
            {
                CbxEstado.SelectedIndex = -1;
            }
        }

       

        private void CbxCiudad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedCiudad = CbxCiudad.SelectedItem as Ciudad;

            }
            catch (NullReferenceException)
            {
                CbxCiudad.SelectedIndex = -1;
            }
        }

        #endregion

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTipoOrg == null)
            {
                MessageBox.Show("Antes de continuar debes de seleccionar el tipo de organismo");
                return;
            }

            if ((selectedTipoOrg.IdTipoOrganismo > 2 && selectedTipoOrg.IdTipoOrganismo < 10) && (CbxOrdinal.SelectedIndex == -1 || CbxCircuito.SelectedIndex == -1))
            {
                MessageBox.Show("Antes de continuar debes de seleccionar el Circuito y Ordinal del organismo que intentas crear");
                return;
            }
            else
            {
                organismo.Ordinal = Convert.ToInt32(CbxOrdinal.SelectedValue);
                organismo.Circuito = Convert.ToInt32(CbxCircuito.SelectedValue);

                if (CbxMateria1.SelectedIndex != -1)
                {
                    organismo.Materia += ((Materia)CbxMateria1.SelectedItem).IdMateria.ToString();

                    if (CbxMateria2.SelectedIndex != -1)
                    {
                        organismo.Materia += ((Materia)CbxMateria2.SelectedItem).IdMateria.ToString();

                        if (CbxMateria3.SelectedIndex != -1)
                        {
                            organismo.Materia += ((Materia)CbxMateria3.SelectedItem).IdMateria.ToString();
                        }
                    }
                }
                else
                {
                    organismo.Materia = "0";
                }
            }

            if (String.IsNullOrEmpty(TxtOrganismo.Text) || String.IsNullOrWhiteSpace(TxtOrganismo.Text))
            {
                MessageBox.Show("Para continuar ingresa el nombre o descripción del organismo que intentas crear");
                return;
            }

            if (CbxDistribucion.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar el tipo de distribución");
                return;
            }

            if (CbxEstado.SelectedIndex == -1)
            {
                MessageBox.Show("Debes seleccionar el estado donde esta localizado el organismo");
                return;
            }

            if (String.IsNullOrEmpty(TxtCalle.Text) || String.IsNullOrWhiteSpace(TxtCalle.Text))
            {
                MessageBox.Show("Ingresa la calle y número donde se encuentra ubicado el organismo");
                return;
            }
            else if (TxtCalle.Text.StartsWith("Calle"))
            {
                char[] letras = TxtCalle.Text.ToCharArray();

                if (!VerificationUtilities.IsNumber(letras[7].ToString()))
                {
                    MessageBox.Show("El campo Calle no puede contener la palabra calle");
                    return;
                }

            }

            if (TxtCalle.Text.StartsWith("Col ") || TxtCalle.Text.StartsWith("Col. ") || TxtCalle.Text.StartsWith("Colonia "))
            {
                MessageBox.Show("El campo Colonia no puede contener la palabra colonia, ni ninguna de sus abreviaturas");
                return;
            }

            if (TxtCalle.Text.StartsWith("Del ") || TxtCalle.Text.StartsWith("Del. ") || TxtCalle.Text.StartsWith("Delegacion ") || TxtCalle.Text.StartsWith("Delegación ") || TxtCalle.Text.StartsWith("Municipio ") || TxtCalle.Text.StartsWith("Mun. ") || TxtCalle.Text.StartsWith("Mun "))
            {
                MessageBox.Show("El campo Dlegación/Municipio no puede contener estas palabras, ni ninguna de sus abreviaturas");
                return;
            }


            if ((String.IsNullOrEmpty(TxtCp.Text) || String.IsNullOrWhiteSpace(TxtCp.Text)) || TxtCp.Text.Length < 4)
            {
                MessageBox.Show("Ingresa un Código Postal válido");
                return;
            }

            if (selectedPais.IdPais == 1 && (String.IsNullOrEmpty(TxtDelMun.Text) || String.IsNullOrWhiteSpace(TxtDelMun.Text)))
            {
                if (selectedEstado.IdEstado == 9)
                {
                    MessageBox.Show("Ingresa la delegación donde se encuentra ubicado el organismo");
                    return;
                }
                else
                {
                    MessageBox.Show("Ingresa el Municipio donde se encuentra ubicado el organismo");
                    return;
                }
            }

            organismo.OrganismoDesc = TxtOrganismo.Text;
            organismo.TipoOrganismo = Convert.ToInt32(CbxTipoOrg.SelectedValue);
            organismo.TipoDistr = Convert.ToInt32(CbxDistribucion.SelectedValue);
            organismo.Estado = Convert.ToInt32(CbxEstado.SelectedValue);
            organismo.Distribucion = CbxDistribucion.Text;
            organismo.TipoOrganismoStr = CbxTipoOrg.Text;
            organismo.Activo = 1;

            if (CbxCiudad.SelectedIndex != -1)
                organismo.Ciudad = Convert.ToInt32(CbxCiudad.SelectedValue);

            organismo.OrganismoStr = StringUtilities.PrepareToAlphabeticalOrder(organismo.OrganismoDesc);

            OrganismoModel model = new OrganismoModel();

            if (model.DoOrganismoExist(organismo.OrganismoStr))
            {
                MessageBox.Show("El organismo que estas intentando crear ya existe, si no lo ves en el listado verifica el listado de organismos desactivados", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                bool complete = model.InsertaOrganismo(organismo);

                if (complete)
                {
                    listaOrganismos.Insert(0, organismo);
                    if (organismo.Adscripciones != null)
                        organismo.TotalAdscritos = organismo.Adscripciones.Count;

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hubo un error al intentar crear el organismo, intentelo nuevamente", "Atención", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LblDescripcion_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string materias = String.Empty;

            int mat1 = Convert.ToInt16(CbxMateria1.SelectedValue);

            if (mat1 != 0)
            {


                if (mat1 == 2 || mat1 == 6 || mat1 == 7)
                    materias += "en Materia de " + CbxMateria1.Text;
                else
                    materias += "en Materia " + CbxMateria1.Text;

                int mat2 = Convert.ToInt16(CbxMateria2.SelectedValue);

                if (mat2 != 0)
                {
                    string materia2 = String.Empty;

                    if (mat2 == 2 || mat2 == 6 || mat2 == 7)
                        materia2 = "de " + CbxMateria2.Text;
                    else
                        materia2 = CbxMateria2.Text;

                    int mat3 = Convert.ToInt16(CbxMateria3.SelectedValue);

                    if (mat3 != 0)
                    {
                        string materia3 = String.Empty;

                        if (mat3 == 2 || mat3 == 6 || mat3 == 7)
                            materia3 = "de " + CbxMateria3.Text;
                        else
                            materia3 = CbxMateria3.Text;


                        materias += ", " + materia2 + " y " + materia3 + " ";
                    }
                    else
                    {
                        materias += " y " + materia2 + " ";
                    }
                    materias = materias.Replace("Materia", "Materias");
                }
                BtnAceptar.IsEnabled = true;
            }

            if (selectedTipoOrg.IdTipoOrganismo == 2)
            {
                if (CbxCircuito.SelectedIndex != -1)
                    TxtOrganismo.Text = ((CbxOrdinal.SelectedIndex != -1) ? CbxOrdinal.Text + " " : String.Empty) +
                        "Tribunal Colegiado " + materias + "del " + CbxCircuito.Text;
                else
                    MessageBox.Show("Selecciona el circuito al cual pertenece el tribunal");
            }
            else if (selectedTipoOrg.IdTipoOrganismo == 4)
            {
                if (CbxCircuito.SelectedIndex != -1)
                    TxtOrganismo.Text = ((CbxOrdinal.SelectedIndex != -1) ? CbxOrdinal.Text + " " : String.Empty) +
                        "Tribunal Unitario " + materias + " del " + CbxCircuito.Text;
                else
                    MessageBox.Show("Selecciona el circuito al cual pertenece el tribunal");
            }
            else if (selectedTipoOrg.IdTipoOrganismo == 8)
            {
                if (CbxCircuito.SelectedIndex != -1)
                    TxtOrganismo.Text = "Juzgado " + ((CbxOrdinal.SelectedIndex != -1) ? CbxOrdinal.Text + " " : String.Empty) + " de Distrito " +
                         materias + " del " + CbxCircuito.Text;
                else
                    MessageBox.Show("Selecciona el ordinal y el circuito al cual pertenece el tribunal");
            }
        }

        private void CbxMateria1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int materia1 = Convert.ToInt16(CbxMateria1.SelectedValue);

            if (materia1 > 0)
            {
                CbxMateria2.IsEnabled = true;
                CbxMateria2.DataContext = (from n in listaMaterias
                                           where n.IdMateria != materia1
                                           select n);
            }
            else
            {
                CbxMateria2.IsEnabled = false;
                CbxMateria2.SelectedValue = null;
                CbxMateria2_SelectionChanged(null, null);
            }
        }

        private void CbxMateria2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int materia2 = Convert.ToInt16(CbxMateria2.SelectedValue);

            if (materia2 > 0)
            {
                CbxMateria3.IsEnabled = true;
                CbxMateria3.DataContext = (from n in listaMaterias
                                           where n.IdMateria != Convert.ToInt16(CbxMateria1.SelectedValue) &&
                                           n.IdMateria != Convert.ToInt16(CbxMateria2.SelectedValue)
                                           select n);
            }
            else
            {
                CbxMateria3.IsEnabled = false;
                CbxMateria3.SelectedValue = null;
            }
        }

        private void TxtCp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void CbxCircuito_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ordinales ordinal = CbxCircuito.SelectedItem as Ordinales;

            CbxEstado.SelectedValue = ordinal.IdEstado;
        }

        private void GridIntegrantes_SelectionChanged_1(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            selectedAdscripcion = GridIntegrantes.SelectedItem as Adscripcion;
        }

        

        

        
    }
}