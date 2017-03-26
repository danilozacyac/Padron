using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PadronApi.Dto;
using PadronApi.Model;
using ScjnUtilities;
using System.Windows;

namespace Padron.Model
{
    public class PadronModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        /// <summary>
        /// Contabiliza el número de ejemplares que no se entregarán por preferencia de los titulares,
        /// al final este total será sumado al almacen Zaragoza
        /// </summary>
        private int totalDescartes = 0;

        /// <summary>
        /// Obtiene el listado de obras de las cuales no se ha generado la distribución de las
        /// mismas
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Obra> GetObrasSinPadron()
        {
            ObservableCollection<Obra> obrasSinPadron = new ObservableCollection<Obra>();

            string sqlCadena = "SELECT O.* FROM C_Obra O " +
                               "WHERE O.IdObra NOT IN (SELECT P.IdObra FROM Padron P where idObra is not null) AND O.IdObra > 3999";

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
                        Obra obra = new Obra();
                        obra.IdObra = Convert.ToInt32(reader["IdObra"]);
                        obra.Orden = reader["Orden"] as int? ?? 0;
                        obra.Titulo = reader["Titulo"].ToString();
                        obra.TituloStr = reader["TituloTxt"].ToString();
                        obra.AnioPublicacion = Convert.ToInt32(reader["AnioPublicacion"]);
                        obra.TipoObra = Convert.ToInt32(reader["IdTipoObra"]);
                        obra.Tiraje = Convert.ToInt32(reader["Tiraje"]);

                        obrasSinPadron.Add(obra);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return obrasSinPadron;
        }

        /// <summary>
        /// Obtiene el listado de las personas que recibirán una obra de acuerdo al tiraje señalado,
        /// descartando a aquellos que por preferencias particulares no deseen recibir ese tipo de obra
        /// </summary>
        /// <param name="idTiraje">Tiraje de la obra </param>
        /// <param name="idTipoObra">Tipo de la obra de la cual se generará el padrón</param>
        /// <returns></returns>
        public ObservableCollection<PlantillaDto> GetPlantillaForPadron(int idTiraje, int idTipoObra)
        {
            ObservableCollection<PlantillaDto> plantilla = new ObservableCollection<PlantillaDto>();

            string sqlCadena = "SELECT A.IDAcuNum, A.IdOrg, A.IdTitular, A.Particular, A.Autor, A.Personal, A.Oficina, A.Biblioteca, " +
                               "A.Resguardo, A.TipoObra, A.idFuncion, T.Nombre, T.Apellidos, " +
                               "O.DescOrg, O.IdCircuito, O.IdOrdinal, O.IdCiudad, O.IdEstado, O.IdTpodist, C.Ciudad, E.Estado, Ti.Titulo,Ti.TituloAbr, O.IdTpoOrg  " +
                               "FROM ((((AcuerdoPadron AS A INNER JOIN C_Titular AS T ON A.IdTitular = T.IdTitular) INNER JOIN C_Organismo AS O ON A.IdOrg = O.IdOrg) " +
                               "INNER JOIN C_Ciudad AS C ON O.IdCiudad = C.IdCiudad) INNER JOIN C_Estado AS E ON O.IdEstado = E.IdEstado) INNER JOIN C_Titulo AS Ti ON T.IdTitulo = Ti.IdTitulo  " +
                               "WHERE A.IDAcuNum =@Tiraje AND t.quieredist >= 0  ORDER BY O.Idtpodist, IdTpoOrg, IdCircuito,IdOrdinal,Ti.Orden ";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Tiraje", idTiraje);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlantillaDto integraPlantilla = new PlantillaDto();
                        integraPlantilla.IdOrganismo = Convert.ToInt32(reader["IdOrg"]);
                        integraPlantilla.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                        integraPlantilla.Titulo = reader["TituloAbr"].ToString();
                        integraPlantilla.Particular = Convert.ToInt32(reader["Particular"]);
                        integraPlantilla.Oficina = Convert.ToInt32(reader["Oficina"]);
                        integraPlantilla.Biblioteca = Convert.ToInt32(reader["Biblioteca"]);
                        integraPlantilla.Resguardo = Convert.ToInt32(reader["Resguardo"]);
                        integraPlantilla.Personal = Convert.ToInt32(reader["Personal"]);
                        integraPlantilla.Autor = Convert.ToInt32(reader["Autor"]);
                        integraPlantilla.Nombre = integraPlantilla.Titulo + " " + reader["Nombre"].ToString() + " " + reader["Apellidos"];
                        integraPlantilla.Ciudad = Convert.ToInt32(reader["IdCiudad"]);
                        integraPlantilla.CiudadStr = reader["Ciudad"].ToString();
                        integraPlantilla.EstadoStr = reader["Estado"].ToString();
                        integraPlantilla.Estado = Convert.ToInt32(reader["IdEstado"]);
                        integraPlantilla.Organismo = reader["DescOrg"].ToString();
                        integraPlantilla.TipoDistribucion = Convert.ToInt32(reader["IdTpoDist"]);
                        integraPlantilla.Funcion = Convert.ToInt32(reader["IdFuncion"]);


                        char[] queRecibe = reader["TipoObra"].ToString().ToCharArray();

                        if (queRecibe[idTipoObra - 1].Equals('1'))
                            plantilla.Add(integraPlantilla);
                        else
                            totalDescartes += integraPlantilla.Particular + integraPlantilla.Oficina + integraPlantilla.Biblioteca + integraPlantilla.Personal + integraPlantilla.Autor;

                        if (integraPlantilla.IdOrganismo == 6001)
                            integraPlantilla.Resguardo += totalDescartes;
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return plantilla;
        }

