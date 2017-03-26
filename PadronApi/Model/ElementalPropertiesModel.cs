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
    public class ElementalPropertiesModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        #region Titulares
        public ObservableCollection<Titulo> GetTitulos()
        {
            ObservableCollection<Titulo> catalogoTitulos = new ObservableCollection<Titulo>();

            const string SqlQuery = "SELECT * FROM C_Titulo ORDER BY Titulo";

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
                        Titulo titulo = new Titulo() { 
                            IdTitulo = Convert.ToInt32(reader["IdTitulo"]), 
                            TituloDesc = reader["Titulo"].ToString(), 
                            TituloAbr = reader["TituloAbr"].ToString(), Orden = Convert.ToInt32(reader["Orden"]), IdGenero = Convert.ToInt32(reader["IdGenero"]) };

                        catalogoTitulos.Add(titulo);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulos;
        }



        public ObservableCollection<ElementalProperties> GetFunciones()
        {
            ObservableCollection<ElementalProperties> catalogoTitulos = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_Funcion ORDER BY Funcion";


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
                        ElementalProperties elemento = new ElementalProperties() { 
                            IdElemento = Convert.ToInt32(reader["IdFuncion"]), 
                            Descripcion = reader["Funcion"].ToString(), 
                            ElementoAuxiliar = reader["Orden"].ToString() 
                        };

                        catalogoTitulos.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulos;
        }


        public bool SetFuncion(string funcion)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            int idFuncion = DataBaseUtilities.GetNextIdForUse("C_Funcion", "IdFuncion", connection);
            int orden = DataBaseUtilities.GetNextIdForUse("C_Funcion", "Orden", connection);

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Funcion(IdFuncion,Funcion,Orden)" +
                                "VALUES (@IdFuncion,@Funcion,@Orden)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdFuncion", idFuncion);
                cmd.Parameters.AddWithValue("@Funcion", funcion);
                cmd.Parameters.AddWithValue("@Orden", orden);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Permite actualizar la descripción de una función
        /// </summary>
        /// <param name="funcion"></param>
        /// <returns></returns>
        public bool SetFuncion(ElementalProperties funcion)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;


            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE C_Funcion SET Funcion = @Funcion WHERE IdFuncion = @IdFuncion", connection);
                
                cmd.Parameters.AddWithValue("@Funcion", funcion.Descripcion);
                cmd.Parameters.AddWithValue("@IdFuncion", funcion.IdElemento);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }



        /// <summary>
        /// Actualiza el orden en que deben ser mostradas las funciones existentes
        /// </summary>
        /// <param name="property">Funcion cuyo orden será actualizado</param>
        public void UpdateFuncionesOrder(ElementalProperties property)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                const string SqlQuery = "SELECT * FROM C_Funcion WHERE IdFuncion = @IdFuncion";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(SqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdFuncion", property.IdElemento);
                dataAdapter.Fill(dataSet, "C_Funcion");

                dr = dataSet.Tables["C_Funcion"].Rows[0];
                dr.BeginEdit();
                dr["Orden"] = property.ElementoAuxiliar;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                dataAdapter.UpdateCommand.CommandText = "UPDATE C_Funcion SET Orden = @Orden WHERE IdFuncion = @IdFuncion";

                dataAdapter.UpdateCommand.Parameters.Add("@Orden", SqlDbType.Int, 0, "Orden");
                dataAdapter.UpdateCommand.Parameters.Add("@IdFuncion", SqlDbType.Int, 0, "IdFuncion");

                dataAdapter.Update(dataSet, "C_Funcion");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

        }


        public ObservableCollection<ElementalProperties> GetEstatus()
        {
            ObservableCollection<ElementalProperties> catalogoTitulos = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_Estatus ORDER BY IdEstatus";


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
                        ElementalProperties elemento = new ElementalProperties() { 
                            IdElemento = Convert.ToInt32(reader["IdEstatus"]), 
                            Descripcion = reader["Estatus"].ToString()
                        };

                        catalogoTitulos.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulos;
        }


        #endregion

        #region Obras

        public ObservableCollection<ElementalProperties> GetPresentacion()
        {
            ObservableCollection<ElementalProperties> catalogoPresentacion = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_Presentacion ORDER BY IdPresentacion";


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
                        ElementalProperties elemento = new ElementalProperties() { 
                            IdElemento = Convert.ToInt32(reader["IdPresentacion"]), 
                            Descripcion = reader["Presentacion"].ToString() 
                        };

                        catalogoPresentacion.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoPresentacion;
        }


        public ObservableCollection<ElementalProperties> GetTipoObra()
        {
            ObservableCollection<ElementalProperties> catalogoTipoObra = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_TipoObra ORDER BY IdTipoObra";


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
                        ElementalProperties elemento = new ElementalProperties()
                        {
                            IdElemento = Convert.ToInt32(reader["IdTipoObra"]),
                            Descripcion = reader["TipoObra"].ToString()
                        };

                        catalogoTipoObra.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTipoObra;
        }

        public ObservableCollection<ElementalProperties> GetTipoObra(int idPresentacion)
        {
            ObservableCollection<ElementalProperties> catalogoTipoObra = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_TipoObra WHERE IdPresentacion = @IdPresentacion ORDER BY IdTipoObra";


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;


            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdPresentacion", idPresentacion);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ElementalProperties elemento = new ElementalProperties() { 
                            IdElemento = Convert.ToInt32(reader["IdTipoObra"]), 
                            Descripcion = reader["TipoObra"].ToString() 
                        };

                        catalogoTipoObra.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTipoObra;
        }


        public ObservableCollection<ElementalProperties> GetMedioPublicacion()
        {
            ObservableCollection<ElementalProperties> catalogoMediosPublicacion = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_MedioPublicacion ORDER BY IdMedio";


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
                        ElementalProperties elemento = new ElementalProperties()
                        {
                            IdElemento = Convert.ToInt32(reader["IdMedio"]),
                            Descripcion = reader["Medio"].ToString()
                        };

                        catalogoMediosPublicacion.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoMediosPublicacion;
        }

        public ObservableCollection<ElementalProperties> GetTipoPublicacion()
        {
            ObservableCollection<ElementalProperties> catalogoTipoPublicacion = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_TipoPublicacion ORDER BY IdTipoPub";


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
                        ElementalProperties elemento = new ElementalProperties()
                        {
                            IdElemento = Convert.ToInt32(reader["IdTipoPub"]),
                            Descripcion = reader["TipoPub"].ToString()
                        };

                        catalogoTipoPublicacion.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTipoPublicacion;
        }

        public ObservableCollection<ElementalProperties> GetIdioma()
        {
            ObservableCollection<ElementalProperties> catalogoTipoPublicacion = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_IdiomaPublicacion ORDER BY IdIdioma";


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
                        ElementalProperties elemento = new ElementalProperties()
                        {
                            IdElemento = Convert.ToInt32(reader["IdIdioma"]),
                            Descripcion = reader["Idioma"].ToString()
                        };

                        catalogoTipoPublicacion.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTipoPublicacion;
        }


        #endregion


        #region Organismos

        public ObservableCollection<ElementalProperties> GetTiposDistribucion()
        {
            ObservableCollection<ElementalProperties> catalogoDistribucion = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_Distribucion ORDER BY IdDistribucion";


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
                        ElementalProperties elemento = new ElementalProperties() { 
                            IdElemento = Convert.ToInt32(reader["IdDistribucion"]), 
                            Descripcion = reader["Distribucion"].ToString() 
                        };

                        catalogoDistribucion.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoDistribucion;
        }


        public ObservableCollection<TipoOrganismo> GetTipoOrganismo()
        {
            ObservableCollection<TipoOrganismo> catalogoTipoObras = new ObservableCollection<TipoOrganismo>();

            const string SqlQuery = "SELECT * FROM C_TipoOrganismo ORDER BY IdTpoOrg";


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
                        TipoOrganismo tipo = new TipoOrganismo() { 
                            IdTipoOrganismo = Convert.ToInt32(reader["IdTpoOrg"]), 
                            Descripcion = reader["TpoOrg"].ToString(), 
                            DescripcionAbr = reader["TpoOrgAvr"].ToString(), 
                            IdGrupo = Convert.ToInt32(reader["IdGrupo"]), 
                            Orden = Convert.ToInt32(reader["Orden"]) 
                        };

                        catalogoTipoObras.Add(tipo);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTipoObras;
        }


        public ObservableCollection<Ordinales> GetOrdinales()
        {
            ObservableCollection<Ordinales> catalogoOrdinales = new ObservableCollection<Ordinales>();

            const string SqlQuery = "SELECT * FROM C_Ordinal ORDER BY IdOrdinal";


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
                        Ordinales elemento = new Ordinales() { 
                            IdOrdinal = Convert.ToInt32(reader["IdOrdinal"]), 
                            Ordinal = reader["Ordinal"].ToString() 
                        };

                        catalogoOrdinales.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoOrdinales;
        }

        public ObservableCollection<Ordinales> GetCircuitos()
        {
            ObservableCollection<Ordinales> catalogoCircuitos = new ObservableCollection<Ordinales>();

            const string SqlQuery = "SELECT * FROM C_Ordinal ORDER BY IdOrdinal";


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
                        Ordinales elemento = new Ordinales();
                        elemento.IdOrdinal = Convert.ToInt32(reader["IdOrdinal"]);
                        elemento.Ordinal = (elemento.IdOrdinal > 0) ? reader["Ordinal"] + " Circuito" : String.Empty;
                        elemento.IdEstado = Convert.ToInt32(reader["IdEstado"]);

                        catalogoCircuitos.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoCircuitos;
        }

        #endregion


        #region Kiosko


        public ObservableCollection<ElementalProperties> GetTipoAutor()
        {
            ObservableCollection<ElementalProperties> catalogoDistribucion = new ObservableCollection<ElementalProperties>();

            const string SqlQuery = "SELECT * FROM C_TipoAutor ORDER BY IdTipoAutor";


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
                        ElementalProperties elemento = new ElementalProperties()
                        {
                            IdElemento = Convert.ToInt32(reader["IdTipoAutor"]),
                            Descripcion = reader["TipoAutor"].ToString()
                        };

                        catalogoDistribucion.Add(elemento);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ElementalPropertiesModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoDistribucion;
        }


        #endregion

    }
}
