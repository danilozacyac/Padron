using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Kiosko.Dto;

namespace Kiosko
{
    /// <summary>
    /// Interaction logic for ObrasKiosko.xaml
    /// </summary>
    public partial class ObrasKiosko
    {
        //private readonly string noPictureString = String.Format("{0}{1}", ConfigurationManager.AppSettings["Imagenes"], "picture.png");

        private readonly string catalograficaRootDir = ConfigurationManager.AppSettings["Catalografica"];

        private Obra obra;
        private bool isUpdating;

        private string rutaImagen = String.Empty;

        private bool doImageChange = false;

        public ObrasKiosko(Obra obra, bool isUpdating)
        {
            InitializeComponent();
            this.obra = obra;
            this.isUpdating = isUpdating;
            
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CbxPresentacion.DataContext = ElementalPropertiesSingleton.Presentacion;
            CbxTipoObra.DataContext = new ElementalPropertiesModel().GetTipoObra();
            CbxTipoPub.DataContext = ElementalPropertiesSingleton.TipoPublicacion;
            CbxMedioPub.DataContext = ElementalPropertiesSingleton.MedioPublicacion;
            CbxIdioma.DataContext = ElementalPropertiesSingleton.Idioma;
            CbxPais.DataContext = PaisesSingleton.Paises;
            

            this.DataContext = obra;
            LoadData();

            if (!isUpdating)
            {
                BtnImagePath.Visibility = Visibility.Collapsed;
                BtnAutores.Visibility = Visibility.Collapsed;
                BtnGuardar.Visibility = Visibility.Collapsed;
                BtnCancelar.Content = "Salir";
            }

            DatosObra.IsEnabled = isUpdating;
        }

        private void LoadData()
        {
            try
            {
                CbxPresentacion.SelectedValue = obra.Presentacion;
                CbxTipoObra.SelectedValue = obra.TipoObra;
                CbxTipoPub.SelectedValue = obra.IdTipoPublicacion;
                CbxMedioPub.SelectedValue = obra.MedioPublicacion;
                CbxIdioma.SelectedValue = obra.IdIdioma;
                CbxPais.SelectedValue = obra.Pais;
                CargaAutores();

                if (!String.IsNullOrEmpty(obra.ImagePath))
                {
                    BtnImagePath.Visibility = Visibility.Collapsed;
                    BtnDelImagePath.Visibility = Visibility.Visible;
                }

                if (!String.IsNullOrEmpty(obra.Catalografica) && File.Exists(String.Format("{0}{1}", catalograficaRootDir, obra.Catalografica)))
                {
                    BtnAddCatalografica.Visibility = Visibility.Collapsed;
                    BtnVerCatalografica.Visibility = Visibility.Visible;
                    BtnDelCatalografica.Visibility = Visibility.Visible;
                }
                else
                {
                    BtnAddCatalografica.Visibility = Visibility.Visible;
                    BtnVerCatalografica.Visibility = Visibility.Collapsed;
                    BtnDelCatalografica.Visibility = Visibility.Collapsed;
                }
               
            }
            catch (FileNotFoundException) { }
        }

        private void CargaAutores()
        {
            AutorModel model = new AutorModel();

            List<Autor> autores = model.GetAutores(obra).ToList();
            List<Autor> instituciones = model.GetInstituciones(obra).ToList();

            autores.AddRange(instituciones);

            GAutores.GAutorObra.DataContext = (from n in autores
                                               orderby n.NombreCompleto
                                               select n);

            if (autores.Count > 0)
                obra.Autores = new ObservableCollection<Autor>(autores);
            else
                obra.Autores = new ObservableCollection<Autor>();

            if (autores.Count > 1)
                BtnTipcolabora.Visibility = Visibility.Visible;
            else
                BtnTipcolabora.Visibility = Visibility.Collapsed;

        }

        private void BtnImagePath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog() { Filter = "JPG (.jpg)|*.jpg|PNG (.png)|*.png|All Files (*.*)|*.*", FilterIndex = 1, Multiselect = false };

                doImageChange = true;
                if (open.ShowDialog() == true)
                {
                    File.Copy(open.FileName, String.Format("{0}{1}{2}", ConfigurationManager.AppSettings["Imagenes"], obra.IdObra, Path.GetExtension(open.FileName)), true);

                    obra.ImagePath = String.Format("{0}{1}", obra.IdObra, Path.GetExtension(open.FileName));
                    rutaImagen = open.FileName;

                    BtnDelImagePath.Visibility = Visibility.Visible;
                    BtnImagePath.Visibility = Visibility.Collapsed;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Modificar la imagen de la obra constantemente puede provocar errores, vuelve a intentarlo más tarde");
            }
        }

