using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Padron.Model;
using Padron.Plantillas;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Reportes;
using Telerik.Windows.Controls;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for PadGenerados.xaml
    /// </summary>
    public partial class PadGenerados : UserControl
    {
        ObservableCollection<PadronGenerado> padrones = new ObservableCollection<PadronGenerado>();
        PadronGenerado selectedPadron;

        ObservableCollection<PlantillaDto> plantilla;

        RadWindow parentWindow;

        int tipoProceso = 1;
        /// <summary>
        /// Hace referencia a las 5 partes de distribución
        /// </summary>
        int parte = 0;
        string fileName = String.Empty;

        public PadGenerados()
        {
            InitializeComponent();
            worker.DoWork += this.WorkerDoWork;
            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tipoProceso = 1;
            BusyIndicator.BusyContent = "Cargando padrones...";
            LaunchBusyIndicator();

            parentWindow = RadWindow.GetParentRadWindow(this);
        }

        private void GPadrones_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            selectedPadron = GPadrones.SelectedItem as PadronGenerado;
        }

        /// <summary>
        /// Permite realizar una copia exacta de un padrón de distribución ya generado para ser aplicado a una 
        /// nueva obra
        /// </summary>
        /// <returns>Devuelve el identificador del padrón que se esta clonando</returns>
        public PadronGenerado ClonarPadron()
        {
            return selectedPadron;
        }

        /// <summary>
        /// Muestra el detalle de un padrón previamente generado y permite la captura de las fechas de
        /// recepción, tanto de paquetería como del acuse. Permite además adjuntar los archivos de respaldo
        /// de dichos acuses
        /// </summary>
        public void VerAcuses()
        {
            Acuses acuse = new Acuses(null);
            acuse.Owner = parentWindow;
            acuse.ShowDialog();
        }


        public void VerAcusesPorObra()
        {
            if (selectedPadron == null)
            {
                MessageBox.Show("Para continuar debes de seleccionar la obra de la cual quieras ver los acuses");
                return;
            }

            Acuses acuse = new Acuses(selectedPadron);
            acuse.Owner = parentWindow;
            acuse.ShowDialog();
        }

        private bool VerificaFechaDistribucion()
        {
            if (selectedPadron == null)
            {
                MessageBox.Show("Slecciona un padron");
                return false;
            }
            if (selectedPadron.FechaDistribucion == null)
            {
                FechaDistribucion addFecha = new FechaDistribucion(selectedPadron);
                addFecha.ShowDialog();

                if (addFecha.DialogResult != true)
                {
                    MessageBox.Show("Antes de continuar debes ingresar la fecha de distribución");
                    return false;
                }
            }
            return true;
        }

        public void ImprimeAcusesFirma(int parte)
        {
            if (this.VerificaFechaDistribucion())
            {
                this.parte = parte;

                tipoProceso = 2;
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*";

                Nullable<bool> result = save.ShowDialog();
                if (result == true)
                {
                    fileName = save.FileName;

                    BusyIndicator.BusyContent = "Generando acuses para firma...";
                    LaunchBusyIndicator();
                }
            }
        }

        public void ImprimeListado(int parte)
        {
            this.parte = parte;

            if (this.VerificaFechaDistribucion())
            {
                tipoProceso = 3;
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";

                Nullable<bool> result = save.ShowDialog();
                if (result == true)
                {
                    fileName = save.FileName;

                    BusyIndicator.BusyContent = "Generando listado organismos foraneos...";
                    LaunchBusyIndicator();
                }
            }
        }

        public void ImprimeOficios()
        {
            if (this.VerificaFechaDistribucion())
            {
                tipoProceso = 4;

                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";

                Nullable<bool> result = save.ShowDialog();

                if (result == true)
                {
                    fileName = save.FileName;
                    BusyIndicator.BusyContent = "Generando oficios...";
                    LaunchBusyIndicator();
                }
            }
        }

        public void GeneraEtiquetas()
        {
            if (this.VerificaFechaDistribucion())
            {
                tipoProceso = 5;

                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*";

                Nullable<bool> result = save.ShowDialog();

                if (result == true)
                {
                    fileName = save.FileName;
                    BusyIndicator.BusyContent = "Generando etiquetas...";
                    LaunchBusyIndicator();
                }
            }
        }

        public void ImprimeContraloria()
        {
            if (this.VerificaFechaDistribucion())
            {
                tipoProceso = 6;

                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "Word Files (*.docx)|*.docx|All Files (*.*)|*.*";

                Nullable<bool> result = save.ShowDialog();
                if (result == true)
                {
                    BusyIndicator.BusyContent = "Generando listado contraloría...";
                    fileName = save.FileName;
                    LaunchBusyIndicator();
                }
            }
        }

        /// <summary>
        /// Permite revisar la manera en que se distribuyó la obra seleccionada
        /// </summary>
        public void VerificaDistribucion()
        {
            if (selectedPadron == null)
            {
                MessageBox.Show("Debes de seleccionar la distribución que quieres revisar", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TirajePersonal tiraje = new AcuerdosModel().GetAcuerdos(selectedPadron.IdAcuerdo);
            Obra obra = new ObraModel().GetObras(selectedPadron.IdObra);

            VentanaChecaDistr distr = new VentanaChecaDistr(obra, tiraje, selectedPadron);
            distr.Owner = parentWindow;
            distr.ShowDialog();

        }


        public void SetAcuerdoZero()
        {
            if (selectedPadron == null)
            {
                MessageBox.Show("Para continuar selecciona el CD-ROM o Libro de la Gaceta del Semanario al cual se le eliminará el número de acuerdo");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Para continuar confirma que es un CD-ROM o libro de la Gaceta del Semanario", "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                new PadronModel().SetNumeroAcuerdoZero(selectedPadron);
            }
        }

        public void GetCadenaControl()
        {
            if (selectedPadron == null)
            {
                RadWindow.Alert("Debes seleccionar una obra para poder obtener su cadena de control");
                return;
            }
            Clipboard.SetText(String.Format("{0}{1}{2}","&&",selectedPadron.IdObra,"&&"));
            RadWindow.Prompt(new TextBlock() { Text = String.Format("{0}{1}{2}", "La cadena de control para la obra ", selectedPadron.TituloObra, " se copio en el Portapapeles y es la siguiente:"), TextWrapping = TextWrapping.Wrap, Width = 300 }, this.OnClosed, String.Format("{0}{1}{2}", "&&", selectedPadron.IdObra, "&&"));
        }

        public void EliminaPadron()
        {
            if (selectedPadron == null)
            {
                RadWindow.Alert("Debes seleccionar una obra para poder obtener su cadena de control");
                return;
            }

            MessageBoxResult result = MessageBox.Show(String.Format("¿Estás seguro de eliminar el padron de la obra {0}?. Esta acción no podrá revertirse",selectedPadron.TituloObra), "Atención", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                new PadronModel().EliminarPadron(selectedPadron);
                padrones.Remove(selectedPadron);
            }
        }

        private void OnClosed(object sender, WindowClosedEventArgs e)
        {

        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e)
        {
            String tempString = ((TextBox)sender).Text.ToUpper();

            if (!String.IsNullOrEmpty(tempString))
            {
                var resultado = (from n in padrones
                                 where n.TituloObra.ToUpper().Contains(tempString) ||
                                 n.TituloObraStr.ToUpper().Contains(tempString)
                                 select n).ToList();
                GPadrones.DataContext = resultado;
            }
            else
            {
                GPadrones.DataContext = padrones;
            }

        }

        #region Background Worker

        private BackgroundWorker worker = new BackgroundWorker();

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            if (tipoProceso == 1)
            {
                padrones = new PadronModel().GetPadronGenerado();
                
            }

            if (tipoProceso == 2) // Listado Firmas Word SCJN
            {
                plantilla = new ReportesModel().GetDetallesDePadron(selectedPadron.IdPadron, selectedPadron.IdAcuerdo, parte);
                
                WordReports report = new WordReports(plantilla, fileName, selectedPadron.TituloObra);
                report.AcusesReciboFirma();
            }

            if (tipoProceso == 3) //Listados excel
            {
                plantilla = new ReportesModel().GetDetallesDePadron(selectedPadron.IdPadron, selectedPadron.IdAcuerdo, parte);
                ExcelReports report = new ExcelReports(plantilla, fileName, selectedPadron.TituloObra,parte);
                report.ListaDistribucion();
            }

            if (tipoProceso == 4) // Oficios
            {
                PdfReports acuses = new PdfReports(fileName, selectedPadron);
                acuses.GeneraOficiosEnvio(selectedPadron.IdObra, selectedPadron.IdPadron);
            }

            if (tipoProceso == 5) // Etiquetas
            {
                plantilla = new ReportesModel().GetDetallesDePadron(selectedPadron.IdPadron, selectedPadron.IdAcuerdo, 4);

                WordReports acuses = new WordReports(plantilla, fileName, selectedPadron.TituloObra, selectedPadron.OficioInicial);
                acuses.GeneraEtiquetas();
            }

            if (tipoProceso == 6) //Contraloria
            {
                int rowNumber = 0;
                plantilla = new PadronModel().GetDetallesDePadron(selectedPadron.IdPadron, selectedPadron.IdAcuerdo, out rowNumber);

                WordReports report = new WordReports(plantilla, fileName, selectedPadron.TituloObra);
                report.ListadoContraloria(rowNumber);
            }
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tipoProceso == 1)
            {
                GPadrones.DataContext = padrones;
            }

            fileName = String.Empty;

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

       
    }
}