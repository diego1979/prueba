using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTO
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre{ get; set; }
        public string Apellido { get; set; }
        public int Dni { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public ObraSocial ObraSocial { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Email { get; set; }
        public string Calle { get; set; }
        public int Numero{ get; set; }
        public int Piso { get; set; }
        public string Departamento { get; set; }
        public string CodigoPostal { get; set; }
    }
}
