using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_1223319_1003519.Models
{
    public class LlavePaciente : IComparable
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DPI { get; set; }
        public int Prioridad { get; set; }
        public string Estado { get; set; }
        public DateTime FechaEntrada { get; set; }

        public int CompareTo(object obj)
        {
            if (this.Prioridad.CompareTo(((Paciente)obj).Prioridad) == 0)
                return this.FechaEntrada.CompareTo(((Paciente)obj).FechaEntrada);
            else
                return this.Prioridad.CompareTo(((Paciente)obj).Prioridad);
        }

        public static Comparison<LlavePaciente> CompararPrioridad = delegate (LlavePaciente t1, LlavePaciente t2)
        {
            return t1.CompareTo(t2);
        };

        public static Comparison<LlavePaciente> CompararNombre = delegate (LlavePaciente t1, LlavePaciente t2)
        {
            return t1.Nombre.CompareTo(t2.Nombre);
        };

        public static Comparison<LlavePaciente> CompararApellido = delegate (LlavePaciente t1, LlavePaciente t2)
        {
            return t1.Apellido.CompareTo(t2.Apellido);
        };

        public static Comparison<LlavePaciente> CompararDPI = delegate (LlavePaciente t1, LlavePaciente t2)
        {
            return t1.DPI.CompareTo(t2.DPI);
        };
    }
}