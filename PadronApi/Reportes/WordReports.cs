using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using PadronApi.Dto;
using PadronApi.Model;
using ScjnUtilities;
using Word = Microsoft.Office.Interop.Word;
using PadronApi.Singletons;
using PadronApi.Converter;
using Microsoft.Office.Core;
using System.Collections.Generic;

namespace PadronApi.Reportes
{
    public class WordReports
    {

        private readonly ObservableCollection<Obra> obrasImprimir;
        private readonly ObservableCollection<PlantillaDto> plantillaDistr;
        private readonly string tituloObra;
        private string presentacion;

        private int fila = 1;
        int folio = 1;

        System.Drawing.Bitmap autoriza;
        string imagenAutorizada = String.Empty;

        private int oficioInicial = 0;
        private int contadorOficio = 0;

        /// <summary>
        /// Indica si los acuses de recibo de una obra en particular se están generando por primera vez 
        /// o no. De esta forma podemos llevar el control de que oficios se fueron para cada persona
        /// </summary>
        private readonly bool vuelveAGenerarOficio = false;

        PlantillaModel modelPlantilla = null;

        Application oWord;
        Document aDoc;
        object oMissing = Missing.Value;
        object oEndOfDoc = "\\endofdoc";


        readonly string aclaraciones = ConfigurationManager.AppSettings["Aclaraciones"];
        readonly string titularCoord = ConfigurationManager.AppSettings["TitularCoord"];

        readonly string filePath;


        public WordReports(ObservableCollection<Obra> obrasImprimir, string filePath)
        {
            this.obrasImprimir = obrasImprimir;
            this.filePath = filePath;
        }


        public WordReports(ObservableCollection<PlantillaDto> plantillaDistr, string filePath, string tituloObra)
        {
            this.plantillaDistr = plantillaDistr;
            this.filePath = filePath;
            this.tituloObra = tituloObra;
        }

        public WordReports(string filePath, string tituloObra)
        {
            this.filePath = filePath;
            this.tituloObra = tituloObra;
        }

        public WordReports(ObservableCollection<PlantillaDto> plantillaDistr, string filePath, string tituloObra, int oficioInicial)
        {
            this.plantillaDistr = plantillaDistr;
            this.filePath = filePath;
            this.tituloObra = tituloObra;
            this.oficioInicial = oficioInicial;
            if (oficioInicial != 0)
                vuelveAGenerarOficio = true;
        }

        #region Obras

