using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using PadronApi.Dto;
using ScjnUtilities;
using System.Diagnostics;
using PadronApi.Model;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using System.Text.RegularExpressions;

namespace PadronApi.Reportes
{
    public class ExcelReports
    {
        private readonly ObservableCollection<PlantillaDto> plantillaDistr;
        private readonly ObservableCollection<Obra> obrasImprimir;

        readonly string tituloObra;
        readonly string filePath;

        readonly int parte;

        Application oExcel;
        Workbooks oBooks;
        _Workbook oBook;
        Sheets oSheets;
        _Worksheet oSheet;
        
        public ExcelReports(ObservableCollection<Obra> obrasImprimir, string filePath)
        {
            this.filePath = filePath;
            this.obrasImprimir = obrasImprimir;            
        }

        public ExcelReports(ObservableCollection<PlantillaDto> plantillaDistr, string filePath, string tituloObra, int parte)
        {
            this.plantillaDistr = plantillaDistr;
            this.filePath = filePath;
            this.tituloObra = tituloObra;
            this.parte = parte;
        }

        public void InformeGeneralObras()
        {
            try
            {
                oExcel = new Application();
                oBooks = oExcel.Workbooks;
                oBook = oBooks.Add(1);
                oSheets = (Sheets)oBook.Worksheets;
                oSheet = oSheets.get_Item(1);

                this.oSheet.Cells[1, 1] = "Consecutivo";
                this.oSheet.Cells[1, 2] = "Título";
                this.oSheet.Cells[1, 3] = "Núm. de Material";
                this.oSheet.Cells[1, 4] = "Año";
                this.oSheet.Cells[1, 5] = "Tiraje";

                int fila = 2;
                foreach (Obra obra in obrasImprimir)
                {
                    oSheet.Cells[1][fila] = (fila - 1).ToString();
                    oSheet.Cells[2][fila] = obra.Titulo;
                    oSheet.Cells[3][fila] = obra.NumMaterial;
                    oSheet.Cells[4][fila] = obra.AnioPublicacion;
                    oSheet.Cells[5][fila] = obra.Tiraje;
                    fila++;
                }
                this.oExcel.ActiveWorkbook.SaveAs(filePath);
                this.oExcel.ActiveWorkbook.Saved = true;
                this.oExcel.Quit();

                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExcelReports", "PadronApi");
            }
        }

        public void ListaDistribucion()
        {
            if (parte == 4)
                this.ListadoDistribucionForaneos();
            else
                this.ListadoDistribucionSanLazMet();
        }

        private void ListadoDistribucionForaneos()
        {
            try
            {
                oExcel = new Application();
                oBooks = oExcel.Workbooks;
                oBook = oBooks.Add(1);
                oSheets = (Sheets)oBook.Worksheets;
                oSheet = oSheets.get_Item(1);

                oExcel.StandardFontSize = 7;

                this.oSheet.Range[oSheet.Cells[2, 1], oSheet.Cells[2, 6]].Merge();

                this.oSheet.Cells[2, 1] = tituloObra;

                this.oSheet.Cells[4, 1] = "#";
                this.oSheet.Cells[4, 2] = "Ciudad";
                this.oSheet.Cells[4, 3] = "Adscripción";
                this.oSheet.Cells[4, 4] = "Titular";
                this.oSheet.Cells[4, 5] = "Propiedad";
                this.oSheet.Cells[4, 6] = "Cantidad";

                int fila = 5;
                foreach (PlantillaDto plantilla in plantillaDistr)
                {
                    if (plantilla.Particular > 0 || plantilla.Autor > 0)
                    {
                        this.AgregaFilaReporteForaneos(plantilla, "Particular", plantilla.Particular + plantilla.Autor, fila);
                        fila++;
                    }

                    if (plantilla.Oficina > 0)
                    {
                        this.AgregaFilaReporteForaneos(plantilla, "Oficina", plantilla.Oficina, fila);
                        fila++;
                    }

                    if (plantilla.Personal > 0)
                    {
                        this.AgregaFilaReporteForaneos(plantilla, "Personal", plantilla.Personal, fila);
                        fila++;
                    }

                    if (plantilla.Biblioteca > 0)
                    {
                        this.AgregaFilaReporteForaneos(plantilla, "Biblioteca", plantilla.Biblioteca, fila);
                        fila++;
                    }
                }
                this.oExcel.ActiveWorkbook.SaveAs(filePath);
                this.oExcel.ActiveWorkbook.Saved = true;
                this.oExcel.Quit();

                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExcelReports", "PadronApi");
            }
        }

