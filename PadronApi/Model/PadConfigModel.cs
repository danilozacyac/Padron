using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;
using System.Configuration;
using System.Collections.Generic;

namespace PadronApi.Model
{
    public class PadConfigModel
    {

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public void GetConfiguraciones()
        {

            string sqlCadena = "SELECT * FROM C_Config ORDER BY idConfig";

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
                        if (Convert.ToInt16(reader[0]) == 1)
                            PadConfiguracion.Titular = reader[2].ToString();
                        if (Convert.ToInt16(reader[0]) == 2)
                            PadConfiguracion.Rubricas = reader[2].ToString();
                        if (Convert.ToInt16(reader[0]) == 3)
                            PadConfiguracion.LeyendaOficio = reader[2].ToString();
                        if (Convert.ToInt16(reader[0]) == 4)
                            PadConfiguracion.NumOficio = reader[2].ToString();
                        if (Convert.ToInt16(reader[0]) == 5)
                            PadConfiguracion.TxtAclaraciones = reader[2].ToString();
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetObras", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

        }

        public void UpdateConfiguraciones()
        {
            SqlConnection connection = new SqlConnection(connectionString);


            List<string> valores = new List<string>() { PadConfiguracion.Titular, PadConfiguracion.Rubricas, PadConfiguracion.LeyendaOficio, PadConfiguracion.NumOficio, PadConfiguracion.TxtAclaraciones };

            try
            {
                connection.Open();

                for (int index = 1; index <= 5; index++)
                {

                    SqlCommand cmd = new SqlCommand("UPDATE C_Config SET Valor = @Valor WHERE IdConfig = @IdConfig", connection);
                    cmd.Parameters.AddWithValue("@Valor", valores[index - 1]);
                    cmd.Parameters.AddWithValue("@IdConfig", index);

                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    
                }
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

    }
}