        public void InformeGeneralObras()
        {
            oWord = new Application();
            aDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            aDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


            try
            {
                //Insert a paragraph at the beginning of the document.
                Paragraph oPara1 = aDoc.Content.Paragraphs.Add(ref oMissing);
                //oPara1.Range.ParagraphFormat.Space1;
                oPara1.Range.Text = "SUPREMA CORTE DE JUSTICIA DE LA NACIÓN";

                oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oPara1.Range.Font.Bold = 1;
                oPara1.Range.Font.Size = 10;
                oPara1.Range.Font.Name = "Arial";
                oPara1.Format.SpaceAfter = 0;    //24 pt spacing after paragraph.
                this.ParagraphAfter(oPara1, 2);
                oPara1.Range.Text = "COORDINACIÓN DE COMPILACIÓN Y ";
                oPara1.Range.InsertParagraphAfter();
                oPara1.Range.Text = "SISTEMATIZACIÓN DE TESIS";
                this.ParagraphAfter(oPara1, 2);
                oPara1.Range.Text = "RELACIÓN DE TESIS PARA PUBLICAR EN EL SEMANARIO JUDICIAL DE LA FEDERACIÓN Y EN SU GACETA";
                this.ParagraphAfter(oPara1, 2);
                oPara1.Range.Text = String.Format("TOTAL:   {0} Obras", obrasImprimir.Count());
                this.ParagraphAfter(oPara1, 2);


                fila = 1;
                Range wrdRng = aDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

                Table oTable = aDoc.Tables.Add(wrdRng, obrasImprimir.Count + 1, 5, ref oMissing, ref oMissing);
                //oTable.Rows[1].HeadingFormat = 1;
                oTable.Range.ParagraphFormat.SpaceAfter = 6;
                oTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oTable.Range.Font.Size = 9;
                oTable.Range.Font.Name = "Arial";
                oTable.Range.Font.Bold = 0;
                oTable.Borders.Enable = 1;

                int[] tamanios = new int[5] { 60, 400, 80, 60, 60 };

                string[] encabezados = new string[5] { "#", "Título", "Núm. de Material", "Año", "Tiraje" };

                this.SetWidthAndHeader(oTable, tamanios, encabezados);

                fila++;
                int consecutivo = 1;

                foreach (Obra print in obrasImprimir)
                {
                    oTable.Cell(fila, 1).Range.Text = consecutivo.ToString();
                    oTable.Cell(fila, 2).Range.Text = print.Titulo;
                    oTable.Cell(fila, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                    oTable.Cell(fila, 3).Range.Text = print.NumMaterial;
                    oTable.Cell(fila, 4).Range.Text = print.AnioPublicacion.ToString();
                    oTable.Cell(fila, 5).Range.Text = print.Tiraje.ToString();
                    // oTable.Cell(fila, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;

                    fila++;
                    consecutivo++;
                }


                foreach (Section wordSection in aDoc.Sections)
                {
                    object pagealign = WdPageNumberAlignment.wdAlignPageNumberRight;
                    object firstpage = true;
                    wordSection.Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.Add(ref pagealign, ref firstpage);
                }

                oWord.ActiveDocument.SaveAs(filePath);
                oWord.ActiveDocument.Saved = true;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,WordReports", "PadronApi");
            }
            finally
            {
                oWord.Visible = true;
                //oDoc.Close();

            }
        }


        #endregion

        #region Organismos

        readonly ObservableCollection<Organismo> organismosImprimir;

        public WordReports(ObservableCollection<Organismo> organismosImprimir)
        {
            this.organismosImprimir = organismosImprimir;
        }


        public void ImprimeDetalleOrganismo()
        {

            string nuevoDoc = Path.GetTempFileName() + ".docx";

            try
            {

                oWord = new Application();
                aDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);



                Paragraph oPara1 = aDoc.Content.Paragraphs.Add(ref oMissing);
                //oPara1.Range.ParagraphFormat.Space1;
                oPara1.Range.Text = "Reporte de Adscripciones";
                oPara1.Range.Font.Bold = 1;
                oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oPara1.Range.Font.Size = 16;
                oPara1.Range.Font.Name = "Arial";
                oPara1.Format.SpaceAfter = 0;    //24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter();


                foreach (Organismo organismo in organismosImprimir)
                {

                    Estado estado = (from n in PaisesSingleton.Estados
                                     where n.IdEstado == organismo.Estado
                                     select n).ToList()[0];

                    Pais pais = (from n in PaisesSingleton.Paises
                                 where n.IdPais == estado.IdPais
                                 select n).ToList()[0];


                    this.ParagraphAfter(oPara1, 2);

                    Range wrdRng = aDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

                    Table oTable = aDoc.Tables.Add(wrdRng, 7, 2, ref oMissing, ref oMissing);
                    oTable.Range.ParagraphFormat.SpaceAfter = 6;
                    oTable.Range.Font.Size = 9;
                    oTable.Range.Font.Name = "Arial";
                    oTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    oTable.Range.Font.Bold = 0;
                    oTable.Borders.Enable = 0;

                    oTable.Columns[1].SetWidth(100, WdRulerStyle.wdAdjustSameWidth);
                    oTable.Columns[2].SetWidth(300, WdRulerStyle.wdAdjustSameWidth);

                    oTable.Cell(1, 1).Range.Text = organismo.TipoOrganismoStr;
                    oTable.Cell(1, 1).Range.Font.Size = 12;
                    oTable.Cell(1, 1).Range.Font.Bold = 1;
                    oTable.Cell(1, 2).Range.Text = organismo.OrganismoDesc;
                    oTable.Cell(1, 2).Range.Font.Size = 12;
                    oTable.Cell(1, 2).Range.Font.Bold = 1;
                    oTable.Cell(2, 1).Range.Text = "Dirección";
                    oTable.Cell(2, 2).Range.Text = organismo.Calle;
                    oTable.Cell(3, 1).Range.Text = "Colonia";
                    oTable.Cell(3, 2).Range.Text = organismo.Colonia;
                    oTable.Cell(4, 1).Range.Text = "Delegación/Municipio";
                    oTable.Cell(4, 2).Range.Text = organismo.Delegacion;
                    oTable.Cell(5, 2).Range.Text = String.Format("{0}, {1}", pais.PaisDesc, estado.EstadoDesc);
                    oTable.Cell(6, 1).Range.Text = "C.P.";
                    oTable.Cell(6, 2).Range.Text = organismo.Cp;
                    oTable.Cell(7, 1).Range.Text = "Teléfono(s):";
                    oTable.Cell(7, 2).Range.Text = organismo.Telefono;

                    this.ParagraphAfter(oPara1, 1);



                    ObservableCollection<Adscripcion> adscripciones = new TitularModel().GetTitulares(organismo);

                    if (adscripciones.Count > 0)
                    {
                        oPara1.Range.Text = "Integrantes";
                        oPara1.Range.Font.Bold = 1;
                        oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        oPara1.Range.Font.Size = 14;

                        wrdRng = aDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

                        oTable = aDoc.Tables.Add(wrdRng, adscripciones.Count, 2, ref oMissing, ref oMissing);
                        oTable.Range.ParagraphFormat.SpaceAfter = 6;
                        oTable.Range.Font.Size = 9;
                        oTable.Range.Font.Name = "Arial";
                        oTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        oTable.Range.Font.Bold = 0;
                        oTable.Borders.Enable = 0;

                        oTable.Columns[1].SetWidth(150, WdRulerStyle.wdAdjustSameWidth);
                        oTable.Columns[2].SetWidth(300, WdRulerStyle.wdAdjustSameWidth);

                        int fila = 1;
                        foreach (Adscripcion ads in adscripciones)
                        {

                            string titulo = (from n in TituloSingleton.Titulos
                                             where n.IdTitulo == ads.Titular.IdTitulo
                                             select n.TituloDesc).ToList()[0];

                            string funcion = (from n in FuncionesSingleton.Funciones
                                              where n.IdElemento == ads.Funcion
                                              select n.Descripcion).ToList()[0];

                            oTable.Cell(fila, 1).Range.Text = funcion;
                            oTable.Cell(fila, 2).Range.Text = String.Format("{0} {1} {2}", titulo, ads.Titular.Apellidos, ads.Titular.Nombre);

                            fila++;
                        }
                    }
                    oPara1.Range.Text = "__________________________________________________________________________________";
                    oPara1.Range.Font.Bold = 0;
                    oPara1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara1.Range.Font.Size = 14;

                }

                aDoc.SaveAs(nuevoDoc);
                oWord.Visible = true;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,WordReports", "PadronApi");
            }
        }


        #endregion



