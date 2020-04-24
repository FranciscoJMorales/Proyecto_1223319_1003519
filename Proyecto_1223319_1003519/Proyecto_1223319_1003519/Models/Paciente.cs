using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_1223319_1003519.Models
{
    public class Paciente
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DPI { get; set; }
        public int Edad { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Sintomas { get; set; }
        public string Causas { get; set; }
    }
}