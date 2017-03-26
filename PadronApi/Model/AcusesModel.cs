using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;

namespace PadronApi.Model
{
    public class AcusesModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Obtiene los datos de entrega de lo envíos
        /// </summary>
        /// <param name="selectedYear">Indica el año que se seleccionó en la ventana para mostrar los únicamente los acuses de ese año</param>
        /// <returns></returns>
        public ObservableCollection<Acuse> GetDetalleRecepcion(int selectedYear)
        {
            ObservableCollection<Acuse> entregados = new ObservableCollection<Acuse>();

            string sqlCadena = "SELECT A.IdPadron,P.AnioAcuerdo, A.IdTitular, A.IdOrg, A.FechaRecPaq,A.FechaEnvio, A.Guia, A.ArchivoGuia, A.FechaRecAcu, A.ArchivoAcu, A.NumOficio,A.PersonaRecibe, " +
                               "(Ti.TituloAbr+' '+T.Nombre+' '+T.Apellidos) Nombre, O.DescOrg, O.IdTpodist, E.Estado, C.Ciudad, C_Obra.Titulo,C_Obra.IdObra " +
                               "FROM Padron P INNER JOIN Acuses A INNER JOIN C_Titular T ON A.IdTitular = T.IdTitular INNER JOIN C_Organismo O " +
							   "ON A.IdOrg = O.IdOrg INNER JOIN C_Ciudad C ON O.IdCiudad = C.IdCiudad INNER JOIN C_Estado E " +
                               "ON O.IdEstado = E.IdEstado INNER JOIN C_Titulo Ti ON T.IdTitulo = Ti.IdTitulo ON P.IdPadron = A.IdPadron " +
                               "INNER JOIN C_Obra ON P.IdObra = C_Obra.IdObra ";

            if (selectedYear > 0)
                sqlCadena += "WHERE P.AnioAcuerdo = @Year";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                if (selectedYear > 0)
                    cmd.Parameters.AddWithValue("@Year", selectedYear);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Acuse detalleEntrega = new Acuse()
                        {
                            IdPadron = Convert.ToInt32(reader["IdPadron"]),
                            IdTitular = Convert.ToInt32(reader["IdTitular"]),
                            IdOrganismo = Convert.ToInt32(reader["IdOrg"]),
                            Nombre = reader["Nombre"].ToString(),
                            Organismo = reader["DescOrg"].ToString(),
                            CiudadStr = reader["Ciudad"].ToString(),
                            EstadoStr = reader["Estado"].ToString(),
                            TipoDistribucion = Convert.ToInt16(reader["IdTpoDist"]),
                            FechaRecPaqueteria = DateTimeUtilities.IntToDate(reader, "FechaRecPaq"),
                            FechaEnvio = DateTimeUtilities.IntToDate(reader, "FechaEnvio"),
                            NumGuia = reader["Guia"].ToString(),
                            ArchivoGuia = reader["ArchivoGuia"].ToString(),
                            FechaRecAcuse = DateTimeUtilities.IntToDate(reader, "FechaRecAcu"),
                            ArchivoAcuse = reader["ArchivoAcu"].ToString(),
                            Oficio = Convert.ToInt32(reader["NumOficio"]),
                            TituloObra = reader["Titulo"].ToString(),
                            IdObra = Convert.ToInt32(reader["IdObra"]),
                            QuienRecibe = reader["PersonaRecibe"].ToString(),
                            AnioAcuerdo = Convert.ToInt32(reader["AnioAcuerdo"])
                        };

                        entregados.Add(detalleEntrega);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcusesModel " , "PadronApi");
                
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcusesModel ", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return entregados;
        }

