using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;

namespace PadronApi.Model
{
    public class AutorModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;



        public ObservableCollection<Autor> GetAutores()
        {
            ObservableCollection<Autor> catalogoTitulares = new ObservableCollection<Autor>();

            const string SqlQuery = "SELECT * FROM C_Titular WHERE Autor = 2 OR Autor = 3 ORDER BY Nombre";

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
                        

                        Autor titular = new Autor()
                        {
                            IdTitular = Convert.ToInt32(reader["IdTitular"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellidos = reader["Apellidos"].ToString(),
                            NombreCompleto = String.Format("{0} {1}",reader["Nombre"],reader["Apellidos"]),
                            NombreStr = reader["NombMay"].ToString(),
                            IdTitulo = Convert.ToInt32(reader["IdTitulo"]),
                            Observaciones = reader["Obs"].ToString(),
                            QuiereDistribucion = reader["QuiereDist"] as int? ?? 0,
                            Correo = reader["Correo"].ToString(),
                            Genero = Convert.ToInt16(reader["Genero"])
                        };


                        catalogoTitulares.Add(titular);
                    }
                }
                cmd.Dispose();
                reader.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName  + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName  + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }


        public ObservableCollection<Autor> GetInstituciones()
        {
            ObservableCollection<Autor> catalogoOrganismo = new ObservableCollection<Autor>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT IdOrg, DescOrg FROM C_Organismo WHERE IdAutor = 2 Order BY DescOrg", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Autor institucion = new Autor()
                        {
                            IdTitular = Convert.ToInt32(reader["IdOrg"]),
                            NombreCompleto = reader["DescOrg"].ToString(),
                            Nombre = reader["DescOrg"].ToString(),
                            Apellidos = reader["DescOrg"].ToString(),
                            IdTitulo = 11
                        };

                        catalogoOrganismo.Add(institucion);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoOrganismo;
        }


