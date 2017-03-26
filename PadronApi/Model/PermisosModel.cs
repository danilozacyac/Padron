using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;

namespace PadronApi.Model
{
    public class PermisosModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public ObservableCollection<Permisos> GetPermisosTree(int idPadre)
        {
            ObservableCollection<Permisos> permisos = new ObservableCollection<Permisos>();

            const string SqlQuery = "SELECT * FROM C_Seccion WHERE IdPadre = @IdPadre ORDER BY IdSeccion";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdPadre", idPadre);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Permisos permiso = new Permisos();
                        permiso.IsSelected = false;
                        permiso.IdSeccion = Convert.ToInt32(reader["IdSeccion"]);
                        permiso.IdPadre = Convert.ToInt32(reader["IdPadre"]);
                        permiso.Seccion = reader["Seccion"].ToString();
                        permiso.SeccionesHijo = GetPermisosTree(permiso.IdSeccion);
                        permisos.Add(permiso);

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

            return permisos;
        }


        public void EliminaPermisos(int idUsuario)
        {
            SqlConnection connection = new SqlConnection(connectionString);


            try
            {
                connection.Open();

                const string SqlQuery = "DELETE FROM Permisos WHERE IdUsr = @IdUsr";

                SqlCommand cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdUsr", idUsuario);
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


        public bool InsertaPermisos(int idUsuario,int idSeccion)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                const string SqlQuery = "INSERT INTO Permisos(IdUsr,IdSeccion) VALUES(@IdUsr,@IdSeccion)";

                SqlCommand cmd = new SqlCommand(SqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdUsr", idUsuario);
                cmd.Parameters.AddWithValue("@IdSeccion", idSeccion);

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


        public List<int> GetPermisosByUser(int idusuario)
        {
            List<int> permisos = new List<int>();

            const string SqlCadena = "SELECT * FROM Permisos WHERE IdUsr = @IdUsr ORDER BY IdSeccion";


            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;


            try
            {
                connection.Open();

                cmd = new SqlCommand(SqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdUsr", idusuario);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        permisos.Add(Convert.ToInt32(reader["IdSeccion"]));
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

            return permisos;
        }




        public ObservableCollection<Permisos> GetUsuarios()
        {
            ObservableCollection<Permisos> usuarios = new ObservableCollection<Permisos>();

            const string SqlCadena = "SELECT * FROM C_Usuario ORDER BY IdUsr";

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
                        Permisos usuario = new Permisos() { 
                            IdSeccion = Convert.ToInt32(reader["IdUsr"]), 
                            Seccion = String.Format("{0} {1}", reader["Nombre"], reader["Apellidos"]) };
                        usuarios.Add(usuario);
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

            return usuarios;
        }


    }
}