        /// <summary>
        /// Obtiene los detalles de las fechas de recepción de cada uno de los paquetes enviados
        /// en la distribución de una obra particular. Obtiene el recibo de paquetería, así como 
        /// el que genera el área
        /// </summary>
        /// <param name="padronGenerado"></param>
        /// <returns></returns>
        public ObservableCollection<Acuse> GetDetalleRecepcion(PadronGenerado padronGenerado)
        {
            ObservableCollection<Acuse> entregados = new ObservableCollection<Acuse>();

            string sqlCadena = "SELECT A.IdPadron, A.IdTitular, A.IdOrg, A.FechaRecPaq, A.FechaEnvio, A.Guia, A.ArchivoGuia, A.FechaRecAcu, A.ArchivoAcu, A.NumOficio,A.PersonaRecibe, " +
                               "(Ti.TituloAbr + ' ' + T.Nombre + ' ' + T.Apellidos) AS Nombre, O.DescOrg, O.IdTpodist, E.Estado, C.Ciudad   " +
                               "FROM ((((Acuses as A INNER JOIN C_Titular AS T ON A.IdTitular = T.IdTitular) INNER JOIN C_Organismo AS O ON A.IdOrg = O.IdOrg) " +
                               "INNER JOIN C_Ciudad AS C ON O.IdCiudad = C.IdCiudad) INNER JOIN C_Estado AS E ON O.IdEstado = E.IdEstado) " +
                               "INNER JOIN C_Titulo AS Ti ON T.IdTitulo = Ti.IdTitulo  " +
                               "WHERE IdPadron = @IdPadron";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", padronGenerado.IdPadron);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Acuse plantilla = new Acuse()
                        {
                            IdPadron = Convert.ToInt32(reader["IdPadron"]),
                            IdTitular = Convert.ToInt32(reader["IdTitular"]),
                            IdOrganismo = Convert.ToInt32(reader["IdOrg"]),
                            Nombre = reader["Nombre"].ToString(),
                            Organismo = reader["DescOrg"].ToString(),
                            CiudadStr = reader["Ciudad"].ToString(),
                            EstadoStr = reader["Estado"].ToString(),
                            TipoDistribucion = Convert.ToInt16(reader["IdTpoDist"]),
                            FechaRecPaqueteria = DateTimeUtilities.IntToDate(reader, "FechaRecPaq"),
                            FechaEnvio = DateTimeUtilities.IntToDate(reader, "FechaEnvio"),
                            NumGuia = reader["Guia"].ToString(),
                            ArchivoGuia = reader["ArchivoGuia"].ToString(),
                            FechaRecAcuse = DateTimeUtilities.IntToDate(reader, "FechaRecAcu"),
                            ArchivoAcuse = reader["ArchivoAcu"].ToString(),
                            Oficio = Convert.ToInt32(reader["NumOficio"]),
                            QuienRecibe = reader["PersonaRecibe"].ToString()
                        };

                        entregados.Add(plantilla);
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

            return entregados;
        }

        /// <summary>
        /// Devuelve el número de entradas de acuses de el padrón seleccionado que contienen detalle de entrega y/o recepción
        /// </summary>
        /// <param name="padronGenerado"></param>
        /// <returns></returns>
        public int VerificaParaBorrar(PadronGenerado padronGenerado)
        {
            int existeInfo = 0;

            const string SqlCadena = "SELECT COUNT(IdTitular) AS ConInfo FROM Acuses WHERE IdPadron = @IdPadron AND (FechaEnvio <> 0 OR FechaRecPaq <> 0 OR FechaRecAcu <> 0)";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", padronGenerado.IdPadron);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        existeInfo = Convert.ToInt32(reader["ConInfo"]);
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

            return existeInfo;
        }

        /// <summary>
        /// Elimina las entradas de la base de datos de los acuses de una obra específica siempre y cuando en 
        /// ninguno de sus registros exista información de envio y/o entrega
        /// </summary>
        /// <param name="padronGenerado"></param>
        /// <returns></returns>
        public bool DeleteAcusesRecepcion(PadronGenerado padronGenerado)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool deleteCompleted = false;

            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM Acuses WHERE IdPadron = @IdPadron", connection);
                cmd.Parameters.AddWithValue("@IdPadron", padronGenerado.IdPadron);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                deleteCompleted = true;
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

