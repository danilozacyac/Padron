using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;

namespace PadronApi.Model
{
    public class PlantillaModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;



        #region Acuses

        /// <summary>
        /// Inserta el registro de los oficios que fueron enviados a cada titular para cada una de las obras distribuidas
        /// </summary>
        /// <param name="plantilla"></param>
        /// <param name="numOficio">Número de oficio que se envio</param>
        /// <returns></returns>
        public bool RegistroOficiostitular(PlantillaDto plantilla, int numOficio)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO Acuses(IdPadron,IdTitular, IdOrg,NumOficio)" +
                                  "VALUES (@IdPadron,@IdTitular, @IdOrg,@NumOficio)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdPadron", plantilla.IdPadron);
                cmd.Parameters.AddWithValue("@IdTitular", plantilla.IdTitular);
                cmd.Parameters.AddWithValue("@IdOrg", plantilla.IdOrganismo);
                cmd.Parameters.AddWithValue("@NumOficio", numOficio);
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

        #endregion


      


    }
}
