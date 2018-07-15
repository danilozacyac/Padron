using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;

namespace Organismos.Ciudades
{
    /// <summary>
    /// Interaction logic for Mantenimiento.xaml
    /// </summary>
    public partial class Mantenimiento : UserControl
    {

        Pais selectedPais;
        Estado selectedEstado;
        Ciudad selectedCiudad;

        public Mantenimiento()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LstPaises.DataContext = PaisesSingleton.Paises;
        }

        private void LstPaises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedPais = LstPaises.SelectedItem as Pais;

                if (selectedPais.Estados == null)
                    selectedPais.Estados = new PaisEstadoModel().GetEstados(selectedPais.IdPais);

                LstEstados.DataContext = selectedPais.Estados;
            }
            catch (NullReferenceException)
            {
                LstPaises.SelectedIndex = -1;
            }
            
        }

        private void LstEstados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedEstado = LstEstados.SelectedItem as Estado;

                if (selectedEstado.Ciudades == null)
                    selectedEstado.Ciudades = new PaisEstadoModel().GetCiudades(selectedEstado.IdEstado);

                LstCiudades.DataContext = selectedEstado.Ciudades;

            }
            catch (NullReferenceException)
            {
                LstEstados.SelectedIndex = -1;
            }
        }

        private void LstCiudades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedCiudad = LstCiudades.SelectedItem as Ciudad;
            }
            catch (NullReferenceException)
            {
                LstCiudades.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Permite agregar Países o Estados en sus respectivos listados
        /// </summary>
        /// <param name="qAgrego">Indica si el elemento que se va a  agregar es un país (1) o un estado (2)</param>
        public void AgregaPaisEstado(int qAgrego)
        {
            if (qAgrego == 1)
            {
                Pais newPais = new Pais();
                PaisEstadoWin add = new PaisEstadoWin(newPais, false) { Owner = this };
                add.ShowDialog();
                
            }
            else
            {
                if (selectedPais == null)
                {
                    MessageBox.Show("Primero debes seleccionar el país al que pertenece el estado que deseas agregar");
                    return;
                }
                else
                {
                    Estado estado = new Estado() { IdPais = selectedPais.IdPais };
                    PaisEstadoWin addEstado = new PaisEstadoWin(estado, false) { Owner = this };
                    addEstado.ShowDialog();

                }
            }
        }

        /// <summary>
        /// Permite modificar la descripción de un país o un estado
        /// </summary>
        /// <param name="qModifico">Indica si el elemento que se va a modificar es un país (1) o un estado (2)</param>
        public void ModificaPaisEstado(int qModifico)
        {

            if (qModifico == 1)
            {
                if (selectedPais == null)
                {
                    MessageBox.Show("Primero debes seleccionar el país que deseas modificar");
                    return;
                }
                PaisEstadoWin edit = new PaisEstadoWin(selectedPais, true) { Owner = this };
                edit.ShowDialog();

            }
            else
            {
                if (selectedEstado == null)
                {
                    MessageBox.Show("Primero debes seleccionar el estado que deseas modificar");
                    return;
                }
                else
                {
                    PaisEstadoWin edit = new PaisEstadoWin(selectedEstado, true) { Owner = this };
                    edit.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Elimina el país o estado seleccionado
        /// </summary>
        /// <param name="qElimino">Indica si el elemento que se va a eliminar es un país (1) o un estado (2)</param>
        public void EliminaPaisEstado(int qElimino)
        {
            PaisEstadoModel model = new PaisEstadoModel();

            if (selectedPais == null)
            {
                MessageBox.Show("Primero debes seleccionar el país que deseas eliminar");
                return;
            }
            else
            {
                if (qElimino == 1)
                {
                    if (selectedPais.Estados != null && selectedPais.Estados.Count == 0)
                    {
                        model.DeletePais(selectedPais);
                    }
                    else
                    {
                        MessageBox.Show("El país que deseas eliminar tiene asociados uno o más estados. Favor de eliminar cada uno de ellos primero");
                        return;
                    }

                }
                else
                {
                    if (selectedEstado == null)
                    {
                        MessageBox.Show("Primero debes seleccionar el estado que deseas eliminar");
                        return;
                    }
                    else
                    {
                        if (selectedEstado.Ciudades != null && selectedEstado.Ciudades.Count == 0)
                        {
                            model.DeleteEstado(selectedEstado);
                        }
                        else
                        {
                            MessageBox.Show("El estado que deseas eliminar tiene asociadas una o más ciudades. Favor de eliminar cada uno de ellas primero");
                            return;
                        }
                    }
                }
            }
        }

        public void AgregaCiudad()
        {
            if (selectedEstado == null)
            {
                MessageBox.Show("Primero debes seleccionar el estado donde se encuentra la ciudad que deseas agregar");
                return;
            }
            else
            {
                AddCiudad addCiudad = new AddCiudad(selectedEstado) { Owner = this };
                addCiudad.ShowDialog();
                
            }
        }

        
        /// <summary>
        /// Modifica la descripción de una ciudad
        /// </summary>
        public void ModificaCiudad()
        {
            if (selectedCiudad == null)
            {
                MessageBox.Show("Primero debes seleccionar la Ciudad que deseas modificar");
                return;
            }
            else
            {
                AddCiudad editCiudad = new AddCiudad(selectedEstado, selectedCiudad) { Owner = this };
                editCiudad.ShowDialog();

            }
        }

        
        /// <summary>
        /// Elimina una ciudad del catálogo, verificando primero si esta ciudad tiene 
        /// organismos relacionados
        /// </summary>
        public void EliminaCiudad()
        {
            if (selectedCiudad == null)
            {
                MessageBox.Show("Primero debes seleccionar la Ciudad que deseas eliminar");
                return;
            }
            else
            {
                PaisEstadoModel model = new PaisEstadoModel();
                bool complete = model.DeleteCiudad(selectedCiudad);

                if (complete)
                {
                    selectedEstado.Ciudades.Remove(selectedCiudad);
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar la ciudad, verifica que no tenga organismos relacionados");
                }
            }
        }
    }
}