        /// <summary>
        /// Obtiene la información de un padrón generado previamente para que sea replicado en uno nuevo
        /// </summary>
        /// <param name="idPadronBase"></param>
        /// <param name="idTipoObra"></param>
        /// <returns></returns>
        public ObservableCollection<PlantillaDto> GetPadronAClonar(int idPadronBase, int idTipoObra)
        {
            ObservableCollection<PlantillaDto> plantilla = new ObservableCollection<PlantillaDto>();

            string sqlCadena = "SELECT P.IdOrg, P.IdTitular, P.Particular, P.Autor, P.Oficina, P.Biblioteca, P.Resguardo, P.Personal, T.Nombre, T.Apellidos, " +
                                "O.DescOrg, O.IdCiudad, O.IdEstado, O.IdTpodist, C.Ciudad, E.Estado, C_Titulo.TituloAbr  " +
                                "FROM (((PadronHistorico AS P INNER JOIN (C_Titular AS T INNER JOIN C_Titulo ON T.IdTitulo = C_Titulo.IdTitulo) " +
                                "ON P.IdTitular = T.IdTitular) INNER JOIN C_Organismo AS O ON P.IdOrg = O.IdOrg) INNER JOIN C_Ciudad AS C " +
                                "ON O.IdCiudad = C.IdCiudad) INNER JOIN C_Estado AS E ON O.IdEstado = E.IdEstado  " +
                                "WHERE P.IdPadron = @IdPadron ORDER BY O.IdTpoDist,O.IdEstado,O.IdCiudad";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int idFalla = 0;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadronBase);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PlantillaDto integrante = new PlantillaDto();
                        integrante.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                        idFalla = integrante.IdTitular;
                        integrante.IdOrganismo = Convert.ToInt32(reader["IdOrg"]);
                        integrante.Titulo = reader["TituloAbr"].ToString();
                        integrante.Particular = Convert.ToInt32(reader["Particular"]);
                        integrante.Oficina = Convert.ToInt32(reader["Oficina"]);
                        integrante.Biblioteca = Convert.ToInt32(reader["Biblioteca"]);
                        integrante.Resguardo = Convert.ToInt32(reader["Resguardo"]);
                        integrante.Personal = Convert.ToInt32(reader["Personal"]);
                        integrante.Autor = Convert.ToInt32(reader["Autor"]);
                        integrante.Nombre = integrante.Titulo + " " + reader["Nombre"].ToString() + " " + reader["Apellidos"];
                        integrante.Ciudad = Convert.ToInt32(reader["IdCiudad"]);
                        integrante.CiudadStr = reader["Ciudad"].ToString();
                        integrante.EstadoStr = reader["Estado"].ToString();
                        integrante.Estado = Convert.ToInt32(reader["IdEstado"]);
                        integrante.Organismo = reader["DescOrg"].ToString();
                        integrante.TipoDistribucion = Convert.ToInt32(reader["IdTpoDist"]);

