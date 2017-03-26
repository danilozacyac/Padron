using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;
using System.Data;

namespace PadronApi.Model
{
    public class DevolucionModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public int GetTotalDevueltosFuncObra(int idTitular, int idObra)
        {
            int totalDevueltos = 0;
            string sqlCadena = "SELECT  Sum(Cantidad) AS Total FROM Devolucion " +
                        " WHERE IdObra = @IdObra AND IdTitular = @IdTitular";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdObra", idObra);
                cmd.Parameters.AddWithValue("@IdTitular", idTitular);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        if (reader["Total"] == DBNull.Value)
                            totalDevueltos = 0;
                        else
                            totalDevueltos = Convert.ToInt32(reader["Total"]);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,DevolucionModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,DevolucionModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return totalDevueltos;
        }



        /// <summary>
        /// Obtiene el número de ejemplares devueltos por el titular para la obra específica
        /// </summary>
        /// <param name="idTitular">Identificador del titular</param>
        /// <param name="idObra">Identificador de la obra</param>
        /// <param name="tipoPropiedad">Tipo de propiedad en que le fue entregada la obra</param>
        /// <returns></returns>
        public int GetDevueltosTituProp(int idTitular, int idObra, int tipoPropiedad)
        {
            int devueltos = 0;

            string sqlCadena = "SELECT SUM(Cantidad) AS Devueltos FROM Devolucion " +
                " WHERE IdTitular = @IdTitular AND IdObra = @IdObra AND Propiedad = @Propiedad";


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdTitular", idTitular);
                cmd.Parameters.AddWithValue("@IdObra", idObra);
                cmd.Parameters.AddWithValue("@Propiedad", tipoPropiedad);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["Devueltos"] == DBNull.Value)
                            devueltos = 0;
                        else
                            devueltos = Convert.ToInt32(reader["Devueltos"]);
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

