using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using ScjnUtilities;
using System.Collections.Generic;

namespace Kiosko.Dto
{
    public class AutorColabora
    {
        private int idColaboracion;
        private string nombreAutor;
        private string tipoAutor;
        private string textoColabora;
        private string textoColaboraStr;

        public int IdColaboracion
        {
            get
            {
                return this.idColaboracion;
            }
            set
            {
                this.idColaboracion = value;
            }
        }

        public string NombreAutor
        {
            get
            {
                return this.nombreAutor;
            }
            set
            {
                this.nombreAutor = value;
            }
        }

        public string TipoAutor
        {
            get
            {
                return this.tipoAutor;
            }
            set
            {
                this.tipoAutor = value;
            }
        }

        public string TextoColabora
        {
            get
            {
                return this.textoColabora;
            }
            set
            {
                this.textoColabora = value;
            }
        }

        public string TextoColaboraStr
        {
            get
            {
                return this.textoColaboraStr;
            }
            set
            {
                this.textoColaboraStr = value;
            }
        }

        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public List<AutorColabora> GetAutoresForColaboracion(Obra obra)
        {
            List<AutorColabora> autoresObra = new List<AutorColabora>();

            const string SqlQuery = "SELECT R.Id, A.TipoAutor,(T.Nombre + ' ' + T.Apellidos) Nombre, R.ArticuloColabora " +
                                    "FROM RelObrasAutores R INNER JOIN C_TipoAutor A ON R.IdTipoAutor = A.IdTipoAutor " +
                                    "INNER JOIN C_Titular T ON R.IdTitular = T.IdTitular " +
                                    "WHERE R.IdTitular > 0 AND IdObra = @IdObra ORDER BY Nombre";

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
                        AutorColabora colabora = new AutorColabora()
                        {
                            IdColaboracion = Convert.ToInt32(reader["Id"]),
                            NombreAutor = reader["Nombre"].ToString(),
                            TipoAutor = reader["TipoAutor"].ToString(),
                            TextoColabora = reader["ArticuloColabora"].ToString()
                        };

                        autoresObra.Add(colabora);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorColabora", "Kiosko");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorColabora", "Kiosko");
            }
            finally
            {
                connection.Close();
            }

            return autoresObra;
        }

        public List<AutorColabora> GetInstitucionesForColaboracion(Obra obra)
        {
            List<AutorColabora> autoresObra = new List<AutorColabora>();

            const string SqlQuery = "SELECT R.Id,R.IdObra, A.TipoAutor,O.DescOrg, R.ArticuloColabora " +
                                    "FROM RelObrasAutores R INNER JOIN C_TipoAutor A ON R.IdTipoAutor = A.IdTipoAutor " +
                                    "INNER JOIN C_Organismo O ON R.IdOrg = O.IdOrg " +
                                    "WHERE R.IdOrg > 0 and R.IdObra = @IdObra Order by DescOrg";
            
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
                        AutorColabora colabora = new AutorColabora()
                        {
                            idColaboracion = Convert.ToInt32(reader["Id"]),
                            nombreAutor = reader["DescOrg"].ToString(),
                            TipoAutor = reader["TipoAutor"].ToString(),
                            textoColabora = reader["ArticuloColabora"].ToString()
                        };

                        autoresObra.Add(colabora);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorColabora", "Kiosko");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorColabora", "Kiosko");
            }
            finally
            {
                connection.Close();
            }

            return autoresObra;
        }


        public int GetAutoresSinColaboracion(Obra obra)
        {
            int total = 0;
            const string SqlQuery = "SELECT COUNT(IdTipoAutor) Total FROM RelObrasAutores WHERE IdTipoAutor = 0 and IdObra = @IdObra";

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
                        total = Convert.ToInt32(reader["Total"]);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorColabora", "Kiosko");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AutorColabora", "Kiosko");
            }
            finally
            {
                connection.Close();
            }

            return total;
        }
    }
}