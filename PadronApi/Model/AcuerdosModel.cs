using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;

namespace PadronApi.Model
{
    public class AcuerdosModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public ObservableCollection<TirajePersonal> GetAcuerdos()
        {
            ObservableCollection<TirajePersonal> listaTirajesIncluye = new ObservableCollection<TirajePersonal>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM C_Acuerdo WHERE Activo = 1 ORDER BY Acuerdo", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TirajePersonal tiraje = new TirajePersonal()
                        {
                            IdAcuerdo = Convert.ToInt32(reader["IdAcuNum"]),
                            Acuerdo = reader["Acuerdo"].ToString(),
                            AcDescripcion = String.Format("{0} {1}",
                            reader["Acuerdo"], reader["Descripcion"]),
                            Particular = 0,
                            Oficina = 0,
                            Biblioteca = 0,
                            Resguardo = 0
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return listaTirajesIncluye;
        }


        public TirajePersonal GetAcuerdos(int idAcuerdo)
        {
            TirajePersonal tiraje = null;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM C_Acuerdo WHERE Activo = 1 AND IdAcuNum = @IdAcuerdo ORDER BY Acuerdo", connection);
                cmd.Parameters.AddWithValue("@IdAcuerdo", idAcuerdo);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tiraje = new TirajePersonal();
                        tiraje.IdAcuerdo = Convert.ToInt16(reader["IdAcuNum"]);
                        tiraje.Acuerdo = reader["Acuerdo"].ToString();
                        tiraje.Particular = 0;
                        tiraje.Oficina = 0;
                        tiraje.Biblioteca = 0;
                        tiraje.Resguardo = 0;
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return tiraje;
        }


        /// <summary>
        /// Agrega la referencia de una nueva plantilla
        /// </summary>
        /// <param name="nuevoAcuerdo"></param>
        /// <returns></returns>
        public bool GeneraAcuerdo(TirajePersonal nuevoAcuerdo,string descripcion)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            nuevoAcuerdo.IdAcuerdo = DataBaseUtilities.GetNextIdForUse("C_Acuerdo", "IdAcuNum", connection);

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Acuerdo(IdAcuNum,Acuerdo,Activo,Descripcion)" +
                                "VALUES (@IdAcuNum,@Acuerdo,1,@Descripcion)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdAcuNum", nuevoAcuerdo.IdAcuerdo);
                cmd.Parameters.AddWithValue("@Acuerdo", nuevoAcuerdo.Acuerdo);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }



        /// <summary>
        /// Obtiene la plantilla de un acuerdo para ser tomada como base en la generación de otra plantilla
        /// </summary>
        /// <param name="idTiraje">Identificador del tiraje base</param>
        /// <returns></returns>
        public ObservableCollection<PlantillaDto> GetPlantillaBase(int idTiraje)
        {
            ObservableCollection<PlantillaDto> plantilla = new ObservableCollection<PlantillaDto>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT * FROM AcuerdoPadron WHERE IDAcuNum =@Tiraje", connection);
                cmd.Parameters.AddWithValue("@Tiraje", idTiraje);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlantillaDto integraPlantilla = new PlantillaDto() { IdOrganismo = Convert.ToInt32(reader["IdOrg"]), IdTitular = Convert.ToInt32(reader["IdTitular"]), Funcion = Convert.ToInt32(reader["IdFuncion"]), Particular = Convert.ToInt32(reader["Particular"]), Oficina = Convert.ToInt32(reader["Oficina"]), Biblioteca = Convert.ToInt32(reader["Biblioteca"]), Resguardo = Convert.ToInt32(reader["Resguardo"]), Personal = Convert.ToInt32(reader["Personal"]), Autor = Convert.ToInt32(reader["Autor"]), ObrasRecibe = reader["TipoObra"].ToString() };
                        plantilla.Add(integraPlantilla);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return plantilla;
        }


        /// <summary>
        /// Genera la referencia de la nueva plantilla que se esta generando
        /// </summary>
        /// <param name="nuevaPlantilla"></param>
        /// <param name="nuevoTiraje"></param>
        /// <returns></returns>
        public bool GeneraPlantilla(ObservableCollection<PlantillaDto> nuevaPlantilla, TirajePersonal nuevoTiraje)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                foreach (PlantillaDto nuevo in nuevaPlantilla)
                {

                    string sqlQuery = "INSERT INTO AcuerdoPadron(IdAcuNum,IdOrg,IdTitular,IdFuncion,Particular,Autor,Personal,Oficina,Biblioteca,Resguardo,TipoObra)" +
                                    "VALUES (@IdAcuNum,@IdOrg,@IdTitular,@IdFuncion,@Particular,@Autor,@Personal,@Oficina,@Biblioteca,@Resguardo,@TipoObra)";

                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@IdAcuNum", nuevoTiraje.IdAcuerdo);
                    cmd.Parameters.AddWithValue("@IdOrg", nuevo.IdOrganismo);
                    cmd.Parameters.AddWithValue("@IdTitular", nuevo.IdTitular);
                    cmd.Parameters.AddWithValue("@IdFuncion", nuevo.Funcion);
                    cmd.Parameters.AddWithValue("@Particular", nuevo.Particular);
                    cmd.Parameters.AddWithValue("@Autor", nuevo.Autor);
                    cmd.Parameters.AddWithValue("@Personal", nuevo.Personal);
                    cmd.Parameters.AddWithValue("@Oficina", nuevo.Oficina);
                    cmd.Parameters.AddWithValue("@Biblioteca", nuevo.Biblioteca);
                    cmd.Parameters.AddWithValue("@Resguardo", nuevo.Resguardo);
                    cmd.Parameters.AddWithValue("@TipoObra", nuevo.ObrasRecibe);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }


        /// <summary>
        /// Obtiene el total de ejemplares de distribución asigandos a un acuerdo
        /// </summary>
        /// <param name="acuerdo"></param>
        /// <returns></returns>
        public int GetTotalDistribucionAcuerdo(TirajePersonal acuerdo)
        {
            int total = 0;

            string sqlCadena = "SELECT (SUM(Particular) + SUM(Oficina) + SUM(Biblioteca) + SUM(Personal) + SUM(Autor)) AS Total FROM AcuerdoPadron " +
                               "WHERE IdAcuNum = @Acuerdo AND IdOrg <> 6000 AND IdOrg <> 6001 AND IdOrg <> 6002 AND IdOrg <> 32630";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Acuerdo", acuerdo.IdAcuerdo);
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return total;
        }

        /// <summary>
        /// Obtiene la cantidad de ejemplares asignados en resguardo para el organismo señalado;
        /// </summary>
        /// <param name="acuerdo"></param>
        /// <param name="idOrg"></param>
        /// <returns></returns>
        public int GetTotalResguardoAcuerdo(TirajePersonal acuerdo, int idOrg)
        {
            int total = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT Resguardo FROM AcuerdoPadron WHERE IdAcuNum = @Acuerdo AND IdOrg = @IdOrg", connection);
                cmd.Parameters.AddWithValue("@Acuerdo", acuerdo.IdAcuerdo);
                cmd.Parameters.AddWithValue("@IdOrg", idOrg);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        total = Convert.ToInt32(reader["Resguardo"]);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return total;
        }

    }
}
