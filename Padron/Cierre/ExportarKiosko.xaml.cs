using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;

namespace Padron.Cierre
{
    /// <summary>
    /// Interaction logic for ExportarKiosko.xaml
    /// </summary>
    public partial class ExportarKiosko
    {
        public ExportarKiosko()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExportaKiosko_Click(object sender, RoutedEventArgs e)
        {
            ExportarModel model = new ExportarModel();
            model.DeleteCurrentInfo();


            foreach (ElementalProperties element in ElementalPropertiesSingleton.Idioma)
            {
                model.InsertaIdioma(element);
            }

            foreach (Pais pais in PaisesSingleton.Paises)
            {
                model.InsertaPais(pais);
            }

            foreach (Obra obra in new ObraModel().GetObras())
            {
                if (obra.MuestraEnKiosko == true && obra.Orden > 0 && obra.Padre > 0)
                    model.InsertaObra(obra);
            }


            ObservableCollection<Autor> autores = new AutorModel().GetAutores();

            foreach (Autor autor in autores)
            {
                model.InsertaAutor(autor);
            }

            ObservableCollection<Autor> instituciones = new AutorModel().GetInstituciones();

            foreach (Autor institucion in instituciones)
            {
                model.InsertaOrganismoAutor(institucion);
            }

            //BackgroundWorker worker = new BackgroundWorker();
            //worker.WorkerReportsProgress = true;
            //worker.DoWork += worker_DoWork;
            //worker.ProgressChanged += worker_ProgressChanged;

            //worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            LblProceso.Content = "Eliminando datos previos...";
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PbMainProcess.Value = e.ProgressPercentage;
        }
    }
}