        public ObservableCollection<Autor> GetAutores(Obra obra)
        {
            ObservableCollection<Autor> catalogoTitulares = new ObservableCollection<Autor>();

            const string SqlQuery = "SELECT T.IdTitular,T.Nombre,T.Apellidos,R.IdTipoAutor FROM C_Titular T INNER JOIN RelObrasAutores R " + 
                "ON R.IdTitular = T.IdTitular WHERE R.IdObra = @IdObra AND R.IdTitular > 0  ORDER BY IdTipoAutor,T.Nombre";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdObra", obra.IdObra);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Autor autor = catalogoTitulares.SingleOrDefault(n => n.IdTitular == Convert.ToInt32(reader["IdTitular"]));
                        if (autor == null)
                        {

                            Autor titular = new Autor()
                            {
                                IdTitular = Convert.ToInt32(reader["IdTitular"]),
                                IdTitulo = 1,
                                Nombre = reader["Nombre"].ToString(),
                                Apellidos = reader["Apellidos"].ToString(),
                                NombreCompleto = String.Format("{0} {1}", reader["Nombre"], reader["Apellidos"]),
                            };

                            switch (Convert.ToInt32(reader["IdTipoAutor"]))
                            {
                                case 1: titular.IsAutor = true;
                                    break;
                                case 2: titular.IsCompilador = true;
                                    break;
                                case 3: titular.IsTraductor = true;
                                    break;
                                case 4: titular.IsCoordinador = true;
                                    break;
                                case 5: titular.IsComentarista = true;
                                    break;
                                case 6: titular.IsCoedicion = true;
                                    break;
                                case 7: titular.IsEstudio = true;
                                    break;
                                case 8: titular.IsPrologo = true;
                                    break;
                                default:
                                    break;
                            }

                            catalogoTitulares.Add(titular);
                        }
                        else
                        {
                            switch (Convert.ToInt32(reader["IdTipoAutor"]))
                            {
                                case 1: autor.IsAutor = true;
                                    break;
                                case 2: autor.IsCompilador = true;
                                    break;
                                case 3: autor.IsTraductor = true;
                                    break;
                                case 4: autor.IsCoordinador = true;
                                    break;
                                case 5: autor.IsComentarista = true;
                                    break;
                                case 6: autor.IsCoedicion = true;
                                    break;
                                case 7: autor.IsEstudio = true;
                                    break;
                                case 8: autor.IsPrologo = true;
                                    break;
                                default:
                                    break;
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName  + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }


        public ObservableCollection<Autor> GetInstituciones(Obra obra)
        {
            ObservableCollection<Autor> catalogoInstituciones = new ObservableCollection<Autor>();

            const string SqlQuery = "SELECT O.IdOrg, O.DescOrg, R.IdTipoAutor FROM C_Organismo O INNER JOIN RelObrasAutores R ON O.IdOrg = R.IdOrg " +
                "WHERE IdObra = @IdObra AND R.IdOrg > 0 Order BY DescOrg";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdObra", obra.IdObra);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Autor autor = catalogoInstituciones.SingleOrDefault(n => n.IdTitular == Convert.ToInt32(reader["IdOrg"]));

                        if (autor == null)
                        {

                            Autor titular = new Autor()
                            {
                                IdTitular = Convert.ToInt32(reader["IdOrg"]),
                                Nombre= reader["DescOrg"].ToString(),
                                Apellidos = reader["DescOrg"].ToString(),
                                IdTitulo = 1,
                                NombreCompleto = reader["DescOrg"].ToString()
                            };

                            switch (Convert.ToInt32(reader["IdTipoAutor"]))
                            {
                                case 1: titular.IsAutor = true;
                                    break;
                                case 2: titular.IsCompilador = true;
                                    break;
                                case 3: titular.IsTraductor = true;
                                    break;
                                case 4: titular.IsCoordinador = true;
                                    break;
                                case 5: titular.IsComentarista = true;
                                    break;
                                case 6: titular.IsCoedicion = true;
                                    break;
                                case 7: titular.IsEstudio = true;
                                    break;
                                case 8: titular.IsPrologo = true;
                                    break;
                                default:
                                    break;
                            }

                            catalogoInstituciones.Add(titular);
                        }
                        else
                        {
                            switch (Convert.ToInt32(reader["IdTipoAutor"]))
                            {
                                case 1: autor.IsAutor = true;
                                    break;
                                case 2: autor.IsCompilador = true;
                                    break;
                                case 3: autor.IsTraductor = true;
                                    break;
                                case 4: autor.IsCoordinador = true;
                                    break;
                                case 5: autor.IsComentarista = true;
                                    break;
                                case 6: autor.IsCoedicion = true;
                                    break;
                                case 7: autor.IsEstudio = true;
                                    break;
                                case 8: autor.IsPrologo = true;
                                    break;
                                default:
                                    break;
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoInstituciones;
        }

        /// <summary>
        /// Obtiene el Id de las obras que tienen un autor relacionado
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<int> GetObrasConAutor()
        {
            ObservableCollection<int> listadoObras = new ObservableCollection<int>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand("SELECT IdObra FROM RelObrasAutores GROUP BY IdObra", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listadoObras.Add(Convert.ToInt32(reader["IdObra"]));
                    }
                }
                cmd.Dispose();
                reader.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return listadoObras;
        }


        /// <summary>
        /// Indica que un titular es autor de cierta obra sin señalar el tipo de participación que tiene dentro de la misma
        /// </summary>
        /// <param name="idTitular">Identificador del Titular</param>
        /// <param name="idObra">Identificador de la obra</param>
        public void SetAutorObra(int idTitular, int idObra)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                int id = DataBaseUtilities.GetNextIdForUse("RelObrasAutores", "Id", connection);

                connection.Open();

                const string SqlQuery = "INSERT INTO RelObrasAutores(Id, IdObra, IdTitular) VALUES (@Id,@IdObra, @IdTitular)";

                SqlCommand cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdObra", idObra);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IdTitular", idTitular);


                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

        }


        /// <summary>
        /// Permite actualizar la información de los autores de una obra
        /// </summary>
        /// <param name="idObra"></param>
        /// <param name="idTitular"></param>
        /// <param name="idOrganismo"></param>
        /// <param name="idTipoAutor"></param>
        public void SetAutorObra(int idObra, int idTitular, int idOrganismo, int idTipoAutor)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                int id = DataBaseUtilities.GetNextIdForUse("RelObrasAutores", "Id", connection);

                connection.Open();

                const string SqlQuery = "INSERT INTO RelObrasAutores(Id, IdObra, IdTitular,IdOrg,IdTipoAutor) VALUES (@Id,@IdObra, @IdTitular,@IdOrg,@IdTipoAutor)";

                SqlCommand cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdObra", idObra);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IdTitular", idTitular);
                cmd.Parameters.AddWithValue("@IdOrg", idOrganismo);
                cmd.Parameters.AddWithValue("@IdTipoAutor", idTipoAutor);

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

        }

        /// <summary>
        /// Elimina la totalidad de las relaciones autor obra para una obra específica
        /// </summary>
        /// <param name="idObra"></param>
        public void DeleteAutorObra(int idObra)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {

                connection.Open();

                const string SqlQuery = "DELETE FROM RelObrasAutores WHERE IdObra = @IdObra";

                SqlCommand cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdObra", idObra);

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

        }


        public bool InsertaAutor(Titular titular)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            titular.IdTitular = DataBaseUtilities.GetNextIdForUse("C_Titular", "IdTitular", connection);
            titular.QuiereDistribucion = 1;
            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO C_Titular(IdTitular, Nombre,Apellidos,IdTitulo,NombMay,IdUsr,Fecha,Obs,Correo,Genero,Autor,IdEstatus)" +
                                  "VALUES (@IdTitular, @Nombre,@Apellidos,@IdTitulo,@NombMay,@IdUsr,@Fecha,@Obs,@Correo,@Genero,2,1)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);
                cmd.Parameters.AddWithValue("@Nombre", titular.Nombre);
                cmd.Parameters.AddWithValue("@Apellidos", titular.Apellidos);
                cmd.Parameters.AddWithValue("@IdTitulo", titular.IdTitulo);
                cmd.Parameters.AddWithValue("@NombMay", titular.NombreStr);
                cmd.Parameters.AddWithValue("@IdUsr", AccesoUsuario.Llave);
                cmd.Parameters.AddWithValue("@Fecha", DateTimeUtilities.DateToInt(DateTime.Now));
                cmd.Parameters.AddWithValue("@Obs", titular.Observaciones);
                cmd.Parameters.AddWithValue("@Correo", titular.Correo);
                cmd.Parameters.AddWithValue("@Genero", titular.Genero);

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Actualiza los datos del autor seleccionado
        /// </summary>
        /// <param name="titular"></param>
        /// <returns></returns>
        public bool UpdateAutor(Titular titular)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "UPDATE C_Titular SET Nombre = @Nombre,Apellidos = @Apellidos," +
                                  "NombMay = @NombMay,Obs = @Obs, IdTitulo = @IdTitulo, Correo = @Correo, " +
                                  " IdUsr = @IdUsr, Fecha = @Fecha, Genero = @Genero  WHERE IdTitular = @IdTitular";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Nombre", titular.Nombre);
                cmd.Parameters.AddWithValue("@Apellidos", titular.Apellidos);
                cmd.Parameters.AddWithValue("@NombMay", titular.NombreStr);
                cmd.Parameters.AddWithValue("@Obs", titular.Observaciones);
                cmd.Parameters.AddWithValue("@IdTitulo", titular.IdTitulo);
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }


        /// <summary>
        /// Señala como autor al integrante de un órgano jurisdiccional que hay publicado alguna obra y haya sido distribuida por la Coordinación
        /// </summary>
        /// <param name="titular"></param>
        /// <returns></returns>
        public bool SetAsAutor(Titular titular)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE C_Titular SET Autor = 3 WHERE IdTitular = @IdTitular", connection);
                cmd.Parameters.AddWithValue("@IdTitular", titular.IdTitular);

                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }


        public bool UpdateTextoColabora(int idColaboracion, string textoColabora)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool updateCompleted = false;

            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("UPDATE RelObrasAutores SET ArticuloColabora = @ArticuloColabora, ArticuloColaboraStr = @ArticuloColaboraStr WHERE Id = @Id", connection);
                cmd.Parameters.AddWithValue("@ArticuloColabora", textoColabora);
                cmd.Parameters.AddWithValue("@ArticuloColaboraStr", StringUtilities.PrepareToAlphabeticalOrder(textoColabora));
                cmd.Parameters.AddWithValue("@Id", idColaboracion);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                updateCompleted = true;

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return updateCompleted;
        }


    }
}
