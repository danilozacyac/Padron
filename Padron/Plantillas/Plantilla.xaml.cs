using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Padron.Model;
using PadronApi.Dto;
using PadronApi.Model;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using ScjnUtilities;

namespace Padron.Plantillas
{
    /// <summary>
    /// Interaction logic for Plantilla.xaml
    /// </summary>
    public partial class Plantilla : UserControl
    {

        ObservableCollection<PlantillaDto> plantilla = new ObservableCollection<PlantillaDto>();
        Obra obraPadron;
        TirajePersonal tiraje;
        PadronGenerado padronClonar;

        PlantillaDto selectedTitular;

        readonly bool isReadOnly;

        PlantillaDto reservaHistorica, almacenZaragoza, almacenSede, ventas;
        //6000,6001,6002,32630

        int distScjn = 0;
        int distSanL = 0;
        int distAMet = 0;
        int distFora = 0;
        int distAlma = 0;
        int totalZona = 0;

        int particular, personal, oficina, biblioteca, resguardo, autor, totalPropiedad,
            totalDistr, totalZara, totalASede, totalReserva, totalVentas, total;

        int tipoProceso = 1;
        //string fileName = String.Empty;

        bool plantillaSaveComplete = false;

        /// <summary>
        /// Obtiene la plantilla predeterminada para cada uno de los tirajes señalados
        /// </summary>
        /// <param name="obraPadron">Obra de la cual se va a generar el padron</param>
        /// <param name="tiraje">Número total de ejemplares</param>
        public Plantilla(Obra obraPadron, TirajePersonal tiraje)
        {
            InitializeComponent();
            this.obraPadron = obraPadron;
            this.tiraje = tiraje;

            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }
        /// <summary>
        /// Obtiene una copia de un padrón generado previamente con las mismas cantidades
        /// asignadas a cada uno de los titulares
        /// </summary>
        /// <param name="obraPadron">Obra de la cual se va a generar el padrón a partir de otro</param>
        /// <param name="tiraje">Número total de ejemplares</param>
        /// <param name="padronClonar">Padrón que se utilizará como base</param>
        public Plantilla(Obra obraPadron, TirajePersonal tiraje,PadronGenerado padronClonar, bool isReadOnly)
        {
            InitializeComponent();
            this.obraPadron = obraPadron;
            this.tiraje = tiraje;
            this.padronClonar = padronClonar;
            this.isReadOnly = isReadOnly;

            this.GPlantilla.IsReadOnly = isReadOnly;

            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LblTitulo.Content = obraPadron.Titulo;
            LblEjemplares.Content = tiraje.Acuerdo + " ejemplares";

            BusyIndicator.BusyContent = "Cargando plantilla...";
            LaunchBusyIndicator();
        }


        private void AddGroupDescriptor(string columna, string encabezado)
        {
            GroupDescriptor descriptor = new GroupDescriptor()
            {
                Member = columna,
                SortDirection = ListSortDirection.Ascending,
                DisplayContent = encabezado
            };

            this.GPlantilla.GroupDescriptors.Add(descriptor);
        }

        private void GPlantilla_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            selectedTitular = GPlantilla.SelectedItem as PlantillaDto;
        }

        public void IncluyeTitular()
        {
            IncluyeFuncionario incluye = new IncluyeFuncionario(plantilla) { Owner = this };
            incluye.ShowDialog();

            if (incluye.DialogResult == true)
            {
                PlantillaDto nuevo = plantilla[plantilla.Count - 1];

                if (nuevo.Particular > 0)
                {
                    particular += nuevo.Particular;
                    resguardo -= nuevo.Particular;
                }

                if (nuevo.Personal > 0)
                {
                    personal += nuevo.Personal;
                    resguardo -= nuevo.Personal;
                }

                if (nuevo.Oficina > 0)
                {
                    oficina += nuevo.Oficina;
                    resguardo -= nuevo.Oficina;
                }

                if (nuevo.Biblioteca > 0)
                {
                    biblioteca += nuevo.Biblioteca;
                    resguardo -= nuevo.Biblioteca;
                }

                if (nuevo.Autor > 0)
                {
                    autor += nuevo.Autor;
                    resguardo -= nuevo.Autor;
                }

                int totalUsuario = nuevo.Particular + nuevo.Personal + nuevo.Oficina
                    + nuevo.Biblioteca + nuevo.Autor;

                if (nuevo.TipoDistribucion == 1)
                {
                    distScjn += totalUsuario;
                    distAlma -= totalUsuario;
                }
                else if (nuevo.TipoDistribucion == 2)
                {
                    distSanL += totalUsuario;
                    distAlma -= totalUsuario;
                }
                else if (nuevo.TipoDistribucion == 3)
                {
                    distAMet += totalUsuario;
                    distAlma -= totalUsuario;
                }
                else if (nuevo.TipoDistribucion == 4)
                {
                    distFora += totalUsuario;
                    distAlma -= totalUsuario;
                }

                totalDistr += totalUsuario;
                almacenZaragoza.Resguardo -= totalUsuario;
                this.SetTotalesOnTxt();
            }

        }

