using Kiosko.Dto;
using Microsoft.Office.Interop.Word;
using PadronApi.Dto;
using PadronApi.Model;
using PadronApi.Singletons;
using ScjnUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kiosko.ReportesKiosko
{
    public class WordKiosko
    {

        Document oDoc;

        Application oWord;
        object oMissing = Missing.Value;
        object oEndOfDoc = "\\endofdoc";


        int fila = 0;

        



        private List<AutorColabora> colaboradores;

        /// <summary>
        /// Genera un documento con la síntesis y autores de las obras seleccionadas
        /// </summary>
        /// <param name="obras"></param>
        public void PrintInfoPorObra(List<Obra> obras)
        {
            oWord = new Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = WdOrientation.wdOrientPortrait;

            //Insert a paragraph at the beginning of the document.
            Paragraph oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);

            try
            {
                foreach (Obra obra in obras)
                {

                    oPara1.Range.Text = obra.Titulo;
                    oPara1.Range.Font.Bold = 1;
                    oPara1.Range.Font.Size = 16;
                    oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara1.Range.InsertParagraphAfter();


                    oPara1.Range.Font.Bold = 0;
                    oPara1.Range.Font.Size = 13;
                    oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                    oPara1.Range.Text = obra.Sintesis;
                    oPara1.Range.InsertParagraphAfter();
                    oPara1.Range.InsertParagraphAfter();

                    AutorColabora colabora = new AutorColabora();
                    colaboradores = colabora.GetAutoresForColaboracion(obra);
                    colaboradores.AddRange(colabora.GetInstitucionesForColaboracion(obra));

                    if (colaboradores.Count == 1)
                    {
                        oPara1.Range.Font.Bold = 1;
                        oPara1.Range.Font.Size = 14;
                        oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        oPara1.Range.Text = "Autor";
                        oPara1.Range.InsertParagraphAfter();

                        oPara1.Range.Font.Bold = 0;
                        oPara1.Range.Font.Size = 13;
                        oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        oPara1.Range.Text = colaboradores[0].NombreAutor;
                        oPara1.Range.InsertParagraphAfter();
                    }
                    else if (colaboradores.Count > 1)
                    {
                        fila = 1;
                        Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

                        Table oTable = oDoc.Tables.Add(wrdRng, colaboradores.Count + 1, 4, ref oMissing, ref oMissing);
                        //oTable.Rows[1].HeadingFormat = 1;
                        oTable.Range.ParagraphFormat.SpaceAfter = 6;
                        oTable.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        oTable.Range.Font.Size = 9;
                        oTable.Range.Font.Name = "Arial";
                        oTable.Range.Font.Bold = 0;
                        oTable.Borders.Enable = 1;

                        int[] tamanios = new int[4] { 60, 100, 80, 200 };

                        string[] encabezados = new string[4] { " ", "Nombre", "Participación", "Texto de colaboración" };

                        this.SetWidthAndHeader(oTable, tamanios, encabezados);

                        fila++;
                        int consecutivo = 1;

                        foreach (AutorColabora print in colaboradores)
                        {
                            oTable.Cell(fila, 1).Range.Text = consecutivo.ToString();
                            oTable.Cell(fila, 2).Range.Text = print.NombreAutor;
                            oTable.Cell(fila, 3).Range.Text = print.TipoAutor;
                            oTable.Cell(fila, 4).Range.Text = print.TextoColabora;

                            fila++;
                            consecutivo++;
                        }
                    }

                }
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


        /// <summary>
        /// Genera un documento de Word en el cual se incluyen las obras en las que participa el autor seleccionado
        /// </summary>
        /// <param name="autores">Autores de los cuales se requiere la información</param>
        /// <param name="incluyeSintesis">Establece si el documento contendrá o no la sísntesis de cada una de las obras</param>
        /// <param name="tipoAutor">Indica si se trata de una persona o institución</param>
        public void PrintObrasPorAutor(List<Autor> autores, bool incluyeSintesis, int tipoAutor)
        {
            oWord = new Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = WdOrientation.wdOrientPortrait;

            //Insert a paragraph at the beginning of the document.
            Paragraph oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);

            try
            {
                foreach (Autor autor in autores)
                {

                    oPara1.Range.Text = autor.NombreCompleto;
                    oPara1.Range.Font.Bold = 1;
                    oPara1.Range.Font.Name = "Arial";
                    oPara1.Range.Font.Size = 18;
                    oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    oPara1.Range.InsertParagraphAfter();

                    int senuelo = 0;

                    int numObra = 1;

                    if (autor.Obras == null)
                    {
                        if (tipoAutor == 1)
                            autor.Obras = new ObraModel().GetObras(autor.IdTitular, "IdTitular");

                        if (tipoAutor == 2)
                            autor.Obras = new ObraModel().GetObras(autor.IdTitular, "IdOrg");
                    }

                    foreach (Obra obra in autor.Obras)
                    {
                        if (obra.IdIdioma != senuelo)
                        {
                            oPara1.Range.Text = (from n in ElementalPropertiesSingleton.TipoAutor
                                                 where n.IdElemento == obra.IdIdioma
                                                 select n.Descripcion).ToList()[0];
                            oPara1.Range.Font.Bold = 1;
                            oPara1.Range.Font.Size = 12;
                            oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            oPara1.Range.InsertParagraphAfter();
                            senuelo = obra.IdIdioma;
                        }

                        oPara1.Range.Text = String.Format("{0}  {1}", numObra + ".", obra.Titulo);
                        oPara1.Range.Font.Bold = 0;
                        oPara1.Range.Font.Size = 14;
                        oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                        oPara1.Range.InsertParagraphAfter();

                        if (incluyeSintesis)
                        {
                            Paragraph paraSintesis = oDoc.Content.Paragraphs.Add(ref oMissing);
                            paraSintesis.Range.Text = obra.Sintesis;

                            paraSintesis.Range.Font.Bold = 0;
                            paraSintesis.Range.Font.Name = "Arial";
                            paraSintesis.Range.Font.Size = 12;
                            paraSintesis.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                            //paraSintesis.Range.Paragraphs.LeftIndent = Convert.ToSingle(point);
                            paraSintesis.Range.InsertParagraphAfter();
                            //paraSintesis = null;
                        }
                        numObra++;

                        string textoColabora = new AutorModel().GetTextoColaboracion(obra.IdObra, autor.IdTitular, obra.IdIdioma, tipoAutor);

                        if (!String.IsNullOrEmpty(textoColabora))
                        {
                            oPara1.Range.Text = "Artículo: " + textoColabora;
                            oPara1.Range.Font.Bold = 0;
                            oPara1.Range.Font.Size = 12;
                            oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                            oPara1.Range.InsertParagraphAfter();
                        }

                    }
                    oDoc.Words.Last.InsertBreak(WdBreakType.wdPageBreak);
                }
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





        /// <summary>
        /// Genera un documento de Word en el cual se incluyen las obras puestas a disposición del personal del PJF
        /// </summary>
        /// <param name="obras"></param>
        public void PrintObrasADisposicion(List<Obra> obras)
        {
            oWord = new Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = WdOrientation.wdOrientPortrait;

            //Insert a paragraph at the beginning of the document.
            Paragraph oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);

            try
            {
                int consecutivo = 1;
                int medioPublicacion = 0;

                foreach (Obra obra in obras)
                {

                    if (obra.MedioPublicacion != medioPublicacion)
                    {
                        medioPublicacion = obra.MedioPublicacion;
                        oPara1.Range.Text = (from n in ElementalPropertiesSingleton.MedioPublicacion
                                             where n.IdElemento == medioPublicacion
                                             select n.Descripcion).ToList()[0];
                        oPara1.Range.Font.Name = "Arial";
                        oPara1.Range.Font.Bold = 1;
                        oPara1.Range.Font.Size = 14;
                        oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                        oPara1.Range.InsertParagraphAfter();
                        oPara1.Range.InsertParagraphAfter();
                    }


                    oPara1.Range.Text = String.Format("{0}.  {1}", consecutivo, obra.Titulo);
                    oPara1.Range.Font.Name = "Arial";
                    oPara1.Range.Font.Bold = 0;
                    oPara1.Range.Font.Size = 10;
                    oPara1.Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                    oPara1.Range.InsertParagraphAfter();

                    consecutivo++;

                }
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

    }
}