        private void AgregaFilaReporteForaneos(PlantillaDto plantilla, string propiedad, int cantidad, int fila)
        {
            oSheet.Cells[1][fila] = (fila - 1).ToString();
            oSheet.Cells[2][fila] = plantilla.CiudadStr;
            oSheet.Cells[3][fila] = plantilla.Organismo;
            oSheet.Cells[4][fila] = plantilla.Nombre;
            oSheet.Cells[5][fila] = propiedad;
            oSheet.Cells[6][fila] = cantidad.ToString();
        }

        private void ListadoDistribucionSanLazMet()
        {
            try
            {
                oExcel = new Application();
                oBooks = oExcel.Workbooks;
                oBook = oBooks.Add(1);
                oSheets = (Sheets)oBook.Worksheets;
                oSheet = oSheets.get_Item(1);

                oExcel.StandardFontSize = 7;

                this.oSheet.Range[oSheet.Cells[2, 1], oSheet.Cells[2, 6]].Merge();

                this.oSheet.Cells[2, 1] = tituloObra;

                this.oSheet.Cells[4, 1] = "#";
                this.oSheet.Cells[4, 2] = "Adscripción";
                this.oSheet.Cells[4, 3] = "Nombre";
                this.oSheet.Cells[4, 4] = "Prop.";
                this.oSheet.Cells[4, 5] = "Cant.";
                this.oSheet.Cells[4, 6] = "Fecha";
                this.oSheet.Cells[4, 7] = "Nombre y Firma";

                int fila = 5;
                foreach (PlantillaDto plantilla in plantillaDistr)
                {
                    if (plantilla.Particular > 0 || plantilla.Autor > 0)
                    {
                        this.AgregaFilaReporteSanLazMet(plantilla, "Particular", plantilla.Particular + plantilla.Autor, fila);
                        fila++;
                    }

                    if (plantilla.Oficina > 0)
                    {
                        this.AgregaFilaReporteSanLazMet(plantilla, "Oficina", plantilla.Oficina, fila);
                        fila++;
                    }

                    if (plantilla.Personal > 0)
                    {
                        this.AgregaFilaReporteSanLazMet(plantilla, "Personal", plantilla.Personal, fila);
                        fila++;
                    }

                    if (plantilla.Biblioteca > 0)
                    {
                        this.AgregaFilaReporteSanLazMet(plantilla, "Biblioteca", plantilla.Biblioteca, fila);
                        fila++;
                    }
                }
                this.oExcel.ActiveWorkbook.SaveAs(filePath);
                this.oExcel.ActiveWorkbook.Saved = true;
                this.oExcel.Quit();

                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExcelReports", "PadronApi");
            }
        }

        private void AgregaFilaReporteSanLazMet(PlantillaDto plantilla, string propiedad, int cantidad, int fila)
        {
            oSheet.Cells[1][fila] = (fila - 1).ToString();
            oSheet.Cells[2][fila] = plantilla.Organismo;
            oSheet.Cells[3][fila] = plantilla.Nombre;
            oSheet.Cells[4][fila] = propiedad;
            oSheet.Cells[5][fila] = cantidad.ToString();
        }
        
