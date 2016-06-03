using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Paciente
    {
        string cadenaConn = "Data Source=localhost\\SQL2K8;Initial Catalog=parkme_model;Persist Security Info=True;User ID=sa;Password=sqlsqlC64";

        public int Agregar(CapaDTO.Paciente paciente)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = cadenaConn;
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into Paciente (nombre, apellido, tipoDocumentoId, dni) values (@nombre, @apellido, @tipoDocumentoId, @dni)";

                cmd.Parameters.Add(new SqlParameter("@nombre", paciente.Nombre));
                cmd.Parameters.Add(new SqlParameter("@apellido", paciente.Apellido));
                cmd.Parameters.Add(new SqlParameter("@dni", paciente.Dni));
                cmd.Parameters.Add(new SqlParameter("@tipoDocumentoId", paciente.TipoDocumento.Id));

                cmd.ExecuteNonQuery();

                return 0;
            }
        }

        public CapaDTO.Paciente ConsultarUno(int pacienteId) // Devuelve solo un Paciente (notar como carga los tipos complejos)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = cadenaConn;
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;

                // Notar que la siguiente consulta la arme primero en el ManagementStudio, y luego de verificar que funcionara, la puse aca.
                cmd.CommandText = @"select Paciente.*, TipoDocumento.Id as 'TipoDocumentoId', TipoDocumento.Descripcion as 'TipoDocumentoDescripcion' 
                                    from Paciente 
                                    inner join TipoDocumento on Paciente.TipoDocumentoId = TipoDocumento.Id
                                    where Paciente.Id = @pacienteId";
                cmd.Parameters.Add(new SqlParameter("@pacienteId", pacienteId));

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    CapaDTO.Paciente paciente = new CapaDTO.Paciente();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            paciente.Id = int.Parse(dataReader[dataReader.GetOrdinal("Id")].ToString());
                            paciente.Nombre = dataReader[dataReader.GetOrdinal("Nombre")].ToString();
                            paciente.Apellido = dataReader[dataReader.GetOrdinal("Apellido")].ToString();
                            paciente.Dni = int.Parse(dataReader[dataReader.GetOrdinal("Dni")].ToString());

                            // Notar aca como armo el tipo complejo "TipoDocumento"
                            paciente.TipoDocumento = new CapaDTO.TipoDocumento() { Id = int.Parse(dataReader[dataReader.GetOrdinal("TipoDocumentoId")].ToString()), Descripcion = dataReader[dataReader.GetOrdinal("TipoDocumentoDescripcion")].ToString() };
                        } 
                    }
                    else
                    {
                        paciente = null;
                    }
                    return paciente;
                }
            }
        }
                
        public List<CapaDTO.Paciente> ConsultarLista() // Devuelve una LISTA de pacientes. No metemos las colecciones adentro, porque esta lista es para llenar la grilla inicial.
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = cadenaConn;
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;

                // Con esto armamos la lista incial de pacientes. Se puede editar la consulta y meter un top 20, mostrar ordenado, etc.
                cmd.CommandText = @"select *
                                    from Paciente";
                
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    List<CapaDTO.Paciente> listaPacientes = new List<CapaDTO.Paciente>();

                    while (dataReader.Read()) // Recorro el datareader de esta forma, porque va a traer mas de un paciente
                    {
                        CapaDTO.Paciente paciente = new CapaDTO.Paciente();

                        paciente.Id = int.Parse(dataReader[dataReader.GetOrdinal("Id")].ToString());
                        paciente.Nombre = dataReader[dataReader.GetOrdinal("Nombre")].ToString();
                        paciente.Apellido = dataReader[dataReader.GetOrdinal("Apellido")].ToString();
                        paciente.Dni = int.Parse(dataReader[dataReader.GetOrdinal("Dni")].ToString());
                        
                        listaPacientes.Add(paciente); // Agrego el paciente a la lista
                    }

                    return listaPacientes; // retorno la lista
                }
            }
        }

        public List<CapaDTO.Paciente> ConsultarListaConFiltros(int dni, string nombre) // Devuelve una LISTA de pacientes. No metemos las colecciones adentro, porque esta lista es para llenar la grilla inicial.
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = cadenaConn;
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = @"select *  from Paciente where 1 = 1"; // Este es solo un truquito para poder concatenarle los filtros en caso de que haga falta. La idea es que sino vienen filtros, entonces no filtre por nada y devuelva todo

                if (dni > 0) // Si se manda 0, entonces NO filtramos por dni
                {
                    cmd.CommandText += " and dni = @dni";
                    cmd.Parameters.Add(new SqlParameter("@dni", dni));
                }

                if (!string.IsNullOrEmpty(nombre)) // Si se manda vacio, entonces NO filtramos por nombre o apellido
                {
                    cmd.CommandText += " and (nombre like @nombre or apellido like @nombre)";
                    cmd.Parameters.Add(new SqlParameter("@nombre", string.Format("%{0}%", nombre)));
                    //cmd.Parameters.Add(new SqlParameter("@apellido", nombre));
                }
                
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    List<CapaDTO.Paciente> listaPacientes = new List<CapaDTO.Paciente>();

                    while (dataReader.Read()) // Recorro el datareader de esta forma, porque va a traer mas de un paciente
                    {
                        CapaDTO.Paciente paciente = new CapaDTO.Paciente();

                        paciente.Id = int.Parse(dataReader[dataReader.GetOrdinal("Id")].ToString());
                        paciente.Nombre = dataReader[dataReader.GetOrdinal("Nombre")].ToString();
                        paciente.Apellido = dataReader[dataReader.GetOrdinal("Apellido")].ToString();
                        paciente.Dni = int.Parse(dataReader[dataReader.GetOrdinal("Dni")].ToString());

                        listaPacientes.Add(paciente); // Agrego el paciente a la lista
                    }

                    return listaPacientes; // retorno la lista
                }
            }
        }

        public int Modificar(CapaDTO.Paciente paciente)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = cadenaConn;
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Paciente set nombre = @nombre, apellido = @apellido, tipoDocumentoId = @tipoDocumentoId where id = @id";
                cmd.Parameters.Add(new SqlParameter("@nombre", paciente.Nombre));
                cmd.Parameters.Add(new SqlParameter("@apellido", paciente.Apellido));
                cmd.Parameters.Add(new SqlParameter("@dni", paciente.Dni));
                cmd.Parameters.Add(new SqlParameter("@tipoDocumentoId", paciente.TipoDocumento.Id));
                cmd.Parameters.Add(new SqlParameter("@id", paciente.Id));

                return cmd.ExecuteNonQuery();                
            }
        }

        public int Eliminar(CapaDTO.Paciente paciente)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = cadenaConn;
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Delete from  Paciente where id = @id";
                cmd.Parameters.Add(new SqlParameter("@id", paciente.Id));

                return cmd.ExecuteNonQuery();
            }
        }
    }
}