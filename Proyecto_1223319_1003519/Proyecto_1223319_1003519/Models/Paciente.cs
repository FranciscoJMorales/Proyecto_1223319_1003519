using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_1223319_1003519.Models
{
    //Clase donde se almacenan todos los datos de un paciente
    public class Paciente : IComparable
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string DPI { get; set; }
        [Required]
        public int Edad { get; set; }
        [Required]
        public string Departamento { get; set; }
        [Required]
        public string Municipio { get; set; }
        [Required]
        public string Sintomas { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public int Prioridad { get; set; }
        public string Estado { get; set; }
        public DateTime FechaEntrada { get; set; }

        //Delegados de comparaciones usados en los AVL
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

        //Constructor por defecto
        public Paciente()
        {
        }

        //Constructor que asigna automáticamente la prioridad, el estado y la fecha de entrada
        public Paciente(string nombre, string apellido, string dpi, int edad, string departamento, string municipio, string sintomas, string descripcion)
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

        //Método heredado de IComparable
        public int CompareTo(object obj)
        {
            if (this.Prioridad.CompareTo(((Paciente)obj).Prioridad) == 0)
                return this.FechaEntrada.CompareTo(((Paciente)obj).FechaEntrada);
            else
                return this.Prioridad.CompareTo(((Paciente)obj).Prioridad);
        }

        //Método que devuelve a qué hospital se debe de asignar al paciente
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
                case "izabal":
                case "zacapa":
                case "jalapa":
                case "chiquimula":
                case "jutiapa":
                    return 4;
                default:
                    return -1;
            }
        }

        //Método que realiza la prueba del covid-19 en base a la descripción del paciente. Devuelve si el resultado fue positivo o negativo
        public bool RealizarPrueba()
        {
            int probabilidad = 5;
            //Palabras clave
            string[] ProbEuropa = new string[] {
                "viaje", "europa", "españa", "italia", "francia", "alemania", "tour", "reino unido", "inglaterra", "belgica",
                "bélgica", "pais", "país", "europeo"
            };
            string[] ConocidoCont = new string[] {
                "amig", "novi", "vecin", "conocid"
            };
            string[] FamiliarCont = new string[] {
                "mama", "mamá", "madre", "papa", "papá", "padre", "herman", "prim", "tio", "tío", "tia", "tía", "abuel", "suegr",
                "espos", "hij", "familia"
            };
            string[] ReunionSocial = new string[] {
                "fiesta", "reunion", "reunión", "trabajo", "velada", "agrupación", "agrupacion", "celebración", "celebracion", "asamblea",
                "grupo", "restaurante", "hotel", "spa", "social"
            };
            //Se revisa si contiene alguna de las palabras clave para aumentar el porcentaje
            for (int i = 0; i < ProbEuropa.Length; i++)
            {
                if (Descripcion.ToLower().Contains(ProbEuropa[i]))
                {
                    probabilidad += 10;
                    i = ProbEuropa.Length;
                }
            }
            for (int i = 0; i < ConocidoCont.Length; i++)
            {
                if (Descripcion.ToLower().Contains(ConocidoCont[i]))
                {
                    probabilidad += 15;
                    i = ConocidoCont.Length;
                }
            }
            for (int i = 0; i < FamiliarCont.Length; i++)
            {
                if (Descripcion.ToLower().Contains(FamiliarCont[i]))
                {
                    probabilidad += 30;
                    i = FamiliarCont.Length;
                }
            }
            for (int i = 0; i < ReunionSocial.Length; i++)
            {
                if (Descripcion.ToLower().Contains(ReunionSocial[i]))
                {
                    probabilidad += 5;
                    i = ReunionSocial.Length;
                }
            }
            Random rng = new Random();
            //Se cambia el estado y la prioridad dependiendo del resultado de la prueba
            if (rng.Next(0, 100) < probabilidad)
            {
                Estado = "Contagiado";
                switch (Prioridad)
                {
                    case 4:
                        Prioridad = 1;
                        break;
                    case 6:
                        Prioridad = 2;
                        break;
                    case 7:
                        Prioridad = 3;
                        break;
                    case 8:
                        Prioridad = 5;
                        break;
                }
                return true;
            }
            else
            {
                Estado = "Sano";
                Prioridad = 100;
                return false;
            }
        }

        //Convierte un paciente a la clase más sencilla LlavePaciente
        public LlavePaciente ToLlavePaciente()
        {
            return new LlavePaciente { DPI = this.DPI, Nombre = this.Nombre, Apellido = this.Apellido, Edad = this.Edad, Estado = this.Estado };
        }
    }
}