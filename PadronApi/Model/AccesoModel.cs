using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;

namespace PadronApi.Model
{
    public class AccesoModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public bool ObtenerUsuarioContraseña(string sUsuario, string sPwd)
        {
            bool bExisteUsuario = false;
            string sSql;

            SqlCommand cmd;
            SqlDataReader reader;
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                sSql = "SELECT * FROM C_Usuario WHERE Usr = @Usr AND Pws = @Pwd";
                cmd = new SqlCommand(sSql, connection);
                cmd.Parameters.AddWithValue("@Usr", sUsuario);
                cmd.Parameters.AddWithValue("@Pwd", sPwd);
                reader = cmd.ExecuteReader();

                AccesoUsuario.Llave = -1;

                while (reader.Read())
                {
                    AccesoUsuario.Usuario = reader["Usr"].ToString();
                    AccesoUsuario.Llave = Convert.ToInt16(reader["IdUsr"].ToString());
                    AccesoUsuario.Nombre = reader["nombre"].ToString();
                    bExisteUsuario = true;
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AccesoModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AccesoModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return bExisteUsuario;
        }
    }
}