            return deleteCompleted;
        }

        public bool UpdateDetalleRecepcion(PadronGenerado padronGenerado, Acuse infoTitular)
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

                const string SqlQuery = "SELECT * FROM Acuses WHERE IdPadron = @IdPadron AND IdOrg = @IdOrg AND IdTitular = @IdTitular AND NumOficio = @NumOficio";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdPadron", infoTitular.IdPadron);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdOrg", infoTitular.IdOrganismo);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdTitular", infoTitular.IdTitular);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@NumOficio", infoTitular.Oficio);
                dataAdapter.Fill(dataSet, "Acuses");

                dr = dataSet.Tables["Acuses"].Rows[0];
                dr.BeginEdit();

                if (infoTitular.FechaRecPaqueteria == null)
                    dr["FechaRecPaq"] = 0;
                else
                    dr["FechaRecPaq"] = DateTimeUtilities.DateToInt(infoTitular.FechaRecPaqueteria);

                if (infoTitular.FechaEnvio == null)
                    dr["FechaEnvio"] = 0;
                else
                    dr["FechaEnvio"] = DateTimeUtilities.DateToInt(infoTitular.FechaEnvio);

                dr["PersonaRecibe"] = infoTitular.QuienRecibe;
                dr["Guia"] = infoTitular.NumGuia;
                dr["ArchivoGuia"] = infoTitular.ArchivoGuia;

                if (infoTitular.FechaRecAcuse == null)
                    dr["FechaRecAcu"] = 0;
                else
                    dr["FechaRecAcu"] = DateTimeUtilities.DateToInt(infoTitular.FechaRecAcuse);
                dr["ArchivoAcu"] = infoTitular.ArchivoAcuse;

                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE Acuses SET FechaRecPaq = @FechaRecPaq, FechaEnvio = @FechaEnvio, PersonaRecibe = @PersonaRecibe," +
                                                        " Guia = @Guia, ArchivoGuia = @ArchivoGuia, FechaRecAcu = @FechaRecAcu, ArchivoAcu = @ArchivoAcu " +
                                                        " WHERE IdPadron = @IdPadron AND IdOrg = @IdOrg AND IdTitular = @IdTitular AND NumOficio = @NumOficio";

                dataAdapter.UpdateCommand.Parameters.Add("@FechaRecPaq", SqlDbType.Int, 0, "FechaRecPaq");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaEnvio", SqlDbType.Int, 0, "FechaEnvio");
                dataAdapter.UpdateCommand.Parameters.Add("@PersonaRecibe", SqlDbType.VarChar, 0, "PersonaRecibe");
                dataAdapter.UpdateCommand.Parameters.Add("@Guia", SqlDbType.VarChar, 0, "Guia");
                dataAdapter.UpdateCommand.Parameters.Add("@ArchivoGuia", SqlDbType.VarChar, 0, "ArchivoGuia");
                dataAdapter.UpdateCommand.Parameters.Add("@FechaRecAcu", SqlDbType.Int, 0, "FechaRecAcu");
                dataAdapter.UpdateCommand.Parameters.Add("@ArchivoAcu", SqlDbType.VarChar, 0, "ArchivoAcu");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPadron", SqlDbType.Int, 0, "IdPadron");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTitular", SqlDbType.Int, 0, "IdTitular");
                dataAdapter.UpdateCommand.Parameters.Add("@NumOficio", SqlDbType.Int, 0, "NumOficio");
                dataAdapter.Update(dataSet, "Acuses");

                dataSet.Dispose();
                dataAdapter.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        public bool UpdateFechaRecepcion(ObservableCollection<object> plantilla, string campo, DateTime? fecha, int tipoAcuse)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                foreach (Acuse infoTitular in plantilla)
                {
                    DataSet dataSet = new DataSet();
                    DataRow dr;

                    const string SqlQuery = "SELECT * FROM Acuses WHERE IdPadron = @IdPadron AND NumOficio = @Oficio";

                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@IdPadron", infoTitular.IdPadron);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@Oficio", infoTitular.Oficio);
                    dataAdapter.Fill(dataSet, "Acuses");

                    dr = dataSet.Tables["Acuses"].Rows[0];
                    dr.BeginEdit();
                    dr[campo] = DateTimeUtilities.DateToInt(fecha);
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = String.Format("UPDATE Acuses SET {0} = @Fecha  WHERE IdPadron = @IdPadron AND NumOficio = @NumOficio", campo);

                    dataAdapter.UpdateCommand.Parameters.Add("@Fecha", SqlDbType.Int, 0, campo);
                    dataAdapter.UpdateCommand.Parameters.Add("@IdPadron", SqlDbType.Int, 0, "IdPadron");
                    dataAdapter.UpdateCommand.Parameters.Add("@NumOficio", SqlDbType.Int, 0, "NumOficio");
                    dataAdapter.Update(dataSet, "Acuses");

                    dataSet.Dispose();
                    dataAdapter.Dispose();

                    if (tipoAcuse == 1)
                        infoTitular.FechaRecPaqueteria = fecha;
                    else
                        infoTitular.FechaRecAcuse = fecha;
                }
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        public bool UpdateDetallesEnvioPaqueteria(int idPadron, int idTitular, string guia, string fechaEnvio)
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

                const string SqlQuery = "SELECT * FROM Acuses WHERE IdPadron = @IdPadron AND IdTitular = @IdTitular";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdPadron", idPadron);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdTitular", idTitular);
                dataAdapter.Fill(dataSet, "Acuses");

                dr = dataSet.Tables["Acuses"].Rows[0];
                dr.BeginEdit();

                dr["FechaEnvio"] = fechaEnvio;
                dr["Guia"] = guia;

                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE Acuses SET FechaEnvio = @FechaEnvio, " +
                                                        " Guia = @Guia WHERE IdPadron = @IdPadron AND IdTitular = @IdTitular";

                dataAdapter.UpdateCommand.Parameters.Add("@FechaEnvio", SqlDbType.Int, 0, "FechaEnvio");
                dataAdapter.UpdateCommand.Parameters.Add("@Guia", SqlDbType.VarChar, 0, "Guia");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPadron", SqlDbType.Int, 0, "IdPadron");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTitular", SqlDbType.Int, 0, "IdTitular");
                dataAdapter.Update(dataSet, "Acuses");

                dataSet.Dispose();
                dataAdapter.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// De los datos de entrega de paquetería que se obtienen del archivo de excel completa la información de recepción de las 
        /// obras, a partir de el número de guía
        /// </summary>
        /// <param name="guia"></param>
        /// <param name="fechaRecepcion"></param>
        /// <param name="quienRecibe"></param>
        /// <returns>Devuelve el número de filas que fueron actualizadas</returns>
        public int UpdateFechaRecepcionPaqueteria(string guia, string fechaRecepcion, string quienRecibe)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            int updatedRowNumber = 0;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM Acuses WHERE Guia = @Guia ", connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Guia", guia);
                dataAdapter.Fill(dataSet, "Acuses");

                if (dataSet.Tables["Acuses"].Rows.Count > 0)
                {
                    dr = dataSet.Tables["Acuses"].Rows[0];
                    dr.BeginEdit();
                    dr["FechaRecPaq"] = fechaRecepcion;
                    dr["PersonaRecibe"] = quienRecibe;
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = "UPDATE Acuses SET FechaRecPaq = @FechaRecPaq, " +
                                                            " PersonaRecibe = @PersonaRecibe WHERE Guia = @Guia";

                    dataAdapter.UpdateCommand.Parameters.Add("@FechaRecPaq", SqlDbType.Int, 0, "FechaRecPaq");
                    dataAdapter.UpdateCommand.Parameters.Add("@PersonaRecibe", SqlDbType.VarChar, 0, "PersonaRecibe");
                    dataAdapter.UpdateCommand.Parameters.Add("@Guia", SqlDbType.VarChar, 0, "Guia");
                    updatedRowNumber = dataAdapter.Update(dataSet, "Acuses");
                }

                dataSet.Dispose();
                dataAdapter.Dispose();
                
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return updatedRowNumber;
        }

        /// <summary>
        /// Obtiene y asigan el número de guía a los titulares de un organismos. Estos titulares son los compañeros de aquellos
        /// para quienes se personalizaron las etiquetas
        /// </summary>
        /// <param name="idPadron"></param>
        public void SetGuiaColegas(int idPadron)
        {
            ObservableCollection<Acuse> porCompletar = GetSinGuiaByPadron(idPadron);

            ObservableCollection<Acuse> conGuia = GetGuiaEntrega(idPadron);

            foreach (Acuse sinGuia in porCompletar)
            {
                try
                {
                    Acuse titularBase = (from n in conGuia
                                         where n.IdOrganismo == sinGuia.IdOrganismo
                                         select n).ToList()[0];

                    if (sinGuia.IdTitular != 14913 && sinGuia.IdTitular != 14916)
                        this.UpdateDetallesEnvioPaqueteria(idPadron, sinGuia.IdTitular, titularBase.NumGuia, DateTimeUtilities.DateToInt(titularBase.FechaEnvio));
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }
        }

        /// <summary>
        /// Obtiene los datos de aquellos titulares a los que se les envió una obra pero no tienen asignado el número de guía con 
        /// el que se realizó dicho envio
        /// </summary>
        /// <param name="idPadron">Identificador de la obra que se esta completando</param>
        /// <returns></returns>
        private ObservableCollection<Acuse> GetSinGuiaByPadron(int idPadron)
        {
            ObservableCollection<Acuse> entregados = new ObservableCollection<Acuse>();

            const string SqlCadena = "SELECT * FROM Acuses WHERE FechaEnvio = 0 AND IdPadron = @IdPadron";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Acuse detalleEntrega = new Acuse() { 
                            IdPadron = Convert.ToInt32(reader["IdPadron"]), 
                            IdTitular = Convert.ToInt32(reader["IdTitular"]), 
                            IdOrganismo = Convert.ToInt32(reader["IdOrg"]), 
                            Oficio = Convert.ToInt32(reader["NumOficio"]) 
                        };
                        entregados.Add(detalleEntrega);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcusesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcusesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return entregados;
        }

        private ObservableCollection<Acuse> GetGuiaEntrega(int idPadron)
        {
            ObservableCollection<Acuse> entregados = new ObservableCollection<Acuse>();

            const string SqlCadena = "SELECT * FROM Acuses WHERE FechaEnvio > 0 AND IdPadron = @IdPadron";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Acuse detalleEntrega = new Acuse() { 
                            IdPadron = Convert.ToInt32(reader["IdPadron"]), 
                            IdTitular = Convert.ToInt32(reader["IdTitular"]), 
                            IdOrganismo = Convert.ToInt32(reader["IdOrg"]), 
                            NumGuia = reader["Guia"].ToString(), 
                            FechaEnvio = DateTimeUtilities.IntToDate(reader, "FechaEnvio"), 
                            Oficio = Convert.ToInt32(reader["NumOficio"]) 
                        };
                        entregados.Add(detalleEntrega);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcusesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcusesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return entregados;
        }
    }
}