        public bool GeneraOficiosEnvio(int idObra,int idPadron)
        {
            modelPlantilla = new PlantillaModel();

            bool isComplete = false;

            folio = 1;

            string rutaBase = ConfigurationManager.AppSettings["Documentos"]; 

            string machote = rutaBase + "DocEncabezado.docx";
            string nuevoDoc =  filePath;

            Obra obra = new ObraModel().GetObras(idObra);
            presentacion = (from n in ElementalPropertiesSingleton.Presentacion
                            where n.IdElemento == obra.Presentacion
                            select n.Descripcion).ToList()[0];

            if (oficioInicial == 0)
            {
                contadorOficio = new ReportesModel().GetNextNumOficio();
                oficioInicial = contadorOficio;
            }
            else
                contadorOficio = oficioInicial;

            autoriza = new System.Drawing.Bitmap(PadronApi.Properties.Resources.autho2);

            imagenAutorizada = Path.GetTempPath() + "\\auto2.jpg";
            autoriza.Save(imagenAutorizada);

            try
            {
                //  Just to kill WINWORD.EXE if it is running
                //  copy letter format to temp.doc
                File.Copy(machote, nuevoDoc, true);
                //  create missing object
                object missing = Missing.Value;
                //  create Word application object
                Application wordApp = new Application();
                //  create Word document object
                aDoc = null;
                //  create & define filename object with temp.doc
                object filename = nuevoDoc;
                //  if temp.doc available
                if (File.Exists((string)filename))
                {
                    object readOnly = false;
                    object isVisible = false;
                    //  make visible Word application
                    wordApp.Visible = false;
                    //  open Word document named temp.doc
                    aDoc = wordApp.Documents.Open(ref filename, ref missing,
                        ref readOnly, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref isVisible, ref missing, ref missing,
                        ref missing, ref missing);
                    aDoc.Activate();



                    foreach (PlantillaDto plantilla in plantillaDistr)
                    {

                        if (plantilla.TipoDistribucion < 4)
                        {
                            if (plantilla.Particular > 0)
                                this.GeneraPagina1a3(aDoc, obra, plantilla, 1);

                            if (plantilla.Oficina > 0)
                                this.GeneraPagina1a3(aDoc, obra, plantilla, 2);

                            if (plantilla.Biblioteca > 0)
                                this.GeneraPagina1a3(aDoc, obra, plantilla, 3);

                            if (plantilla.Personal > 0)
                                this.GeneraPagina1a3(aDoc, obra, plantilla, 1);
                        }
                        else
                        {
                            if (plantilla.Particular > 0)
                                this.GeneraPagina4(aDoc, obra, plantilla, 1, plantilla.Particular + plantilla.Autor);

                            if (plantilla.Oficina > 0)
                                this.GeneraPagina4(aDoc, obra, plantilla, 2, plantilla.Oficina);

                            if (plantilla.Biblioteca > 0)
                                this.GeneraPagina4(aDoc, obra, plantilla, 3, plantilla.Biblioteca);

                            if (plantilla.Personal > 0)
                                this.GeneraPagina4(aDoc, obra, plantilla, 1, plantilla.Personal);
                        }

                        
                    }

                    FindAndReplace(wordApp, "<<Fecha Distribucion>>", DateTimeUtilities.ToLongDateFormat(obra.FechaDistribuye),2);
                    aDoc.Save();

                   wordApp.Visible = true;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }

                if(!vuelveAGenerarOficio)
                    new ReportesModel().UpdateNumerosOficio(idPadron,oficioInicial, contadorOficio - 1 );
                isComplete = true;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,WordReports", "PadronApi");
            }
            return isComplete;
        }

        /// <summary>
        /// Genera la página del oficio respectivo de cada uno de los titulares de los organismos que pertenecen 
        /// a las partes que van de la uno a la 4
        /// </summary>
        /// <param name="aDoc"></param>
        /// <param name="obra"></param>
        /// <param name="plantilla"></param>
        /// <param name="propiedad"></param>
        private void GeneraPagina1a3(Document aDoc, Obra obra, PlantillaDto plantilla, int propiedad)
        {
            string propStr = String.Empty;

            if (propiedad == 1)
                propStr = "particular.";
            else if (propiedad == 2)
                propStr = "de la oficina.";
            else if (propiedad == 3)
                propStr = "de la biblioteca";

            Paragraph oPara1 = aDoc.Content.Paragraphs.Add(ref oMissing);
            //oPara1.Range.ParagraphFormat.Space1;
            oPara1.Range.Text = String.Format("Of. Núm. CCST/DDP-PAD-{0}-{1}-{2}", ((DateTime.Now.Month < 10) ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString()), contadorOficio, DateTime.Now.Year.ToString().Substring(2, 2));
            oPara1.Range.Font.Bold = 0;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            oPara1.Range.Font.Size = 10;
            oPara1.Range.Font.Name = "Arial";
            oPara1.Format.SpaceAfter = 0;    //24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "Ciudad de México, a <<Fecha Distribucion>>";
            this.ParagraphAfter(oPara1, 3);
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara1.Range.Text = plantilla.Nombre;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.InsertParagraphAfter();


            if (plantilla.GrupoOrganismo == 1 && plantilla.Funcion == 1)
                oPara1.Range.Text = "Presidente del " + plantilla.Organismo;
            else if (plantilla.GrupoOrganismo == 1 && plantilla.Funcion == 0)
                oPara1.Range.Text = plantilla.Organismo;
            else
                oPara1.Range.Text = String.Format("{0} de {1}", new FuncionConverter().Convert(plantilla.Funcion, null, null, null), plantilla.Organismo);


            oPara1.Range.ParagraphFormat.RightIndent = 150;
            oPara1.Range.Font.Bold = 0;
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "P r e s e n t e";
            oPara1.Range.ParagraphFormat.RightIndent = 0;
            this.ParagraphAfter(oPara1, 3);
            oPara1.Range.Text = "Distinguido " + plantilla.Nombre;
            this.ParagraphAfter(oPara1, 2);
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            oPara1.Range.Text = "Me es grato enviarle por este medio la publicación oficial que se detalla:";
            this.ParagraphAfter(oPara1, 2);

            oPara1.Range.Text = tituloObra;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            this.ParagraphAfter(oPara1, 2);

            if (obra.Presentacion == 4)
            {
                oPara1.Range.Text = String.Format("TOTAL: {0} {1}", obra.NumLibros, ((obra.TipoObra == 1) ? "USB" : "DISCO ÓPTICO"));
                this.ParagraphAfter(oPara1, 2);
            }
            else
            {
                oPara1.Range.Text = String.Format("{0} EJEMPLAR EN PRESENTACIÓN {1}", obra.NumLibros, presentacion.ToUpper());
                this.ParagraphAfter(oPara1, 2);
                oPara1.Range.Text = String.Format("TOTAL: {0} {1}", obra.NumLibros, ((obra.NumLibros > 1) ? "LIBROS" : "LIBRO"));
                this.ParagraphAfter(oPara1, 2);
            }

            oPara1.Range.Text = "Me permito comunicarle que la obra citada es en propiedad " + propStr;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            oPara1.Range.Font.Bold = 0;
            this.ParagraphAfter(oPara1, 3);
            oPara1.Range.Text = aclaraciones;
            this.ParagraphAfter(oPara1, 3);
            oPara1.Range.Text = "Sin otro particular, le envío un cordial saludo.";
            this.ParagraphAfter(oPara1, 2);

            oPara1.Range.Text = "A T E N T A M E N T E";
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            this.ParagraphAfter(oPara1, 3);


            InlineShape inlineShape = oPara1.Range.InlineShapes.AddPicture(imagenAutorizada, oMissing, oMissing, oMissing);
            Word.Shape shape = inlineShape.ConvertToShape();
            shape.WrapFormat.Type = WdWrapType.wdWrapTopBottom;
            shape.Left = (float)WdShapePosition.wdShapeCenter;
            shape.LockAspectRatio = MsoTriState.msoTrue;
            shape.ScaleHeight(0.35f, MsoTriState.msoTrue, MsoScaleFrom.msoScaleFromTopLeft);

            //oPara1.Range.Paste();
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = titularCoord;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "COORDINADORA";
            oPara1.Range.Font.Bold = 0;
            this.ParagraphAfter(oPara1, 3);


            oPara1.Range.Text = "c.c.p- El archivo.-";
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara1.Range.Font.Size = 8;
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "JZG/LVP/JJMM";
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara1.Range.Font.Size = 8;
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = String.Format("Folio {0}-{1}", plantilla.TipoDistribucion, folio);
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            oPara1.Range.Font.Size = 8;
            oPara1.Range.InsertParagraphAfter();

            aDoc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);
            folio++;