        public void Excluirtitular()
        {
            if (selectedTitular != null)
            {
                MessageBoxResult result = MessageBox.Show(String.Format("¿Estas seguro de excluir a {0} de la distribución de esta publicación?", selectedTitular.Nombre), "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if(selectedTitular.Particular > 0)
                    {
                        particular -= selectedTitular.Particular;
                        resguardo += selectedTitular.Particular;
                    }

                    if (selectedTitular.Personal > 0)
                    {
                        personal -= selectedTitular.Personal;
                        resguardo += selectedTitular.Personal;
                    }

                    if (selectedTitular.Oficina > 0)
                    {
                        oficina -= selectedTitular.Oficina;
                        resguardo += selectedTitular.Oficina;
                    }

                    if (selectedTitular.Biblioteca > 0)
                    {
                        biblioteca -= selectedTitular.Biblioteca;
                        resguardo += selectedTitular.Biblioteca;
                    }

                    if (selectedTitular.Autor > 0)
                    {
                        autor -= selectedTitular.Autor;
                        resguardo += selectedTitular.Autor;
                    }

                    int totalUsuario = selectedTitular.Particular + selectedTitular.Personal + selectedTitular.Oficina
                        + selectedTitular.Biblioteca + selectedTitular.Autor;

                    if (selectedTitular.TipoDistribucion == 1)
                    {
                        distScjn -= totalUsuario;
                        distAlma += totalUsuario;
                    }
                    else if (selectedTitular.TipoDistribucion == 2)
                    {
                        distSanL -= totalUsuario;
                        distAlma += totalUsuario;
                    }
                    else if (selectedTitular.TipoDistribucion == 3)
                    {
                        distAMet -= totalUsuario;
                        distAlma += totalUsuario;
                    }
                    else if (selectedTitular.TipoDistribucion == 4)
                    {
                        distFora -= totalUsuario;
                        distAlma += totalUsuario;
                    }

                    
                    totalDistr -= totalUsuario;
                    almacenZaragoza.Resguardo += totalUsuario;

                    plantilla.Remove(selectedTitular);

                    this.SetTotalesOnTxt();
                }
            }
            else
            {
                MessageBox.Show("Selecciona al titular que deseas excluir");
            }
        }

        private void RadNumericUpDown_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            RadNumericUpDown control = sender as RadNumericUpDown;


            if (e.OldValue > e.NewValue)
            {
                if (selectedTitular.TipoDistribucion == 1)
                {
                    distScjn -= 1;
                    distAlma += 1;
                }
                else if (selectedTitular.TipoDistribucion == 2)
                {
                    distSanL -= 1;
                    distAlma += 1;
                }
                else if (selectedTitular.TipoDistribucion == 3)
                {
                    distAMet -= 1;
                    distAlma += 1;
                }
                else if (selectedTitular.TipoDistribucion == 4)
                {
                    distFora -= 1;
                    distAlma += 1;
                }
                totalDistr -= 1;
                almacenZaragoza.Resguardo += 1;

                switch (control.Name)
                {
                    case "NudPart": particular -= 1;
                        resguardo += 1;
                        break;
                    case "NudPers": personal -= 1;
                        resguardo += 1;
                        break;
                    case "NudOfic": oficina -= 1;
                        resguardo += 1;
                        break;
                    case "NudBiblio": biblioteca -= 1;
                        resguardo += 1;
                        break;
                    case "NudAutor": autor -= 1;
                        resguardo += 1;
                        break;
                }

            }
            else if (e.OldValue < e.NewValue)
            {
                if (selectedTitular.TipoDistribucion == 1)
                {
                    distScjn += 1;
                    distAlma -= 1;
                }
                else if (selectedTitular.TipoDistribucion == 2)
                {
                    distSanL += 1;
                    distAlma -= 1;
                }
                else if (selectedTitular.TipoDistribucion == 3)
                {
                    distAMet += 1;
                    distAlma -= 1;
                }
                else if (selectedTitular.TipoDistribucion == 4)
                {
                    distFora += 1;
                    distAlma -= 1;
                }

                totalDistr += 1;
                almacenZaragoza.Resguardo -= 1;

                switch (control.Name)
                {
                    case "NudPart": particular += 1;
                        resguardo -= 1;
                        break;
                    case "NudPers": personal += 1;
                        resguardo -= 1;
                        break;
                    case "NudOfic": oficina += 1;
                        resguardo -= 1;
                        break;
                    case "NudBiblio": biblioteca += 1;
                        resguardo -= 1;
                        break;
                    case "NudAutor": autor += 1;
                        resguardo -= 1;
                        break;
                }

            }

