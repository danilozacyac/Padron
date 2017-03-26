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
    public class PaisEstadoModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        #region Pais

        /// <summary>
        /// Obtiene el listado de paises capturados en el sistema
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Pais> GetPaises()
        {
            ObservableCollection<Pais> catalogoPaises = new ObservableCollection<Pais>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM C_Pais ORDER BY PaisMay", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Pais pais = new Pais() { 
                            IdPais = Convert.ToInt32(reader["IdPais"]), 
                            PaisDesc = reader["Pais"].ToString(), 
                            PaisStr = reader["PaisMay"].ToString() 
                        };

                        catalogoPaises.Add(pais);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoPaises;
        }


        /// <summary>
        /// Obtiene el país al que pertenece un estado
        /// </summary>
        /// <param name="idEstado">Estado del cual se quiere conocer el país</param>
        /// <returns></returns>
        public Pais GetPaises(int idEstado)
        {
            Pais pais = new Pais();
            const string SqlCadena = "SELECT P.* FROM C_Pais P INNER JOIN C_Estado E ON P.IdPais = E.IdPais WHERE IdEstado = @IdEstado ORDER BY PaisMay";


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;


            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdEstado", idEstado);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        pais.IdPais = Convert.ToInt32(reader["IdPais"]);
                        pais.PaisDesc = reader["Pais"].ToString();
                        pais.PaisStr = reader["PaisMay"].ToString();

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return pais;
        }


        public bool InsertaPais(Pais pais)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

           pais.IdPais = DataBaseUtilities.GetNextIdForUse("C_Pais", "IdPais", connection);

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Pais(IdPais,Pais,PaisMay)" +
                                "VALUES (@IdPais,@Pais,@PaisMay)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdPais", pais.IdPais);
                cmd.Parameters.AddWithValue("@Pais", pais.PaisDesc);
                cmd.Parameters.AddWithValue("@PaisMay", StringUtilities.PrepareToAlphabeticalOrder(pais.PaisDesc));

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        public bool UpdatePais(Pais pais)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool updateCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlQuery = "SELECT * FROM C_Pais WHERE IdPais = " + pais.IdPais;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlQuery, connection);

                dataAdapter.Fill(dataSet, "C_Pais");

                dr = dataSet.Tables["C_Pais"].Rows[0];
                dr.BeginEdit();
                dr["Pais"] = pais.PaisDesc;
                dr["PaisMay"] = StringUtilities.PrepareToAlphabeticalOrder(pais.PaisDesc);
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE C_Pais SET Pais = @Pais, PaisMay = @PaisMay WHERE IdPais = @IdPais";

                dataAdapter.UpdateCommand.Parameters.Add("@Pais", SqlDbType.VarChar, 0, "Pais");
                dataAdapter.UpdateCommand.Parameters.Add("@PaisMay", SqlDbType.VarChar, 0, "PaisMay");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPais", SqlDbType.Int, 0, "IdPais");

                dataAdapter.Update(dataSet, "C_Pais");

                dataSet.Dispose();
                dataAdapter.Dispose();

                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }

        public bool DeletePais(Pais pais)
        {
            bool complete = false;

            const string SqlCadena = "DELETE FROM C_Pais WHERE IdPais = @IdPais";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPais", pais.IdPais);
                cmd.ExecuteNonQuery();

                cmd.Dispose();

                complete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return complete;
        }

       

        #endregion



        #region Estado

        /// <summary>
        /// Obtiene el listado de todos los estados capturados en el sistema sin importar el país al que pertenecen
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Estado> GetEstados()
        {
            ObservableCollection<Estado> catalogoEstados = new ObservableCollection<Estado>();

            const string SqlCadena = "SELECT * FROM C_Estado WHERE IdPais = 39 ORDER BY EstadoMay";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Estado estado = new Estado()
                        {
                            IdEstado = Convert.ToInt32(reader["IdEstado"]),
                            EstadoDesc = reader["Estado"].ToString(),
                            EstadoStr = reader["EstadoMay"].ToString(),
                            Abreviatura = reader["EstadoAbr"].ToString(),
                            IdPais = Convert.ToInt32(reader["IdPais"])
                        };

                        catalogoEstados.Add(estado);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoEstados;
        }

        /// <summary>
        /// Obtiene el listado de estados del pais solicitado
        /// </summary>
        /// <param name="idPais"></param>
        /// <returns></returns>
        public ObservableCollection<Estado> GetEstados(int idPais)
        {
            ObservableCollection<Estado> catalogoEstados = new ObservableCollection<Estado>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                cmd = new SqlCommand("SELECT * FROM C_Estado WHERE IdPais = @IdPais ORDER BY EstadoMay", connection);
                cmd.Parameters.AddWithValue("@IdPais", idPais);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Estado estado = new Estado()
                        {
                            IdEstado = Convert.ToInt32(reader["IdEstado"]),
                            EstadoDesc = reader["Estado"].ToString(),
                            EstadoStr = reader["EstadoMay"].ToString(),
                            Abreviatura = reader["EstadoAbr"].ToString(),
                            IdPais = Convert.ToInt32(reader["IdPais"])
                        };

                        catalogoEstados.Add(estado);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoEstados;
        }


        public bool InsertaEstado(Estado estado)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            estado.IdEstado = DataBaseUtilities.GetNextIdForUse("C_Estado", "IdEstado", connection);

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Estado(IdEstado,Estado,EstadoAbr,EstadoMay,IdPais)" +
                                "VALUES (@IdEstado,@Estado,@EstadoAbr,@EstadoMay,@IdPais)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdEstado", estado.IdEstado);
                cmd.Parameters.AddWithValue("@Estado", estado.EstadoDesc);
                cmd.Parameters.AddWithValue("@EstadoAbr", estado.EstadoDesc);
                cmd.Parameters.AddWithValue("@EstadoMay", StringUtilities.PrepareToAlphabeticalOrder(estado.EstadoDesc));
                cmd.Parameters.AddWithValue("@IdPais", estado.IdPais);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        public bool UpdateEstado(Estado estado)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool updateCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM C_Estado WHERE IdEstado = @IdEstado", connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdEstado", estado.IdEstado);
                dataAdapter.Fill(dataSet, "C_Estado");

                dr = dataSet.Tables["C_Estado"].Rows[0];
                dr.BeginEdit();
                dr["Estado"] = estado.EstadoDesc;
                dr["EstadoMay"] = StringUtilities.PrepareToAlphabeticalOrder(estado.EstadoDesc);
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE C_Estado SET Estado = @Estado, EstadoMay = @EstadoMay WHERE IdEstado = @IdEstado";

                dataAdapter.UpdateCommand.Parameters.Add("@Estado", SqlDbType.VarChar, 0, "Estado");
                dataAdapter.UpdateCommand.Parameters.Add("@EstadoMay", SqlDbType.VarChar, 0, "EstadoMay");
                dataAdapter.UpdateCommand.Parameters.Add("@IdEstado", SqlDbType.Int, 0, "IdEstado");

                dataAdapter.Update(dataSet, "C_Estado");

                dataSet.Dispose();
                dataAdapter.Dispose();

                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }


        public bool DeleteEstado(Estado estado)
        {
            bool complete = false;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("DELETE FROM C_Estado WHERE IdEstado = @IdEstado", connection);
                cmd.Parameters.AddWithValue("@IdEstado", estado.IdEstado);
                cmd.ExecuteNonQuery();
                
                cmd.Dispose();

                complete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return complete;
        }


        

        #endregion 



        #region Ciudad

        /// <summary>
        /// Obtiene el listado completo de ciudades capturadas en el sistema
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Ciudad> GetCiudades()
        {
            ObservableCollection<Ciudad> catalogoCiudades = new ObservableCollection<Ciudad>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM C_Ciudad ORDER BY CiudadMay", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Ciudad ciudad = new Ciudad()
                        {
                            IdCiudad = Convert.ToInt32(reader["IdCiudad"]),
                            CiudadDesc = reader["Ciudad"].ToString(),
                            CiudadStr = reader["CiudadMay"].ToString(),
                            IdEstado = Convert.ToInt32(reader["IdEstado"])
                        };

                        catalogoCiudades.Add(ciudad);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoCiudades;
        }

        /// <summary>
        /// Obtiene el listado de ciudades del estado solicitado
        /// </summary>
        /// <param name="idEstado">Identificador del Estado del cual se quieren obtener las ciudades</param>
        /// <returns></returns>
        public ObservableCollection<Ciudad> GetCiudades(int idEstado)
        {
            ObservableCollection<Ciudad> catalogoCiudades = new ObservableCollection<Ciudad>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM C_Ciudad WHERE IdEstado = @IdEstado ORDER BY CiudadMay", connection);
                cmd.Parameters.AddWithValue("@IdEstado", idEstado);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Ciudad ciudad = new Ciudad()
                        {
                            IdCiudad = Convert.ToInt32(reader["IdCiudad"]),
                            CiudadDesc = reader["Ciudad"].ToString(),
                            CiudadStr = reader["CiudadMay"].ToString(),
                            IdEstado = Convert.ToInt32(reader["IdEstado"])
                        };

                        catalogoCiudades.Add(ciudad);

                    }
                }
                cmd.Dispose();
                reader.Close();

                //catalogoCiudades.Add(dummyCiudad);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoCiudades;
        }


        public bool InsertaCiudad(Ciudad ciudad)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            ciudad.IdCiudad = DataBaseUtilities.GetNextIdForUse("C_Ciudad", "IdCiudad", connection);

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Ciudad(IdCiudad,Ciudad,CiudadMay,IdEstado)" +
                                "VALUES (@IdCiudad,@Ciudad,@CiudadMay,@IdEstado)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdCiudad", ciudad.IdCiudad);
                cmd.Parameters.AddWithValue("@Ciudad", ciudad.CiudadDesc);
                cmd.Parameters.AddWithValue("@CiudadMay", StringUtilities.PrepareToAlphabeticalOrder(ciudad.CiudadDesc));
                cmd.Parameters.AddWithValue("@IdEstado", ciudad.IdEstado);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }


        public bool UpdateCiudad(Ciudad ciudad)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool updateCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("SELECT * FROM C_Ciudad WHERE IdCiudad = @IdCiudad", connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdCiudad", ciudad.IdCiudad);
                dataAdapter.Fill(dataSet, "C_Ciudad");

                dr = dataSet.Tables["C_Ciudad"].Rows[0];
                dr.BeginEdit();
                dr["Ciudad"] = ciudad.CiudadDesc;
                dr["CiudadMay"] = StringUtilities.PrepareToAlphabeticalOrder(ciudad.CiudadDesc);
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE C_Ciudad SET Ciudad = @Ciudad, CiudadMay = @CiudadMay WHERE IdCiudad = @IdCiudad";

                dataAdapter.UpdateCommand.Parameters.Add("@Ciudad", SqlDbType.VarChar, 0, "Ciudad");
                dataAdapter.UpdateCommand.Parameters.Add("@CiudadMay", SqlDbType.VarChar, 0, "CiudadMay");
                dataAdapter.UpdateCommand.Parameters.Add("@IdCiudad", SqlDbType.Int, 0, "IdCiudad");

                dataAdapter.Update(dataSet, "C_Ciudad");

                dataSet.Dispose();
                dataAdapter.Dispose();

                updateCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }


        public bool DeleteCiudad(Ciudad ciudad)
        {
            bool complete = false;

            int total = this.GetOrgRelCiudad(ciudad.IdCiudad);

            if (total > 0)
                return complete;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("DELETE FROM C_Ciudad WHERE IdCiudad = @IdCiudad", connection);
                cmd.Parameters.AddWithValue("@IdCiudad", ciudad.IdCiudad);
                cmd.ExecuteNonQuery();

                cmd.Dispose();

                complete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return complete;
        }

        /// <summary>
        /// Obtiene el número de organismos relacionados a una ciudad
        /// </summary>
        /// <param name="idCiudad"></param>
        /// <returns></returns>
        private int GetOrgRelCiudad(int idCiudad)
        {
            int totalRelaciones = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT IdOrg FROM C_Organismo WHERE  Ciudad = @Ciudad", connection);
                cmd.Parameters.AddWithValue("@Ciudad", idCiudad);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        totalRelaciones += 1;
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PaisEstadoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return totalRelaciones;
        }


        #endregion

    }
}
