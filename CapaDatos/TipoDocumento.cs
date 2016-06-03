using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class TipoDocumento    {
        string cadenaConn = "Data Source=localhost\\SQL2K8;Initial Catalog=parkme_model;Persist Security Info=True;User ID=sa;Password=sqlsqlC64";
        public List<CapaDTO.TipoDocumento> ConsultarLista() // Devuelve una LISTA de tipoDocumentos. No metemos las colecciones adentro, porque esta lista es para llenar la grilla inicial.
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = cadenaConn;
                conn.Open();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                cmd.CommandTimeout = 120;
                cmd.CommandType = CommandType.Text;

                // Con esto armamos la lista incial de tipoDocumentos. Se puede editar la consulta y meter un top 20, mostrar ordenado, etc.
                cmd.CommandText = @"select *
                                    from TipoDocumento";

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    List<CapaDTO.TipoDocumento> listaTipoDocumento= new List<CapaDTO.TipoDocumento>();

                    while (dataReader.Read()) // Recorro el datareader de esta forma, porque va a traer mas de un tipoDocumento
                    {
                        CapaDTO.TipoDocumento tipoDocumento = new CapaDTO.TipoDocumento();

                        tipoDocumento.Id = int.Parse(dataReader[dataReader.GetOrdinal("Id")].ToString());
                        tipoDocumento.Descripcion = dataReader[dataReader.GetOrdinal("Descripcion")].ToString();

                        listaTipoDocumento.Add(tipoDocumento); // Agrego el tipoDocumento a la lista
                    }

                    return listaTipoDocumento; // retorno la lista
                }
            }
        }

        public CapaDTO.TipoDocumento ConsultarUno(int tipoDocumentoId) // Devuelve solo un TipoDocumento (notar como carga los tipos complejos)
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
                cmd.CommandText = @"select * 
                                    from TipoDocumento                                    
                                    where TipoDocumento.Id = @tipoDocumentoId";
                cmd.Parameters.Add(new SqlParameter("@tipoDocumentoId", tipoDocumentoId));

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    CapaDTO.TipoDocumento tipoDocumento = new CapaDTO.TipoDocumento();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            tipoDocumento.Id = int.Parse(dataReader[dataReader.GetOrdinal("Id")].ToString());
                            tipoDocumento.Descripcion = dataReader[dataReader.GetOrdinal("Descripcion")].ToString();                            
                        }
                    }
                    else
                    {
                        tipoDocumento = null;
                    }
                    return tipoDocumento;
                }
            }
        }
    }
}