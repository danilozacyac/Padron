using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;

namespace PadronApi.Model
{
    public class ObraModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Obtiene el catálogo de obras que se mostrará en padrón
        /// </summary>
        /// <param name="activo">Indica si las obras que se mostrarán estan activas o inactivas</param>
        /// <returns></returns>
        public ObservableCollection<Obra> GetObras(bool activo)
        {
            ObservableCollection<Obra> catalogoObras = new ObservableCollection<Obra>();

            string sqlCadena = "SELECT * " + 
                "FROM C_Obra WHERE Activo = @Activo AND EsCategoria = 0 ORDER BY AnioPublicacion desc,Fecha desc, TituloTxt";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Activo", ( activo ) ? 1 : 0);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    string[] fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {
                        Obra obra = this.GetObraDetail(reader, fieldNames);
                        obra.FechaRecibeInt = Convert.ToInt32(reader["FechaRec"]);
                        obra.FechaRecibe = (obra.FechaRecibeInt != 0) ? DateTimeUtilities.IntToDate(obra.FechaRecibeInt) : null;
                        obra.FechaDistribuyeInt = Convert.ToInt32(reader["FechaDis"]);
                        obra.FechaDistribuye = (obra.FechaDistribuyeInt != 0) ? DateTimeUtilities.IntToDate(obra.FechaDistribuyeInt) : null;

                        catalogoObras.Add(obra);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoObras;
        }

