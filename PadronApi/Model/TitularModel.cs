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
    public class TitularModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Obtiene la lista complete de funcionarios sin importar si al momento estan o no adscritos a algún organismo
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Titular> GetTitulares(bool activos)
        {
            ObservableCollection<Titular> catalogoTitulares = new ObservableCollection<Titular>();

            string condition = String.Empty;

            if (activos)
                condition = "IdEstatus <> 5";
            else
                condition = "IdEstatus = 5";

            string sqlQuery = String.Format("SELECT * FROM C_Titular WHERE {0} AND Autor <> 2 ORDER BY Apellidos", condition);

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                
                cmd = new SqlCommand(sqlQuery, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Titular titular = new Titular() { 
                            IdTitular = Convert.ToInt32(reader["IdTitular"]), 
                            Nombre = reader["Nombre"].ToString(), 
                            Apellidos = reader["Apellidos"].ToString(), 
                            NombreStr = reader["NombMay"].ToString(), 
                            IdTitulo = Convert.ToInt32(reader["IdTitulo"]), 
                            Observaciones = reader["Obs"].ToString(), 
                            Estado = Convert.ToInt32(reader["IdEstatus"]), 
                            QuiereDistribucion = Convert.ToInt16(reader["QuiereDist"]), 
                            Correo = reader["Correo"].ToString(), 
                            Genero = Convert.ToInt16(reader["Genero"]),
                            HaPublicado = (Convert.ToInt16(reader["Autor"]) == 3) ? true : false
                        };

                        if (titular.QuiereDistribucion == -1)
                            titular.TotalAdscripciones = -1;

                        catalogoTitulares.Add(titular);
                    }
                }
                cmd.Dispose();
                reader.Close();

                foreach (KeyValuePair<int, int> pair in this.GetAdscripcionesTitular())
                {
                    try
                    {


                        Titular tit = (from n in catalogoTitulares
                                       where n.IdTitular == pair.Key
                                       select n).ToList()[0];
                        if(tit.TotalAdscripciones != -1)
                            tit.TotalAdscripciones = pair.Value;
                    }
                    catch (Exception) { }
                }

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }

        /// <summary>
        /// Obtiene los integrantes del organismo seleccionado
        /// </summary>
        /// <param name="organismo">Organismo seleccionado</param>
        /// <returns></returns>
        public ObservableCollection<Adscripcion> GetTitulares(Organismo organismo)
        {
            ObservableCollection<Adscripcion> titularesAdscritos = new ObservableCollection<Adscripcion>();

            string sqlQuery = "SELECT T.IdTitular, T.Nombre, T.Apellidos, T.IdTitulo, T.NombMay, T.Obs, T.QuiereDist, T.IdEstatus, T.Correo, A.idFuncion, A.TipoObra " +
                               "FROM AcuerdoPadron AS A INNER JOIN C_Titular AS T ON A.IdTitular = T.IdTitular WHERE A.IdOrg = @IdOrg " +
                               "GROUP BY T.IdTitular, T.Nombre, T.Apellidos, T.IdTitulo, T.NombMay, T.Obs, T.QuiereDist, T.IdEstatus, T.Correo, A.idFuncion, A.TipoObra  ORDER BY A.IdFuncion desc, T.Apellidos asc  ";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdOrg", organismo.IdOrganismo);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Titular titular = new Titular() { 
                            IdTitular = Convert.ToInt32(reader["IdTitular"]), 
                            Nombre = reader["Nombre"].ToString(), 
                            Apellidos = reader["Apellidos"].ToString(), 
                            NombreStr = reader["NombMay"].ToString(), 
                            IdTitulo = reader["IdTitulo"] as int? ?? 0, 
                            Observaciones = reader["Obs"].ToString(), 
                            Estado = reader["IdEstatus"] as int? ?? 0, 
                            QuiereDistribucion = Convert.ToInt16(reader["QuiereDist"]), 
                            Correo = reader["Correo"].ToString() 
                        };

                        Adscripcion adscripcion = new Adscripcion() { 
                            Organismo = organismo, 
                            Titular = titular, 
                            ObrasRecibe = reader["TipoObra"].ToString(), 
                            Funcion = Convert.ToInt32(reader["IdFuncion"]) 
                        };

                        titularesAdscritos.Add(adscripcion);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return titularesAdscritos;
        }

        /// <summary>
        /// Obtiene la información completa de un idTitular a partir de su identificador
        /// </summary>
        /// <param name="idTitular">Identificador del titular</param>
        /// <returns></returns>
        public Titular GetTitulares(int idTitular)
        {
            ObservableCollection<Titular> catalogoTitulares = new ObservableCollection<Titular>();

            const string SqlQuery = "SELECT T.* FROM C_Titular T WHERE T.IdTitular = @IdTitular ORDER BY Apellidos";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            Titular dummyTitular = null;
            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdOrg", idTitular);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dummyTitular = new Titular();
                        dummyTitular.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                        dummyTitular.Nombre = reader["Nombre"].ToString();
                        dummyTitular.Apellidos = reader["Apellidos"].ToString();
                        dummyTitular.NombreStr = reader["NombMay"].ToString();
                        dummyTitular.IdTitulo = reader["IdTitulo"] as int? ?? 0;
                        dummyTitular.Observaciones = reader["Obs"].ToString();
                        dummyTitular.Estado = reader["IdEstatus"] as int? ?? 0;
                        dummyTitular.QuiereDistribucion = Convert.ToInt16(reader["QuiereDist"]);
                        dummyTitular.Correo = reader["Correo"].ToString();

                        catalogoTitulares.Add(dummyTitular);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return dummyTitular;
        }


        public Titular GetTitulares(string nombreDesc)
        {
            ObservableCollection<Titular> catalogoTitulares = new ObservableCollection<Titular>();

            const string SqlQuery = "SELECT T.* FROM C_Titular T WHERE T.NombMay = @NombMay ORDER BY Apellidos";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            Titular titular = null;
            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@NombMay", nombreDesc);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        titular = new Titular();
                        titular.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                        titular.Nombre = reader["Nombre"].ToString();
                        titular.Apellidos = reader["Apellidos"].ToString();
                        titular.NombreStr = reader["NombMay"].ToString();
                        titular.IdTitulo = reader["IdTitulo"] as int? ?? 0;
                        titular.Observaciones = reader["Obs"].ToString();
                        titular.Estado = reader["IdEstatus"] as int? ?? 0;
                        titular.QuiereDistribucion = Convert.ToInt16(reader["QuiereDist"]);
                        titular.Correo = reader["Correo"].ToString();

                        catalogoTitulares.Add(titular);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return titular;
        }


       
        /// <summary>
        /// Verifica si el titular que se esta intentando agregar existe o no en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del titular normalizado</param>
        /// <returns></returns>
        public bool DoTitularExist(string nombre)
        {
            const string SqlQuery = "SELECT * FROM C_Titular WHERE NombMay = @Nombre ORDER BY Apellidos";

            int cuantos = 0;
            bool existe = false;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cuantos++;
                    }
                }
                cmd.Dispose();
                reader.Close();

                if (cuantos > 0)
                    existe = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return existe;
        }

        public bool InsertaTitular(Titular titular)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            titular.IdTitular = DataBaseUtilities.GetNextIdForUse("C_Titular", "IdTitular", connection);
            titular.QuiereDistribucion = 1;
            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Titular(IdTitular, Nombre,Apellidos,IdTitulo,NombMay,IdUsr,Fecha,Obs,QuiereDist,IdEstatus,Correo,Genero)" +
                                  "VALUES (@IdTitular, @Nombre,@Apellidos,@IdTitulo,@NombMay,@IdUsr,@Fecha,@Obs,@QuiereDist,@IdEstatus,@Correo,@Genero)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);
                cmd.Parameters.AddWithValue("@Nombre", titular.Nombre);
                cmd.Parameters.AddWithValue("@Apellidos", titular.Apellidos);
                cmd.Parameters.AddWithValue("@IdTitulo", titular.IdTitulo);
                cmd.Parameters.AddWithValue("@NombMay", titular.NombreStr);
                cmd.Parameters.AddWithValue("@IdUsr", AccesoUsuario.Llave);
                cmd.Parameters.AddWithValue("@Fecha", DateTimeUtilities.DateToInt(DateTime.Now));

                if (titular.Observaciones == null)
                {
                    cmd.Parameters.AddWithValue("@Obs", String.Empty);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Obs", titular.Observaciones);
                }
                
                cmd.Parameters.AddWithValue("@QuiereDist", titular.QuiereDistribucion);
                cmd.Parameters.AddWithValue("@IdEstatus", titular.Estado);
                cmd.Parameters.AddWithValue("@Correo", titular.Correo);
                cmd.Parameters.AddWithValue("@Genero", titular.Genero);

                cmd.ExecuteNonQuery();
                cmd.Dispose();


                foreach (Adscripcion adscripcion in titular.Adscripciones)
                {
                    adscripcion.Titular = titular;

                    foreach (TirajePersonal tiraje in adscripcion.Tirajes)
                    {
                        if (tiraje.IsChecked == true)
                            this.EstableceAdscripcion(adscripcion, tiraje,true);
                    }

                    this.InsertaHistoricoFechaAlta(adscripcion);
                }

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Actualiza los datos del titular seleccionado
        /// </summary>
        /// <param name="titular"></param>
        /// <returns></returns>
        public bool UpdateTitular(Titular titular)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "UPDATE C_Titular SET Nombre = @Nombre,Apellidos = @Apellidos," +
                                  "NombMay = @NombMay,Obs = @Obs, IdTitulo = @IdTitulo, IdEstatus = @IdEstatus, Correo = @Correo, " +
                                  " IdUsr = @IdUsr, Fecha = @Fecha, Genero = @Genero  WHERE IdTitular = @IdTitular";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Nombre", titular.Nombre);
                cmd.Parameters.AddWithValue("@Apellidos", titular.Apellidos);
                cmd.Parameters.AddWithValue("@NombMay", titular.NombreStr);
                cmd.Parameters.AddWithValue("@Obs", titular.Observaciones);
                cmd.Parameters.AddWithValue("@IdTitulo", titular.IdTitulo);
                cmd.Parameters.AddWithValue("@IdEstatus", titular.Estado);
                cmd.Parameters.AddWithValue("@Correo", titular.Correo);
                cmd.Parameters.AddWithValue("@IdUsr", AccesoUsuario.Llave);
                cmd.Parameters.AddWithValue("@Fecha", DateTimeUtilities.DateToInt(DateTime.Now));
                cmd.Parameters.AddWithValue("@Genero", titular.Genero);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;
                
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }


        /// <summary>
        /// Modifica las preferencias de un titular en cuanto a si quiere o no recibir distribución de las obras
        /// </summary>
        /// <param name="titular"></param>
        /// <param name="quiereDistribucion"></param>
        /// <returns></returns>
        public bool UpdateTitular(Titular titular, bool quiereDistribucion)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            int quiere = (quiereDistribucion) ? 1 : -1;

            try
            {
                connection.Open();

                const string SqlQuery = "UPDATE C_Titular SET QuiereDist = @QuiereDist WHERE IdTitular = @IdTitular";

                SqlCommand cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@QuiereDist", quiere);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;

                this.EliminaDistribucion(titular);

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }


        /// <summary>
        /// Devuelve la lista de organismos a los que esta adscrito un titular
        /// </summary>
        /// <param name="titular">Titular del que se obtienen las adscripciones</param>
        /// <returns></returns>
        public ObservableCollection<Adscripcion> GetAdscripcionesTitular(Titular titular)
        {
            ObservableCollection<Adscripcion> adscripcionesTitular = new ObservableCollection<Adscripcion>();

            const string SqlQuery = "SELECT IdOrg,IdFuncion,TipoObra FROM AcuerdoPadron WHERE IdTitular = @IdTitular GROUP BY IdOrg,Idfuncion,TipoObra";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Adscripcion adscripcion = new Adscripcion() { 
                            Titular = titular, 
                            Funcion = Convert.ToInt32(reader["IdFuncion"]),
                            ObrasRecibe = reader["TipoObra"].ToString(), 
                            Organismo = new OrganismoModel().GetOrganismos(Convert.ToInt32(reader["IdOrg"])) 
                        };

                        adscripcionesTitular.Add(adscripcion);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return adscripcionesTitular;
        }

        /// <summary>
        /// Obtienen el número de adscripciones que tiene un titular
        /// </summary>
        /// <param name="idTitular"></param>
        /// <returns></returns>
        private ObservableCollection<KeyValuePair<int,int>> GetAdscripcionesTitular()
        {
            ObservableCollection<KeyValuePair<int, int>> asignados = new ObservableCollection<KeyValuePair<int, int>>();

            const string SqlQuery = "SELECT IdTitular,COUNT(IdTitular) AS Ads FROM vTitularOrg GROUP BY IdTitular HAVING COUNT(IdTitular)  > 1";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        asignados.Add(new KeyValuePair<int, int>(Convert.ToInt32(reader["IdTitular"]), Convert.ToInt32(reader["Ads"])));
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return asignados;
        }


        /// <summary>
        /// Agrega a un titular a las plantillas de tiraje en los que recibirá ejemplares   
        /// </summary>
        /// <param name="adscripcion"></param>
        /// <param name="tiraje"></param>
        private void EstableceAdscripcion(Adscripcion adscripcion, TirajePersonal tiraje, bool insertaHistorial)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO AcuerdoPadron(IdAcuNum, IdOrg,Idtitular,Particular,Oficina,Biblioteca,Resguardo,Personal,Autor,IdFuncion,TipoObra)" +
                                  "VALUES (@IdAcuNum,@IdOrg,@Idtitular,@Particular,@Oficina,@Biblioteca,@Resguardo,@Personal,@Autor,@IdFuncion,@TipoObra)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdAcuNum", tiraje.IdAcuerdo);
                cmd.Parameters.AddWithValue("@IdOrg", adscripcion.Organismo.IdOrganismo);
                cmd.Parameters.AddWithValue("@Idtitular", adscripcion.Titular.IdTitular);
                cmd.Parameters.AddWithValue("@Particular", tiraje.Particular);
                cmd.Parameters.AddWithValue("@Oficina", tiraje.Oficina);
                cmd.Parameters.AddWithValue("@Biblioteca", tiraje.Biblioteca);
                cmd.Parameters.AddWithValue("@Resguardo", tiraje.Resguardo);
                cmd.Parameters.AddWithValue("@Personal", tiraje.Personal);
                cmd.Parameters.AddWithValue("@Autor", tiraje.Autor);
                cmd.Parameters.AddWithValue("@IdFuncion", adscripcion.Funcion);
                cmd.Parameters.AddWithValue("@TipoObra", adscripcion.ObrasRecibe);

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if (insertaHistorial)
                    this.InsertaHistoricoFechaAlta(adscripcion);

                this.UpdateTotalAlmacenZaragoza(tiraje);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
        }

        public void EstableceAdscripcion(Adscripcion adscripcion, bool insertaHistorial)
        {
            foreach (TirajePersonal tiraje in adscripcion.Tirajes)
                if (tiraje.IsChecked)
                    this.EstableceAdscripcion(adscripcion, tiraje, insertaHistorial);
        }


        public bool EliminaAdscripcion(Adscripcion adscripcion,bool insertaHistorial)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                const string SqlQuery = "DELETE FROM AcuerdoPadron WHERE IdTitular = @IdTitular AND IdOrg = @IdOrg";

                SqlCommand cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", adscripcion.Titular.IdTitular);
                cmd.Parameters.AddWithValue("@IdOrg", adscripcion.Organismo.IdOrganismo);

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                if(insertaHistorial)
                    this.UpdateHistoricoFechaBaja(adscripcion);


                foreach (TirajePersonal tiraje in new AcuerdosModel().GetAcuerdos())
                    this.UpdateTotalAlmacenZaragoza(tiraje);


                updateCompleted = true;
                
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return updateCompleted;
        }

        /// <summary>
        /// PAra los titulares que no desean recibir ninguna obra pero se debe de mantener
        /// su adscripción al organismo donde se desempeñan
        /// </summary>
        /// <param name="titular">Titular que ya no desea recibir obras</param>
        /// <returns></returns>
        private bool EliminaDistribucion(Titular titular)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;


            try
            {
                connection.Open();

                const string SqlQuery = "UPDATE AcuerdoPadron SET Particular = 0, Oficina = 0, Biblioteca = 0, Autor = 0 WHERE IdTitular = @IdTitular";

                SqlCommand cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }

        

        /// <summary>
        /// Asigana el cargo de presidente de un tribunal al titular seleccionado
        /// </summary>
        /// <param name="idTitular">Identificador del titular que será nombrado presidente</param>
        /// <param name="idOrg">Identificador del Organismo</param>
        /// <returns></returns>
        public bool UpdatePresidente(int idTitular, int idOrg)
        {
            ObservableCollection<KeyValuePair<int, int>> totalSecretarios = this.GetIntegrantesPorAcuerdo(idOrg);

            this.UpdatePresidente(idOrg);

            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                const string SqlQuery = "SELECT * FROM AcuerdoPadron WHERE IdTitular = @IdTitular AND IdOrg = @IdOrg";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdTitular", idTitular);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdOrg", idOrg);
                dataAdapter.Fill(dataSet, "AcuerdoPadron");

                dr = dataSet.Tables["AcuerdoPadron"].Rows[0];
                dr.BeginEdit();
                dr["IdFuncion"] = 1;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE AcuerdoPadron SET IdFuncion = @IdFuncion WHERE IdTitular = @IdTitular AND IdOrg = @IdOrg";
                dataAdapter.UpdateCommand.Parameters.Add("@IdFuncion", SqlDbType.Int, 0, "IdFuncion");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTitular", SqlDbType.Int, 0, "IdTitular");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");

                dataAdapter.Update(dataSet, "AcuerdoPadron");

                dataSet.Dispose();
                dataAdapter.Dispose();

                insertCompleted = this.UpdateEjemplaresPersonal(idOrg);
                insertCompleted = this.UpdateEjemplaresPersonal(totalSecretarios,idTitular,idOrg);

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Elimina la función de presidente asignada previamente al titular de un organismo, 
        /// para que se pueda asignar al nuevo presidente
        /// </summary>
        /// <param name="idOrg"></param>
        /// <returns></returns>
        private bool UpdatePresidente(int idOrg)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                const string SqlQuery = "SELECT * FROM AcuerdoPadron WHERE IdOrg = @IdOrg";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdOrg", idOrg);
                dataAdapter.Fill(dataSet, "AcuerdoPadron");

                dr = dataSet.Tables["AcuerdoPadron"].Rows[0];
                dr.BeginEdit();
                dr["IdFuncion"] = 0;
                dr["Personal"] = 0;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE AcuerdoPadron SET IdFuncion = @IdFuncion, Personal = @Personal WHERE IdOrg = @IdOrg AND IdFuncion = 1";
                dataAdapter.UpdateCommand.Parameters.Add("@IdFuncion", SqlDbType.Int, 0, "IdFuncion");
                dataAdapter.UpdateCommand.Parameters.Add("@Personal", SqlDbType.Int, 0, "Personal");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");

                dataAdapter.Update(dataSet, "AcuerdoPadron");

                dataSet.Dispose();
                dataAdapter.Dispose();

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (DBConcurrencyException) { }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Obtiene el número de ejemplares que se asignan a un Magistrado presidente para los Secretarios del Tribunal
        /// </summary>
        /// <param name="idOrganismo">Identificador del organismo</param>
        /// <returns></returns>
        private ObservableCollection<KeyValuePair<int, int>> GetIntegrantesPorAcuerdo(int idOrganismo)
        {
            ObservableCollection<KeyValuePair<int, int>> totalsecretarios = new ObservableCollection<KeyValuePair<int, int>>();

            string sqlQuery = "SELECT A.IDAcuNum, A.Personal, O.IdTpoOrg, A.IdFuncion, A.IdTitular, A.IdOrg FROM AcuerdoPadron A " + 
                               "INNER JOIN C_Organismo O ON A.IdOrg = O.IdOrg " +
                               "WHERE O.IdTpoOrg = 2 and A.IdFuncion = 1 AND A.IdOrg = @IdOrg ORDER BY IdAcuNum";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdOrg", idOrganismo);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        totalsecretarios.Add(new KeyValuePair<int, int>(Convert.ToInt32(reader["IdAcuNum"]), Convert.ToInt32(reader["Personal"])));
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return totalsecretarios;
        }

        /// <summary>
        /// Actualiza el número de ejemplares que se enviarán para los secretarios de tribunal o de juzgado para los secretarios de los mismos
        /// </summary>
        /// <param name="idTitular"></param>
        /// <param name="idOrg"></param>
        /// <returns></returns>
        private bool UpdateEjemplaresPersonal(ObservableCollection<KeyValuePair<int, int>> totalSecretarios, int idTitular, int idOrg)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                foreach (KeyValuePair<int, int> acuerdo in totalSecretarios)
                {

                    DataSet dataSet = new DataSet();
                    DataRow dr;

                    const string SqlQuery = "SELECT * FROM AcuerdoPadron WHERE IdTitular = @IdTitular AND IdOrg = @IdOrg AND IdAcuNum = @IdAcuNum";

                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@IdTitular", idTitular);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@IdOrg", idOrg);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@IdAcuNum", acuerdo.Key);
                    dataAdapter.Fill(dataSet, "AcuerdoPadron");

                    dr = dataSet.Tables["AcuerdoPadron"].Rows[0];
                    dr.BeginEdit();
                    dr["Personal"] = acuerdo.Value;
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = "UPDATE AcuerdoPadron SET Personal = @Personal WHERE IdTitular = @IdTitular AND IdOrg = @IdOrg AND IdAcuNum = @IdAcuNum";
                    dataAdapter.UpdateCommand.Parameters.Add("@Personal", SqlDbType.Int, 0, "Personal");
                    dataAdapter.UpdateCommand.Parameters.Add("@IdTitular", SqlDbType.Int, 0, "IdTitular");
                    dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");
                    dataAdapter.UpdateCommand.Parameters.Add("@IdAcuNum", SqlDbType.Int, 0, "IdAcuNum");

                    dataAdapter.Update(dataSet, "AcuerdoPadron");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                }
                insertCompleted = true;

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Actualiza el número de ejemplares que se distribuyen para el personal, reiniciando a todos los integrantes del mismo en 0
        /// </summary>
        /// <param name="idOrg"></param>
        /// <returns></returns>
        private bool UpdateEjemplaresPersonal(int idOrg)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                    DataSet dataSet = new DataSet();
                    DataRow dr;

                    const string SqlQuery = "SELECT * FROM AcuerdoPadron WHERE IdOrg = @IdOrg";

                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@IdOrg", idOrg);
                    dataAdapter.Fill(dataSet, "AcuerdoPadron");

                    dr = dataSet.Tables["AcuerdoPadron"].Rows[0];
                    dr.BeginEdit();
                    dr["Personal"] = 0;
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = "UPDATE AcuerdoPadron SET Personal = @Personal WHERE IdOrg = @IdOrg";
                    dataAdapter.UpdateCommand.Parameters.Add("@Personal", SqlDbType.Int, 0, "Personal");
                    dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");

                    dataAdapter.Update(dataSet, "AcuerdoPadron");

                    dataSet.Dispose();
                    dataAdapter.Dispose();
                
                insertCompleted = true;

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        #region Historico

        /// <summary>
        /// Obtiene el historial de los tribunales a los que ha estado adscrito el titular
        /// </summary>
        /// <param name="titular"></param>
        /// <returns></returns>
        public ObservableCollection<Adscripcion> GetTrayectoria(Titular titular)
        {
            ObservableCollection<Adscripcion> listaAdscripciones = new ObservableCollection<Adscripcion>();

            const string SqlQuery = "SELECT T.*, O.DescOrg FROM TitularHistorico T INNER JOIN C_Organismo O ON T.IdOrg = O.IdOrg WHERE T.IdTitular = @IdTitular ORDER BY FechaAlta Desc";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Adscripcion adscripcion = new Adscripcion();

                        Organismo organismo = new Organismo() { OrganismoDesc = reader["DescOrg"].ToString() };
                        adscripcion.Organismo = organismo;
                        adscripcion.FechaAlta = DateTimeUtilities.IntToDate(reader, "FechaAlta");
                        adscripcion.FechaBaja = DateTimeUtilities.IntToDate(reader, "FechaBaja");

                        listaAdscripciones.Add(adscripcion);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return listaAdscripciones;
        }

        /// <summary>
        /// Ingresa la fecha en la que un titular es dado de alta en un organismo
        /// </summary>
        /// <param name="adscripcion"></param>
        /// <returns></returns>
        public bool InsertaHistoricoFechaAlta(Adscripcion adscripcion)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO TitularHistorico(IdTitular, IdOrg,IdFuncion,IdUsr,FechaAlta,IdEstatus)" +
                                  "VALUES (@IdTitular,@IdOrg,@IdFuncion,@IdUsr,@FechaAlta,@IdEstatus)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", adscripcion.Titular.IdTitular);
                cmd.Parameters.AddWithValue("@IdOrg", adscripcion.Organismo.IdOrganismo);
                cmd.Parameters.AddWithValue("@IdFuncion", adscripcion.Funcion);
                cmd.Parameters.AddWithValue("@IdUsr", AccesoUsuario.Llave);
                cmd.Parameters.AddWithValue("@FechaAlta", DateTimeUtilities.DateToInt(DateTime.Now));
                cmd.Parameters.AddWithValue("@IdEstatus", adscripcion.Titular.Estado);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Ingresa la fecha en que un titular es dado de baja de un organismo
        /// </summary>
        /// <param name="adscripcion"></param>
        /// <returns></returns>
        public bool UpdateHistoricoFechaBaja(Adscripcion adscripcion)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                const string SqlQuery = "SELECT * FROM TitularHistorico WHERE IdOrg = @IdOrg AND IdTitular = @IdTitular AND FechaBaja = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdOrg", adscripcion.Organismo.IdOrganismo);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdTitular", adscripcion.Titular.IdTitular);
                dataAdapter.Fill(dataSet, "TitularHistorico");

                dr = dataSet.Tables["TitularHistorico"].Rows[0];
                dr.BeginEdit();
                dr["FechaBaja"] = DateTimeUtilities.DateToInt(DateTime.Now);
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE TitularHistorico SET FechaBaja = @FechaBaja WHERE IdOrg = @IdOrg AND IdTitular = @IdTitular AND FechaBaja = 0";

                dataAdapter.UpdateCommand.Parameters.Add("@FechaBaja", SqlDbType.Int, 0, "FechaBaja");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTitular", SqlDbType.Int, 0, "IdTitular");
                dataAdapter.Update(dataSet, "TitularHistorico");

                dataSet.Dispose();
                dataAdapter.Dispose();

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        
        
        #endregion

        #region TirajeDistribucion

        /// <summary>
        /// Devuelve la lista de acuerdos en las que esta incluido el titular, así como el número de ejemplares
        /// que se le entregan y bajo que propiedad para el organismo señalado
        /// </summary>
        /// <param name="idTitular">Identificador del titular </param>
        /// <param name="idOrganismo">Identificador del organismo asociado</param>
        /// <returns></returns>
        public ObservableCollection<TirajePersonal> GetAcuerdosPorTitular(int idTitular, int idOrganismo)
        {
            ObservableCollection<TirajePersonal> listaTirajesIncluye = new ObservableCollection<TirajePersonal>();

            string sqlQuery = "SELECT P.*, A.Acuerdo FROM AcuerdoPadron P INNER JOIN C_Acuerdo A ON P.IDAcuNum = A.IDAcuNum " +
                               "WHERE P.IdTitular=@IdTitular AND IdOrg = @IdOrg ORDER BY P.IdAcuNum;";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", idTitular);
                cmd.Parameters.AddWithValue("@IdOrg", idOrganismo);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TirajePersonal tiraje = new TirajePersonal() { 
                            IdAcuerdo = Convert.ToInt16(reader["IdAcuNum"]), 
                            Particular = Convert.ToInt32(reader["Particular"]), 
                            Oficina = Convert.ToInt32(reader["Oficina"]), 
                            Biblioteca = Convert.ToInt32(reader["Biblioteca"]), 
                            Resguardo = Convert.ToInt32(reader["Resguardo"]), 
                            Personal = Convert.ToInt32(reader["Personal"]),
                            Autor = Convert.ToInt32(reader["Autor"]), 
                            Acuerdo = reader["Acuerdo"].ToString() 
                        };
                        listaTirajesIncluye.Add(tiraje);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return listaTirajesIncluye;
        }

        /// <summary>
        /// Obtiene el número total de ejemplares que se distribuirán en cada acuerdo sin tomar en cuenta
        /// la cantidad asignada al almacén de Zaragoza
        /// </summary>
        /// <param name="idAcuerdo"></param>
        private int GetTotalSinAlmacenZaragoza(int idAcuerdo)
        {
            int total = 0;

            string sqlQuery = "select SUM(Particular) + SUM(Oficina) + SUM(Biblioteca) + SUM(Resguardo) + " + 
                " SUM(Autor) + SUM(Personal) AS Total from AcuerdoPadron WHERE IdAcuNum = @IdAcuerdo AND IdOrg <> 6001";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdAcuerdo", idAcuerdo);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        total = Convert.ToInt32(reader["Total"]);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return total;
        }

        /// <summary>
        /// Actualiza la cantidad de ejemplares que permanecen en el Almacén de Zaragoza de acuerdo a las 
        /// modificaciones que va sufriendo cada una de las plantillas
        /// </summary>
        /// <param name="tiraje"></param>
        public void UpdateTotalAlmacenZaragoza(TirajePersonal tiraje)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                int totalZaragoza = Convert.ToInt32(tiraje.Acuerdo) - this.GetTotalSinAlmacenZaragoza(tiraje.IdAcuerdo);

                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                const string SqlQuery = "SELECT * FROM AcuerdoPadron WHERE IdAcuNum = @IdAcuerdo AND IdOrg = 6001";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdAcuerdo", tiraje.IdAcuerdo);
                dataAdapter.Fill(dataSet, "AcuerdoPadron");

                dr = dataSet.Tables["AcuerdoPadron"].Rows[0];
                dr.BeginEdit();
                dr["Resguardo"] = totalZaragoza;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText = "UPDATE AcuerdoPadron SET Resguardo = @Resguardo " +
                    "WHERE IdOrg = 6001 AND IdAcuNum = @IdAcuNum";
                dataAdapter.UpdateCommand.Parameters.Add("@Resguardo", SqlDbType.Int, 0, "Resguardo");
                dataAdapter.UpdateCommand.Parameters.Add("@IdAcuNum", SqlDbType.Int, 0, "IdAcuNum");
                dataAdapter.Update(dataSet, "AcuerdoPadron");

                dataSet.Dispose();
                dataAdapter.Dispose();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
        }

        public ObservableCollection<Adscripcion> GetTitularesIncluidosEnTodo()
        {
            ObservableCollection<TirajePersonal> acuerdos = new AcuerdosModel().GetAcuerdos();

            ObservableCollection<Adscripcion> incluidos = this.GetIdsIncluidosEnTodo(acuerdos.Count);

            OrganismoModel organismoModel = new OrganismoModel();

            foreach (Adscripcion ads in incluidos)
            {
                ads.Titular = this.GetTitulares(ads.Titular.IdTitular);
                ads.Organismo = organismoModel.GetOrganismos(ads.Organismo.IdOrganismo);
            }

            return new ObservableCollection<Adscripcion>(from n in incluidos orderby n.Organismo.Circuito select n);
        }


        /// <summary>
        /// Obtiene el Id del titular y su organismos de aquellos que se encuentran incluidos en todos los tirajes
        /// </summary>
        /// <param name="numeroAcuerdos"></param>
        /// <returns></returns>
        private ObservableCollection<Adscripcion> GetIdsIncluidosEnTodo(int numeroAcuerdos)
        {
            ObservableCollection<Adscripcion> incluidos = new ObservableCollection<Adscripcion>();

            const string SqlQuery = "Select IdOrg,IdTitular,COUNT(IdTitular) FROM AcuerdoPadron GROUP BY IdOrg,IdTitular HAVING COUNT(IdTitular) = @Total";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@Total", numeroAcuerdos);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Adscripcion adscripcion = new Adscripcion();

                        adscripcion.Organismo.IdOrganismo = Convert.ToInt32(reader["IdOrg"]);
                        adscripcion.Titular.IdTitular = Convert.ToInt32(reader["IdTitular"]);

                        incluidos.Add(adscripcion);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return incluidos;
        }

        
        #endregion
    }
}