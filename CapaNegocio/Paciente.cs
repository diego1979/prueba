using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class Paciente
    {
        public int Agregar(CapaDTO.Paciente paciente)
        {
            Validar(paciente);
            CapaDatos.Paciente pacienteDatos = new CapaDatos.Paciente();
            return pacienteDatos.Agregar(paciente);            
        }

        private void Validar(CapaDTO.Paciente paciente)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(paciente.Nombre))
            {
                stringBuilder.AppendLine("* Debe ingresar el nombre");
            }
            if (string.IsNullOrEmpty(paciente.Apellido))
            {
                stringBuilder.AppendLine("* Debe ingresar el apellido");
            }
            if (paciente.TipoDocumento == null)
            {
                stringBuilder.AppendLine("* Debe ingresar el tipo de documento");
            }

            if (stringBuilder.Length > 0)
            {
                throw new ApplicationException(stringBuilder.ToString());
            }
        }

        public CapaDTO.Paciente ConsultarUno(int pacienteId)
        {
            CapaDatos.Paciente pacienteDatos = new CapaDatos.Paciente();
            return pacienteDatos.ConsultarUno(pacienteId);
        }

        public List<CapaDTO.Paciente> ConsultarLista()
        {
            CapaDatos.Paciente pacienteDatos = new CapaDatos.Paciente();
            return pacienteDatos.ConsultarLista();
        }

        public List<CapaDTO.Paciente> ConsultarListaConFiltros(int dni, string nombre) // Devuelve una LISTA de pacientes.
        {
            CapaDatos.Paciente pacienteDatos = new CapaDatos.Paciente();
            return pacienteDatos.ConsultarListaConFiltros(dni, nombre);
        }

        public int Modificar(CapaDTO.Paciente paciente)
        {
            Validar(paciente);
            CapaDatos.Paciente pacienteDatos = new CapaDatos.Paciente();
            return pacienteDatos.Modificar(paciente);
        }

        public int Eliminar(CapaDTO.Paciente paciente)
        {
            CapaDatos.Paciente pacienteDatos = new CapaDatos.Paciente();
            return pacienteDatos.Eliminar(paciente);
        }

    }
}