            return devueltos;
        }



        /// <summary>
        /// Ingresa la referencia de las publicaciones devueltas por los titulares
        /// </summary>
        /// <param name="idTitular">Identificador del titular</param>
        /// <param name="idObra">Identificador de la obra que esta devolviendo</param>
        /// <param name="tipoPropiedad">Propiedad en la que le fue entregada la obra</param>
        /// <param name="fechaDevolucion">Fecha en la que realiza la devolución</param>
        /// <param name="observaciones">Observaciones generales de la devolución</param>
        /// <param name="cantidad">Cantidad que esta devolviendo</param>
        /// <returns></returns>
        public bool InsertaDevolucion(Devoluciones devolucion)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO Devolucion(IdTitular, IdObra,Fecha,Observaciones,Cantidad,Propiedad,OficioDev,TipoDevolucion)" +
                                  "VALUES (@IdTitular, @IdObra,@Fecha,@Observaciones,@Cantidad,@Propiedad,@OficioDev,@TipoDevolucion)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", devolucion.IdTitular);
                cmd.Parameters.AddWithValue("@IdObra", devolucion.IdObra);
                cmd.Parameters.AddWithValue("@Fecha", DateTimeUtilities.DateToInt(devolucion.FechaDevolucion));
                cmd.Parameters.AddWithValue("@Observaciones", devolucion.Observaciones);
                cmd.Parameters.AddWithValue("@Cantidad", devolucion.Cantidad);
                cmd.Parameters.AddWithValue("@Propiedad", devolucion.Propiedad);
                cmd.Parameters.AddWithValue("@OficioDev", devolucion.OficioDevolucion);
                cmd.Parameters.AddWithValue("@TipoDevolucion", devolucion.TipoDevolucion);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PlantillaModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PlantillaModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Obtiene el historial de las obras que ha recibido el titular seleccionado
        /// </summary>
        /// <param name="titular">Titular del cual se quiere conocer el historial</param>
        /// <returns></returns>
        public ObservableCollection<Devoluciones> GetHistorialTitularObras(Titular titular)
        {
            ObservableCollection<Devoluciones> plantilla = new ObservableCollection<Devoluciones>();

            string sqlCadena = "SELECT H.*, P.AcuerdoNum, P.AnioAcuerdo, O.Titulo,O.IdObra,P.Fecha, " + 
                               "(SELECT SUM(Cantidad) FROM Devolucion WHERE IdTitular = H.IdTitular AND IdObra = P.IdObra) AS Total, " +
                               "(SELECT TipoDevolucion FROM Devolucion WHERE IdTitular = H.IdTitular AND IdObra = P.IdObra) AS Tipo " +
                               "FROM (Padron P INNER JOIN C_Obra O ON P.IdObra = O.IdObra) INNER JOIN PadronHistorico H " +
                               "ON P.IdPadron = H.IdPadron WHERE H.IdTitular = @IdTitular ORDER BY P.Fecha";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Devoluciones item = new Devoluciones()
                        {
                            IdPadron = Convert.ToInt32(reader["IdPadron"]),
                            IdOrganismo = Convert.ToInt32(reader["IdOrg"]),
                            IdObra = Convert.ToInt32(reader["IdObra"]),
                            IdTitular = Convert.ToInt32(reader["IdTitular"]),
                            Particular = Convert.ToInt32(reader["Particular"]),
                            Oficina = Convert.ToInt32(reader["Oficina"]),
                            Biblioteca = Convert.ToInt32(reader["Biblioteca"]),
                            Resguardo = Convert.ToInt32(reader["Resguardo"]),
                            Personal = Convert.ToInt32(reader["Personal"])
                        };

                        item.Particular += Convert.ToInt32(reader["Autor"]);
                        item.Titulo = reader["Titulo"].ToString();
                        item.NumAcuerdo = Convert.ToInt32(reader["AcuerdoNum"]);
                        item.AnioAcuerdo = Convert.ToInt32(reader["AnioAcuerdo"]);
                        item.FechaEnvio = DateTimeUtilities.IntToDate(reader, "Fecha");
                        item.TotalDevoluciones = (reader["Total"]) as int? ?? 0;
                        item.TipoDevolucion = Convert.ToInt16(reader["Cancelado"]);

                        plantilla.Add(item);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,DevolucionModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,DevolucionModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return plantilla;
        }



        public void GetDevoluciones(Devoluciones devolucion)
        {
            const string SqlCadena = "SELECT * FROM Devolucion WHERE IdTitular = @IdTitular AND IdObra = @IdObra";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdTitular", devolucion.IdTitular);
                cmd.Parameters.AddWithValue("@IdObra", devolucion.IdObra);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["IdTitular"] == DBNull.Value || Convert.ToInt32(reader["IdTitular"]) == 0)
                            break;

                        devolucion.OficioDevolucion += reader["OficioDev"] + ", ";
                        devolucion.FechaDevolucionString += DateTimeUtilities.IntToDate(reader, "Fecha").Value.ToShortDateString() + ", ";
                        devolucion.Observaciones += reader["Observaciones"] + ". \r\n";
                        devolucion.Cantidad += Convert.ToInt32(reader["Cantidad"]);
                        devolucion.TipoDevolucion = Convert.ToInt32(reader["TipoDevolucion"]);
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

        }


        public bool SetCancelacion(PlantillaDto plantilla)
        {
            bool updateCompleted = false;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM PadronHistorico WHERE IdPadron = @IdPadron AND IdOrg = @IdOrg AND IdTitular = @IdTitular", connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdPadron", plantilla.IdPadron);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdOrg", plantilla.IdOrganismo);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdTitular", plantilla.IdTitular);
                dataAdapter.Fill(dataSet, "PadronHistorico");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Cancelado"] = 1;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();
                dataAdapter.UpdateCommand.CommandText =
                                                       "UPDATE PadronHistorico SET Cancelado = @Cancelado WHERE IdPadron = @IdPadron AND IdOrg = @IdOrg AND IdTitular = @IdTitular";

                dataAdapter.UpdateCommand.Parameters.Add("@Cancelado", SqlDbType.Int, 0, "Cancelado");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPadron", SqlDbType.Int, 0, "IdPadron");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTitular", SqlDbType.Int, 0, "IdTitular");
                

                dataAdapter.Update(dataSet, "PadronHistorico");
                dataSet.Dispose();
                dataAdapter.Dispose();

                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,DevolucionModel", "Contradicciones");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,DevolucionModel", "Contradicciones");
            }
            finally
            {
                connection.Close();
            }
            return updateCompleted;
        }

    }
}
