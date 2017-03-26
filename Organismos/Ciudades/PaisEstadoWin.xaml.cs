using System;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;

namespace Organismos.Ciudades
{
    /// <summary>
    /// Interaction logic for PaisEstado.xaml
    /// </summary>
    public partial class PaisEstadoWin 
    {
        private bool isUpdating;

        private Pais pais;
        private Estado estado;

        private string paisOriginal;
        private string estadoOriginal;

        public PaisEstadoWin(Pais pais,bool isUpdating)
        {
            InitializeComponent();
            this.pais = pais;
            this.isUpdating = isUpdating;
            paisOriginal = pais.PaisDesc;
        }

        public PaisEstadoWin(Estado estado, bool isUpdating)
        {
            InitializeComponent();
            this.estado = estado;
            this.isUpdating = isUpdating;
            estadoOriginal = estado.EstadoDesc;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (pais != null)
            {
                TxtPaisEstado.Text = pais.PaisDesc;
            }
            else
            {
                TxtPaisEstado.Text = estado.EstadoDesc;
            }
            TxtPaisEstado.Focus();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TxtPaisEstado.Text) || String.IsNullOrWhiteSpace(TxtPaisEstado.Text))
            {
                MessageBox.Show("Para poder continuar debes ingresar el nombre del país o estado que deseas agregar");
                return;

            }
            else
            {
                PaisEstadoModel model = new PaisEstadoModel();

                if (pais != null)
                {
                    pais.PaisDesc = TxtPaisEstado.Text;
                    bool complete = false;

                    if (isUpdating)
                    {
                        complete = model.UpdatePais(pais);
                        this.Close();
                    }
                    else
                    {
                        complete = model.InsertaPais(pais);

                        if (complete)
                        {
                            PaisesSingleton.Paises.Add(pais);
                            DialogResult = true;
                            this.Close();
                        }
                    }
                }
                else
                {
                    estado.EstadoDesc = TxtPaisEstado.Text;
                    bool complete = false;

                    if (isUpdating)
                    {
                        complete = model.UpdateEstado(estado);
                        this.Close();
                    }
                    else
                    {
                        complete = model.InsertaEstado(estado);

                        if (complete)
                        {
                            Pais myPais = (from n in PaisesSingleton.Paises
                                           where n.IdPais == estado.IdPais
                                           select n).ToList()[0];

                            if (myPais.Estados == null)
                                myPais.Estados = new System.Collections.ObjectModel.ObservableCollection<Estado>();

                            myPais.Estados.Add(estado);
                            DialogResult = true;
                            this.Close();
                        }
                    }

                }
            }
        }

        private void TxtPaisEstado_LostFocus(object sender, RoutedEventArgs e)
        {
            TxtPaisEstado.Text = VerificationUtilities.TextBoxStringValidation(TxtPaisEstado.Text);
        }


        private void RadWindow_PreviewClosed(object sender, Telerik.Windows.Controls.WindowPreviewClosedEventArgs e)
        {
            if (DialogResult != true)
            {
                if (pais != null)
                {
                    pais.PaisDesc = paisOriginal;
                }
                else
                {
                    estado.EstadoDesc = estadoOriginal;
                }
            }
        }
    }
}
