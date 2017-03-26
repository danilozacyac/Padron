using System;
using System.Linq;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using Telerik.Windows.Controls;

namespace Padron.ManttoCatalogos
{
    /// <summary>
    /// Interaction logic for Funciones.xaml
    /// </summary>
    public partial class Funciones
    {
        private ElementalProperties selectedFuncion;

        int selectedIndex;

        public Funciones()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LstFunciones.DataContext = FuncionesSingleton.Funciones;
        }

        private void BtnSubir_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFuncion == null)
            {
                MessageBox.Show("Selecciona la función de la cual quieres modificar su posición");
                return;
            }

            int index = FuncionesSingleton.Funciones.IndexOf(selectedFuncion);

            if (index > 0)
            {
                FuncionesSingleton.Funciones.Move(index, index - 1);
                ReasignaOrden();
            }

            LstFunciones.SelectedIndex = selectedIndex + 1;
            selectedIndex = LstFunciones.SelectedIndex;
            LstFunciones.ScrollIntoView(selectedIndex);
        }

        private void BtnBajar_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFuncion == null)
            {
                MessageBox.Show("Selecciona la función de la cual quieres modificar su posición");
                return;
            }
            
            int index = FuncionesSingleton.Funciones.IndexOf(selectedFuncion);

            if (index < FuncionesSingleton.Funciones.Count - 1)
            {
                FuncionesSingleton.Funciones.Move(index, index + 1);
                ReasignaOrden();
            }

            LstFunciones.SelectedIndex = selectedIndex + 1;
            selectedIndex = LstFunciones.SelectedIndex;
            LstFunciones.ScrollIntoView(selectedIndex);
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            foreach (ElementalProperties property in FuncionesSingleton.Funciones)
            {
                ElementalPropertiesModel model = new ElementalPropertiesModel();
                model.UpdateFuncionesOrder(property);
            }
            this.Close();
        }


        private void ReasignaOrden()
        {
            int orden = 1;

            foreach (ElementalProperties funcion in FuncionesSingleton.Funciones)
            {
                funcion.ElementoAuxiliar = orden.ToString();
                orden++;
            }
        }

        private void LstFunciones_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            selectedFuncion = LstFunciones.SelectedItem as ElementalProperties;

            selectedIndex = LstFunciones.SelectedIndex;
            
        }

        private void BtnModificar_Click(object sender, RoutedEventArgs e)
        {
            DialogParameters dialog = new DialogParameters() { Header = "Modificar función", Content = "Ingresa la descripción correcta de la función", Closed = this.OnNewFunctionClosed, DefaultPromptResultValue = selectedFuncion.Descripcion };

            RadWindow.Prompt(dialog);
        }

        private void OnNewFunctionClosed(object sender, WindowClosedEventArgs e)
        {
            string previousDesc = selectedFuncion.Descripcion;

            RadWindow win = sender as RadWindow;

            if (String.IsNullOrEmpty(win.PromptResult))
            {
                MessageBox.Show("La descripción no puede estar vacía");

            }
            else if (win.DialogResult == true)
            {
                selectedFuncion.Descripcion = win.PromptResult;
                if (!new ElementalPropertiesModel().SetFuncion(selectedFuncion))
                {
                    MessageBox.Show("No se pudo completar la operación, favor de volver a intentarlo");
                    selectedFuncion.Descripcion = previousDesc;
                }
            }
        }
    }
}