                        plantilla.Add(integrante);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel " + idFalla, "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel " + idFalla, "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return plantilla;
        }

        /// <summary>
        /// Obtiene el detalle de el padron generado previamente, ya sea para generar papelería o para realizar alguna devolución
        /// </summary>
        /// <param name="idPadron">Identificador del padrón del cual se genera la papelería</param>
        /// <returns></returns>
        public ObservableCollection<PlantillaDto> GetDetallesDePadron(int idPadron, int idAcuerdo, out int rowNumber)
        {
            rowNumber = 0;

            ObservableCollection<PlantillaDto> plantilla = new ObservableCollection<PlantillaDto>();

            string sqlCadena = "SELECT A.IdPadron, A.Funcion, A.IdOrg, A.IdTitular, A.Particular, A.Autor, A.Personal, A.Oficina, " +
                               "A.Biblioteca, A.Resguardo, T.Nombre, T.Apellidos, O.DescOrg, O.IdCircuito, O.IdOrdinal, O.IdCiudad, " +
                               "O.IdEstado, O.IdTpodist, C.Ciudad, E.EstadoAbr, Ti.Titulo, Ti.TituloAbr, O.IdTpoOrg, (SELECT SUM(Cantidad) FROM Devolucion " +
                               "WHERE IdTitular = A.IdTitular AND IdPadron = A.IdPadron) AS Devueltos, Tpo.IdGrupo  " +
                               "FROM ((((((PadronHistorico AS A INNER JOIN C_Titular AS T ON A.IdTitular = T.IdTitular) " +
                               "INNER JOIN C_Organismo AS O ON A.IdOrg = O.IdOrg) INNER JOIN C_Ciudad AS C ON O.IdCiudad = C.IdCiudad)   " +
                               "INNER JOIN C_Estado AS E ON O.IdEstado = E.IdEstado) INNER JOIN C_Titulo AS Ti ON T.IdTitulo = Ti.IdTitulo)  " +
                               "INNER JOIN C_Funcion AS F ON A.Funcion = F.IdFuncion) INNER JOIN C_TipoOrganismo Tpo ON O.IdTpoOrg = Tpo.IdTpoOrg   " +
                               "WHERE A.IdPadron = @IdPadron   " +
                               "ORDER BY O.IdTpodist, O.IdTpoOrg, O.IdCircuito, O.IdOrdinal, A.IdOrg, F.Orden, Ti.Orden";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int idFalla = 0;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PlantillaDto integrantePlantilla = new PlantillaDto();
                    integrantePlantilla.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                    idFalla = integrantePlantilla.IdTitular;

                    integrantePlantilla.IdOrganismo = Convert.ToInt32(reader["IdOrg"]);
                    integrantePlantilla.Titulo = reader["TituloAbr"].ToString();
                    integrantePlantilla.Particular = Convert.ToInt32(reader["Particular"]);
                    integrantePlantilla.Oficina = Convert.ToInt32(reader["Oficina"]);
                    integrantePlantilla.Biblioteca = Convert.ToInt32(reader["Biblioteca"]);
                    integrantePlantilla.Resguardo = Convert.ToInt32(reader["Resguardo"]);
                    integrantePlantilla.Personal = Convert.ToInt32(reader["Personal"]);
                    integrantePlantilla.Autor = Convert.ToInt32(reader["Autor"]);
                    integrantePlantilla.Nombre = integrantePlantilla.Titulo + " " + reader["Nombre"].ToString() + " " + reader["Apellidos"];
                    integrantePlantilla.Ciudad = Convert.ToInt32(reader["IdCiudad"]);
                    integrantePlantilla.CiudadStr = reader["Ciudad"].ToString();
                    integrantePlantilla.EstadoStr = reader["EstadoAbr"].ToString();
                    integrantePlantilla.Estado = Convert.ToInt32(reader["IdEstado"]);
                    integrantePlantilla.Organismo = reader["DescOrg"].ToString();
                    integrantePlantilla.TipoDistribucion = Convert.ToInt32(reader["IdTpoDist"]);
                    integrantePlantilla.TipoOrganismo = Convert.ToInt32(reader["IdTpoOrg"]);
                    integrantePlantilla.Funcion = Convert.ToInt32(reader["Funcion"]);
                    integrantePlantilla.GrupoOrganismo = Convert.ToInt32(reader["IdGrupo"]);

                    if (integrantePlantilla.Particular > 0)
                        rowNumber++;
                    if (integrantePlantilla.Oficina > 0)
                        rowNumber++;
                    if (integrantePlantilla.Biblioteca > 0)
                        rowNumber++;
                    if (integrantePlantilla.Resguardo > 0)
                        rowNumber++;
                    if (integrantePlantilla.Personal > 0)
                        rowNumber++;
                    if (integrantePlantilla.Particular == 0 && integrantePlantilla.Autor > 0)
                        rowNumber++;

                    plantilla.Add(integrantePlantilla);
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel " + idFalla, "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel " + idFalla, "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return plantilla;
        }


        /// <summary>
        /// Obtiene el listado de aquellos titulares con adscripcion que no estan incluidos en la plantilla
        /// que se esta generando
        /// </summary>
        /// <param name="incluidos">Lista de identificadores de los titulares ya incluidos</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public ObservableCollection<Titular> GetTitularesNoincluidos(List<int> incluidos)
        {
            ObservableCollection<Titular> catalogoTitulares = new ObservableCollection<Titular>();

            string noIncluir = string.Join(",", incluidos);

            string sqlCadena = "SELECT distinct T.*, O.IdOrg, O.DescOrg FROM (AcuerdoPadron AS A INNER JOIN C_Organismo AS O ON A.IdOrg = O.IdOrg) " +
                               "INNER JOIN C_Titular AS T ON A.IdTitular = T.IdTitular " +
                               " WHERE (IdEstatus <> 4 AND IdEstatus <> 5) AND  T.IdTitular NOT IN (" + noIncluir + ")" +
                               " ORDER BY T.Apellidos";

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
                        Titular titular = new Titular();
                        titular.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                        titular.Nombre = reader["Nombre"].ToString();
                        titular.Apellidos = reader["Apellidos"].ToString();
                        titular.NombreStr = reader["NombMay"].ToString();
                        titular.IdTitulo = reader["IdTitulo"] as int? ?? 0;
                        titular.Observaciones = reader["Obs"].ToString();
                        titular.Estado = reader["IdEstatus"] as int? ?? 0;
                        titular.QuiereDistribucion = Convert.ToInt32(reader["QuiereDist"]);
                        titular.Correo = reader["Correo"].ToString();
                        titular.TotalAdscripciones = 1;
                        catalogoTitulares.Add(titular);

                        noIncluir += "," + titular.IdTitular;
                    }
                }
                cmd.Dispose();
                reader.Close();

                foreach (Titular titular in GetSinAdscripcionNoincluidos(noIncluir))
                    catalogoTitulares.Add(titular);

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,TitularModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }

        /// <summary>
        /// Obtiene el listado de titulares sin adscripción y autores que pueden recibir alguna de las obras
        /// </summary>
        /// <param name="incluidos"></param>
        /// <returns></returns>
        public ObservableCollection<Titular> GetSinAdscripcionNoincluidos(string noIncluir)
        {
            ObservableCollection<Titular> catalogoTitulares = new ObservableCollection<Titular>();

            string sqlCadena = "SELECT * FROM C_Titular " +
                               " WHERE (IdEstatus <> 4 AND IdEstatus <> 5) AND IdTitular NOT IN (" + noIncluir + ")" +
                               " ORDER BY Apellidos";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int queRegistro = 0;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Titular titular = new Titular();
                        titular.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                        queRegistro = titular.IdTitular;
                        titular.Nombre = reader["Nombre"].ToString();
                        titular.Apellidos = reader["Apellidos"].ToString();
                        titular.NombreStr = reader["NombMay"].ToString();
                        titular.IdTitulo = reader["IdTitulo"] as int? ?? 0;
                        titular.Observaciones = reader["Obs"].ToString();
                        titular.Estado = reader["IdEstatus"] as int? ?? 0;
                        titular.QuiereDistribucion = Convert.ToInt32(reader["QuiereDist"]);
                        titular.Correo = reader["Correo"].ToString();
                        titular.TotalAdscripciones = 0;
                        catalogoTitulares.Add(titular);
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + queRegistro + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return catalogoTitulares;
        }

        /// <summary>
        /// Verifica si alguno de los titulares que integran el padrón tiene registro duplicado
        /// para cualquiera de los tirajes
        /// </summary>
        /// <returns></returns>
        public int VerificaDuplicidadTitulares()
        {
            string sqlCadena = "SELECT IDAcuNum,IdOrg,IdTitular,Particular,Autor,Personal,Oficina,Biblioteca,Resguardo,TipoObra,idFuncion,count(idtitular) FROM AcuerdoPadron " +
                               "GROUP BY IDAcuNum,IdOrg,IdTitular,Particular,Autor,Personal,Oficina,Biblioteca,Resguardo,TipoObra,idFuncion HAVING count(idtitular) > 1  ORDER BY count(idtitular) desc";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            int total = 0;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        total++;
                    }
                }
                cmd.Dispose();
                reader.Close();


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return total;
        }


        /// <summary>
        /// Obtiene los datos generales del acuerdo de distribución así como el número de acuerdo que le corresponderá
        /// </summary>
        /// <param name="plantilla">Lista de personal que recibe y las cantidades que recibe</param>
        /// <param name="obra">Obra que se distribuirá</param>
        /// <param name="tiraje">Número de ejemplares de la obra</param>
        /// <returns></returns>
        public bool InsertaPadron(ObservableCollection<PlantillaDto> plantilla, Obra obra, TirajePersonal tiraje)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            int idPadron = DataBaseUtilities.GetNextIdForUse("Padron", "IdPadron", connection);
            int numAcuerdo = this.GetNextAcuerdo();

            try
            {
                connection.Open();

                string sqlQuery = "INSERT INTO Padron(IdPadron,AcuerdoNum,AnioAcuerdo,IdObra,Fecha,IdAcuNum)" +
                                  "VALUES (@IdPadron,@AcuerdoNum,@AnioAcuerdo,@IdObra,@Fecha,@IdAcuNum)";

                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                cmd.Parameters.AddWithValue("@AcuerdoNum", numAcuerdo);
                cmd.Parameters.AddWithValue("@AnioAcuerdo", DateTime.Now.Year);
                cmd.Parameters.AddWithValue("@IdObra", obra.IdObra);
                cmd.Parameters.AddWithValue("@Fecha", DateTimeUtilities.DateToInt(DateTime.Now));
                cmd.Parameters.AddWithValue("@IdAcuNum", tiraje.IdAcuerdo);

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                insertCompleted = true;
                insertCompleted = this.InsertaPlantillaAlHistorico(idPadron, plantilla, obra);

                if (!insertCompleted)
                    this.DeleteUnfinishedPAdron(idPadron);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Obtiene el número de acuerdo que le corresponde al padrón que se esta generando
        /// </summary>
        /// <returns></returns>
        private int GetNextAcuerdo()
        {
            int nextAcuerdo = 1;

            string sqlCadena = "SELECT MAX(AcuerdoNum) + 1 AS Siguiente FROM Padron WHERE AnioAcuerdo = @Anio";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Anio", DateTime.Now.Year);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nextAcuerdo = reader["Siguiente"] as int? ?? 1;
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return nextAcuerdo;
        }



        /// <summary>
        /// Inserta la plantilla generada al histórico
        /// </summary>
        /// <param name="idPadron">Identificador del padrón recién generado</param>
        /// <param name="plantilla">Datos que se ingresan al histórico</param>
        /// <returns></returns>
        private bool InsertaPlantillaAlHistorico(int idPadron, ObservableCollection<PlantillaDto> plantilla, Obra obra)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            bool insertCompleted = false;

            try
            {
                connection.Open();

                foreach (PlantillaDto item in plantilla)
                {
                    string sqlQuery = "INSERT INTO PadronHistorico(IdPadron,IdOrg,IdTitular,Particular,Autor,Oficina,Biblioteca,Resguardo,Personal,Funcion)" +
                                      "VALUES (@IdPadron,@IdOrg,@IdTitular,@Particular,@Autor,@Oficina,@Biblioteca,@Resguardo,@Personal,@IdFuncion)";

                    SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                    cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                    cmd.Parameters.AddWithValue("@IdOrg", item.IdOrganismo);
                    cmd.Parameters.AddWithValue("@IdTitular", item.IdTitular);
                    cmd.Parameters.AddWithValue("@Particular", item.Particular);
                    cmd.Parameters.AddWithValue("@Autor", item.Autor);
                    cmd.Parameters.AddWithValue("@Oficina", item.Oficina);
                    cmd.Parameters.AddWithValue("@Biblioteca", item.Biblioteca);
                    cmd.Parameters.AddWithValue("@Resguardo", item.Resguardo);
                    cmd.Parameters.AddWithValue("@Personal", item.Personal);
                    cmd.Parameters.AddWithValue("@IdFuncion", item.Funcion);

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (item.Autor > 0)
                        new AutorModel().SetAutorObra(item.IdTitular, obra.IdObra);
                }

                insertCompleted = true;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        /// <summary>
        /// Elimina un padrón de distribución que tuvo problemas al ser guardado
        /// </summary>
        /// <param name="idPadron">Identificador del padron que será eliminado</param>
        private void DeleteUnfinishedPAdron(int idPadron)
        {
            string sqlCadena = "DELETE FROM Padron WHERE IdPadron = @IdPadron";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                sqlCadena = "DELETE FROM PadronHistorico WHERE IdPadron = @IdPadron";
                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }
        }


        #region EliminaPadron

        /// <summary>
        /// Permite eliminar el padrón de una obra una vez que este fue generado verificando que no se hayam generado 
        /// padrones posteriores ya que esto impediría continuar con la acción
        /// </summary>
        /// <param name="padron"></param>
        public void EliminarPadron(PadronGenerado padron)
        {
            bool? existeMasReciente = this.DoLatePadronesExist(padron);

            if (existeMasReciente == true)
            {
                MessageBox.Show("No se puede eliminar el padrón seleccionado ya que se generaron oficios con posterioridad a este padrón", "Atención:", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (existeMasReciente == false)
            {
                this.DeletePadron(padron.IdPadron);
            }
            else
            {
                MessageBox.Show("No se realizar la acción seleccionada, favor de intentarlo más tarde", "Atención:", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Verifica que no se haya generado papelería posterior a la del padrón que se desea eliminar
        /// </summary>
        /// <param name="padron"></param>
        /// <returns></returns>
        private bool? DoLatePadronesExist(PadronGenerado padron)
        {
            bool? lateExist = null;

            string sqlCadena = "SELECT * FROM Padron WHERE AnioAcuerdo = @Year AND OfIni > @OficioFinal";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Year", padron.AnioAcuerdo);
                cmd.Parameters.AddWithValue("@OficioFinal", padron.OficioFinal);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lateExist = true;
                    }
                }
                else
                {
                    lateExist = false;
                }
                cmd.Dispose();
                reader.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return lateExist;
        }

        /// <summary>
        /// Elimina la información del padrón seleccionado en las tablas de Acuses, PadronHistorico y Padron
        /// </summary>
        /// <param name="idPadron"></param>
        private void DeletePadron(int idPadron)
        {
            bool acuses = false, historico = false, padron = false;

            string sqlCadena = "DELETE FROM Acuses WHERE IdPadron = @IdPadron";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = null;

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                acuses = true;

                sqlCadena = "DELETE FROM PadronHistorico WHERE IdPadron = @IdPadron";
                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                historico = true;

                sqlCadena = "DELETE FROM Padron WHERE IdPadron = @IdPadron";
                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdPadron", idPadron);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                padron = true;

                if (acuses && historico && padron)
                {
                    // Se eliminó correctamente
                }
                else
                {
                    MessageBox.Show("Hubo un error al eliminar este padrón, favor de contactar al desarrollador", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }
        }


        #endregion


        #region Cierre

        /// <summary>
        /// Obtiene el listado de los padrones generados en base a la obra
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PadronGenerado> GetPadronGenerado()
        {
            ObservableCollection<PadronGenerado> plantilla = new ObservableCollection<PadronGenerado>();

            string sqlCadena = "SELECT P.*, A.Acuerdo,O.Titulo, O.FechaDis, O.IdObra " +
                               "FROM Padron P INNER JOIN C_Acuerdo A ON P.IDAcuNum = A.IDAcuNum " +
                               " INNER JOIN C_Obra O ON P.IdObra = O.IdObra  ORDER BY AnioAcuerdo desc, OfIni desc";

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
                        PadronGenerado padron = new PadronGenerado();
                        padron.IdPadron = Convert.ToInt32(reader["IdPadron"]);
                        padron.NumAcuerdo = Convert.ToInt32(reader["AcuerdoNum"]);
                        padron.AnioAcuerdo = Convert.ToInt32(reader["AnioAcuerdo"]);
                        padron.IdObra = Convert.ToInt32(reader["IdObra"]);
                        padron.TituloObra = reader["Titulo"].ToString();
                        padron.Tiraje = Convert.ToInt32(reader["Acuerdo"]);
                        padron.FechaGenerado = DateTimeUtilities.IntToDate(reader, "Fecha");
                        padron.FechaDistribucion = DateTimeUtilities.IntToDate(reader, "FechaDis");
                        padron.IdAcuerdo = Convert.ToInt32(reader["IdAcuNum"]);
                        padron.OficioInicial = Convert.ToInt32(reader["OfIni"]);
                        padron.OficioFinal = Convert.ToInt32(reader["OfiFin"]);

                        plantilla.Add(padron);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return plantilla;
        }



        /// <summary>
        /// Actualiza el número de acuerdo a 0 para el disco y libro de la gaceta
        /// </summary>
        /// <param name="padron"></param>
        /// <returns></returns>
        public bool SetNumeroAcuerdoZero(PadronGenerado padron)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            bool insertCompleted = false;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlQuery = "SELECT * FROM Padron WHERE IdPadron = @IdPadron";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@IdPadron", padron.IdPadron);
                dataAdapter.Fill(dataSet, "Padron");

                if (dataSet.Tables["Padron"].Rows.Count > 0)
                {
                    dr = dataSet.Tables["Padron"].Rows[0];
                    dr.BeginEdit();
                    dr["AcuerdoNum"] = 0;
                    dr.EndEdit();

                    dataAdapter.UpdateCommand = connection.CreateCommand();

                    dataAdapter.UpdateCommand.CommandText = "UPDATE Padron SET AcuerdoNum = @AcuerdoNum WHERE IdPadron = @IdPadron";

                    dataAdapter.UpdateCommand.Parameters.Add("@AcuerdoNum", SqlDbType.Int, 0, "AcuerdoNum");
                    dataAdapter.UpdateCommand.Parameters.Add("@IdPadron", SqlDbType.Int, 0, "IdPadron");
                    dataAdapter.Update(dataSet, "Padron");
                }

                dataSet.Dispose();
                dataAdapter.Dispose();
                insertCompleted = true;
                padron.NumAcuerdo = 0;
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "Padron");
            }
            finally
            {
                connection.Close();
            }

            return insertCompleted;
        }

        #endregion

        #region Historiales

        /// <summary>
        /// Obtiene el historial de las obras que ha recibido un organismo en propiedad de la oficina
        /// </summary>
        /// <param name="idOrganismo"></param>
        /// <returns></returns>
        public ObservableCollection<PlantillaDto> GetHistorialOrganismoObras(int idOrganismo)
        {
            ObservableCollection<PlantillaDto> obrasRecibidas = new ObservableCollection<PlantillaDto>();

            string sqlCadena = "SELECT P.IdPadron, P.IdObra,h.IdOrg,H.IdTitular, [O].Titulo, H.Oficina, Ti.TituloAbr + ' ' + T.Nombre + ' ' + T.Apellidos AS Nombre " +
                               "FROM ((C_Obra AS O INNER JOIN (PadronHistorico AS H INNER JOIN Padron AS P " +
                               "ON H.IdPadron = P.IdPadron) ON [O].IdObra = P.IdObra) INNER JOIN C_Titular AS T " +
                               "ON H.IdTitular = T.IdTitular) INNER JOIN C_Titulo Ti ON T.IdTitulo = Ti.IdTitulo " +
                               "WHERE H.Oficina>0 AND H.IdOrg = @IdOrg";

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
                        PlantillaDto plantilla = new PlantillaDto();
                        plantilla.IdPadron = Convert.ToInt32(reader["IdPadron"]);
                        plantilla.IdObra = Convert.ToInt32(reader["IdObra"]);
                        plantilla.IdOrganismo = Convert.ToInt32(reader["IdOrg"]);
                        plantilla.IdTitular = Convert.ToInt32(reader["IdTitular"]);
                        plantilla.Oficina = Convert.ToInt32(reader["Oficina"]);
                        plantilla.Titulo = reader["Titulo"].ToString();
                        plantilla.Nombre = reader["Nombre"].ToString();

                        obrasRecibidas.Add(plantilla);
                    }
                }
                cmd.Dispose();
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,PadronModel", "PadronApi");
            }
            finally
            {
                connection.Close();
            }

            return obrasRecibidas;
        }



        #endregion


        #region Reportes

       


        #endregion


    }
}