            if (!vuelveAGenerarOficio)
                modelPlantilla.RegistroOficiostitular(plantilla, contadorOficio);

            contadorOficio++;

            oPara1 = null;
        }

        private void GeneraPagina4(Document aDoc, Obra obra, PlantillaDto plantilla, int propiedad, int cantidad)
        {
            string propStr = String.Empty;

            if (propiedad == 1)
                propStr = "Particular.";
            else if (propiedad == 2)
                propStr = "Oficina.";
            else if (propiedad == 3)
                propStr = "Biblioteca";

            Paragraph oPara1 = aDoc.Content.Paragraphs.Add(ref oMissing);
            //oPara1.Range.ParagraphFormat.Space1;
            oPara1.Range.Text = String.Format("Of. Núm. CCST/DDP-PAD-{0}-{1}-{2}", ((DateTime.Now.Month < 10) ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString()), contadorOficio, DateTime.Now.Year.ToString().Substring(2, 2));
            oPara1.Range.Font.Bold = 0;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            oPara1.Range.Font.Size = 9;
            oPara1.Range.Font.Name = "Arial";
            oPara1.Format.SpaceAfter = 0;    //24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "Ciudad de México, a <<Fecha Distribucion>>";
            this.ParagraphAfter(oPara1, 2);
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara1.Range.Text = plantilla.Nombre;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.InsertParagraphAfter();

            if (plantilla.Funcion == 0)
                oPara1.Range.Text = plantilla.Organismo;
            else
                oPara1.Range.Text = String.Format("{0} de {1}", new FuncionConverter().Convert(plantilla.Funcion, null, null, null), plantilla.Organismo);

            oPara1.Range.ParagraphFormat.RightIndent = 150;
            oPara1.Range.Font.Bold = 0;
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "P r e s e n t e";
            oPara1.Range.ParagraphFormat.RightIndent = 0;
            this.ParagraphAfter(oPara1, 2);
            oPara1.Range.Text = "Distinguido " + plantilla.Nombre;
            this.ParagraphAfter(oPara1, 2);
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            oPara1.Range.Text = "Me es grato enviarle por este medio la publicación oficial que se detalla:";
            this.ParagraphAfter(oPara1, 2);

            oPara1.Range.Text = tituloObra;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            this.ParagraphAfter(oPara1, 2);

            Range wrdRng = aDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            Table oTable = aDoc.Tables.Add(wrdRng, 2, 3, ref oMissing, ref oMissing);
            oTable.Rows[1].HeadingFormat = -1; //Repite el encabezado de la tabla en cada hoja
            oTable.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            oTable.Range.ParagraphFormat.SpaceAfter = 6;
            oTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oTable.Range.Font.Size = 9;
            oTable.Range.Font.Name = "Arial";
            oTable.Range.Font.Bold = 0;
            oTable.Borders.Enable = 0;

            int[] tamanios = new int[3] { 100, 100, 100 };

            string[] encabezados = new string[3] { "Presentación", "Cantidad", "Propiedad" };

            this.SetWidthAndHeader(oTable, tamanios, encabezados);

            oTable.Cell(2, 1).Range.Text = presentacion;
            oTable.Cell(2, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oTable.Cell(2, 2).Range.Text = cantidad.ToString();
            oTable.Cell(2, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oTable.Cell(2, 3).Range.Text = propStr;
            oTable.Cell(2, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            this.ParagraphAfter(oPara1, 1);

            oTable = null;
            oPara1.Range.Text = "Sin otro particular, le envío un cordial saludo.";
            oPara1.Range.Font.Bold = 0;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            this.ParagraphAfter(oPara1, 2);

            oPara1.Range.Text = "A T E N T A M E N T E";
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            this.ParagraphAfter(oPara1, 2);

            InlineShape inlineShape = oPara1.Range.InlineShapes.AddPicture(imagenAutorizada, oMissing, oMissing, oMissing);
            Word.Shape shape = inlineShape.ConvertToShape();
            shape.WrapFormat.Type = WdWrapType.wdWrapTopBottom;
            shape.Left = (float)WdShapePosition.wdShapeCenter;
            shape.LockAspectRatio = MsoTriState.msoTrue;
            shape.ScaleHeight(0.35f, MsoTriState.msoTrue, MsoScaleFrom.msoScaleFromTopLeft);

            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = titularCoord;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "COORDINADORA";
            oPara1.Range.Font.Bold = 0;
            this.ParagraphAfter(oPara1, 1);

            oPara1.Range.Text = "FAVOR DE NO RECORTAR EL OFICIO-ACUSE";
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.Font.Size = 7;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara1.Range.InsertParagraphAfter();

            wrdRng = aDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            Table oTable1 = aDoc.Tables.Add(wrdRng, 6, 1, ref oMissing, ref oMissing);
            oTable1.Rows[1].HeadingFormat = -1; //Repite el encabezado de la tabla en cada hoja
            oTable1.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            oTable1.Range.ParagraphFormat.SpaceAfter = 6;
            oTable1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oTable1.Range.Font.Size = 9;
            oTable1.Range.Font.Name = "Arial";
            oTable1.Range.Font.Bold = 0;
            oTable1.Borders.InsideLineStyle = WdLineStyle.wdLineStyleNone;
            oTable1.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;


            oTable1.Cell(1, 1).Range.Text = "Acuso a Usted de recibido por la publicación arriba señalada";
            oTable1.Cell(1, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            oTable1.Cell(2, 1).Range.Text = "_____________________________________________\r\n" + plantilla.Nombre;
            oTable1.Cell(2, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oTable1.Cell(3, 1).Range.Text = "Correo electrónico\r\n(Institucional)    _____________________________________________";
            oTable1.Cell(3, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oTable1.Cell(4, 1).Range.Text = "Si existiera alguna de las siguientes modificaciones, favor de indicarla en el espacio correspondiente:";
            oTable1.Cell(4, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oTable1.Cell(5, 1).Range.Text = "Cambio de titular\r\nCambio de plantilla\r\nOtros datos      ___________________________________________________________________";
            oTable1.Cell(5, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oTable1.Cell(6, 1).Range.Text = "FAVOR DE ACUSAR A LA BREVEDAD POSIBLE\r\n" +
                                           "ENVIAR RECIBO ESCANEADO EN PDF AL CORREO ELECTRÓNICO majimenez@mail.scjn.gob.mx o tgalicia@mail.scjn.gob.mx\r\n" +
                                           "O VÍA FAX A LOS TELÉFONOS 01 (55) 4113-1127 Ó 4113-1335\r\n" +
                                           "Y CONFIRMAR AL 01 (55) 4113-1000 EXT. 2280";
            oTable1.Cell(6, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

            oPara1.Range.Text = "JZG/LVP/JJMM";
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oPara1.Range.Font.Size = 8;
            oPara1.Range.InsertParagraphAfter();
            oPara1.Range.Text = "Folio 4-" + folio;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            oPara1.Range.Font.Size = 8;
            oPara1.Range.InsertParagraphAfter();

            aDoc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);
            folio++;

            if (!vuelveAGenerarOficio)
                modelPlantilla.RegistroOficiostitular(plantilla, contadorOficio);

            contadorOficio++;

            oPara1 = null;
            oTable = null;
            shape = null;
            inlineShape = null;
            oTable1 = null;
        }

        /// <summary>
        /// Inserta saltos de línea en un documento
        /// </summary>
        /// <param name="oPara"></param>
        /// <param name="cuantos">Cuantos saltos de línea consecutivos agregará</param>
        private void ParagraphAfter(Paragraph oPara, int cuantos)
        {
            while (cuantos > 0)
            {
                oPara.Range.InsertParagraphAfter();
                cuantos--;
            }
        }

        /// <summary>
        /// Genera el listado de titulares cuya adscripción se encuentra en el edificio sede de la SCJN, San Lázaro o Área metropolitana
        /// </summary>
        public void AcusesReciboFirma()
        {
            oWord = new Application();
            aDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            aDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;


            try
            {
                //Insert a paragraph at the beginning of the document.
                Paragraph oPara1 = aDoc.Content.Paragraphs.Add(ref oMissing);
                oPara1.Range.Text = "SUPREMA CORTE DE JUSTICIA DE LA NACIÓN";
                oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                oPara1.Range.Font.Bold = 1;
                oPara1.Range.Font.Size = 10;
                oPara1.Range.Font.Name = "Arial";
                oPara1.Format.SpaceAfter = 0;    //24 pt spacing after paragraph.
                this.ParagraphAfter(oPara1, 2);
                oPara1.Range.Text = "ACUSES DE RECIBO DEL PADRÓN DE DISTRIBUCIÓN ";
                oPara1.Range.InsertParagraphAfter();
                oPara1.Range.Text = "OBRA : " + tituloObra;
                oPara1.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                this.ParagraphAfter(oPara1, 2);


                fila = 1;
                Range wrdRng = aDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

                Table oTable = aDoc.Tables.Add(wrdRng, plantillaDistr.Count + 1, 7, ref oMissing, ref oMissing);
                oTable.Rows[1].HeadingFormat = -1; //Repite el encabezado de la tabla en cada hoja
                oTable.Range.ParagraphFormat.SpaceAfter = 6;
                oTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oTable.Range.Font.Size = 9;
                oTable.Range.Font.Name = "Arial";
                oTable.Range.Font.Bold = 0;
                oTable.Borders.Enable = 1;

                int[] tamanios = new int[7] { 40, 220, 130, 60, 50, 40, 100 };

                string[] encabezados = new string[7] { "#",  "Adscripción", "Titular",
                     "Propiedad", "Cantidad", "Fecha", "Nombre y Firma" };

                this.SetWidthAndHeader(oTable, tamanios, encabezados);

                fila++;
                const int DondeAgrego = 1;

                foreach (PlantillaDto print in plantillaDistr)
                {
                    bool tieneFila = false;

                    if (print.Particular > 0 || print.Autor > 0)
                    {
                        this.AddFilaAcuseCorte(oTable, print.Organismo, print.Nombre, print.Particular + print.Autor, "Particular");
                        tieneFila = true;
                    }

                    if (print.Oficina > 0)
                    {
                        if (tieneFila)
                            oTable.Rows.Add(oTable.Rows[plantillaDistr.Count + DondeAgrego]);

                        this.AddFilaAcuseCorte(oTable, print.Organismo, print.Nombre, print.Oficina, "Oficina");
                        tieneFila = true;
                    }

                    if (print.Personal > 0)
                    {
                        if (tieneFila)
                            oTable.Rows.Add(oTable.Rows[plantillaDistr.Count + DondeAgrego]);

                        this.AddFilaAcuseCorte(oTable, print.Organismo, print.Nombre, print.Personal, "Personal");
                        tieneFila = true;
                    }
                    if (print.Biblioteca > 0)
                    {
                        if (tieneFila)
                            oTable.Rows.Add(oTable.Rows[plantillaDistr.Count + DondeAgrego]);

                        this.AddFilaAcuseCorte(oTable, print.Organismo, print.Nombre, print.Biblioteca, "Biblioteca");
                        tieneFila = true;
                    }
                    if (print.Resguardo > 0)
                    {
                        if (tieneFila)
                            oTable.Rows.Add(oTable.Rows[plantillaDistr.Count + DondeAgrego]);

                        this.AddFilaAcuseCorte(oTable, print.Organismo, print.Nombre, print.Resguardo, "Resguardo");
                        tieneFila = true;
                    }
                    //if (print.Autor > 0)
                    //{
                    //    if (tieneFila)
                    //        oTable.Rows.Add(oTable.Rows[plantillaDistr.Count + dondeAgrego]);

                    //    this.AddFilaAcuseCorte(oTable, print.Organismo, print.Nombre, print.Autor, "Autor");
                    //    tieneFila = true;
                    //}
                }


                foreach (Section wordSection in aDoc.Sections)
                {
                    object pagealign = WdPageNumberAlignment.wdAlignPageNumberRight;
                    object firstpage = true;
                    wordSection.Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.Add(ref pagealign, ref firstpage);
                }

                oWord.ActiveDocument.SaveAs(filePath);
                oWord.ActiveDocument.Saved = true;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,WordReports", "PadronApi");
            }
            finally
            {
                oWord.Visible = true;
                //oDoc.Close();

            }
        }


        private void AddFilaAcuseCorte(Table oTable,string organismo, string nombre,int cantidad, string propiedad)
        {
            oTable.Cell(fila, 1).Range.Text = (fila - 1).ToString();
            oTable.Cell(fila, 2).Range.Text = organismo;
            oTable.Cell(fila, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            oTable.Cell(fila, 3).Range.Text = nombre;
            oTable.Cell(fila, 4).Range.Text = propiedad;
            oTable.Cell(fila, 5).Range.Text = cantidad.ToString();
            oTable.Cell(fila, 6).Range.Text = String.Empty;
            oTable.Cell(fila, 7).Range.Text = String.Empty;

            fila++;
        }


        public void ListadoContraloria(int rowNumber)
        {
            oWord = new Word.Application();
            aDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            aDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;
            aDoc.PageSetup.LeftMargin = 20f;

            int distribucion = 0, reserva = 0, zaragoza = 0, sede = 0, ventas = 0;

            try
            {
                fila = 1;
                Range wrdRng = aDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

                Table oTable = aDoc.Tables.Add(wrdRng, rowNumber + 1, 11, ref oMissing, ref oMissing);
                oTable.Rows[1].HeadingFormat = -1; //Repite el encabezado de la tabla en cada hoja
                oTable.Range.ParagraphFormat.SpaceAfter = 6;
                oTable.Range.Font.Size = 7;
                oTable.Range.Font.Name = "Arial";
                oTable.Range.Font.Bold = 0;
                oTable.Borders.Enable = 1;


                int[] tamanios = new int[11] { 25, 100, 250, 160, 40, 40, 30, 35, 20, 25, 25 };

                string[] encabezados = new string[11] { "#", "Lugar", "Adscripción", "Titular",
                    "Acuerdo", "Prop.", "Total", "Rústica", "Piel", "Hol.", "Mes." };

                this.SetWidthAndHeader(oTable, tamanios, encabezados);

                fila++;

                oWord.Visible = true;

                foreach (PlantillaDto print in plantillaDistr)
                {
                    if (print.Particular > 0 || print.Autor > 0)
                    {
                        this.AddFilaContraloria(oTable, print, print.Particular + print.Autor, "Particular");
                        distribucion += print.Particular + print.Autor;
                    }

                    if (print.Oficina > 0)
                    {
                        
                        this.AddFilaContraloria(oTable, print, print.Oficina, "Oficina");
                        distribucion += print.Oficina;
                    }

                    if (print.Personal > 0)
                    {
                        

                        this.AddFilaContraloria(oTable, print, print.Personal, "Integrantes");
                        distribucion += print.Personal;
                    }
                    if (print.Biblioteca > 0)
                    {
                        

                        this.AddFilaContraloria(oTable, print, print.Biblioteca, "Biblioteca");
                        distribucion += print.Biblioteca;
                    }
                    if (print.Resguardo > 0)
                    {
                        if (print.IdOrganismo == 6002)
                        {
                            this.AddFilaContraloria(oTable, print, print.Resguardo, "Ventas");
                            ventas = print.Resguardo;
                        }
                        else
                        {
                            if (print.IdOrganismo == 6000)
                                sede = print.Resguardo;
                            else if (print.IdOrganismo == 6001)
                                zaragoza = print.Resguardo;
                            else if (print.IdOrganismo == 32630)
                                reserva = print.Resguardo;

                            this.AddFilaContraloria(oTable, print, print.Resguardo, "Resguardo");
                        }
                    }
                }

                Paragraph oPara1 = aDoc.Content.Paragraphs.Add(ref oMissing);
                oPara1.Range.Text = " ";
                this.ParagraphAfter(oPara1, 2);

                Range range2 = aDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

                Table oTableTotales = aDoc.Tables.Add(range2, 6, 2, ref oMissing, ref oMissing);
                oTableTotales.Rows[1].HeadingFormat = -1; //Repite el encabezado de la tabla en cada hoja
                oTableTotales.Range.ParagraphFormat.SpaceAfter = 6;
                oTableTotales.Range.Font.Size = 7;
                oTableTotales.Range.Font.Name = "Arial";
                oTableTotales.Range.Font.Bold = 0;
                oTableTotales.Borders.Enable = 1;


                tamanios = new int[2] { 80, 80 };

                encabezados = new string[2] { " ", "Total" };
                fila = 1;

                this.SetWidthAndHeader(oTableTotales, tamanios, encabezados);

                oTableTotales.Cell(2, 1).Range.Text = "Distribución";
                oTableTotales.Cell(2, 2).Range.Text = distribucion.ToString();
                oTableTotales.Cell(2, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oTableTotales.Cell(3, 1).Range.Text = "Reserva Histórica";
                oTableTotales.Cell(3, 2).Range.Text = reserva.ToString();
                oTableTotales.Cell(3, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oTableTotales.Cell(4, 1).Range.Text = "Almacén Zaragoza";
                oTableTotales.Cell(4, 2).Range.Text = zaragoza.ToString();
                oTableTotales.Cell(4, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oTableTotales.Cell(5, 1).Range.Text = "Almacén Edificio Sede";
                oTableTotales.Cell(5, 2).Range.Text = sede.ToString();
                oTableTotales.Cell(5, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                oTableTotales.Cell(6, 1).Range.Text = "Ventas";
                oTableTotales.Cell(6, 2).Range.Text = ventas.ToString();
                oTableTotales.Cell(6, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                foreach (Section section in aDoc.Sections)
                {
                    Range headerRange = section.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.Size = 14;
                    headerRange.Font.Bold = 1;
                    headerRange.Text = "Reporte del Padrón de Distribución\r\n" + tituloObra;

                }


                foreach (Section wordSection in aDoc.Sections)
                {
                    object pagealign = WdPageNumberAlignment.wdAlignPageNumberRight;
                    object firstpage = true;
                    wordSection.Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.Add(ref pagealign, ref firstpage);
                }

                oWord.ActiveDocument.SaveAs(filePath);
                oWord.ActiveDocument.Saved = true;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,WordReports", "PadronApi");
            }
            finally
            {
                oWord.Visible = true;
            }
        }


        private void SetWidthAndHeader(Table oTable, int[] tamColumna, string[] encabezado)
        {
            for (int columna = 1; columna < tamColumna.Count() + 1; columna++)
            {
                oTable.Columns[columna].SetWidth(tamColumna[columna - 1], WdRulerStyle.wdAdjustSameWidth);
                oTable.Cell(fila, columna).Range.Text = encabezado[columna - 1];
                oTable.Cell(fila, columna).Range.Font.Bold = 1;
            }
        }


        private void AddFilaContraloria(Table oTable, PlantillaDto item, int cantidad, string propiedad)
        {
            oTable.Cell(fila, 1).Range.Text = (fila - 1).ToString();
            oTable.Cell(fila, 2).Range.Text = String.Format("{0}, {1}", item.CiudadStr, item.EstadoStr);
            oTable.Cell(fila, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            oTable.Cell(fila, 3).Range.Text = item.Organismo;
            oTable.Cell(fila, 4).Range.Text = item.Nombre;
            oTable.Cell(fila, 5).Range.Text = "Acuerdo";
            oTable.Cell(fila, 6).Range.Text = propiedad;
            oTable.Cell(fila, 7).Range.Text = cantidad.ToString();
            oTable.Cell(fila, 8).Range.Text = cantidad.ToString();
            oTable.Cell(fila, 9).Range.Text = "0";
            oTable.Cell(fila, 10).Range.Text = "0";
            oTable.Cell(fila, 11).Range.Text = "0";

            fila++;
        }


        public bool GeneraEtiquetas()
        {
            List<int> yaExiste = new List<int>();

            ObservableCollection<PlantillaDto> soloImprime = new ObservableCollection<PlantillaDto>();

            modelPlantilla = new PlantillaModel();

            bool isComplete = false;

            folio = 1;

            string rutaBase = ConfigurationManager.AppSettings["Documentos"];

            string machote = rutaBase + "Etiquetas2.docx";
            string nuevoDoc = filePath;

            foreach (PlantillaDto plantilla in plantillaDistr)
            {
                if (!yaExiste.Contains(plantilla.IdOrganismo))
                {
                    yaExiste.Add(plantilla.IdOrganismo);
                    soloImprime.Add(plantilla);
                }
            }



            try
            {
                //  Just to kill WINWORD.EXE if it is running
                //  copy letter format to temp.doc
                File.Copy(machote, nuevoDoc, true);
                //  create missing object
                object missing = Missing.Value;
                //  create Word application object
                Application wordApp = new Application();
                //  create Word document object
                aDoc = null;
                //  create & define filename object with temp.doc
                object filename = nuevoDoc;
                //  if temp.doc available
                if (File.Exists((string)filename))
                {
                    object readOnly = false;
                    object isVisible = false;
                    //  make visible Word application
                    wordApp.Visible = false;
                    //  open Word document named temp.doc
                    aDoc = wordApp.Documents.Open(ref filename, ref missing,
                        ref readOnly, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref isVisible, ref missing, ref missing,
                        ref missing, ref missing);
                    aDoc.Activate();


                    object start = aDoc.Content.Start;
                    object end = aDoc.Content.End;


                    aDoc.Range(ref start, ref end).Copy();

                    int divide = soloImprime.Count / 10;

                    while (divide > 0)
                    {

                        object newStart = aDoc.Content.End - 1;
                        object newEnd = aDoc.Content.End;

                        Range rng = aDoc.Range(ref newStart, ref newEnd);
                        rng.Paste();
                        divide--;
                    }

                    foreach (PlantillaDto plantilla in soloImprime)
                    {
                        Organismo currentOrganismo = new OrganismoModel().GetOrganismos(plantilla.IdOrganismo);
                        //Titular currentTitular = new TitularModel().GetTitulares(plantilla.IdTitular);

                        FindAndReplace(wordApp, "<<Organismo>>", plantilla.Organismo, 1);
                        if (plantilla.Funcion == 1)
                            FindAndReplace(wordApp, "<<Funcion>>", "Presidente. ", 1);
                        else
                            FindAndReplace(wordApp, "<<Funcion>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Titulo>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Nombre>>", plantilla.Nombre, 1);
                        FindAndReplace(wordApp, "<<Calle>>", currentOrganismo.Calle, 1);
                        FindAndReplace(wordApp, "<<Colonia>>", " Col. " + currentOrganismo.Colonia, 1);
                        FindAndReplace(wordApp, "<<Delegacion>>", "  " + currentOrganismo.Delegacion, 1);
                        FindAndReplace(wordApp, "<<CP>>", "C.P. " + currentOrganismo.Cp, 1);
                        FindAndReplace(wordApp, "<<Telefono>>", String.Format("Tels. {0} {1}     {2} {3}     {4} {5}     {6} {7}     ", 
                            currentOrganismo.Telefono, currentOrganismo.Extension, currentOrganismo.Telefono2, currentOrganismo.Extension2, 
                            currentOrganismo.Telefono3, currentOrganismo.Extension3, currentOrganismo.Telefono4, currentOrganismo.Extension4), 1);
                        FindAndReplace(wordApp, "<<Ciudad>>", plantilla.CiudadStr + ", ", 1);
                        FindAndReplace(wordApp, "<<Estado>>", plantilla.EstadoStr, 1);
                        if (currentOrganismo.Circuito > 0)
                            FindAndReplace(wordApp, "<<Circuito>>", String.Format("Circuito: {0}°", currentOrganismo.Circuito), 1);
                        else
                            FindAndReplace(wordApp, "<<Circuito>>", String.Empty, 1);
                    }

                    for (int index = 1; index < 10; index++)
                    {
                        FindAndReplace(wordApp, "<<Organismo>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Funcion>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Titulo>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Nombre>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Calle>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Colonia>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Delegacion>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<CP>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Telefono>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Ciudad>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Estado>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Circuito>>", String.Empty, 1);
                    }


                    aDoc.Save();

                    wordApp.Visible = true;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }

                isComplete = true;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,WordReports", "PadronApi");
            }
            return isComplete;
        }

        public bool GeneraEtiquetas(ObservableCollection<Organismo> organismosEtiquetas)
        {
            modelPlantilla = new PlantillaModel();

            bool isComplete = false;
            folio = 1;

            string rutaBase = ConfigurationManager.AppSettings["Documentos"];
            string machote = rutaBase + "Etiquetas.docx";
            string nuevoDoc = filePath;

            try
            {
                //  Just to kill WINWORD.EXE if it is running
                //  copy letter format to temp.doc
                File.Copy(machote, nuevoDoc, true);
                //  create missing object
                object missing = Missing.Value;
                //  create Word application object
                Application wordApp = new Application();
                //  create Word document object
                aDoc = null;
                //  create & define filename object with temp.doc
                object filename = nuevoDoc;
                //  if temp.doc available
                if (File.Exists((string)filename))
                {
                    object readOnly = false;
                    object isVisible = false;
                    //  make visible Word application
                    wordApp.Visible = false;
                    //  open Word document named temp.doc
                    aDoc = wordApp.Documents.Open(ref filename, ref missing,
                        ref readOnly, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref isVisible, ref missing, ref missing,
                        ref missing, ref missing);
                    aDoc.Activate();

                    object start = aDoc.Content.Start;
                    object end = aDoc.Content.End;

                    aDoc.Range(ref start, ref end).Copy();

                    int divide = organismosEtiquetas.Count / 10;

                    while (divide > 0)
                    {
                        object newStart = aDoc.Content.End - 1;
                        object newEnd = aDoc.Content.End;

                        Range rng = aDoc.Range(ref newStart, ref newEnd);
                        rng.Paste();
                        divide--;
                    }

                    foreach (Organismo organismo in organismosEtiquetas)
                    {
                        organismo.Adscripciones = new TitularModel().GetTitulares(organismo);

                        if (organismo.Adscripciones == null || organismo.Adscripciones.Count == 0)
                        {

                            Titular titular = null;
                            int funcion = 0;

                            if (organismo.IdGrupo == 1)
                            {
                                foreach (Adscripcion ads in organismo.Adscripciones)
                                {
                                    if (ads.Funcion == 1)
                                    {
                                        titular = ads.Titular;
                                        funcion = 1;
                                        break;
                                    }
                                }

                                if (titular == null)
                                {
                                    titular = organismo.Adscripciones[0].Titular;
                                }
                            }
                            else
                            {
                                titular = organismo.Adscripciones[0].Titular;
                            }

                            FindAndReplace(wordApp, "<<Organismo>>", organismo.OrganismoDesc, 1);

                            if (funcion == 1)
                                FindAndReplace(wordApp, "<<Funcion>>", "Presidente. ", 1);
                            else
                                FindAndReplace(wordApp, "<<Funcion>>", String.Empty, 1);

                            FindAndReplace(wordApp, "<<Titulo>>", String.Empty, 1);
                            FindAndReplace(wordApp, "<<Nombre>>", String.Format("{0} {1}", titular.Nombre, titular.Apellidos), 1);
                            FindAndReplace(wordApp, "<<Calle>>", organismo.Calle, 1);
                            FindAndReplace(wordApp, "<<Colonia>>", " Col. " + organismo.Colonia, 1);
                            FindAndReplace(wordApp, "<<Delegacion>>", "  " + organismo.Delegacion, 1);
                            FindAndReplace(wordApp, "<<CP>>", "C.P. " + organismo.Cp, 1);
                            FindAndReplace(wordApp, "<<Telefono>>", String.Format("Tels. {0} {1}     {2} {3}     {4} {5}     {6} {7}     ", organismo.Telefono, organismo.Extension, organismo.Telefono2, organismo.Extension2, organismo.Telefono3, organismo.Extension3, organismo.Telefono4, organismo.Extension4)
                                , 1);

                            string ciudad = (from n in PaisesSingleton.Ciudades
                                             where n.IdCiudad == organismo.Ciudad
                                             select n.CiudadDesc).ToList()[0];

                            string estado = (from n in PaisesSingleton.Estados
                                             where n.IdEstado == organismo.Estado
                                             select n.EstadoDesc).ToList()[0];

                            FindAndReplace(wordApp, "<<Ciudad>>", ciudad + ", ", 1);
                            FindAndReplace(wordApp, "<<Estado>>", estado, 1);
                            if (organismo.Circuito > 0)
                                FindAndReplace(wordApp, "<<Circuito>>", String.Format("Circuito: {0}°", organismo.Circuito), 1);
                            else
                                FindAndReplace(wordApp, "<<Circuito>>", String.Empty, 1);
                        }
                    }

                    for (int index = 1; index < 10; index++)
                    {
                        FindAndReplace(wordApp, "<<Organismo>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Funcion>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Titulo>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Nombre>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Calle>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Colonia>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Delegacion>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<CP>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Telefono>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Ciudad>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Estado>>", String.Empty, 1);
                        FindAndReplace(wordApp, "<<Circuito>>", String.Empty, 1);
                    }


                    aDoc.Save();

                    wordApp.Visible = true;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }

                isComplete = true;
            }
            catch (Exception ex)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,WordReports", "PadronApi");
            }
            return isComplete;
        }



        private void FindAndReplace(Application wordApp,
            object findText, object replaceText, int replaceType)
        {
            object matchCase = true;
            object matchWholeWord = false;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = replaceType;
            object wrap = 1;
            wordApp.Selection.Find.Execute(ref findText, ref matchCase,
                ref matchWholeWord, ref matchWildCards, ref matchSoundsLike,
                ref matchAllWordForms, ref forward, ref wrap, ref format,
                ref replaceText, ref replace, ref matchKashida,
                ref matchDiacritics,
                ref matchAlefHamza, ref matchControl);
        }

        //private WdColorIndex GetCellColor(int idColor)
        //{
        //    if (idColor == 2)
        //        return WdColorIndex.wdRed;
        //    else if (idColor == 3)
        //        return WdColorIndex.wdBlue;
        //    else if (idColor == 4)
        //        return WdColorIndex.wdViolet;
        //    else if (idColor == 5)
        //        return WdColorIndex.wdDarkRed;
        //    else if (idColor == 6)
        //        return WdColorIndex.wdGreen;
        //    else
        //        return WdColorIndex.wdBlack;
        //}




    
    }
}
