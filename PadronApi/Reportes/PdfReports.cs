using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using PadronApi.Converter;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PadronApi.Reportes
{
    public class PdfReports
    {
        private readonly ObservableCollection<Obra> obrasImprimir;
       ObservableCollection<PlantillaDto> plantillaDistr;
        readonly string filepath;

        //private readonly int idAcuerdo;

        readonly string aclaraciones = ConfigurationManager.AppSettings["Aclaraciones"];
        readonly string titularCoord = ConfigurationManager.AppSettings["TitularCoord"];


        public PdfReports(ObservableCollection<Obra> obrasImprimir, string filepath)
        {
            this.filepath = filepath;
            this.obrasImprimir = obrasImprimir;
        }

        public PdfReports(string filePath)
        {
            this.filepath = filePath;
        }

        


        /// <summary>
        /// Listado de obras con información general de las mismas
        /// </summary>
        public void InformeGenerlaObras()
        {
            Document myDocument = new Document(PageSize.A4, 15, 15, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filepath, FileMode.Create));
            myDocument.AddTitle("Listado de Obras");
            myDocument.AddCreator("Padrón de Distribución de la Dirección de Distribución del Semanario Judicial de la Federación");
            myDocument.Open();

            Paragraph para;

            try
            {

                para = new Paragraph("Suprema Corte de Justicia de la Nación", BoldFont(black, arialFont, 18));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Padrón de distribución del Semanario", BoldFont(black, arialFont, 14));
                para.Add(Environment.NewLine);
                para.Add("Judicial de la Federación");
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Lista de obras", BoldFont(black, arialFont, 14));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                Paragraph white = new Paragraph(" ");
                myDocument.Add(white);

                //se crea un objeto PdfTable con el numero de columnas del dataGridView 
                PdfPTable table = new PdfPTable(5) { WidthPercentage = 100 };
                //asignamos algunas propiedades para el diseño del pdf 
                table.DefaultCell.Padding = 1;
                float[] headerwidths = new float[5] { .3f, 2f, .5f, .4f, .4f };
                table.SetWidths(headerwidths);

                table.DefaultCell.BorderWidth = 1;

                string[] encabezado = { "#", "Título", "Núm. de Material", "Año", "Tiraje." };
                PdfPCell cell;

                foreach (string cabeza in encabezado)
                {
                    cell = new PdfPCell(new Phrase(cabeza, BoldFont(black, arialFont, 12)));
                    cell.Colspan = 0;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                }

                //Indicamos que se repitan los encabezados de columna en cada una de las páginas
                table.HeaderRows = 2;
                table.DefaultCell.BorderWidth = 1;

                int consecutivo = 1;

                foreach (Obra obra in obrasImprimir)
                {
                    string[] descs = { consecutivo.ToString(), obra.Titulo, obra.NumMaterial, obra.AnioPublicacion.ToString(), obra.Tiraje.ToString() };

                    foreach (string desc in descs)
                    {
                        cell = new PdfPCell(new Phrase(desc, NormalFont(black, arialFont, 10)));
                        cell.Colspan = 0;
                        cell.HorizontalAlignment = (desc.Length > 15) ? 3 : 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    consecutivo++;
                }

                myDocument.Add(table);
                myDocument.Close();

                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
            }
        }


        /// <summary>
        /// Genera el reporte de las obras recibidas por el titular en propiedad de la oficina o de biblioteca
        /// para entregarlo a contraloría
        /// </summary>
        /// <param name="titular">Titular del cual se solicita el reporte</param>
        /// <param name="obrasRecibio">Obras que ha recibido</param>
        public void ReporteFuncionarioContraloria(Titular titular, ObservableCollection<Devoluciones> obrasRecibio)
        {
            Document myDocument = new Document(PageSize.LETTER.Rotate(), 15, 15, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filepath, FileMode.Create));
            myDocument.AddTitle("Reporte para contraloría");
            myDocument.AddCreator("Padrón de Distribución de la Dirección de Distribución del Semanario Judicial de la Federación");
            myDocument.Open();

            Paragraph para;

            try
            {

                para = new Paragraph("Suprema Corte de Justicia de la Nación", BoldFont(black, arialFont, 18));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Padrón de distribución del Semanario", BoldFont(black, arialFont, 14));
                para.Add(Environment.NewLine);
                para.Add("Judicial de la Federación");
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph(String.Format("Listado de obras en resguardo de: {0} {1} {2}", new TituloConverter().Convert(titular.IdTitulo, null, null, null), titular.Nombre, titular.Apellidos), BoldFont(black, arialFont, 14));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                Paragraph white = new Paragraph(" ");
                myDocument.Add(white);

                //se crea un objeto PdfTable con el numero de columnas del dataGridView 
                PdfPTable table = new PdfPTable(9) { WidthPercentage = 100 };
                //asignamos algunas propiedades para el diseño del pdf 
                table.DefaultCell.Padding = 1;
                float[] headerwidths = new float[9] { .25f, 1.5f, 1f, .35f, .40f, .4f, .45f, .45f, .9f };
                table.SetWidths(headerwidths);

                table.DefaultCell.BorderWidth = 1;

                string[] encabezado = { "#", "Título", "Organismos donde recibió", "Oficina", "Biblioteca", "Oficio", "F. de devolución", "Devueltos", "Observaciones" };
                PdfPCell cell;

                foreach (string cabeza in encabezado)
                {
                    cell = new PdfPCell(new Phrase(cabeza, BoldFont(black, arialFont, 10)));
                    cell.Colspan = 0;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                }

                //Indicamos que se repitan los encabezados de columna en cada una de las páginas
                table.HeaderRows = 1;
                table.DefaultCell.BorderWidth = 1;

                int consecutivo = 1;

                DevolucionModel devModel = new DevolucionModel();

                foreach (Devoluciones obra in obrasRecibio)
                {
                    Organismo organismo = new OrganismoModel().GetOrganismos(obra.IdOrganismo);

                    devModel.GetDevoluciones(obra);

                    string[] descs = { consecutivo.ToString(), obra.Titulo, organismo.OrganismoDesc, 
                                        obra.Oficina.ToString(), 
                                        obra.Biblioteca.ToString(), obra.OficioDevolucion, obra.FechaDevolucionString,
                                        obra.Cantidad.ToString(), obra.Observaciones };


                    foreach (string desc in descs)
                    {
                        cell = new PdfPCell(new Phrase(desc, NormalFont(black, arialFont, 10)));
                        cell.Colspan = 0;
                        cell.HorizontalAlignment = (desc.Length > 15) ? 3 : 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    consecutivo++;
                }

                myDocument.Add(table);
                myDocument.Close();

                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
            }
        }

        #region Organismos

        public void ReporteSecretarios()
        {
            Document myDocument = new Document(PageSize.A4, 15, 15, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filepath, FileMode.Create));
            myDocument.AddTitle("Total de secretarios por organismos");
            myDocument.AddCreator("Padrón de Distribución de la Dirección de Distribución del Semanario Judicial de la Federación");
            myDocument.Open();

            Paragraph para;


            try
            {

                para = new Paragraph("Suprema Corte de Justicia de la Nación", BoldFont(black, arialFont, 18));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Padrón de distribución del Semanario", BoldFont(black, arialFont, 14));
                para.Add(Environment.NewLine);
                para.Add("Judicial de la Federación");
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Total de secretarios por organismos", BoldFont(black, arialFont, 14));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                Paragraph white = new Paragraph(" ");
                myDocument.Add(white);

                foreach (Ordinales circuito in OrdinalSingleton.Ordinales)
                {
                    if (circuito.IdOrdinal == 0) { }
                    else
                    {

                        para = new Paragraph(circuito.Ordinal + " Circuito", BoldFont(black, arialFont, 14));
                        para.Alignment = Element.ALIGN_LEFT;
                        myDocument.Add(para);
                        myDocument.Add(white);

                        //se crea un objeto PdfTable con el numero de columnas del dataGridView 
                        PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
                        //asignamos algunas propiedades para el diseño del pdf 
                        table.DefaultCell.Padding = 1;
                        float[] headerwidths = new float[2] { .8f, .2f };
                        table.SetWidths(headerwidths);

                        table.DefaultCell.BorderWidth = 1;

                        string[] encabezado = { "Organismos", "Total Secretarios" };
                        PdfPCell cell;

                        foreach (string cabeza in encabezado)
                        {
                            cell = new PdfPCell(new Phrase(cabeza, BoldFont(black, arialFont, 12)));
                            cell.Colspan = 0;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }

                        //Indicamos que se repitan los encabezados de columna en cada una de las páginas
                        table.HeaderRows = 2;
                        table.DefaultCell.BorderWidth = 1;

                        int consecutivo = 1;

                        ObservableCollection<KeyValuePair<string, int>> organismos = new ReportesModel().GetSecretariosByCircuito(circuito.IdOrdinal);

                        foreach (KeyValuePair<string, int> org in organismos)
                        {
                            string[] descs = { org.Key, org.Value.ToString() };

                            foreach (string desc in descs)
                            {
                                cell = new PdfPCell(new Phrase(desc, NormalFont(black, arialFont, 10)));
                                cell.Colspan = 0;
                                cell.HorizontalAlignment = (desc.Length > 15) ? 3 : 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }
                            consecutivo++;
                        }

                        myDocument.Add(table);
                        myDocument.Add(white);
                        myDocument.Add(white);

                        if (circuito.IdOrdinal % 4 == 0)
                        {
                            myDocument.NewPage();
                        }

                    }
                }


                myDocument.Close();

                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
            }
        }


        public void ReporteOrganismosxCircuito()
        {
            Document myDocument = new Document(PageSize.A4, 15, 15, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filepath, FileMode.Create));
            myDocument.AddTitle("Total de organismos por circuito");
            myDocument.AddCreator("Padrón de Distribución de la Dirección de Distribución del Semanario Judicial de la Federación");
            myDocument.Open();

            Paragraph para;


            try
            {

                para = new Paragraph("Suprema Corte de Justicia de la Nación", BoldFont(black, arialFont, 18));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Padrón de distribución del Semanario", BoldFont(black, arialFont, 14));
                para.Add(Environment.NewLine);
                para.Add("Judicial de la Federación");
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Total de organismos por circuito", BoldFont(black, arialFont, 14));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                Paragraph white = new Paragraph(" ");
                myDocument.Add(white);

                foreach (Ordinales circuito in OrdinalSingleton.Ordinales)
                {
                    if (circuito.IdOrdinal == 0) { }
                    else
                    {

                        para = new Paragraph(circuito.Ordinal + " Circuito", BoldFont(black, arialFont, 14));
                        para.Alignment = Element.ALIGN_LEFT;
                        myDocument.Add(para);
                        myDocument.Add(white);

                        //se crea un objeto PdfTable con el numero de columnas del dataGridView 
                        PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
                        //asignamos algunas propiedades para el diseño del pdf 
                        table.DefaultCell.Padding = 1;
                        float[] headerwidths = new float[2] { .8f, .2f };
                        table.SetWidths(headerwidths);

                        table.DefaultCell.BorderWidth = 1;

                        string[] encabezado = { "Organismos", "Total" };
                        PdfPCell cell;

                        foreach (string cabeza in encabezado)
                        {
                            cell = new PdfPCell(new Phrase(cabeza, BoldFont(black, arialFont, 12)));
                            cell.Colspan = 0;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }

                        //Indicamos que se repitan los encabezados de columna en cada una de las páginas
                        table.HeaderRows = 2;
                        table.DefaultCell.BorderWidth = 1;

                        int consecutivo = 1;

                        ObservableCollection<KeyValuePair<string, int>> organismos = new ReportesModel().GetOrganosByCircuito(circuito.IdOrdinal);

                        foreach (KeyValuePair<string, int> org in organismos)
                        {
                            string[] descs = { org.Key, org.Value.ToString() };

                            foreach (string desc in descs)
                            {
                                cell = new PdfPCell(new Phrase(desc, NormalFont(black, arialFont, 10)));
                                cell.Colspan = 0;
                                cell.HorizontalAlignment = (desc.Length > 15) ? 3 : 1; //0=Left, 1=Centre, 2=Right
                                table.AddCell(cell);
                            }
                            consecutivo++;
                        }

                        myDocument.Add(table);
                        myDocument.Add(white);
                        myDocument.Add(white);

                        if (circuito.IdOrdinal % 4 == 0)
                        {
                            myDocument.NewPage();
                        }

                    }
                }


                myDocument.Close();

                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
            }
        }


       

        #endregion

        #region Titulares

        public void ReporteIncluidosTodosTirajes(ObservableCollection<Adscripcion> adscripciones)
        {
            Document myDocument = new Document(PageSize.A4, 15, 15, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filepath, FileMode.Create));
            myDocument.AddTitle("Funcionarios incluidos en todas las distribuciones");
            myDocument.AddCreator("Padrón de Distribución de la Dirección de Distribución del Semanario Judicial de la Federación");
            myDocument.Open();

            Paragraph para;


            try
            {

                para = new Paragraph("Suprema Corte de Justicia de la Nación", BoldFont(black, arialFont, 18));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Padrón de distribución del Semanario", BoldFont(black, arialFont, 14));
                para.Add(Environment.NewLine);
                para.Add("Judicial de la Federación");
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Funcionarios incluidos en todas las distribuciones", BoldFont(black, arialFont, 14));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                Paragraph white = new Paragraph(" ");
                myDocument.Add(white);
                myDocument.Add(white);

                //se crea un objeto PdfTable con el numero de columnas del dataGridView 
                PdfPTable table = new PdfPTable(3) { WidthPercentage = 100 };
                //asignamos algunas propiedades para el diseño del pdf 
                table.DefaultCell.Padding = 1;
                float[] headerwidths = new float[3] { .2f, 1f, 1f };
                table.SetWidths(headerwidths);

                table.DefaultCell.BorderWidth = 1;

                string[] encabezado = { "#", "Funcionario", "Organismos" };
                PdfPCell cell;

                foreach (string cabeza in encabezado)
                {
                    cell = new PdfPCell(new Phrase(cabeza, BoldFont(black, arialFont, 12)));
                    cell.Colspan = 0;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                }

                //Indicamos que se repitan los encabezados de columna en cada una de las páginas
                table.HeaderRows = 1;
                table.DefaultCell.BorderWidth = 1;

                int consecutivo = 1;

                foreach (Adscripcion ads in adscripciones)
                {
                    string[] descs = { consecutivo.ToString(), String.Format("{0} {1}", ads.Titular.Nombre, ads.Titular.Apellidos), ads.Organismo.OrganismoDesc };

                    foreach (string desc in descs)
                    {
                        cell = new PdfPCell(new Phrase(desc, NormalFont(black, arialFont, 10)));
                        cell.Colspan = 0;
                        cell.HorizontalAlignment = (desc.Length > 15) ? 3 : 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    consecutivo++;
                }

                myDocument.Add(table);
                myDocument.Add(white);
                myDocument.Add(white);

                myDocument.Close();

                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
            }
        }


        public void ReporteGeneroTitulares()
        {
            Document myDocument = new Document(PageSize.A4, 15, 15, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filepath, FileMode.Create));
            myDocument.AddTitle("Total de organismos por circuito");
            myDocument.AddCreator("Padrón de Distribución de la Dirección de Distribución del Semanario Judicial de la Federación");
            myDocument.Open();

            Paragraph para;

            int[] tribunales = { 2, 4, 256, 257, 258 };
            int[] juzgados = { 8, 259, 260, 261, 262, 263, 266 };
            int[] centros = { 255 };


            try
            {

                para = new Paragraph("Suprema Corte de Justicia de la Nación", BoldFont(black, arialFont, 18));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("Padrón de distribución del Semanario", BoldFont(black, arialFont, 14));
                para.Add(Environment.NewLine);
                para.Add("Judicial de la Federación");
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);

                Paragraph white = new Paragraph(" ");
                myDocument.Add(white);

                para = new Paragraph("Órganos Jurisdiccionales", BoldFont(black, arialFont, 14));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);
                myDocument.Add(white);

                //se crea un objeto PdfTable con el numero de columnas del dataGridView 
                //PdfPTable table = new PdfPTable(5);
                PdfPTable table = new PdfPTable(3) { WidthPercentage = 100 };
                //asignamos algunas propiedades para el diseño del pdf 
                table.DefaultCell.Padding = 1;
                //float[] headerwidths = new float[5] { 1f, .5f, .5f, .5f, .5f };
                float[] headerwidths = new float[3] { 1f, .5f, .5f };
                table.SetWidths(headerwidths);

                table.DefaultCell.BorderWidth = 1;

                string[] encabezado = { "Órgano Jurisdiccional", "Hombres", "Mujeres" };
                PdfPCell cell;

                foreach (string cabeza in encabezado)
                {
                    cell = new PdfPCell(new Phrase(cabeza, BoldFont(black, arialFont, 12)));
                    cell.Colspan = 0;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell);
                }

                //Indicamos que se repitan los encabezados de columna en cada una de las páginas
                table.HeaderRows = 2;
                table.DefaultCell.BorderWidth = 1;

                int consecutivo = 1;

                ReportesModel repModel = new ReportesModel();

                ObservableCollection<Organismo> tiposOrganismo = repModel.GetOrganosJurisdiccionales();

                foreach (Organismo org in tiposOrganismo)
                {
                    int totalOrganismos = repModel.GetTotalOrganosByTipo(org.IdOrganismo);

                    int totalFunc = 0, totalHombre = 0, totalMujeres = 0;

                    if (tribunales.Contains(org.IdOrganismo))
                    {
                        totalFunc = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 1, 2, 0);
                        totalHombre = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 1, 2, 1);
                        totalMujeres = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 1, 2, 2);
                    }
                    else if (juzgados.Contains(org.IdOrganismo))
                    {
                        totalFunc = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 8, 19, 0);
                        totalHombre = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 8, 19, 1);
                        totalMujeres = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 8, 19, 2);
                    }
                    else if (centros.Contains(org.IdOrganismo))
                    {
                        totalFunc = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 131, 131, 0);
                        totalHombre = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 131, 131, 1);
                        totalMujeres = repModel.GetTotalFuncionariosByTipoOrg(org.IdOrganismo, 131, 131, 2);
                    }


                    //string[] descs = { org.OrganismoDesc, totalOrganismos.ToString(), totalFunc.ToString(), totalHombre.ToString(), totalMujeres.ToString() };
                    string[] descs = { org.OrganismoDesc, totalHombre.ToString(), totalMujeres.ToString() };

                    foreach (string desc in descs)
                    {
                        cell = new PdfPCell(new Phrase(desc, NormalFont(black, arialFont, 10)));
                        cell.Colspan = 0;
                        cell.HorizontalAlignment = (desc.Length > 15) ? 3 : 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    consecutivo++;
                }

                myDocument.Add(table);
                myDocument.Add(white);
                myDocument.Add(white);

                myDocument.Close();

                System.Diagnostics.Process.Start(filepath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
            }
        }


        #endregion



        #region Acuses


        readonly PadronGenerado padronGenerado;


        string fechaDistribucion = String.Empty;

        private int folio = 1;
        private int contadorOficio = 0;

       

        PlantillaModel modelPlantilla = null;
        //Titular currentTitular;
        private Image logo, autho;

        string presentacion;

        /// <summary>
        /// Permite Generar los Oficios de Envio de las obras a distribuir
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="padronGenerado"></param>
        public PdfReports(string filePath, PadronGenerado padronGenerado)
        {
            this.padronGenerado = padronGenerado;
            this.filepath = filePath;
            
        }


        public void GeneraOficiosEnvio(int idObra, int idPadron)
        {

            int cuantosConInfo = new AcusesModel().VerificaParaBorrar(padronGenerado);

            if (cuantosConInfo == 0)
            {

                    bool deleteCompleted = new AcusesModel().DeleteAcusesRecepcion(padronGenerado);

                    if (deleteCompleted)
                    {

                        Obra obra = new ObraModel().GetObras(idObra);

                        fechaDistribucion = "Ciudad de México, a " + DateTimeUtilities.ToLongDateFormat(obra.FechaDistribuye);

                        modelPlantilla = new PlantillaModel();

                        folio = 1;

                        presentacion = (from n in ElementalPropertiesSingleton.Presentacion
                                        where n.IdElemento == obra.Presentacion
                                        select n.Descripcion).ToList()[0];

                        if (padronGenerado.OficioInicial == 0)
                        {
                            contadorOficio = new ReportesModel().GetNextNumOficio();
                            padronGenerado.OficioInicial = contadorOficio;

                        }
                        else
                        {
                            contadorOficio = padronGenerado.OficioInicial;
                        }
                        var bmp = new System.Drawing.Bitmap(PadronApi.Properties.Resources.logo);
                        logo = Image.GetInstance(bmp, System.Drawing.Imaging.ImageFormat.Bmp);
                        logo.ScalePercent(12f);

                        bmp = new System.Drawing.Bitmap(PadronApi.Properties.Resources.autho2);
                        autho = Image.GetInstance(bmp, System.Drawing.Imaging.ImageFormat.Bmp);
                        autho.ScalePercent(24f);

                        Document myDocument = new Document(PageSize.A4, 50, 50, 15, 15);
                        PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(filepath, FileMode.Create));
                        myDocument.AddTitle("Oficios de envio de obras");
                        myDocument.AddCreator("Padrón de Distribución de la Dirección de Distribución del Semanario Judicial de la Federación");
                        myDocument.Open();


                        try
                        {
                            for (int parteImprime = 1; parteImprime < 5; parteImprime++)
                            {
                                plantillaDistr = new ReportesModel().GetDetallesDePadron(idPadron, padronGenerado.IdAcuerdo, parteImprime);

                                foreach (PlantillaDto plantilla in plantillaDistr)
                                {
                                    plantilla.IdPadron = idPadron;

                                    if (plantilla.TipoDistribucion < 4)
                                    {
                                        if (plantilla.Particular > 0 || plantilla.Autor > 0)
                                            this.GeneraTipoDistr1a3(myDocument, obra, plantilla, 1, plantilla.Particular + plantilla.Autor);

                                        if (plantilla.Oficina > 0)
                                            this.GeneraTipoDistr1a3(myDocument, obra, plantilla, 2, plantilla.Oficina);

                                        if (plantilla.Biblioteca > 0)
                                            this.GeneraTipoDistr1a3(myDocument, obra, plantilla, 3, plantilla.Biblioteca);

                                        if (plantilla.Personal > 0)
                                            this.GeneraTipoDistr1a3(myDocument, obra, plantilla, 4, plantilla.Personal);
                                    }
                                    else
                                    {
                                        if (plantilla.Particular > 0 || plantilla.Autor > 0)
                                            this.GeneraTipoDistr4(myDocument, obra, plantilla, 1, plantilla.Particular + plantilla.Autor);

                                        if (plantilla.Oficina > 0)
                                            this.GeneraTipoDistr4(myDocument, obra, plantilla, 2, plantilla.Oficina);

                                        if (plantilla.Biblioteca > 0)
                                            this.GeneraTipoDistr4(myDocument, obra, plantilla, 3, plantilla.Biblioteca);

                                        if (plantilla.Personal > 0)
                                            this.GeneraTipoDistr4(myDocument, obra, plantilla, 4, plantilla.Personal);
                                    }

                                }
                            }
                            myDocument.Close();



                            if (padronGenerado.OficioFinal == 0)
                            {
                                new ReportesModel().UpdateNumerosOficio(idPadron, padronGenerado.OficioInicial, contadorOficio - 1);
                                padronGenerado.OficioFinal = contadorOficio - 1;
                            }

                            System.Diagnostics.Process.Start(filepath);
                        }
                        catch (Exception ex)
                        {
                            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                            ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
                        }
                    }
                
            }
            else
            {
                MessageBox.Show("La obra para la cual estás intentando generar oficios ya contiene información de envio y/o recepción, no se generará el archivo nuevamente", "PadronApi");

            }
        }

        public void GeneraTipoDistr1a3(Document myDocument, Obra obra, PlantillaDto plantilla, int propiedad, int cantidad)
        {
            string propStr = String.Empty;

            if (propiedad == 1)
                propStr = "particular.";
            else if (propiedad == 2)
                propStr = "de la oficina.";
            else if (propiedad == 3)
                propStr = "de la biblioteca.";
            else if (propiedad == 4)
                propStr = "particular de los secretarios.";

            Paragraph para;

            try
            {
                this.SetPageHeader(myDocument);
                para = new Paragraph(String.Format("Of. Núm. CCST/DDSJF-PAD-{0}-{1}-{2}", ((DateTime.Now.Month < 10) ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString()), contadorOficio, DateTime.Now.Year.ToString().Substring(2, 2)), NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_RIGHT;
                myDocument.Add(para);
                para = new Paragraph(fechaDistribucion, NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_RIGHT;
                myDocument.Add(para);
                InsertLineBreak(myDocument, 1);

                para = new Paragraph(plantilla.Nombre, BoldFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                if (plantilla.TipoOrganismo == 265)
                    para = new Paragraph("Ministros Jubilados", NormalFont(black, arialFont, 10));
                else if (plantilla.GrupoOrganismo == 1 && plantilla.Funcion == 1)
                    para = new Paragraph("Presidente del " + plantilla.Organismo, NormalFont(black, arialFont, 10));
                else if ((plantilla.GrupoOrganismo == 1 && plantilla.Funcion == 0) || plantilla.Funcion == 0)
                    para = new Paragraph(plantilla.Organismo, NormalFont(black, arialFont, 10));
                else
                    para = new Paragraph(String.Format("{0} de {1}", new FuncionConverter().Convert(plantilla.Funcion, null, null, null), plantilla.Organismo), NormalFont(black, arialFont, 10));


                para.IndentationRight = 150;

                myDocument.Add(para);

                para = new Paragraph("P r e s e n t e", NormalFont(black, arialFont, 10));
                para.SpacingBefore = 0;
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph("Distinguido(a) " + plantilla.Nombre, NormalFont(black, arialFont, 10));
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph("Me es grato enviarle por este medio la publicación oficial que se detalla:", NormalFont(black, arialFont, 10));
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph(obra.Titulo, BoldFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                InsertLineBreak(myDocument, 1);

                if (obra.Presentacion == 4)
                {
                    para = new Paragraph(String.Format("TOTAL: {0} {1}", cantidad, ((obra.TipoObra == 1) ? "USB" : "DISCO ÓPTICO")), BoldFont(black, arialFont, 10));
                    para.Alignment = Element.ALIGN_CENTER;
                    myDocument.Add(para);
                    InsertLineBreak(myDocument, 1);
                }
                else
                {

                    para = new Paragraph(String.Format("{0}{1} EN PRESENTACIÓN {2}", cantidad, ((cantidad > 1) ? " EJEMPLARES" : " EJEMPLAR"), presentacion.ToUpper()), BoldFont(black, arialFont, 10));
                    para.Alignment = Element.ALIGN_CENTER;
                    myDocument.Add(para);
                    InsertLineBreak(myDocument, 1);
                    para = new Paragraph(String.Format("TOTAL: {0} {1}", cantidad, ((cantidad > 1) ? "LIBROS" : "LIBRO")), BoldFont(black, arialFont, 10));
                    para.Alignment = Element.ALIGN_CENTER;
                    myDocument.Add(para);
                    InsertLineBreak(myDocument, 1);
                }

                para = new Paragraph("Me permito comunicarle que la obra citada es en propiedad " + propStr, BoldFont(black, arialFont, 10));
                myDocument.Add(para);
                InsertLineBreak(myDocument, 1);

                para = new Paragraph(aclaraciones, NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_JUSTIFIED;
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph("Sin otro particular, le envío un cordial saludo.", NormalFont(black, arialFont, 10));
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph("A T E N T A M E N T E", NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                autho.SpacingAfter = 3f;
                autho.Alignment = Element.ALIGN_CENTER;

                myDocument.Add(autho);

                para = new Paragraph(titularCoord, BoldFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("COORDINADORA", NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);
                para = new Paragraph("c.c.p- El archivo.-", NormalFont(black, arialFont, 8));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);
                para = new Paragraph("LVP/JJMM", NormalFont(black, arialFont, 8));
                myDocument.Add(para);
                para = new Paragraph(String.Format("Folio {0}-{1}", plantilla.TipoDistribucion, folio), BoldFont(black, arialFont, 8));
                para.Alignment = Element.ALIGN_RIGHT;
                myDocument.Add(para);

                myDocument.NewPage();
                folio++;

                modelPlantilla.RegistroOficiostitular(plantilla, contadorOficio);

                contadorOficio++;

                Paragraph white = new Paragraph(" ");
                myDocument.Add(white);

            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
            }
        }

        public void GeneraTipoDistr4(Document myDocument, Obra obra, PlantillaDto plantilla, int propiedad, int cantidad)
        {
            string propStr = String.Empty;

            if (propiedad == 1)
                propStr = "Particular.";
            else if (propiedad == 2)
                propStr = "Oficina.";
            else if (propiedad == 3)
                propStr = "Biblioteca";
            else if (propiedad == 4)
                propStr = "Particular";

            Paragraph para;

            try
            {
                this.SetPageHeader(myDocument);
                para = new Paragraph(String.Format("Of. Núm. CCST/DDSJF-PAD-{0}-{1}-{2}", ((DateTime.Now.Month < 10) ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()), contadorOficio, DateTime.Now.Year.ToString().Substring(2, 2)), NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_RIGHT;
                myDocument.Add(para);
                para = new Paragraph(fechaDistribucion, NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_RIGHT;
                myDocument.Add(para);
                InsertLineBreak(myDocument, 1);

                para = new Paragraph(plantilla.Nombre, BoldFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                string complOrganismo;

                if (plantilla.GrupoOrganismo == 1)
                    complOrganismo = String.Format(" en {0}, {1}", plantilla.CiudadStr, plantilla.EstadoStr);
                else
                    complOrganismo = String.Empty;

                if (plantilla.TipoOrganismo == 265)
                    para = new Paragraph("Ministros Jubilados", NormalFont(black, arialFont, 10));
                else if (plantilla.GrupoOrganismo == 1 && plantilla.Funcion == 1)
                    para = new Paragraph(String.Format("Presidente del {0}{1}", plantilla.Organismo, complOrganismo), NormalFont(black, arialFont, 10));
                else if ((plantilla.GrupoOrganismo == 1 && plantilla.Funcion == 0) || plantilla.Funcion == 0)
                    para = new Paragraph(plantilla.Organismo + complOrganismo, NormalFont(black, arialFont, 10));
                else
                    para = new Paragraph(String.Format("{0} de {1}{2}", new FuncionConverter().Convert(plantilla.Funcion, null, null, null), plantilla.Organismo, complOrganismo), NormalFont(black, arialFont, 10));
                para.IndentationRight = 150;

                myDocument.Add(para);

                para = new Paragraph("P r e s e n t e", NormalFont(black, arialFont, 10));
                para.SpacingBefore = 0;
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph("Distinguido(a) " + plantilla.Nombre, NormalFont(black, arialFont, 10));
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph("Me es grato enviarle por este medio la publicación oficial que se detalla:", NormalFont(black, arialFont, 10));
                myDocument.Add(para);

                //InsertLineBreak(myDocument, 1);

                para = new Paragraph(obra.Titulo, BoldFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                //InsertLineBreak(myDocument, 1);

                //prv inicio
                //InsertLineBreak(myDocument, 1);
                PdfPTable table = new PdfPTable(3) { WidthPercentage = 100 };
                table.DefaultCell.Padding = 1;
                float[] headerwidths = new float[3] { .5f, .5f, .5f };
                table.SetWidths(headerwidths);
                table.DefaultCell.BorderWidth = 1;
                string[] encabezado = { "Presentación", "Cantidad", "Propiedad" };
                PdfPCell cell;
                foreach (string cabeza in encabezado)
                {
                    cell = new PdfPCell(new Phrase(cabeza, BoldFont(black, arialFont, 12)));
                    cell.Colspan = 0;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                }

                InsertLineBreak(myDocument, 1);

                string[] descs = { presentacion.ToString(), cantidad.ToString(), propStr };

                foreach (string desc in descs)
                {
                    cell = new PdfPCell(new Phrase(desc, NormalFont(black, arialFont, 10)));
                    cell.Colspan = 0;
                    cell.HorizontalAlignment = (desc.Length > 15) ? 3 : 1; //0=Left, 1=Centre, 2=Right
                    cell.BorderWidth = 0;
                    table.AddCell(cell);
                }

                myDocument.Add(table);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph("Sin otro particular, le envío un cordial saludo.", NormalFont(black, arialFont, 10));
                myDocument.Add(para);

                InsertLineBreak(myDocument, 1);

                para = new Paragraph("A T E N T A M E N T E", NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);

                //InsertLineBreak(myDocument, 1);
                autho.SpacingAfter = 0f;
                autho.Alignment = Element.ALIGN_CENTER;

                myDocument.Add(autho);

                para = new Paragraph(titularCoord, BoldFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                para = new Paragraph("COORDINADORA", NormalFont(black, arialFont, 10));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);
                InsertLineBreak(myDocument, 1);

                para = new Paragraph("FAVOR DE NO RECORTAR EL OFICIO-ACUSE", NormalFont(black, arialFont, 7));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                PdfPTable tableAc = new PdfPTable(1) { WidthPercentage = 100 };
                tableAc.DefaultCell.Padding = 1;
                tableAc.DefaultCell.BorderWidth = 1;

                PdfPCell cellAc = new PdfPCell(new Phrase("Acuso a Usted de recibido por la publicación arriba señalada", NormalFont(black, arialFont, 7))) { Colspan = 0, Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER, HorizontalAlignment = 0 /*0=Left, 1=Centre, 2=Right*/ };
                tableAc.AddCell(cellAc);

                cellAc = new PdfPCell(new Phrase("_____________________________________________\r\n" + plantilla.Nombre, BoldFont(black, arialFont, 7))) { Colspan = 0, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = 1 /*0=Left, 1=Centre, 2=Right*/ };
                tableAc.AddCell(cellAc);

                cellAc = new PdfPCell(new Phrase("Correo electrónico\r\n(Institucional)    _____________________________________________", NormalFont(black, arialFont, 7))) { Colspan = 0, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = 0 /*0=Left, 1=Centre, 2=Right*/ };
                tableAc.AddCell(cellAc);

                cellAc = new PdfPCell(new Phrase("Si existiera alguna de las siguientes modificaciones, favor de indicarla en el espacio correspondiente:", NormalFont(black, arialFont, 7))) { Colspan = 0, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = 0 /*0=Left, 1=Centre, 2=Right*/ };
                tableAc.AddCell(cellAc);

                cellAc = new PdfPCell(new Phrase("Cambio de titular\r\nCambio de plantilla\r\nOtros datos      ___________________________________________________________________", NormalFont(black, arialFont, 7))) { Colspan = 0, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = 0 /*0=Left, 1=Centre, 2=Right*/ };
                tableAc.AddCell(cellAc);

                cellAc = new PdfPCell(new Phrase("FAVOR DE ACUSAR A LA BREVEDAD POSIBLE\r\n" + "ENVIAR RECIBO ESCANEADO EN PDF AL CORREO ELECTRÓNICO gortegas@mail.scjn.gob.mx\r\n" + "O VÍA FAX A LOS TELÉFONOS 01 (55) 4113-1127 Ó 4113-1335\r\n" + "Y CONFIRMAR AL 01 (55) 4113-1000 EXT. 2280", NormalFont(black, arialFont, 9))) { Colspan = 0, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER, HorizontalAlignment = 1 /*0=Left, 1=Centre, 2=Right*/ };
                tableAc.AddCell(cellAc);

                myDocument.Add(tableAc);

                //prv fin
                para = new Paragraph("LVP/JJMM", NormalFont(black, arialFont, 8));
                myDocument.Add(para);
                para = new Paragraph(String.Format("Folio {0}-{1}", plantilla.TipoDistribucion, folio), BoldFont(black, arialFont, 8));
                para.Alignment = Element.ALIGN_RIGHT;
                myDocument.Add(para);

                myDocument.NewPage();
                folio++;

                modelPlantilla.RegistroOficiostitular(plantilla, contadorOficio);

                contadorOficio++;

                Paragraph white = new Paragraph(" ");
                myDocument.Add(white);




            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PdfReports", "PadronApi");
            }
        }

       

        readonly Paragraph whitePara = new Paragraph(" ");
        private void InsertLineBreak(Document myDocument, int cuantos)
        {
            while (cuantos > 0)
            {
                myDocument.Add(whitePara);
                cuantos--;
            }
        }


        #endregion





        public Document SetPageHeader(Document myDocument)
        {
            PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };

            table.SetWidths(new Single[] { 35, 150 });

            table.SpacingBefore = 20f;
            table.SpacingAfter = 10f;




            PdfPCell cell = new PdfPCell() { Border = 0 };
            cell.AddElement(logo);
            table.AddCell(cell);

            cell = new PdfPCell() { Border = 0 };
            Paragraph para = new Paragraph(" ", BoldFont(black, arialFont, 10));
            cell.AddElement(para);
            table.AddCell(cell);

            cell = new PdfPCell();
            cell.Border = 0;
            cell.Border = Rectangle.BOTTOM_BORDER;
            para = new Paragraph(" ", BoldFont(black, arialFont, 10));
            cell.AddElement(para);
            table.AddCell(cell);

            cell = new PdfPCell() { Border = 0 };
            Chunk c1 = new Chunk("PRESIDENCIA DE LA SUPREMA CORTE DE JUSTICIA DE LA NACIÓN", BoldFont(black, arialFont, 10));
            Chunk c2 = new Chunk("COORDINACIÓN DE COMPILACIÓN Y SISTEMATIZACIÓN DE TESIS", NormalFont(black, arialFont, 10));
            Phrase phrase = new Phrase();
            phrase.Add(c1);
            phrase.Add(Environment.NewLine);
            phrase.Add(c2);
            para = new Paragraph();
            para.Add(phrase);
            para.Alignment = Element.ALIGN_RIGHT;
            cell.AddElement(para);
            cell.Border = Rectangle.BOTTOM_BORDER;
            table.AddCell(cell);



            myDocument.Add(table);

            return myDocument;
        }

        #region Fonts

        private const String arialFont = "Arial";

        #endregion


        #region Font Colors

        private readonly BaseColor black = new BaseColor(0, 0, 0);
        private readonly BaseColor red = new BaseColor(255, 0, 0);

        #endregion

        #region Font Style

        private Font BoldFont(BaseColor color, string fontName, int fontSize)
        {
            Font font = FontFactory.GetFont(fontName, fontSize, Font.BOLD, color);

            return font;
        }


        private Font NormalFont(BaseColor color, string fontName, int fontSize)
        {
            Font font = FontFactory.GetFont(fontName, fontSize, Font.NORMAL, color);

            return font;
        }
        #endregion
    }
}
