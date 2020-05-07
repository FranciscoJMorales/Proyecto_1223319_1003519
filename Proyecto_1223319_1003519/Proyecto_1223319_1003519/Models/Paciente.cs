using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_1223319_1003519.Models
{
    public class Paciente : IComparable
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DPI { get; set; }
        public int Edad { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Sintomas { get; set; }
        public string Descripcion { get; set; }
        public int Prioridad { get; set; }
        public string Estado { get; set; }
        public DateTime FechaEntrada { get; set; }

        public static Comparison<Paciente> CompararNombre = delegate (Paciente p1, Paciente p2)
        {
            return p1.Nombre.ToLower().CompareTo(p2.Nombre.ToLower());
        };

        public static Comparison<Paciente> CompararApellido = delegate (Paciente p1, Paciente p2)
        {
            return p1.Apellido.ToLower().CompareTo(p2.Apellido.ToLower());
        };

        public static Comparison<Paciente> CompararDpi = delegate (Paciente p1, Paciente p2)
        {
            return p1.DPI.CompareTo(p2.DPI);
        };

        public static Comparison<Paciente> CompararPrioridad = delegate (Paciente p1, Paciente p2)
        {
            return p1.CompareTo(p2);
        };

        public Paciente()
        {
        }

        public Paciente(string nombre, string apellido, int dpi, int edad, string departamento, string municipio, string sintomas, string descripcion)
        {
            Nombre = nombre;
            Apellido = apellido;
            DPI = dpi;
            Edad = edad;
            Departamento = departamento;
            Municipio = municipio;
            Sintomas = sintomas;
            Descripcion = descripcion;
            if (Edad < 4)
                Prioridad = 6;
            else if (Edad < 18)
                Prioridad = 8;
            else if (Edad < 60)
                Prioridad = 7;
            else
                Prioridad = 4;
            Estado = "Sospechoso";
            FechaEntrada = DateTime.Now;
        }

        public int CompareTo(object obj)
        {
            if (this.Prioridad.CompareTo(((Paciente)obj).Prioridad) == 0)
                return this.FechaEntrada.CompareTo(((Paciente)obj).FechaEntrada);
            else
                return this.Prioridad.CompareTo(((Paciente)obj).Prioridad);
        }

        public int HospitalMasCercano()
        {
            switch (Departamento.ToLower())
            {
                case "guatemala":
                case "sacatepequez":
                case "sacatepéquez":
                case "chimaltenango":
                case "el progreso":
                    return 0;
                case "quetzaltenango":
                case "san marcos":
                case "huehuetenango":
                case "totonicapan":
                case "totonicapán":
                case "retalhuleu":
                    return 1;
                case "peten":
                case "petén":
                case "alta verapaz":
                case "baja verapaz":
                case "quiche":
                case "quiché":
                    return 2;
                case "escuintla":
                case "suchitepequez":
                case "suchitepéquez":
                case "santa rosa":
                case "solola":
                case "sololá":
                    return 3;
                default:
                    return 4;
            }
        }

        public bool RealizarPrueba()
        {
            //int probabilidad = 5;
            int probabilidad = 50;
            //Agregar porcentajes
            Random rng = new Random();
            if (rng.Next(0, 100) < probabilidad)
            {
                Estado = "Confirmado";
                return true;
            }
            else
            {
                Estado = "Sano";
                return false;
            }
        }

        public LlavePaciente ToLlavePaciente()
        {
            return new LlavePaciente { DPI = this.DPI, Nombre = this.Nombre, Apellido = this.Apellido, Edad = this.Edad, Estado = this.Estado };
        }
    }
}