        private void BtnAutores_Click(object sender, RoutedEventArgs e)
        {
            RelAutorObras setAutores = new RelAutorObras(obra) { Owner = this };
            setAutores.ShowDialog();

            if (setAutores.DialogResult == true)
            {
                CargaAutores();
            }

        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {

            if ((TxtIsbn.Text.Length > 0) && !VerificationUtilities.IsbnValidation(TxtIsbn.Text))
            {
                MessageBoxResult result = MessageBox.Show("El número de ISBN que ingresaste es incorrecto sino cuentas con el y deseas continuar presiona SI y el campo se vaciará, para revisar el ISBN presiona NO", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    TxtIsbn.Text = String.Empty;
                else
                    return;
            }

            obra.Presentacion = Convert.ToInt32(CbxPresentacion.SelectedValue);
            obra.TipoObra = Convert.ToInt32(CbxTipoObra.SelectedValue);
            obra.IdTipoPublicacion = Convert.ToInt32(CbxTipoPub.SelectedValue);
            obra.MedioPublicacion = Convert.ToInt32(CbxMedioPub.SelectedValue);
            obra.Pais = Convert.ToInt32(CbxPais.SelectedValue);
            obra.IdIdioma = Convert.ToInt32(CbxIdioma.SelectedValue);

            bool sinTweakApply = false;
            if((obra.TipoObra == 1 || obra.TipoObra == 2) && (String.IsNullOrEmpty(obra.Sintesis) || String.IsNullOrWhiteSpace(obra.Sintesis))){
                obra.Sintesis = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
                sinTweakApply = true;
            }

            var oVldResults = new List<ValidationResult>();
            var oVldContext = new ValidationContext(obra, null, null);

            if (Validator.TryValidateObject(obra, oVldContext, oVldResults, true))
            {
                if (sinTweakApply)
                    obra.Sintesis = String.Empty;

                bool complete = new ObraModel().UpdateObraKiosko(obra);

                if (complete)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo completar la operación, favor de volver a intentarlo");
                }
            }
            else
            {
                var sVldErrors = String.Join("\n", oVldResults.Select(t => String.Format("- {0}", t.ErrorMessage)));
                ErrorUtilities.MostrarMensajes("ERRORES DE VALIDACIÓN", sVldErrors);
            }


        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (doImageChange)
                obra.ImagePath = String.Empty;

            this.Close();
        }



        private void Txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = VerificationUtilities.IsNumber(e.Text);
        }

        

        private void Txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void BtnDelImagePath_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Estás segur@ de eliminar la imagen asociada a esta obra? Una vez realizada la acción no podrá deshacerse", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {

                new ObraModel().UpdateImagenObra(obra.IdObra, String.Empty);
                obra.ImagePath = String.Empty;

                BtnDelImagePath.Visibility = Visibility.Collapsed;
                BtnImagePath.Visibility = Visibility.Visible;

                doImageChange = true;
                
            }
        }

        private void BtnAddCatalografica_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog() { Filter = "PDF (.pdf)|*.pdf|All Files (*.*)|*.*", FilterIndex = 1, Multiselect = false };

                if (open.ShowDialog() == true)
                {
                    string newPath = String.Format("{0}PadronCata{1}{2}", ConfigurationManager.AppSettings["Catalografica"], obra.IdObra, Path.GetExtension(open.FileName));

                    File.Copy(open.FileName, newPath, true);

                    obra.Catalografica = String.Format("PadronCata{0}{1}", obra.IdObra, Path.GetExtension(open.FileName));

                    new ObraModel().UpdateCatalografica(obra.IdObra, obra.Catalografica);

                    MessageBox.Show("La ficha catalográfica se agregó correctamente");

                    BtnAddCatalografica.Visibility = Visibility.Collapsed;
                    BtnVerCatalografica.Visibility = Visibility.Visible;
                    BtnDelCatalografica.Visibility = Visibility.Visible;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Modificar la imagen de la obra constantemente puede provocar errores, vuelve a intentarlo más tarde");
            }



           

        }

        private void BtnVerCatalografica_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(String.Format("{0}{1}", catalograficaRootDir, obra.Catalografica));
        }

        private void BtnDelCatalografica_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("¿Estás segur@ de eliminar la ficha catalográfica asociada a esta obra? Una vez realizada la acción no podrá deshacerse", "Atención:", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                File.Delete(String.Format("{0}{1}", catalograficaRootDir, obra));

                obra.Catalografica = String.Empty;
                new ObraModel().UpdateCatalografica(obra.IdObra, String.Empty);
                MessageBox.Show("La ficha catalográfica se eliminó correctamente");

                BtnAddCatalografica.Visibility = Visibility.Visible;
                BtnVerCatalografica.Visibility = Visibility.Collapsed;
                BtnDelCatalografica.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnTipcolabora_Click(object sender, RoutedEventArgs e)
        {
            int totalNoAsigna = new AutorColabora().GetAutoresSinColaboracion(obra);

            if (totalNoAsigna == 1)
            {
                MessageBox.Show("Existe un autor sin asignación de tipo de colaboración en la obra. Favor de revisar antes de continuar.");
                return;
            }
            else if (totalNoAsigna > 1)
            {
                MessageBox.Show(String.Format("Existen {0} autores sin asignación de tipo de colaboración en la obra. Favor de revisar antes de continuar",totalNoAsigna));
                return;
            }
            else
            {
                TextoColaboracion colaboraWin = new TextoColaboracion(obra) { Owner= this};
                colaboraWin.ShowDialog();
            }
        }
    }
}
