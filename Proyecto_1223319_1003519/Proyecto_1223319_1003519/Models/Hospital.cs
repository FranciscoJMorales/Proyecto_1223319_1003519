using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClasesGenericas.Estructuras;

namespace Proyecto_1223319_1003519.Models
{
    public class Hospital
    {
        public string Nombre { get; set; }
        public int Camas { get; set; } = 0;
        public int Cola { get; set; } = 0;
        public TablaHash<LlavePaciente> EstadoCamas = new TablaHash<LlavePaciente>();
        public ColaPrioridad<LlavePaciente> EstadoCola = new ColaPrioridad<LlavePaciente>();
    }
}