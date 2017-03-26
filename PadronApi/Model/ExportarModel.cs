using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Converter;
using PadronApi.Dto;
using ScjnUtilities;

namespace PadronApi.Model
{
    public class ExportarModel
    {

        //private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;
        private readonly string connectionCatalogo = ConfigurationManager.ConnectionStrings["Catalogo"].ConnectionString;



        public bool DeleteCurrentInfo()
        {
            bool deleteCompleted = false;
            const string SqlCadena = "DELETE FROM CatObras WHERE Id <> 1 AND Id <>2 AND Id <> 3 AND ID <> 425 AND Id <> 3043";

            OleDbConnection connection = new OleDbConnection(connectionCatalogo);
            OleDbCommand cmd = null;

            try
            {
                connection.Open();

                cmd = new OleDbCommand(SqlCadena, connection);
                cmd.ExecuteNonQuery();

                cmd = new OleDbCommand("DELETE FROM Autores", connection);
                cmd.ExecuteNonQuery();

                cmd = new OleDbCommand("DELETE FROM RelObrasAutores", connection);
                cmd.ExecuteNonQuery();

                cmd = new OleDbCommand("DELETE FROM Idioma_Publicacion", connection);
                cmd.ExecuteNonQuery();

                cmd = new OleDbCommand("DELETE FROM Pais_Publicacion", connection);
                cmd.ExecuteNonQuery();


                cmd.Dispose();
                deleteCompleted = true;
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AcuerdosModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return deleteCompleted;
        }


        public bool InsertaObra(Obra obra)
        {
            OleDbConnection connection = new OleDbConnection(connectionCatalogo);

            bool insertCompleted = false;


            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO CatObras(Id,Orden,Autor,Titulo,TituloTxt,Sintesis,SintesisTxt,NumeroMaterial,AnioPublicacion,Edicion,Isbn,Pais,Idioma,Paginas,Clasificacion,TipoPublicacion,MedioPublicacion,Materia,Descripcion,DescMay,Consec,Precio,ArchivoImagen,ConMay,Nivel,Padre,IdUsr,FechaUsr,Agotado,Activo,HabilitaCatalogo)" +
                                "VALUES (@Id,@Orden,@Autor,@Titulo,@TituloTxt,@Sintesis,@SintesisTxt,@NumeroMaterial,@AnioPublicacion,@Edicion,@Isbn,@Pais,@Idioma,@Paginas,@Clasificacion,@TipoPublicacion,@MedioPublicacion,@Materia,@Descripcion,@DescMay,@Consec,@Precio,@ArchivoImagen,@ConMay,@Nivel,@Padre,@IdUsr,@FechaUsr,@Agotado,@Activo,@HabilitaCatalogo)";

                OleDbCommand cmd = new OleDbCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Id", obra.IdObra);
                cmd.Parameters.AddWithValue("@Orden", obra.Orden);
                cmd.Parameters.AddWithValue("@Autor", String.Empty);
                cmd.Parameters.AddWithValue("@Titulo", obra.Titulo);
                cmd.Parameters.AddWithValue("@TituloTxt", obra.TituloStr);
                cmd.Parameters.AddWithValue("@Sintesis", obra.Sintesis);
                cmd.Parameters.AddWithValue("@SintesisTxt", obra.SintesisStr);
                cmd.Parameters.AddWithValue("@NumeroMaterial", obra.NumMaterial);
                cmd.Parameters.AddWithValue("@AnioPublicacion", obra.AnioPublicacion);
                cmd.Parameters.AddWithValue("@Edicion", obra.Edicion);
                cmd.Parameters.AddWithValue("@Isbn", obra.Isbn);
                cmd.Parameters.AddWithValue("@Pais", obra.Pais);
                cmd.Parameters.AddWithValue("@Idioma", obra.IdIdioma);
                cmd.Parameters.AddWithValue("@Paginas", obra.Paginas);
                cmd.Parameters.AddWithValue("@Clasificacion", DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoPublicacion", obra.IdTipoPublicacion);
                cmd.Parameters.AddWithValue("@MedioPublicacion", obra.MedioPublicacion);
                cmd.Parameters.AddWithValue("@Materia", obra.Materia);
                cmd.Parameters.AddWithValue("@Descripcion", (obra.Descripcion == null) ? obra.Titulo : obra.Descripcion);
                cmd.Parameters.AddWithValue("@DescMay", (obra.Descripcion == null) ? obra.TituloStr : obra.DescripcionStr);
                cmd.Parameters.AddWithValue("@Consec", obra.Consecutivo);
                cmd.Parameters.AddWithValue("@Precio", obra.Precio);
                cmd.Parameters.AddWithValue("@ArchivoImagen", obra.ImagePath);
                cmd.Parameters.AddWithValue("@ConMay", obra.ConMay);
                cmd.Parameters.AddWithValue("@Nivel", obra.Nivel);
                cmd.Parameters.AddWithValue("@Padre", obra.Padre);
                cmd.Parameters.AddWithValue("@IdUsr", 0);
                cmd.Parameters.AddWithValue("@FechaUsr", DateTimeUtilities.DateToInt(DateTime.Now));
                cmd.Parameters.AddWithValue("@Agotado", obra.Agotado);
                cmd.Parameters.AddWithValue("@Activo", obra.Activo);
                cmd.Parameters.AddWithValue("@HabilitaCatalogo", Convert.ToInt16(obra.MuestraEnKiosko));
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }


        public bool InsertaAutor(Autor autor)
        {
            OleDbConnection connection = new OleDbConnection(connectionCatalogo);

            bool insertCompleted = false;


            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO Autores(Id,[Desc],DescMay,GradoAcademico)" +
                                "VALUES (@Id,@Desc,@DescMay,@GradoAcademico)";

                OleDbCommand cmd = new OleDbCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Id", autor.IdTitular);
                cmd.Parameters.AddWithValue("@Desc", autor.NombreCompleto);
                cmd.Parameters.AddWithValue("@DescMay", autor.NombreStr);
                cmd.Parameters.AddWithValue("@GradoAcademico", new TituloConverter().Convert(autor.IdTitulo,null,null,null));
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                autor.Obras = new ObraModel().GetObras(autor.IdTitular, "IdTitular");

                foreach (Obra obra in autor.Obras)
                    this.InsertaRelObraAutor(obra.IdObra, autor.IdTitular, 1);

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }


        public bool InsertaOrganismoAutor(Autor autor)
        {
            OleDbConnection connection = new OleDbConnection(connectionCatalogo);

            bool insertCompleted = false;


            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO Autores(Id,[Desc],DescMay)" +
                                "VALUES (@Id,@Desc,@DescMay)";

                OleDbCommand cmd = new OleDbCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Id", autor.IdTitular + 1000000);
                cmd.Parameters.AddWithValue("@Desc", autor.NombreCompleto);
                cmd.Parameters.AddWithValue("@DescMay", StringUtilities.PrepareToAlphabeticalOrder(autor.NombreCompleto));
                cmd.ExecuteNonQuery();

                cmd.Dispose();

                autor.Obras = new ObraModel().GetObras(autor.IdTitular, "IdOrg");

                foreach (Obra obra in autor.Obras)
                    this.InsertaRelObraAutor(obra.IdObra, autor.IdTitular + 1000000, 1);

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }


        public bool InsertaRelObraAutor(int idObra, int idAutor,int idtipoAutor)
        {
            OleDbConnection connection = new OleDbConnection(connectionCatalogo);

            bool insertCompleted = false;

            

            try
            {
                int newId = DataBaseUtilities.GetNextIdForUse("RelObrasAutores", "Id", connection);

                if (newId == 0)
                    newId = 1;

                connection.Open();

                string sqlQuery = "INSERT INTO RelObrasAutores(Id,IdObra,IdAutor,IdTipoAutor)" +
                                "VALUES (@Id,@IdObra,@IdAutor,@IdTipoAutor)";

                OleDbCommand cmd = new OleDbCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Id", newId);
                cmd.Parameters.AddWithValue("@IdObra", idObra);
                cmd.Parameters.AddWithValue("@IdAutor", idAutor);
                cmd.Parameters.AddWithValue("@IdTipoAutor", idtipoAutor);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }




        public void InsertaIdioma(ElementalProperties idioma)
        {
            OleDbConnection connection = new OleDbConnection(connectionCatalogo);

            string sSql;
            OleDbDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {

                const string SqlCadena = "SELECT * FROM Idioma_Publicacion WHERE Id = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(SqlCadena, connection);

                dataAdapter.Fill(dataSet, "Idioma_Publicacion");

                dr = dataSet.Tables["Idioma_Publicacion"].NewRow();

                dr["Id"] = idioma.IdElemento;
                dr["Desc"] = idioma.Descripcion;
                dr["DescMay"] = StringUtilities.PrepareToAlphabeticalOrder(idioma.Descripcion);

                dataSet.Tables["Idioma_Publicacion"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();

                sSql = "INSERT INTO Idioma_Publicacion (Id,[Desc],DescMay) VALUES (@Id,@Desc,@DescMay)";

                dataAdapter.InsertCommand.CommandText = sSql;

                dataAdapter.InsertCommand.Parameters.Add("@Id", OleDbType.Numeric, 0, "Id");
                dataAdapter.InsertCommand.Parameters.Add("@Desc", OleDbType.VarChar, 0, "Desc");
                dataAdapter.InsertCommand.Parameters.Add("@DescMay", OleDbType.VarChar, 0, "DescMay");

                dataAdapter.Update(dataSet, "Idioma_Publicacion");
                dataSet.Dispose();
                dataAdapter.Dispose();

            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }
        }


        public bool InsertaPais(Pais pais)
        {
            OleDbConnection connection = new OleDbConnection(connectionCatalogo);

            bool insertCompleted = false;


            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO Pais_Publicacion(Id,[Desc],DescMay) " +
                                "VALUES (@Id,@Desc,@DescMay)";

                OleDbCommand cmd = new OleDbCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@Id", pais.IdPais);
                cmd.Parameters.AddWithValue("@Desc", pais.PaisDesc);
                cmd.Parameters.AddWithValue("@DescMay", pais.PaisStr);
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ExportaModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

    }
}