            SetTotalesOnTxt();
        }


        private void CalcularTotales()
        {
            foreach (PlantillaDto item in plantilla)
            {
                if (item.TipoDistribucion == 1)
                    distScjn += item.Particular + item.Oficina + item.Biblioteca + item.Personal + item.Autor;
                else if (item.TipoDistribucion == 2)
                    distSanL += item.Particular + item.Oficina + item.Biblioteca + item.Personal + item.Autor;
                else if (item.TipoDistribucion == 3)
                    distAMet += item.Particular + item.Oficina + item.Biblioteca + item.Personal + item.Autor;
                else if (item.TipoDistribucion == 4)
                    distFora += item.Particular + item.Oficina + item.Biblioteca + item.Personal + item.Autor;
                else if (item.TipoDistribucion == 5)
                    distAlma += item.Resguardo;

                totalDistr += item.Particular + item.Oficina + item.Biblioteca + item.Personal + item.Autor;

                particular += item.Particular;
                personal += item.Personal;
                oficina += item.Oficina;
                biblioteca += item.Biblioteca;
                resguardo += item.Resguardo;
                autor += item.Autor;

            }

            total += totalDistr;

            totalZona = distScjn + distSanL + distAMet + distFora + distAlma;
            totalPropiedad = particular + personal + oficina + biblioteca + resguardo + autor;
        }


        private void SetTotalesOnTxt()
        {
            TxtCorte.Text = distScjn.ToString();
            TxtSanL.Text = distSanL.ToString();
            TxtAMetr.Text = distAMet.ToString();
            TxtForan.Text = distFora.ToString();
            TxtAlmacen.Text = distAlma.ToString();
            TxtTotalZona.Text = totalZona.ToString();

            TxtParticular.Text = particular.ToString();
            TxtPersonal.Text = personal.ToString();
            TxtOficina.Text = oficina.ToString();
            TxtBiblioteca.Text = biblioteca.ToString();
            TxtResguardo.Text = resguardo.ToString();
            TxtAutores.Text = autor.ToString();
            TxtTotalPropi.Text = totalPropiedad.ToString();

            TxtTotDistr.Text = totalDistr.ToString();
            TxtTotHist.Text = reservaHistorica.Resguardo.ToString();
            TxtTotSede.Text = almacenSede.Resguardo.ToString();
            TxtTotVenta.Text = ventas.Resguardo.ToString();
            TxtTotZara.Text = almacenZaragoza.Resguardo.ToString();
            TxtTotal.Text = total.ToString();

           
            
        }

        public bool Guardar()
        {
            bool okNumeros = VerificaTotales();

            if (okNumeros)
            {
                int duplicados = this.VerificaDuplicados();

                if (duplicados == 0)
                {

                    MessageBoxResult result = MessageBox.Show("Una vez guardado el padrón no se podrán realizar modificaciones. ¿Deseas continuar?", "Atención:",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        tipoProceso = 2;
                        BusyIndicator.BusyContent = "Guardando distribución...";
                        LaunchBusyIndicator();
                    }
                }
                else
                {
                    MessageBox.Show("No se puede continuar con la operación debido a que existen titulares que se encuentran duplicados. ", "Atención:",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return okNumeros;
        }

        /// <summary>
        /// Verifica que la suma de cada una de los totales de distribución sea mayor a cero,
        /// de lo contrario no permititrá guardar la plantilla de la obra que se está generando
        /// </summary>
        private bool VerificaTotales()
        {

            if (distScjn < 0 || distSanL < 0 || distAMet < 0 || distFora < 0 || distAlma < 0 || totalZona < 0 || particular < 0 || personal < 0 || oficina < 0 || biblioteca < 0 || resguardo < 0 || autor < 0 || totalPropiedad < 0 ||
                totalDistr < 0 || totalZara < 0 || totalASede < 0 || totalReserva < 0 || totalVentas < 0 || total < 0)
            {
                MessageBox.Show("Verifica las cantidades de distribución ya que ninguna de ellas puede ser menor a cero");
                return false;
            }
            else
                return true;
             
            
        }

        /// <summary>
        /// Permite modificar el número de ejemplares asignados en las obras especiales
        /// </summary>
        public void ObrasEspeciales()
        {
            this.ActualizaResguardos(reservaHistorica);
            this.ActualizaResguardos(almacenSede);
            this.ActualizaResguardos(ventas);
            this.ActualizaResguardos(almacenZaragoza);
        }

        private void ActualizaResguardos(PlantillaDto plantilla)
        {
            DialogParameters parameters = new DialogParameters()
            {
                Content = String.Format("Ingresa el número de ejemplares destinados para {0}", plantilla.Organismo),
                Header = String.Format("Ejemplares {0}", plantilla.Organismo),
                DialogStartupLocation = WindowStartupLocation.CenterScreen,
                Closed = this.OnClosed,
                DefaultPromptResultValue = plantilla.Resguardo.ToString()
            };
            RadWindow.Prompt(parameters);
        }

        /// <summary>
        /// Indica la cantidad de ejemplares que se asignaron manualmente para cada uno de los organismos de resguardo
        /// </summary>
        //int cantidadReguardo = 0;
        
        private void OnClosed(object sender, WindowClosedEventArgs e)
        {

        }

        private int VerificaDuplicados()
        {
            return new PadronModel().VerificaDuplicidadTitulares();
        }
       
        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();
        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoProceso == 1)
            {
                new TitularModel().UpdateTotalAlmacenZaragoza(tiraje);


                if (padronClonar == null)
                    plantilla = new PadronModel().GetPlantillaForPadron(tiraje.IdAcuerdo,obraPadron.TipoObra);
                else
                    plantilla = new PadronModel().GetPadronAClonar(padronClonar.IdPadron,obraPadron.TipoObra);


                
                reservaHistorica = (from n in plantilla
                                    where n.IdOrganismo == 32630
                                    select n).ToList()[0];

                try
                {
                    almacenSede = (from n in plantilla
                                   where n.IdOrganismo == 6000
                                   select n).ToList()[0];
                }
                catch (Exception)
                {
                    almacenSede = new PlantillaDto();
                }
               

               try
                {
                    ventas = (from n in plantilla
                              where n.IdOrganismo == 6002
                              select n).ToList()[0];
                }
                catch(Exception )
                {
                    ventas = new PlantillaDto();
                }

                almacenZaragoza = (from n in plantilla
                                   where n.IdOrganismo == 6001
                                   select n).ToList()[0];


                total += reservaHistorica.Resguardo + almacenSede.Resguardo + ventas.Resguardo + almacenZaragoza.Resguardo;
            }
            else if (tipoProceso == 2)
            {
                PadronModel model = new PadronModel();
                plantillaSaveComplete = model.InsertaPadron(plantilla, obraPadron, tiraje);
            }

            
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tipoProceso == 1)
            {
                GPlantilla.DataContext = plantilla;

                this.AddGroupDescriptor("TipoDistribucion", "Distribución");
                this.AddGroupDescriptor("EstadoStr", "Estado");
                this.AddGroupDescriptor("CiudadStr", "Ciudad");
                this.AddGroupDescriptor("Organismo", "Organismo");

                this.CalcularTotales();
                this.SetTotalesOnTxt();

            }
            else if (tipoProceso == 2)
            {
                if (!plantillaSaveComplete)
                {
                    MessageBox.Show("No se pudo completar la operación, favor de volver a intentar");
                    return;
                }
                else
                {
                    GPlantilla.IsReadOnly = true;
                }
            }

            

            //Dispatcher.BeginInvoke(new Action<ObservableCollection<Organismos>>(this.UpdateGridDataSource), e.Result);
            this.BusyIndicator.IsBusy = false;
        }

        private void LaunchBusyIndicator()
        {
            if (!worker.IsBusy)
            {
                this.BusyIndicator.IsBusy = true;
                worker.RunWorkerAsync();

            }
        }

        #endregion

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper().Trim();

            if (!String.IsNullOrEmpty(tempString))
            {
                tempString = StringUtilities.PrepareToAlphabeticalOrder(tempString);

                var temporal = (from n in plantilla
                                where n.Nombre.ToUpper().Contains(tempString) || n.Organismo.ToUpper().Contains(tempString)
                                select n).ToList();
                GPlantilla.DataContext = temporal;
            }
            else
            {
                GPlantilla.DataContext = plantilla;
            }
        }
    }
}
