using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_1223319_1003519.Models
{
    //Clase que contiene los datos más importantes de un paciente
    public class LlavePaciente
    {
        public string DPI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Estado { get; set; }
    }
}