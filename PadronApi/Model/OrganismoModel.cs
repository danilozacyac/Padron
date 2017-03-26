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
    public class OrganismoModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Obtiene un listado de los organismos que se encuentran dentro del padrón, el listado puede ser de los
        /// organismos activos o de aquellos que ya fueron desactivados
        /// </summary>
        /// <param name="activos">Indica que el estado de los organismos solicitados</param>
        /// <returns></returns>
        public ObservableCollection<Organismo> GetOrganismos(bool activos)
        {
            ObservableCollection<Organismo> catalogoOrganismo = new ObservableCollection<Organismo>();

            List<int> tienePresidente = this.GetTienenPresidente();

            string sqlCadena = "SELECT O.*, Tpo.TpoOrg as Texto,IdGrupo, D.Distribucion  " +
                               "FROM C_Distribucion D INNER JOIN (C_TipoOrganismo Tpo INNER JOIN C_Organismo O ON Tpo.IdTpoOrg = O.IdTpoOrg)  " +
                               "ON D.IdDistribucion = O.IdTpodist WHERE O.Activo = @Activo ORDER BY DescOrgMay";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Activo", Convert.ToInt16(activos));
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Organismo organismo = new Organismo()
                        {
                            IdOrganismo = Convert.ToInt32(reader["IdOrg"]),
                            OrganismoDesc = reader["DescOrg"].ToString(),
                            OrganismoStr = reader["DescOrgMay"].ToString(),
                            TipoOrganismo = Convert.ToInt32(reader["IdTpoOrg"]),
                            Circuito = Convert.ToInt32(reader["IdCircuito"]),
                            Ordinal = Convert.ToInt32(reader["IdOrdinal"]),
                            Materia = reader["Materia"].ToString(),
                            Ciudad = Convert.ToInt32(reader["IdCiudad"]),
                            Estado = Convert.ToInt32(reader["IdEstado"]),
                            Orden = reader["OrdenVer"] as int? ?? 0,
                            Calle = reader["Calle"].ToString(),
                            Colonia = reader["Colonia"].ToString(),
                            Delegacion = reader["Delegacion"].ToString(),
                            Cp = reader["Cp"].ToString(),
                            Telefono = reader["Tel"].ToString(),
                            Extension = reader["Ext"].ToString(),
                            Telefono2 = reader["Tel1"].ToString(),
                            Extension2 = reader["Ext1"].ToString(),
                            Telefono3 = reader["Tel2"].ToString(),
                            Extension3 = reader["Ext2"].ToString(),
                            Telefono4 = reader["Tel3"].ToString(),
                            Extension4 = reader["Ext3"].ToString(),
                            Observaciones = reader["Obs"].ToString(),
                            Activo = Convert.ToInt32(reader["Activo"]),
                            TipoDistr = Convert.ToInt32(reader["IdTpoDist"]),
                            Abreviado = reader["Abreviado"].ToString(),
                            TipoOrganismoStr = reader["Texto"].ToString(),
                            Distribucion = reader["Distribucion"].ToString(),
                            IdGrupo = Convert.ToInt32(reader["IdGrupo"])
                        };

                        catalogoOrganismo.Add(organismo);
                    }

                    foreach (KeyValuePair<int, int> pair in this.GetAdscritosOrganismo())
                    {
                        try
                        {
                            Organismo org = (from n in catalogoOrganismo
                                           where n.IdOrganismo == pair.Key
                                           select n).ToList()[0];
                            
                            org.TotalAdscritos = pair.Value;

                            if (org.TipoOrganismo == 2 && org.TotalAdscritos > 0 && !tienePresidente.Contains(org.IdOrganismo))
                                org.TotalAdscritos = 10000;
                        }
                        catch (Exception) { }
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoOrganismo;
        }


        /// <summary>
        /// Obtiene la información completa de un organismo a partir de su identificador
        /// </summary>
        /// <param name="idOrganismo">Identificador del organismo</param>
        /// <returns></returns>
        public Organismo GetOrganismos(int idOrganismo)
        {
            Organismo organismo = null;
            string sqlCadena = "SELECT O.*, Tpo.TpoOrgAvr, D.Distribucion  " +
                               "FROM C_Distribucion D INNER JOIN (C_TipoOrganismo Tpo INNER JOIN C_Organismo O ON Tpo.IdTpoOrg = O.IdTpoOrg)  " +
                               "ON D.IdDistribucion = O.IdTpodist WHERE O.IdOrg = @IdOrg ORDER BY OrdenVer";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdOrg", idOrganismo);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        organismo = new Organismo();
                        organismo.IdOrganismo = Convert.ToInt32(reader["IdOrg"]);
                        organismo.OrganismoDesc = reader["DescOrg"].ToString();
                        organismo.OrganismoStr = reader["DescOrgMay"].ToString();
                        organismo.TipoOrganismo = Convert.ToInt32(reader["IdTpoOrg"]);
                        organismo.Circuito = Convert.ToInt32(reader["IdCircuito"]);
                        organismo.Ordinal = Convert.ToInt32(reader["IdOrdinal"]);
                        organismo.Materia = reader["Materia"].ToString();
                        organismo.Ciudad = Convert.ToInt32(reader["IdCiudad"]);
                        organismo.Estado = Convert.ToInt32(reader["IdEstado"]);
                        organismo.Orden = reader["OrdenVer"] as int? ?? 0;
                        organismo.Calle = reader["Calle"].ToString();
                        organismo.Colonia = reader["Colonia"].ToString();
                        organismo.Delegacion = reader["Delegacion"].ToString();
                        organismo.Cp = reader["Cp"].ToString();
                        organismo.Telefono = reader["Tel"].ToString();
                        organismo.Extension = reader["Ext"].ToString();
                        organismo.Telefono2 = reader["Tel1"].ToString();
                        organismo.Extension2 = reader["Ext1"].ToString();
                        organismo.Telefono3 = reader["Tel2"].ToString();
                        organismo.Extension3 = reader["Ext2"].ToString();
                        organismo.Telefono4 = reader["Tel3"].ToString();
                        organismo.Extension4 = reader["Ext3"].ToString();
                        organismo.Observaciones = reader["Obs"].ToString();
                        organismo.Activo = Convert.ToInt32(reader["Activo"]);
                        organismo.TipoDistr = Convert.ToInt32(reader["IdTpoDist"]);
                        organismo.Abreviado = reader["Abreviado"].ToString();
                        organismo.TipoOrganismoStr = reader["TpoOrgAvr"].ToString();
                        organismo.Distribucion = reader["Distribucion"].ToString();
                        organismo.IdUsuario = Convert.ToInt32(reader["IdUsr"]);
                        organismo.Fecha = Convert.ToInt32(reader["Fecha"]);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return organismo;
        }

        /// <summary>
        /// Devuelve el listado de organismos a los que pertenece un titular
        /// </summary>
        /// <param name="titular"></param>
        /// <returns></returns>
        public ObservableCollection<Organismo> GetOrganismos(Titular titular)
        {
            ObservableCollection<Organismo> listaOrganismos = new ObservableCollection<Organismo>();
            string sqlCadena = "SELECT O.IdOrg, O.IdTpoOrg, O.DescOrg,O.IdCircuito, O.IdOrdinal, O.IdCiudad,O.IdEstado,O.IdTpodist " +
                               "FROM AcuerdoPadron A INNER JOIN C_Organismo O ON A.IdOrg =O.IdOrg WHERE IdTitular = @IdTitular  " +
                               "GROUP BY O.IdOrg, O.IdTpoOrg, O.DescOrg,O.IdCircuito, O.IdOrdinal, O.IdCiudad,O.IdEstado,O.IdTpodist";

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
                        Organismo organismo = new Organismo() { 
                            IdOrganismo = Convert.ToInt32(reader["IdOrg"]), 
                            OrganismoDesc = reader["DescOrg"].ToString(), 
                            TipoOrganismo = Convert.ToInt32(reader["IdTpoOrg"]), 
                            Circuito = Convert.ToInt32(reader["IdCircuito"]), 
                            Ordinal = Convert.ToInt32(reader["IdOrdinal"]), 
                            Ciudad = Convert.ToInt32(reader["IdCiudad"]), 
                            Estado = Convert.ToInt32(reader["IdEstado"]), 
                            TipoDistr = Convert.ToInt32(reader["IdTpoDist"])
                        };

                        listaOrganismos.Add(organismo);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return listaOrganismos;
        }


        private ObservableCollection<KeyValuePair<int, int>> GetAdscritosOrganismo()
        {
            ObservableCollection<KeyValuePair<int, int>> asignados = new ObservableCollection<KeyValuePair<int, int>>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT IdOrg,COUNT(IdOrg) AS Ads FROM vTitularOrg GROUP BY IdOrg", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        asignados.Add(new KeyValuePair<int, int>(Convert.ToInt32(reader["IdOrg"]), Convert.ToInt32(reader["Ads"])));
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return asignados;
        }

        /// <summary>
        /// Obtiene el listado de los tribunales a los cuales ya se les asigno un presidente
        /// </summary>
        /// <returns></returns>
        private List<int> GetTienenPresidente()
        {
            List<int> asignados = new List<int>();

            const string SqlCadena = "SELECT IdOrg FROM AcuerdoPadron WHERE IdFuncion = 1 GROUP BY IdOrg";

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
                        asignados.Add(Convert.ToInt32(reader["IdOrg"]));
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return asignados;
        }

        /// <summary>
        /// Obtiene el listado de los organismos activos con los datos básicos para relacionar un 
        /// organismo con un titular
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Organismo> GetOrganismoForAdscripcion()
        {
            ObservableCollection<Organismo> catalogoOrganismo = new ObservableCollection<Organismo>();

            const string SqlCadena = "SELECT IdOrg,DescOrg,DescOrgMay,IdTpoOrg,IdEstado,IdCiudad,IdTpoDist,Calle FROM C_Organismo WHERE Activo = 1 ORDER BY IdOrg";

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
                        Organismo organismo = new Organismo()
                        {
                            IdOrganismo = Convert.ToInt32(reader["IdOrg"]),
                            OrganismoDesc = reader["DescOrg"].ToString(),
                            OrganismoStr = reader["DescOrgMay"].ToString(),
                            TipoOrganismo = Convert.ToInt32(reader["IdTpoOrg"]),
                            Estado = Convert.ToInt32(reader["IdEstado"]),
                            Ciudad = Convert.ToInt32(reader["IdCiudad"]),
                            TipoDistr = Convert.ToInt32(reader["IdTpoDist"]),
                            Calle = reader["Calle"].ToString()
                        };

                        catalogoOrganismo.Add(organismo);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoOrganismo;
        }

        /// <summary>
        /// Verifica la exisencia del organismo que se esta intentando crear
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public bool DoOrganismoExist(string nombre)
        {

            const string SqlCadena = "SELECT * FROM C_Organismo WHERE DescOrgMay = @Nombre";

            int cuantos = 0;
            bool existe = false;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return existe;
        }

        public bool InsertaOrganismo(Organismo organismo)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            organismo.IdOrganismo = DataBaseUtilities.GetNextIdForUse(10, "C_Organismo", "IdOrg", connection);

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Organismo(IdOrg,IdTpoOrg,DescOrg,IdCircuito,IdOrdinal,Materia,IdCiudad,IdEstado,OrdenVer,DescOrgMay,Calle,Colonia,Delegacion,CP,Tel,Tel1,Tel2,Tel3,Ext,Ext1,Ext2,Ext3,IdUsr,Fecha,Obs,Activo,IdTpoDist,Abreviado)" +
                                  "VALUES (@IdOrg,@IdTpoOrg,@DescOrg,@IdCircuito,@IdOrdinal,@Materia,@IdCiudad,@IdEstado,@OrdenVer,@DescOrgMay,@Calle,@Colonia,@Delegacion,@CP,@Tel,@Tel1,@Tel2,@Tel3,@Ext,@Ext1,@Ext2,@Ext3,@IdUsr,@Fecha,@Obs,@Activo,@IdTpoDist,@Abreviado)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdOrg", organismo.IdOrganismo);
                cmd.Parameters.AddWithValue("@IdTpoOrg", organismo.TipoOrganismo);
                cmd.Parameters.AddWithValue("@DescOrg", organismo.OrganismoDesc);
                cmd.Parameters.AddWithValue("@IdCircuito", organismo.Circuito);
                cmd.Parameters.AddWithValue("@IdOrdinal", organismo.Ordinal);
                cmd.Parameters.AddWithValue("@Materia", organismo.Materia);
                cmd.Parameters.AddWithValue("@IdCiudad", organismo.Ciudad);
                cmd.Parameters.AddWithValue("@IdEstado", organismo.Estado);
                cmd.Parameters.AddWithValue("@OrdenVer", organismo.Orden);
                cmd.Parameters.AddWithValue("@DescOrgMay", organismo.OrganismoStr);
                cmd.Parameters.AddWithValue("@Calle", organismo.Calle);
                cmd.Parameters.AddWithValue("@Colonia", organismo.Colonia);
                cmd.Parameters.AddWithValue("@Delegacion", organismo.Delegacion);
                cmd.Parameters.AddWithValue("@CP", organismo.Cp);
                cmd.Parameters.AddWithValue("@Tel", organismo.Telefono);
                cmd.Parameters.AddWithValue("@Tel1", organismo.Telefono2);
                cmd.Parameters.AddWithValue("@Tel2", organismo.Telefono3);
                cmd.Parameters.AddWithValue("@Tel3", organismo.Telefono4);
                cmd.Parameters.AddWithValue("@Ext", organismo.Extension);
                cmd.Parameters.AddWithValue("@Ext1", organismo.Extension2);
                cmd.Parameters.AddWithValue("@Ext2", organismo.Extension3);
                cmd.Parameters.AddWithValue("@Ext3", organismo.Extension4);
                cmd.Parameters.AddWithValue("@IdUsr", AccesoUsuario.Llave);
                cmd.Parameters.AddWithValue("@Fecha", DateTimeUtilities.DateToInt(DateTime.Now));
                cmd.Parameters.AddWithValue("@Obs", organismo.Observaciones);
                cmd.Parameters.AddWithValue("@Activo", organismo.Activo);
                cmd.Parameters.AddWithValue("@IdTpoDist", organismo.TipoDistr);
                cmd.Parameters.AddWithValue("@Abreviado", organismo.Abreviado);
                cmd.ExecuteNonQuery();

                cmd.Dispose();

                if (organismo.Adscripciones != null && organismo.Adscripciones.Count > 0)
                {
                    TitularModel model = new TitularModel();

                    foreach (Adscripcion adscripcion in organismo.Adscripciones)
                        model.EstableceAdscripcion(adscripcion, true);
                }

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        public bool UpdateOrganismo(Organismo organismo)
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

                string sqlQuery = "SELECT * FROM C_Organismo WHERE IdOrg = " + organismo.IdOrganismo;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlQuery, connection);

                dataAdapter.Fill(dataSet, "C_Organismo");

                dr = dataSet.Tables["C_Organismo"].Rows[0];
                dr.BeginEdit();
                dr["IdTpoOrg"] = organismo.TipoOrganismo;
                dr["DescOrg"] = organismo.OrganismoDesc;
                dr["IdCircuito"] = organismo.Circuito;
                dr["IdOrdinal"] = organismo.Ordinal;
                dr["Materia"] = organismo.Materia;
                dr["IdCiudad"] = organismo.Ciudad;
                dr["IdEstado"] = organismo.Estado;
                dr["OrdenVer"] = organismo.Orden;
                dr["DescOrgMay"] = StringUtilities.PrepareToAlphabeticalOrder(organismo.OrganismoDesc);
                dr["Calle"] = organismo.Calle;
                dr["Colonia"] = organismo.Colonia;
                dr["Delegacion"] = organismo.Delegacion;
                dr["CP"] = organismo.Cp;
                dr["Tel"] = organismo.Telefono;
                dr["Tel1"] = organismo.Telefono2;
                dr["Tel2"] = organismo.Telefono3;
                dr["Tel3"] = organismo.Telefono4;
                dr["Ext"] = organismo.Extension;
                dr["Ext1"] = organismo.Extension2;
                dr["Ext2"] = organismo.Extension3;
                dr["Ext3"] = organismo.Extension4;
                dr["IdUsr"] = AccesoUsuario.Llave;
                dr["Fecha"] = DateTimeUtilities.DateToInt(DateTime.Now);
                dr["Obs"] = organismo.Observaciones;
                dr["Activo"] = organismo.Activo;
                dr["IdTpoDist"] = organismo.TipoDistr;
                dr["Abreviado"] = organismo.Abreviado;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE C_Organismo SET IdTpoOrg = @IdTpoOrg, DescOrg = @DescOrg, IdCircuito = @IdCircuito,IdOrdinal = @IdOrdinal, Materia = @Materia,IdCiudad = @IdCiudad, IdEstado = @IdEstado, OrdenVer = @OrdenVer, DescOrgMay = @DescOrgMay,Calle = @Calle, Colonia = @Colonia," +
                    "Delegacion = @Delegacion,CP = @CP,Tel = @Tel, Tel1 = @Tel1, Tel2 = @Tel2, Tel3 = @Tel3,Ext = @Ext, Ext1 = @Ext1, Ext2 = @Ext2,Ext3 = @Ext3, IdUsr = @IdUsr, Fecha = @Fecha,Obs = @Obs, Activo = @Activo, IdTpoDist = @IdTpoDist, Abreviado = @Abreviado WHERE IdOrg = @IdOrg";

                dataAdapter.UpdateCommand.Parameters.Add("@IdTpoOrg", SqlDbType.Int, 0, "IdTpoOrg");
                dataAdapter.UpdateCommand.Parameters.Add("@DescOrg", SqlDbType.VarChar, 0, "DescOrg");
                dataAdapter.UpdateCommand.Parameters.Add("@IdCircuito", SqlDbType.Int, 0, "IdCircuito");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrdinal", SqlDbType.Int, 0, "IdOrdinal");
                dataAdapter.UpdateCommand.Parameters.Add("@Materia", SqlDbType.Int, 0, "Materia");
                dataAdapter.UpdateCommand.Parameters.Add("@IdCiudad", SqlDbType.Int, 0, "IdCiudad");
                dataAdapter.UpdateCommand.Parameters.Add("@IdEstado", SqlDbType.Int, 0, "IdEstado");
                dataAdapter.UpdateCommand.Parameters.Add("@OrdenVer", SqlDbType.Int, 0, "OrdenVer");
                dataAdapter.UpdateCommand.Parameters.Add("@DescOrgMay", SqlDbType.VarChar, 0, "DescOrgMay");
                dataAdapter.UpdateCommand.Parameters.Add("@Calle", SqlDbType.VarChar, 0, "Calle");
                dataAdapter.UpdateCommand.Parameters.Add("@Colonia", SqlDbType.VarChar, 0, "Colonia");
                dataAdapter.UpdateCommand.Parameters.Add("@Delegacion", SqlDbType.VarChar, 0, "Delegacion");
                dataAdapter.UpdateCommand.Parameters.Add("@CP", SqlDbType.VarChar, 0, "CP");
                dataAdapter.UpdateCommand.Parameters.Add("@Tel", SqlDbType.VarChar, 0, "Tel");
                dataAdapter.UpdateCommand.Parameters.Add("@Tel1", SqlDbType.VarChar, 0, "Tel1");
                dataAdapter.UpdateCommand.Parameters.Add("@Tel2", SqlDbType.VarChar, 0, "Tel2");
                dataAdapter.UpdateCommand.Parameters.Add("@Tel3", SqlDbType.VarChar, 0, "Tel3");
                dataAdapter.UpdateCommand.Parameters.Add("@Ext", SqlDbType.VarChar, 0, "Ext");
                dataAdapter.UpdateCommand.Parameters.Add("@Ext1", SqlDbType.VarChar, 0, "Ext1");
                dataAdapter.UpdateCommand.Parameters.Add("@Ext2", SqlDbType.VarChar, 0, "Ext2");
                dataAdapter.UpdateCommand.Parameters.Add("@Ext3", SqlDbType.VarChar, 0, "Ext3");
                dataAdapter.UpdateCommand.Parameters.Add("@IdUsr", SqlDbType.Int, 0, "IdUsr");
                dataAdapter.UpdateCommand.Parameters.Add("@Fecha", SqlDbType.Int, 0, "Fecha");
                dataAdapter.UpdateCommand.Parameters.Add("@Obs", SqlDbType.VarChar, 0, "Obs");
                dataAdapter.UpdateCommand.Parameters.Add("@Activo", SqlDbType.Int, 0, "Activo");
                dataAdapter.UpdateCommand.Parameters.Add("@IdTpoDist", SqlDbType.Int, 0, "IdTpoDist");
                dataAdapter.UpdateCommand.Parameters.Add("@Abreviado", SqlDbType.VarChar, 0, "Abreviado");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");

                dataAdapter.Update(dataSet, "C_Organismo");

                dataSet.Dispose();
                dataAdapter.Dispose();

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Desactiva el organismos seleccionado y además elimina la relación existente con los integrantes
        /// que estaban relacionados al mismo. También modifica las plantillas de distribución
        /// </summary>
        /// <param name="organismo"></param>
        /// <returns></returns>
        public bool DesactivaOrganismo(Organismo organismo)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool updatedCompleted = false;

            TitularModel modelTit = new TitularModel();

            if (organismo.Adscripciones != null)
            {
                foreach (Adscripcion adscripciom in organismo.Adscripciones)
                    modelTit.EliminaAdscripcion(adscripciom, true);
            }

            updatedCompleted = this.EstadoOrganismo(organismo, 0);

            return updatedCompleted;
        }

        /// <summary>
        /// Modifica el estado actual del organismo
        /// </summary>
        /// <param name="organismo"></param>
        /// <returns></returns>
        public bool EstadoOrganismo(Organismo organismo, int estado)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool complete = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                const string SqlQuery = "SELECT * FROM C_Organismo WHERE IdOrg = @IdOrg";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdOrg", organismo.IdOrganismo);
                dataAdapter.Fill(dataSet, "C_Organismo");

                dr = dataSet.Tables["C_Organismo"].Rows[0];
                dr.BeginEdit();
                dr["Activo"] = estado;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE C_Organismo SET Activo = @Activo WHERE IdOrg = @IdOrg";
                dataAdapter.UpdateCommand.Parameters.Add("@Activo", SqlDbType.Int, 0, "Activo");
                dataAdapter.UpdateCommand.Parameters.Add("@IdOrg", SqlDbType.Int, 0, "IdOrg");
                dataAdapter.Update(dataSet, "C_Organismo");

                dataSet.Dispose();
                dataAdapter.Dispose();

                complete = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
            return complete;
        }

        public bool InsertaHistorial(Organismo organismo)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO OrganismoHistorico(IdOrg,IdTpoOrg,DescOrg,IdCircuito,IdOrdinal,Materia,IdCiudad,IdEstado,DescOrgMay,Calle,Colonia,Delegacion,CP,IdUsr,Fecha,IdTpoDist,Activo)" +
                                  "VALUES (@IdOrg,@IdTpoOrg,@DescOrg,@IdCircuito,@IdOrdinal,@Materia,@IdCiudad,@IdEstado,@DescOrgMay,@Calle,@Colonia,@Delegacion,@CP,@IdUsr,@Fecha,@IdTpoDist,@Activo)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdOrg", organismo.IdOrganismo);
                cmd.Parameters.AddWithValue("@IdTpoOrg", organismo.TipoOrganismo);
                cmd.Parameters.AddWithValue("@DescOrg", organismo.OrganismoDesc);
                cmd.Parameters.AddWithValue("@IdCircuito", organismo.Circuito);
                cmd.Parameters.AddWithValue("@IdOrdinal", organismo.Ordinal);
                cmd.Parameters.AddWithValue("@Materia", organismo.Materia);
                cmd.Parameters.AddWithValue("@IdCiudad", organismo.Ciudad);
                cmd.Parameters.AddWithValue("@IdEstado", organismo.Estado);
                cmd.Parameters.AddWithValue("@DescOrgMay", StringUtilities.PrepareToAlphabeticalOrder(organismo.OrganismoDesc));
                cmd.Parameters.AddWithValue("@Calle", organismo.Calle);
                cmd.Parameters.AddWithValue("@Colonia", organismo.Colonia);
                cmd.Parameters.AddWithValue("@Delegacion", organismo.Delegacion);
                cmd.Parameters.AddWithValue("@CP", organismo.Cp);
                cmd.Parameters.AddWithValue("@IdUsr", AccesoUsuario.Llave);
                cmd.Parameters.AddWithValue("@Fecha", DateTimeUtilities.DateToInt(DateTime.Now));
                cmd.Parameters.AddWithValue("@IdTpoDist", organismo.TipoDistr);
                cmd.Parameters.AddWithValue("@Activo", organismo.Activo);

                cmd.ExecuteNonQuery();

                cmd.Dispose();

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,OrganismoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }



        

    }
}