        /// <summary>
        /// Obtienen la obra que se esta solicitando mediante su ID
        /// </summary>
        /// <param name="idObra"></param>
        /// <returns></returns>
        public Obra GetObras(int idObra)
        {
            Obra obra = null;

            string sqlCadena = "SELECT * " +
                "FROM C_Obra WHERE IdObra = @IdObra ORDER BY AnioPublicacion desc,Fecha desc, TituloTxt";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdObra", idObra);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    string[] fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();
                    while (reader.Read())
                    {

                        obra = this.GetObraDetail(reader, fieldNames);

                        obra.FechaRecibeInt = reader["FechaRec"] as int? ?? 0;
                        obra.FechaRecibe = (obra.FechaRecibeInt != 0) ? DateTimeUtilities.IntToDate(obra.FechaRecibeInt) : null;
                        obra.FechaDistribuyeInt = reader["FechaDis"] as int? ?? 0;
                        obra.FechaDistribuye = (obra.FechaDistribuyeInt != 0) ? DateTimeUtilities.IntToDate(obra.FechaDistribuyeInt) : null;

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return obra;
        }

        /// <summary>
        /// Obtiene el catálogo de las publicaciones a visualizarse en el Kiosko
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Obra> GetObras()
        {
            AutorModel model = new AutorModel();

            ObservableCollection<int> obrasConAutor = model.GetObrasConAutor();

            ObservableCollection<Obra> catalogoObras = new ObservableCollection<Obra>();

            string sqlCadena = "SELECT C.*, T.TipoPub FROM C_Obra C  INNER JOIN C_TipoPublicacion T ON C.TipoPublicacion = T.IdTipoPub " +
                               " WHERE C.Activo = 1 AND EsCategoria = 0  ORDER BY C.IdMedio,C.TipoPublicacion, C.TituloTxt ";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    string[] fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {

                        Obra obra = this.GetObraDetail(reader, fieldNames);

                        /**
                         * Esta sección se paso a la acción de selección de la obra cuando se despliega el listado completo de obras en el kiosko,
                         * no se valido que no tenga impacto en otro espacio
                         * */
                        //if (obrasConAutor.Contains(obra.IdObra))
                        //{
                        //    List<Autor> autLista = model.GetAutores(obra).ToList();
                        //    List<Autor> autInsti = model.GetInstituciones(obra).ToList();
                        //    autLista.AddRange(autInsti);

                        //    obra.Autores = new ObservableCollection<Autor>(autLista);
                        //}

                        catalogoObras.Add(obra);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoObras;
        }



        public ObservableCollection<Obra> GetObras(int idSelection,string nombreCampo)
        {
            ObservableCollection<Obra> catalogoObras = new ObservableCollection<Obra>();

            string sqlCadena = String.Format("SELECT C.*, R.IdTipoAutor FROM C_Obra C INNER JOIN RelObrasAutores R ON C.IdObra = R.IdObra " +
                               "WHERE R.{0} = @{0} AND C.Activo = 1 ORDER BY R.IdTipoAutor, TituloTxt",nombreCampo);

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@" + nombreCampo,idSelection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    string[] fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        Obra obra = this.GetObraDetail(reader, fieldNames);
                        obra.IdIdioma = Convert.ToInt32(reader["IdTipoAutor"]);

                        catalogoObras.Add(obra);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoObras;
        }


        private Obra GetObraDetail(SqlDataReader reader, string[] fields)
        {


            Obra obra = new Obra();
            obra.IdObra = Convert.ToInt32(reader["IdObra"]);
            obra.Orden = Convert.ToInt32(reader["Orden"]);
            obra.Titulo = reader["Titulo"].ToString();
            obra.TituloStr = reader["TituloTxt"].ToString();
            obra.Sintesis = reader["Sintesis"].ToString();
            obra.SintesisStr = reader["SintesisTxt"].ToString();
            obra.NumMaterial = reader["NumeroMaterial"].ToString();
            obra.AnioPublicacion = Convert.ToInt32(reader["AnioPublicacion"]);
            obra.Isbn = reader["ISBN"].ToString();
            obra.Paginas = Convert.ToInt32(reader["Paginas"]);
            obra.Presentacion = Convert.ToInt32(reader["IdPresentacion"]);
            obra.TipoObra = Convert.ToInt32(reader["IdTipoObra"]);
            obra.MedioPublicacion = Convert.ToInt32(reader["IdMedio"]);
            obra.Tiraje = Convert.ToInt32(reader["Tiraje"]);
            obra.Precio = reader["Precio"].ToString();
            obra.ImagePath = reader["ArchivoImagen"].ToString();
            obra.MedioPublicacion = Convert.ToInt32(reader["IdMedio"]);
            obra.IdTipoPublicacion = Convert.ToInt32(reader["TipoPublicacion"]);
            if (fields.Contains("TipoPub"))
                obra.TipoPublicacionStr = reader["TipoPub"].ToString();
            obra.IdIdioma = Convert.ToInt32(reader["IdIdioma"]);
            obra.Pais = Convert.ToInt32(reader["IdPais"]);
            obra.MuestraEnKiosko = Convert.ToBoolean(reader["HabilitaCatalogo"]);
            obra.Padre = Convert.ToInt32(reader["Padre"]);
            obra.Activo = Convert.ToInt16(reader["Activo"]);
            obra.Nivel = Convert.ToInt32(reader["Nivel"]);
            //Este campo hace referencia a las obras que se ponen a disposición del personal del PJF por considerarse de lento movimiento
            obra.ADisposicion = Convert.ToBoolean(reader["Disponible"]);
            
            //Cuando hago pruebas en casa debo de comentar estas dos líneas
            obra.Catalografica = reader["Catalografica"].ToString();
            obra.EsCategoria = Convert.ToBoolean(reader["EsCategoria"]);

            return obra;
        }

        #region Busqueda estructura Kiosko

        /// <summary>
        /// Las palabras que no se incluyen en busquedas o que no deben ser pintadas en los
        /// resultados de las mismas.
        /// </summary>
        public String[] Stopers = new String[]{"el","la","las", "le","lo", "los", "no", ".", 
            "pero", "puede","se", "sus", "y", "o", "n","a", "al", "aquel", "aun", "cada", "como", "con", "cual", 
            "de", "debe", "deben", "del", "el", "en", "este", "esta", "la", "las", "le", "lo", "los", 
            "para", "pero", "por", "puede", "que", "se", "sin", "sus", "un", "una"};

        List<Obra> obrasEnLista;


        /// <summary>
        /// Obtiene las obras que cumplen con el críterio de busqueda señalado
        /// </summary>
        /// <param name="textoBuscado">texto que debe presentar el título de la obra</param>
        /// <returns></returns>
        public ObservableCollection<Obra> GetObrasBuscadas(string textoBuscado)
        {
            SqlConnection sqlConne = new SqlConnection(connectionString);

            ObservableCollection<Obra> primerNivel = new ObservableCollection<Obra>();

            obrasEnLista = new List<Obra>();
            ObservableCollection<Obra> arbolReady = new ObservableCollection<Obra>();
            try
            {
                sqlConne.Open();

                string sqlCadena = String.Format("SELECT * FROM C_Obra WHERE ({0}) AND Padre > 0 AND EsCategoria = 0 ORDER BY Orden", this.ArmaCadenaBusqueda(textoBuscado));
                SqlCommand cmd = new SqlCommand(sqlCadena, sqlConne);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    string[] fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        Obra obra = this.GetObraDetail(reader, fieldNames);
                        primerNivel.Add(obra);

                        if (obra.Padre > 1)
                        {
                            Obra parentObra = primerNivel.FirstOrDefault(x => x.IdObra == obra.Padre);

                            if (parentObra == null)
                            {
                                parentObra = this.GetObras(obra.Padre);
                                primerNivel.Add(parentObra);

                                if (parentObra.Padre > 1)
                                {
                                    Obra granpaObra = primerNivel.FirstOrDefault(x => x.IdObra == parentObra.Padre);

                                    if (granpaObra == null)
                                    {
                                        granpaObra = this.GetObras(parentObra.Padre);
                                        primerNivel.Add(granpaObra);

                                        if (granpaObra.Padre > 1)
                                        {
                                            Obra bisAbuelo = primerNivel.FirstOrDefault(x => x.IdObra == granpaObra.Padre);

                                            if (bisAbuelo == null)
                                            {
                                                bisAbuelo = this.GetObras(granpaObra.Padre);
                                                primerNivel.Add(bisAbuelo);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    arbolReady = (from n in primerNivel
                                  where n.Padre == 1
                                  orderby n.Orden
                                  select n).ToObservableCollection();
                    List<int> idCategorias = new List<int>();

                    foreach (Obra cat in arbolReady)
                    {
                        idCategorias.Add(cat.IdObra);
                    }

                    foreach (Obra searchObra in (from n in primerNivel
                                                 where n.Padre > 1
                                                 orderby n.Orden
                                                 select n).ToObservableCollection())
                    {
                        if (idCategorias.Contains(searchObra.Padre)) { }
                        else
                        {
                            Obra subCategoria = primerNivel.FirstOrDefault(x => x.IdObra == searchObra.Padre);

                            if (subCategoria.ObraChild == null)
                                subCategoria.ObraChild = new ObservableCollection<Obra>();

                            subCategoria.ObraChild.Add(searchObra);
                        }


                    }


                    foreach (Obra cat in arbolReady)
                    {
                        cat.ObraChild = (from n in primerNivel
                                         where n.Padre == cat.IdObra
                                         orderby n.Orden
                                         select n).ToObservableCollection();
                    }
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            finally
            {
                sqlConne.Close();
            }
            return arbolReady;
        }


        private String ArmaCadenaBusqueda(String textoBuscado)
        {
            String[] listadoPalabras = textoBuscado.Split(' ');

            String resultString1 = "'%";
            foreach (string palabra in listadoPalabras)
            {
                if (!Stopers.Contains(palabra.Trim().ToLower()))
                    resultString1 += BusquedaUtilities.Normaliza(palabra.Trim()) + "%";
                //resultString += "OR DescripcionStr LIKE '%" + FlowDocumentHighlight.Normaliza( palabra.Trim() ) + "%' ";
            }
            resultString1 += "'";

            String resultString2 = "'%";
            foreach (string palabra in listadoPalabras.Reverse())
            {
                if (!Stopers.Contains(palabra.Trim().ToLower()))
                    resultString2 += BusquedaUtilities.Normaliza(palabra.Trim()) + "%";
                //resultString += "OR DescripcionStr LIKE '%" + FlowDocumentHighlight.Normaliza( palabra.Trim() ) + "%' ";
            }
            resultString2 += "'";

            return String.Format("TituloTxt LIKE {0} OR TituloTxt LIKE {1}", resultString1, resultString2);
        }

        #endregion


        /// <summary>
        /// Obtiene el listado de obras que no han sido asignadas a una categoría o familia dentro del Quiosco 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Obra> GetObrasSinPadre()
        {
            ObservableCollection<Obra> arbolObras = new ObservableCollection<Obra>();

            string sqlCadena = "SELECT * " +
                "FROM C_Obra WHERE Padre = @Padre AND HabilitaCatalogo = 1 AND Orden > 0 ORDER BY AnioPublicacion desc,Fecha desc, TituloTxt";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Padre", 0);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    string[] fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                    while (reader.Read())
                    {
                        Obra obra = this.GetObraDetail(reader, fieldNames);

                        arbolObras.Add(obra);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return arbolObras;
        }


        ObservableCollection<Obra> listaCompleta;
        ObservableCollection<Obra> arbolReady;
        /// <summary>
        /// Devuelve la estructura del árbol de obras que se muestra en el Kiosko
        /// </summary>
        public  ObservableCollection<Obra> GetArbolKiosko()
        {
            listaCompleta = new ObservableCollection<Obra>();
            arbolReady = new ObservableCollection<Obra>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM C_Obra WHERE Padre > 0 ORDER BY Orden", connection);
                reader = cmd.ExecuteReader();

                string[] fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Obra obra = this.GetObraDetail(reader, fieldNames);

                        if (obra.Padre == 1)
                        {
                            obra.ImagePath = String.Format("{0}{1}", "../Resources/", reader["ArchivoImagen"]);
                            listaCompleta.Add(obra);
                            arbolReady.Add(obra);
                        }
                        else
                        {
                            Obra parentObra = listaCompleta.FirstOrDefault(x => x.IdObra == obra.Padre);

                            if (parentObra == null)
                            {
                                Obra obraFalta = new ObraModel().GetObras(obra.Padre);

                                if (obraFalta.Padre == 1)
                                {

                                    listaCompleta.Add(obraFalta);
                                    arbolReady.Add(obraFalta);

                                    obraFalta.ObraChild = new ObservableCollection<Obra>();
                                    obra.ParentItem = obraFalta;
                                    obraFalta.ObraChild.Add(obra);
                                    listaCompleta.Add(obra);
                                }
                            }
                            else
                            {
                                if (parentObra.ObraChild == null)
                                    parentObra.ObraChild = new ObservableCollection<Obra>();

                                obra.ParentItem = parentObra;
                                parentObra.ObraChild.Add(obra);
                                listaCompleta.Add(obra);
                            }
                        }
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return arbolReady;
        }


        public ObservableCollection<Obra> GetArbolObras(Obra obraPadre)
        {
            ObservableCollection<Obra> arbolObras = new ObservableCollection<Obra>();

            string sqlCadena = "SELECT IdObra, Titulo, Orden,Nivel,Padre " +
                "FROM C_Obra WHERE Padre = @Padre ORDER BY Orden";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Padre", obraPadre.IdObra);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Obra obra = new Obra(obraPadre);
                        obra.IdObra = Convert.ToInt32(reader["IdObra"]);
                        obra.Orden = Convert.ToInt32(reader["Orden"]);
                        obra.Padre = Convert.ToInt32(reader["Padre"]);
                        obra.Nivel = Convert.ToInt32(reader["Nivel"]);
                        obra.Titulo = reader["Titulo"].ToString();
                        obra.ObraChild = GetArbolObras(obra);

                        arbolObras.Add(obra);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return arbolObras;
        }


        int initialOrden = 20;
        public bool UpdateOrden(ObservableCollection<Obra> obras)
        {
            foreach (Obra obra in obras)
            {
                this.UpdateOrden(obra.IdObra, initialOrden);
                obra.Orden = initialOrden;
                initialOrden += 5;

                if (obra.ObraChild != null && obra.ObraChild.Count > 0)
                    this.UpdateOrden(obra.ObraChild);
            }

            return true;
        }

        /// <summary>
        /// Reasigna el número de orden a las publicaciones
        /// </summary>
        /// <param name="idObra"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        public bool UpdateOrden(int idObra, int orden)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE C_Obra SET Orden = @Orden WHERE IdObra = @IdObra", connection);
                cmd.Parameters.AddWithValue("@Orden", orden);
                cmd.Parameters.AddWithValue("@IdObra", idObra);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }



        /// <summary>
        /// Inserta una obra dentro del catálogo. Solamente ingresa los campos que interesan para la generación del
        /// padrón de distribución
        /// </summary>
        /// <param name="obra">Obra que se integrará al catálogo</param>
        /// <returns></returns>
        public bool InsertaObra(Obra obra)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            obra.IdObra = DataBaseUtilities.GetNextIdForUse("C_Obra", "IdObra", connection);

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Obra(IdObra,Titulo,TituloTxt,NumeroMaterial,NoVolumenes,IdPresentacion,IdTipoObra,AnioPublicacion,Isbn,Activo,Tiraje,IdUsr,Fecha,FechaRec,FechaDis)" +
                                "VALUES (@IdObra,@Titulo,@TituloTxt,@NumeroMaterial,@NoVolumenes,@IdPresentacion,@IdTipoObra,@AnioPublicacion,@Isbn,@Activo,@Tiraje,@IdUsr,@Fecha,@FechaRec,@FechaDis)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdObra", obra.IdObra);
                cmd.Parameters.AddWithValue("@Titulo", obra.Titulo);
                cmd.Parameters.AddWithValue("@TituloTxt", StringUtilities.PrepareToAlphabeticalOrder( obra.Titulo));
                cmd.Parameters.AddWithValue("@NumeroMaterial", obra.NumMaterial);
                cmd.Parameters.AddWithValue("@NoVolumenes", obra.NumLibros);
                cmd.Parameters.AddWithValue("@IdPresentacion", obra.Presentacion);
                cmd.Parameters.AddWithValue("@IdTipoObra", obra.TipoObra);
                cmd.Parameters.AddWithValue("@AnioPublicacion", obra.AnioPublicacion);
                if (obra.Isbn != null)
                    cmd.Parameters.AddWithValue("@Isbn", obra.Isbn);
                else
                    cmd.Parameters.AddWithValue("@Isbn", String.Empty);
                cmd.Parameters.AddWithValue("@Activo", 1);
                cmd.Parameters.AddWithValue("@Tiraje", obra.Tiraje);
                cmd.Parameters.AddWithValue("@IdUsr", AccesoUsuario.Llave);
                cmd.Parameters.AddWithValue("@Fecha", DateTimeUtilities.DateToInt(DateTime.Now));

                if (obra.FechaRecibe == null)
                    cmd.Parameters.AddWithValue("@FechaRec", 0);
                else
                    cmd.Parameters.AddWithValue("@FechaRec", DateTimeUtilities.DateToInt(obra.FechaRecibe));

                if (obra.FechaDistribuye == null)
                    cmd.Parameters.AddWithValue("@FechaDis", 0);
                else
                    cmd.Parameters.AddWithValue("@FechaDis", DateTimeUtilities.DateToInt(obra.FechaDistribuye));

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Permite Actualizar la información de una obra a nivel padron.
        /// </summary>
        /// <param name="obra">Obra que se va a actualizar</param>
        /// <returns></returns>
        public bool UpdateObra(Obra obra)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "UPDATE C_Obra SET Titulo = @Titulo, TituloTxt = @TituloTxt, NumeroMaterial = @NumeroMaterial," +
                    "NoVolumenes = @NoVolumenes, IdPresentacion = @IdPresentacion,IdTipoObra = @IdTipoObra, AnioPublicacion = @AnioPublicacion, Isbn = @Isbn, Tiraje = @Tiraje, " +
                    "FechaRec = @FechaRec, FechaDis = @FechaDis " +
                    "WHERE IdObra = @IdObra";


                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Titulo", obra.Titulo);
                cmd.Parameters.AddWithValue("@TituloTxt", StringUtilities.PrepareToAlphabeticalOrder(obra.Titulo));
                cmd.Parameters.AddWithValue("@NumeroMaterial", obra.NumMaterial);
                cmd.Parameters.AddWithValue("@NoVolumenes", obra.NumLibros);
                cmd.Parameters.AddWithValue("@IdPresentacion", obra.Presentacion);
                cmd.Parameters.AddWithValue("@IdTipoObra", obra.TipoObra);
                cmd.Parameters.AddWithValue("@AnioPublicacion", obra.AnioPublicacion);
                cmd.Parameters.AddWithValue("@Isbn", obra.Isbn);
                cmd.Parameters.AddWithValue("@Tiraje", obra.Tiraje);
                

                if (obra.FechaRecibe == null)
                    cmd.Parameters.AddWithValue("@FechaRec", 0);
                else
                    cmd.Parameters.AddWithValue("@FechaRec", DateTimeUtilities.DateToInt(obra.FechaRecibe));

                if (obra.FechaDistribuye == null)
                    cmd.Parameters.AddWithValue("@FechaDis", 0);
                else
                    cmd.Parameters.AddWithValue("@FechaDis", DateTimeUtilities.DateToInt(obra.FechaDistribuye));

                cmd.Parameters.AddWithValue("@IdObra", obra.IdObra);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }


        public bool UpdateObraKiosko(Obra obra)
        {
            bool updateCompleted = false;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM C_Obra WHERE IdObra = @IdObra", connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdObra", obra.IdObra);
                dataAdapter.Fill(dataSet, "C_Obra");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Titulo"] = obra.Titulo;
                dr["TituloTxt"] = StringUtilities.PrepareToAlphabeticalOrder(obra.Titulo);
                dr["Sintesis"] = obra.Sintesis;
                dr["SintesisTxt"] = (!String.IsNullOrEmpty(obra.Sintesis)) ? StringUtilities.PrepareToAlphabeticalOrder(obra.Sintesis) : String.Empty;
                dr["Orden"] = obra.Orden;
                dr["NoVolumenes"] = obra.NumLibros;
                dr["NumeroMaterial"] = obra.NumMaterial;
                dr["Isbn"] = obra.Isbn;
                dr["AnioPublicacion"] = obra.AnioPublicacion;
                dr["Tiraje"] = obra.Tiraje;
                dr["Paginas"] = obra.Paginas;
                dr["Edicion"] = obra.Edicion;
                dr["Precio"] = obra.Precio;
                dr["IdPais"] = obra.Pais;
                dr["IdIdioma"] = obra.IdIdioma;
                dr["IdPresentacion"] = obra.Presentacion;
                dr["IdTipoObra"] = obra.TipoObra;
                dr["TipoPublicacion"] = obra.IdTipoPublicacion;
                dr["IdMedio"] = obra.MedioPublicacion;
                dr["ArchivoImagen"] = obra.ImagePath;
                dr["HabilitaCatalogo"] = obra.MuestraEnKiosko;
                dr["Agotado"] = obra.Agotado;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE C_Obra SET Titulo = @Titulo,TituloTxt = @TituloTxt," +
                                                       "Sintesis = @Sintesis,SintesisTxt = @SintesisTxt,Orden = @Orden," +
                                                       "NoVolumenes = @NoVolumenes,NumeroMaterial = @NumeroMaterial,Isbn = @Isbn," +
                                                       "AnioPublicacion = @AnioPublicacion,Tiraje = @Tiraje,Paginas = @Paginas,Edicion = @Edicion, " +
                                                       "Precio = @Precio,IdPais = @IdPais,IdIdioma = @IdIdioma,IdPresentacion = @IdPresentacion, " +
                                                       "IdTipoObra = @IdTipoObra,TipoPublicacion = @TipoPublicacion,IdMedio = @IdMedio,ArchivoImagen = @ArchivoImagen, " +
                                                       "HabilitaCatalogo = @HabilitaCatalogo, Agotado = @Agotado " + 
                                                       " WHERE IdObra = @IdObra";

                dataAdapter.UpdateCommand.Parameters.Add("@Titulo", SqlDbType.VarChar, 0, "Titulo");
                dataAdapter.UpdateCommand.Parameters.Add("@TituloTxt", SqlDbType.VarChar, 0, "TituloTxt");
                dataAdapter.UpdateCommand.Parameters.Add("@Sintesis", SqlDbType.VarChar, 0, "Sintesis");
                dataAdapter.UpdateCommand.Parameters.Add("@SintesisTxt", SqlDbType.VarChar, 0, "SintesisTxt");
                dataAdapter.UpdateCommand.Parameters.Add("@Orden", SqlDbType.Int, 0, "Orden");
                dataAdapter.UpdateCommand.Parameters.Add("@NoVolumenes", SqlDbType.Int, 0, "NoVolumenes");
                dataAdapter.UpdateCommand.Parameters.Add("@NumeroMaterial", SqlDbType.VarChar, 0, "NumeroMaterial");
                dataAdapter.UpdateCommand.Parameters.Add("@Isbn", SqlDbType.VarChar, 0, "Isbn");
                dataAdapter.UpdateCommand.Parameters.Add("@AnioPublicacion", SqlDbType.Int, 0, "AnioPublicacion");
                dataAdapter.UpdateCommand.Parameters.Add("@Tiraje", SqlDbType.Int, 0, "Tiraje");
                dataAdapter.UpdateCommand.Parameters.Add("@Paginas", SqlDbType.Int, 0, "Paginas");
                dataAdapter.UpdateCommand.Parameters.Add("@Edicion", SqlDbType.Int, 0, "Edicion");
                dataAdapter.UpdateCommand.Parameters.Add("@Precio", SqlDbType.VarChar, 0, "Precio");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPais", SqlDbType.Int, 0, "IdPais");
                dataAdapter.UpdateCommand.Parameters.Add("@IdIdioma", SqlDbType.Int, 0, "IdIdioma");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPresentacion", SqlDbType.Int, 0, "IdPresentacion");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTipoObra", SqlDbType.Int, 0, "IdTipoObra");
                dataAdapter.UpdateCommand.Parameters.Add("@TipoPublicacion", SqlDbType.Int, 0, "TipoPublicacion");
                dataAdapter.UpdateCommand.Parameters.Add("@IdMedio", SqlDbType.Int, 0, "IdMedio");
                dataAdapter.UpdateCommand.Parameters.Add("@ArchivoImagen", SqlDbType.VarChar, 0, "ArchivoImagen");
                dataAdapter.UpdateCommand.Parameters.Add("@HabilitaCatalogo", SqlDbType.Int, 0, "HabilitaCatalogo");
                dataAdapter.UpdateCommand.Parameters.Add("@Agotado", SqlDbType.Int, 0, "Agotado");
                dataAdapter.UpdateCommand.Parameters.Add("@IdObra", SqlDbType.Int, 0, "IdObra");

                dataAdapter.Update(dataSet, "C_Obra");
                dataSet.Dispose();
                dataAdapter.Dispose();

                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
            return updateCompleted;
        }

        /// <summary>
        /// Actualiza el nombre de la imagen asociada a una obra
        /// </summary>
        /// <param name="idObra">Identificador de la obra a la cual se le asocia la imagen</param>
        /// <param name="fileName">Nombre y extensión del archivo asociado a la obra</param>
        /// <returns></returns>
        public bool UpdateImagenObra(int idObra, string fileName)
        {
            bool updateCompleted = false;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM C_Obra WHERE IdObra = @IdObra", connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdObra", idObra);
                dataAdapter.Fill(dataSet, "C_Obra");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["ArchivoImagen"] = fileName;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText = "UPDATE C_Obra SET ArchivoImagen = @ArchivoImagen WHERE IdObra = @IdObra";

                dataAdapter.UpdateCommand.Parameters.Add("@ArchivoImagen", SqlDbType.VarChar, 0, "ArchivoImagen");
                dataAdapter.UpdateCommand.Parameters.Add("@IdObra", SqlDbType.Int, 0, "IdObra");

                dataAdapter.Update(dataSet, "C_Obra");
                dataSet.Dispose();
                dataAdapter.Dispose();

                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
            return updateCompleted;
        }


        /// <summary>
        /// Actualiza el nombre del archivo de la ficha catalográfica relacionada con la obra seleccionada
        /// </summary>
        /// <param name="idObra">Identificador de la obra a la cual se le asocia la imagen</param>
        /// <param name="fileName">Nombre y extensión del archivo asociado a la obra</param>
        /// <returns></returns>
        public bool UpdateCatalografica(int idObra, string fileName)
        {
            bool updateCompleted = false;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM C_Obra WHERE IdObra = @IdObra", connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdObra", idObra);
                dataAdapter.Fill(dataSet, "C_Obra");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Catalografica"] = fileName;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText = "UPDATE C_Obra SET Catalografica = @Catalografica WHERE IdObra = @IdObra";

                dataAdapter.UpdateCommand.Parameters.Add("@Catalografica", SqlDbType.VarChar, 0, "Catalografica");
                dataAdapter.UpdateCommand.Parameters.Add("@IdObra", SqlDbType.Int, 0, "IdObra");

                dataAdapter.Update(dataSet, "C_Obra");
                dataSet.Dispose();
                dataAdapter.Dispose();

                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
            return updateCompleted;
        }

        /// <summary>
        /// Actualiza el padre de la obra seleccionada para su posicionamiento dentro de la estructura del quiosco
        /// </summary>
        /// <param name="idObra">Obra que se posicionará en la estructura</param>
        /// <param name="idPadre">Identificador de la obra que contendrá a la obra seleccionada</param>
        /// <returns></returns>
        public bool UpdateObraPadre(Obra obraToUpdate)
        {
            bool updateCompleted = false;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM C_Obra WHERE IdObra = @IdObra", connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdObra", obraToUpdate.IdObra);
                dataAdapter.Fill(dataSet, "C_Obra");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Padre"] = obraToUpdate.Padre;
                dr["Nivel"] = obraToUpdate.Nivel;
                dr["Orden"] = obraToUpdate.Orden;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE C_Obra SET Padre = @Padre, Nivel = @Nivel, Orden = @Orden WHERE IdObra = @IdObra";

                dataAdapter.UpdateCommand.Parameters.Add("@Padre", SqlDbType.Int, 0, "Padre");
                dataAdapter.UpdateCommand.Parameters.Add("@Nivel", SqlDbType.Int, 0, "Nivel");
                dataAdapter.UpdateCommand.Parameters.Add("@Orden", SqlDbType.Int, 0, "Orden");
                dataAdapter.UpdateCommand.Parameters.Add("@IdObra", SqlDbType.Int, 0, "IdObra");

                dataAdapter.Update(dataSet, "C_Obra");
                dataSet.Dispose();
                dataAdapter.Dispose();

                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
            return updateCompleted;
        }


        public bool UpdateFechaDis(int idObra, DateTime? fechaDis)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE C_Obra SET FechaDis = @FechaDis WHERE IdObra = @IdObra", connection);
                cmd.Parameters.AddWithValue("@FechaDis", DateTimeUtilities.DateToInt(fechaDis));
                cmd.Parameters.AddWithValue("@IdObra", idObra);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }

        /// <summary>
        /// Activa o Desactiva un registro del catálogo de la base de datos. Si un registro es desactivado dicho registro no se 
        /// mostrará en mantenimiento ni en producción en ninguno de los programas que accedan a 
        /// este catálogo
        /// </summary>
        /// <param name="obra"></param>
        public void EstadoObra(Obra obra,int estado)
        {
            SqlConnection connection = new SqlConnection(connectionString);


            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE C_Obra SET Activo = @Activo WHERE IdObra = @IdObra", connection);
                cmd.Parameters.AddWithValue("@Activo", estado);
                cmd.Parameters.AddWithValue("@IdObra", obra.IdObra);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObraModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

        }



        /// <summary>
        /// Verifica si el valor que deseamos ingresar ya existe dentro de la base de datos
        /// </summary>
        /// <param name="tabla">Tabla donde se realizará la verificación</param>
        /// <param name="campo">Campo que se desea comprobar</param>
        /// <param name="valor">Valor que se pretende ingresar</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public bool CheckIfExist(string tabla,string campo, string valor)
        {
            string sqlCadena = String.Format("SELECT {0} FROM {1} WHERE {0} = @Valor", campo, tabla);

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            bool existe = false;
            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Valor", valor);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        existe = true;
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObrasModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ObrasModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return existe;
        }


        public ObservableCollection<KeyValuePair<int,int>> GetIdPadronByObra()
        {
            ObservableCollection<KeyValuePair<int, int>> identificaPadrones = new ObservableCollection<KeyValuePair<int, int>>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT IdPadron,IdObra FROM Padron ORDER BY IdPadron", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        identificaPadrones.Add(new KeyValuePair<int,int>(Convert.ToInt32(reader["IdObra"]),Convert.ToInt32(reader["IdPadron"])));
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return identificaPadrones;
        }
    }
}
