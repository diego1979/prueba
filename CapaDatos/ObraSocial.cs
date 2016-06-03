using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class ObraSocial
    {
        string cadenaConn = "Data Source=localhost\\SQL2K8;Initial Catalog=parkme_model;Persist Security Info=True;User ID=sa;Password=sqlsqlC64";

        public List<CapaDTO.ObraSocial> ConsultarLista() // Devuelve una LISTA de pacientes. No metemos las colecciones adentro, porque esta lista es para llenar la grilla inicial.
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
                                    from ObraSocial";

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    List<CapaDTO.ObraSocial> listaObraSocial = new List<CapaDTO.ObraSocial>();

                    while (dataReader.Read()) // Recorro el datareader de esta forma, porque va a traer mas de un paciente
                    {
                        CapaDTO.ObraSocial obraSocial = new CapaDTO.ObraSocial();

                        obraSocial.Id = int.Parse(dataReader[dataReader.GetOrdinal("Id")].ToString());
                        obraSocial.Descripcion = dataReader[dataReader.GetOrdinal("Descripcion")].ToString();

                        listaObraSocial.Add(obraSocial); // Agrego el paciente a la lista
                    }

                    return listaObraSocial; // retorno la lista
                }
            }
        }

        public CapaDTO.ObraSocial ConsultarUno(int obraSocialId) // Devuelve solo un ObraSocial (notar como carga los tipos complejos)
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
                                    from ObraSocial                                    
                                    where ObraSocial.Id = @obraSocialId";
                cmd.Parameters.Add(new SqlParameter("@obraSocialId", obraSocialId));

                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {
                    CapaDTO.ObraSocial obraSocial = new CapaDTO.ObraSocial();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            obraSocial.Id = int.Parse(dataReader[dataReader.GetOrdinal("Id")].ToString());
                            obraSocial.Descripcion = dataReader[dataReader.GetOrdinal("Descripcion")].ToString();
                        }
                    }
                    else
                    {
                        obraSocial = null;
                    }
                    return obraSocial;
                }
            }
        }

    }
}