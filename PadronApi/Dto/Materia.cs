using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using ScjnUtilities;

namespace PadronApi.Dto
{
    public class Materia
    {

        private int idMateria;
        private bool isChecked;
        private string materiaDesc;
        private string materiaStr;


        public int IdMateria
        {
            get
            {
                return this.idMateria;
            }
            set
            {
                this.idMateria = value;
            }
        }

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;
            }
        }

        public string MateriaDesc
        {
            get
            {
                return this.materiaDesc;
            }
            set
            {
                this.materiaDesc = value;
            }
        }

        public string MateriaStr
        {
            get
            {
                return this.materiaStr;
            }
            set
            {
                this.materiaStr = value;
            }
        }

        public ObservableCollection<Materia> GetMaterias()
        {
            ObservableCollection<Materia> catalogoMaterias = new ObservableCollection<Materia>();

            string sqlCadena = "SELECT * FROM C_Materia WHERE IdTipoMateria = 1 ORDER BY IdMateria";

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Base"].ConnectionString);
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
                        Materia materia = new Materia();
                        materia.IdMateria = Convert.ToInt32(reader["IdMateria"]);
                        materia.isChecked = false;
                        materia.MateriaDesc = reader["Materia"].ToString();

                        catalogoMaterias.Add(materia);

                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,Materia", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,Materia", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoMaterias;
        }

    }
}
