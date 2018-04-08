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
    public class ReportesModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Obtiene el siguiente número de oficio a utilizarse, la numeración se reinicia
        /// anualmente
        /// </summary>
        /// <returns></returns>
        public int GetNextNumOficio()
        {
            int nextOficio = 1;

            const string SqlCadena = "SELECT MAX(OfiFin) + 1 AS Siguiente FROM Padron WHERE AnioAcuerdo = @Anio";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@Anio", DateTime.Now.Year);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nextOficio = reader["Siguiente"] as int? ?? 1;
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return nextOficio;
        }

        /// <summary>
        /// Ingresa la referencia de los números de oficio utilizados para la papelería de esta distribución
        /// </summary>
        /// <param name="idPAdron">Identificador del padrón que se generó</param>
        /// <param name="numInicio">Número inicial para los oficios de esta obra</param>
        /// <param name="numFin">Número final para los oficios de esta obra</param>
        /// <returns></returns>
        public bool UpdateNumerosOficio(int idPAdron, int numInicio, int numFin)
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

                const string SqlQuery = "SELECT * FROM Padron WHERE IdPadron = @IdPadron";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdPadron", idPAdron);
                dataAdapter.Fill(dataSet, "Padron");

                dr = dataSet.Tables["Padron"].Rows[0];
                dr.BeginEdit();
                dr["OfIni"] = numInicio;
                dr["OfiFin"] = numFin;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE Padron SET OfIni = @Inicio, OfiFin = @Fin WHERE IdPadron = @IdPadron";

                dataAdapter.UpdateCommand.Parameters.Add("@Inicio", SqlDbType.Int, 0, "OfIni");
                dataAdapter.UpdateCommand.Parameters.Add("@Fin", SqlDbType.VarChar, 0, "OfiFin");
                dataAdapter.UpdateCommand.Parameters.Add("@IdPadron", SqlDbType.Int, 0, "IdPadron");

                dataAdapter.Update(dataSet, "Padron");

                dataSet.Dispose();
                dataAdapter.Dispose();

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }




        /// <summary>
        /// Obtiene el detalle de un padrón previamente generado, pero solamente de un tipo de distribución
        /// </summary>
        /// <param name="idPadron"></param>
        /// <param name="idAcuerdo"></param>
        /// <param name="parte">Parte que se esta solicitando, Corte, San Lázaro, Área Metropolitana, Foráneos</param>
        /// <returns></returns>
        public ObservableCollection<PlantillaDto> GetDetallesDePadron(int idPadron, int idAcuerdo, int parte)
        {
            ObservableCollection<PlantillaDto> plantilla = new ObservableCollection<PlantillaDto>();

            string sqlCadena = "SELECT A.IdPadron, A.Funcion, A.IdOrg, A.IdTitular, A.Particular, A.Autor, A.Personal, " +
                               "A.Oficina, A.Biblioteca, A.Resguardo, T.Nombre, T.Apellidos, O.DescOrg, O.IdCircuito, O.IdOrdinal,  " +
                               "O.IdCiudad, O.IdEstado, O.IdTpodist, C.Ciudad, E.Estado, Ti.Titulo, Ti.TituloAbr, O.IdTpoOrg,  " +
                               "(SELECT SUM(Cantidad) FROM Devolucion WHERE IdTitular = A.IdTitular AND IdPadron = A.IdPadron) AS Devueltos, Tpo.IdGrupo  " +
                               "FROM C_TipoOrganismo Tpo INNER JOIN ((((((PadronHistorico AS A INNER JOIN C_Titular AS T ON  " +
                               "A.IdTitular = T.IdTitular) INNER JOIN C_Organismo AS O ON A.IdOrg = O.IdOrg) INNER JOIN  " +
                               "C_Ciudad AS C ON O.IdCiudad = C.IdCiudad) INNER JOIN C_Estado AS E ON O.IdEstado = E.IdEstado)  " +
                               "INNER JOIN C_Titulo AS Ti ON T.IdTitulo = Ti.IdTitulo) INNER JOIN C_Funcion AS F ON A.Funcion = F.IdFuncion)  " +
                               "ON Tpo.IdTpoOrg = O.IdTpoOrg WHERE A.IdPadron = @IdPadron AND O.IdTpodist = @Parte ";
 
            if (parte == 1)
                sqlCadena += "ORDER BY O.OrdenVer, O.IdTpodist, O.IdTpoOrg, O.IdCircuito, O.IdOrdinal, A.IdOrg, F.Orden, Ti.Orden";
            else if (parte == 4)
                sqlCadena += "ORDER BY E.Estado,C.Ciudad, O.IdTpoOrg, O.IdOrdinal, A.IdOrg, F.Orden, Ti.Orden;";
            else
                sqlCadena += "ORDER BY O.IdTpoOrg,E.Estado,C.Ciudad,O.Materia, O.IdOrdinal, A.IdOrg, F.Orden, Ti.Orden;";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                cmd.Parameters.AddWithValue("@Parte", parte);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PlantillaDto integrantePlantilla = new PlantillaDto();
                    integrantePlantilla.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                    integrantePlantilla.IdOrganismo = Convert.ToInt32(reader["IdOrg"]);
                    integrantePlantilla.Titulo = reader["TituloAbr"].ToString();
                    integrantePlantilla.Particular = Convert.ToInt32(reader["Particular"]);
                    integrantePlantilla.Oficina = Convert.ToInt32(reader["Oficina"]);
                    integrantePlantilla.Biblioteca = Convert.ToInt32(reader["Biblioteca"]);
                    integrantePlantilla.Resguardo = Convert.ToInt32(reader["Resguardo"]);
                    integrantePlantilla.Personal = Convert.ToInt32(reader["Personal"]);
                    integrantePlantilla.Autor = Convert.ToInt32(reader["Autor"]);
                    integrantePlantilla.Nombre = String.Format("{0} {1} {2}", integrantePlantilla.Titulo, reader["Nombre"], reader["Apellidos"]);
                    integrantePlantilla.Ciudad = Convert.ToInt32(reader["IdCiudad"]);
                    integrantePlantilla.CiudadStr = reader["Ciudad"].ToString();
                    integrantePlantilla.EstadoStr = reader["Estado"].ToString();
                    integrantePlantilla.Estado = Convert.ToInt32(reader["IdEstado"]);
                    integrantePlantilla.Organismo = reader["DescOrg"].ToString();
                    integrantePlantilla.TipoDistribucion = Convert.ToInt32(reader["IdTpoDist"]);
                    integrantePlantilla.Funcion = Convert.ToInt32(reader["Funcion"]);
                    integrantePlantilla.GrupoOrganismo = Convert.ToInt32(reader["IdGrupo"]);

                    plantilla.Add(integrantePlantilla);
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel ", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel ", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return plantilla;
        }


        /// <summary>
        /// Obtiene el número total de secretarios de los órganos jurisdiccionales que reciben obras de la SCJN
        /// </summary>
        /// <param name="idCircuito"></param>
        /// <returns></returns>
        public ObservableCollection<KeyValuePair<string, int>> GetSecretariosByCircuito(int idCircuito)
        {
            ObservableCollection<KeyValuePair<string, int>> secretarios = new ObservableCollection<KeyValuePair<string, int>>();
            const string SqlCadena = "SELECT * FROM vTotalSecretarios WHERE IdCircuito = @Circuito Order By IdTpoOrg";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@Circuito", idCircuito);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        secretarios.Add(new KeyValuePair<string, int>(reader["TpoOrg"].ToString(), Convert.ToInt32(reader["Secretarios"])));
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return secretarios;
        }


        public ObservableCollection<KeyValuePair<string, int>> GetOrganosByCircuito(int idCircuito)
        {
            ObservableCollection<KeyValuePair<string, int>> secretarios = new ObservableCollection<KeyValuePair<string, int>>();
            string sqlCadena = "SELECT C_Organismo.IdTpoOrg, C_TipoOrganismo.TpoOrg, C_Organismo.IdCircuito, Count(C_Organismo.IdTpoOrg) AS Total " +
                               "FROM C_Organismo INNER JOIN C_TipoOrganismo ON C_Organismo.IdTpoOrg = C_TipoOrganismo.IdTpoOrg " +
                               "WHERE C_TipoOrganismo.IdGrupo = 1 and C_Organismo.IdCircuito = @Circuito GROUP BY C_Organismo.IdTpoOrg,  C_TipoOrganismo.TpoOrg, C_Organismo.IdCircuito";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Circuito", idCircuito);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        secretarios.Add(new KeyValuePair<string, int>(reader["TpoOrg"].ToString(), Convert.ToInt32(reader["Total"])));
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return secretarios;
        }

        /// <summary>
        /// Obtiene el catálogo de los órganos jurisdiccionales
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Organismo> GetOrganosJurisdiccionales()
        {
            ObservableCollection<Organismo> jurisdiccionales = new ObservableCollection<Organismo>();
            string sqlCadena = "SELECT * FROM C_TipoOrganismo WHERE IdGrupo = 1";

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
                    while (reader.Read())
                    {
                        jurisdiccionales.Add(new Organismo()
                        {
                            IdOrganismo = Convert.ToInt32(reader["IdTpoOrg"]),
                            OrganismoDesc = reader["TpoOrg"].ToString()
                        });
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return jurisdiccionales;
        }


        public int GetTotalOrganosByTipo(int tipoOrganismo)
        {
            int total = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("select COUNT(IdTpoOrg) Total from C_Organismo where IdTpoOrg = @Tipo", connection);
                cmd.Parameters.AddWithValue("@Tipo", tipoOrganismo);
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return total;
        }


        public int GetTotalFuncionariosByTipoOrg(int tipoOrganismo, int idTitH, int idTitM, int genero)
        {
            int total = 0;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            string sqlQuery = "SELECT COUNT(distinct T.IdTitular) Total " +
                              "FROM AcuerdoPadron A INNER JOIN C_Titular T On T.IdTitular = A.IdTitular " +
                              "INNER JOIN C_Organismo O ON O.IdOrg = A.IdOrg " +
                              "WHERE O.IdTpoOrg = @Tipo AND (T.IdTitulo = @TitH OR T.IdTitulo = @TitM)";
            if(genero != 0)
                sqlQuery += " AND Genero = @Genero";

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Tipo", tipoOrganismo);
                cmd.Parameters.AddWithValue("@TitH", idTitH);
                cmd.Parameters.AddWithValue("@TitM", idTitM);

                if(genero != 0)
                    cmd.Parameters.AddWithValue("@Genero", genero);

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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return total;
        }

        /// <summary>
        /// Obtiene la distribución nacional por estado y de acuerdo al medio de publicación en el que se produjo la obra
        /// </summary>
        /// <param name="year">Año del cual se desea obtener el dato de distribución</param>
        /// <returns></returns>
        public ObservableCollection<TotalPorTipo> GetDistribucionPorTipo(int year)
        {
            ObservableCollection<TotalPorTipo> totalDistribucion = new ObservableCollection<TotalPorTipo>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            string sqlQuery = "Select Estado,[1] TotalCD,[2] TotalDVD,[3] TotalLibro,[4] TotalEbook,[7] TotalLyCD,[8] TotalAudioL FROM " +
                              "(SELECT Estado,IdMedio,Total FROM V_DistTotalPorOrg WHERE AnioAcuerdo = @Year and IdPais = 39 ) AS SourceTable " +
                              "PIVOT " +
                              "( SUM(Total) FOR IdMedio IN ([1],[2],[3],[4],[7],[8] ) ) AS PivotTable;";

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Year", year);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TotalPorTipo distEstado = new TotalPorTipo()
                        {
                            Estado = reader["Estado"].ToString(),
                            Cd = reader["TotalCD"] as int? ??  0,
                            Dvd = reader["TotalDVD"] as int? ?? 0,
                            Libro = reader["TotalLibro"] as int? ?? 0,
                            Ebook = reader["TotalEbook"] as int? ?? 0,
                            Ambos = reader["TotalLyCD"] as int? ?? 0,
                            AudioLibro = reader["TotalAudioL"] as int? ?? 0
                        };

                        totalDistribucion.Add(distEstado);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReportesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return totalDistribucion;
        }



    }
}
