using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            CapaDTO.Paciente pacienteDTO = new CapaDTO.Paciente();
            CapaNegocio.Paciente pacienteNegocio = new CapaNegocio.Paciente();
            //pacienteDTO = pacienteNegocio.ConsultarUno(3);

            //pacienteDTO.Nombre = "Marcos Agustin";
            //pacienteNegocio.Modificar(pacienteDTO);
            //pacienteNegocio.Eliminar(pacienteDTO);
            List<CapaDTO.Paciente> pacientes = pacienteNegocio.ConsultarListaConFiltros(0, "agus");

            Console.ReadKey();
            
        }
    }
}