        public void ObtenNumGuia()
        {
            Application xlApp ;
            Workbook xlWorkBook ;
            Worksheet xlWorkSheet ;
            Range range ;

            List<int> obrasEnviadas = new List<int>();
            ObservableCollection<KeyValuePair<int, int>> padrones = new ObraModel().GetIdPadronByObra();

            try
            {
                xlApp = new Application();
                xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;

                int fila = 2;

                TitularModel titularModel = new TitularModel();
                AcusesModel acusesModel = new AcusesModel();

                while (fila <= range.Rows.Count)
                {

                    Range rowRange = range.Rows[fila];

                    string nombre = (string)(range.Cells[fila, 3] as Range).Value;

                    if (!nombre.ToUpper().StartsWith("SECRETARIO EN FUNCIONES") && !nombre.ToUpper().StartsWith("SECRETARIA EN FUNCIONES"))
                    {

                        nombre = nombre.Replace("JUEZ", "");
                        nombre = nombre.Replace("JUEZA", "");
                        nombre = nombre.Replace("LIC.", "");
                        nombre = nombre.Replace("LICDA.", "");
                        nombre = nombre.Replace("DR.", "");
                        nombre = nombre.Replace("DRA.", "");
                        nombre = nombre.Replace("MINISTRO.", "");
                        nombre = nombre.Replace("MINISTRA.", "");
                        nombre = nombre.Replace("MINISTRO", "");
                        nombre = nombre.Replace("MINISTRA", "");

                        nombre = StringUtilities.PrepareToAlphabeticalOrder(nombre);

                        Titular titular = titularModel.GetTitulares(nombre);

                        if (titular == null)
                        {
                            rowRange.Interior.Color = System.Drawing.Color.Red;
                        }
                        else
                        {
                            try 
                            {

                                Int64 numGuia = (Int64)(range.Cells[fila, 6] as Range).Value;
                                rowRange.Interior.Color = System.Drawing.Color.White;

                                DateTime? fecha = (range.Cells[fila, 13] as Range).Value;

                                string fechaEnvio = DateTimeUtilities.DateToInt(fecha);

                                string cadenaControl = (range.Cells[fila, 14] as Range).Value;

                                string[] obras = cadenaControl.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string obra in obras)
                                {
                                    int idObra = Convert.ToInt32(obra);

                                    if (!obrasEnviadas.Contains(idObra))
                                        obrasEnviadas.Add(idObra);

                                    List<int> values = (from n in padrones
                                                        where n.Key == idObra
                                                        select n.Value).ToList();

                                    acusesModel.UpdateDetallesEnvioPaqueteria(values[0], titular.IdTitular, numGuia.ToString(), fechaEnvio);
                                }
                            }
                            catch(RuntimeBinderException)
                            {
                                rowRange.Interior.Color = System.Drawing.Color.Green;
                            }
                        }
                    }
                    fila++;
                }

                foreach (int idObra in obrasEnviadas)
                {
                    List<int> values = (from n in padrones
                                        where n.Key == idObra
                                        select n.Value).ToList();

                    acusesModel.SetGuiaColegas(values[0]);
                }

                xlWorkBook.Close(true, filePath, null);
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);
            }
            catch (COMException)
            {
            }
        }

        public void ObtenFechaEntregaPaqueteria()
        {
            Application xlApp;
            Workbook xlWorkBook;
            Worksheet xlWorkSheet;
            Range range = null;

            int fila = 2;
            int filasOk = 0; //N{umero de filas que se actualizaron correctamente
            //string tempLastGuia = String.Empty;
            try
            {
                xlApp = new Application();
                xlWorkBook = xlApp.Workbooks.Open(filePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

                //Estas líneas permiten que al obtener el rango usado me devuelva exclusivamente el rango que 
                //contiene datos y no la hoja completa de Excel
                xlWorkSheet.Rows.ClearFormats();
                xlWorkSheet.Columns.ClearFormats();

                range = xlWorkSheet.UsedRange;



                AcusesModel acusesModel = new AcusesModel();

                int filasContinuasSinDatos = 0;

                while (fila <= range.Rows.Count)
                {
                    Range rowRange = range.Rows[fila];

                    try
                    {
                        Int64 numGuia = (Int64)(range.Cells[fila, 1] as Range).Value;

                        string datosEntrega = ((Range)range.Cells[fila, 21]).Value2.ToString();

                        if (datosEntrega.StartsWith("Delivered"))
                        {
                            string tmp = this.GetDateFromString(datosEntrega);// datosEntrega.Substring(0, indexOfFirstSpace);

                            if (tmp != null)
                            {

                                string[] splitDate = tmp.Split('/');

                                tmp = splitDate[2] + splitDate[0] + splitDate[1];

                                int indexOfSigned = datosEntrega.ToUpper().IndexOf("BY:");

                                string tmpRecibe = datosEntrega.Substring(indexOfSigned);

                                tmpRecibe = tmpRecibe.ToUpper().Replace("BY: ", "");

                                int indexOfState = tmpRecibe.IndexOf(';');

                                if (indexOfState != -1)
                                    tmpRecibe = tmpRecibe.Substring(0, indexOfState);

                                if (acusesModel.UpdateFechaRecepcionPaqueteria(numGuia.ToString(), tmp, tmpRecibe) > 0)
                                    filasOk++;
                            }
                        }
                        filasContinuasSinDatos = 0;
                    }
                    catch (RuntimeBinderException ex)
                    {
                    }
                    catch (Exception)
                    {
                        rowRange.Interior.Color = System.Drawing.Color.Red;
                        filasContinuasSinDatos++;
                    }
                    fila++;

                    if (filasContinuasSinDatos > 3)
                        break;
                }

                xlWorkBook.Close(true, filePath, null);
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);
            }
            catch (COMException ex)
            {

                System.Windows.MessageBox.Show(ex.Message + "   " + fila);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message + "   " + fila);
            }
            System.Windows.MessageBox.Show(String.Format( "Se actualizaron {0} de los {1} registros que contiene el documento",filasOk,range.Rows.Count));
        }




        public string GetDateFromString(string textString)
        {
            Regex reg = new Regex(@"\d{2}.\d{2}.\d{4}");

            Match match = reg.Match(textString);

            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                return null;
            }

        }

        private void ReleaseObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                System.Windows.Forms.MessageBox.Show("Unable to release the Object " + ex);
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}