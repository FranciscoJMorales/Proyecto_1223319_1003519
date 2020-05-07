using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClasesGenericas.Estructuras;

namespace Proyecto_1223319_1003519.Models
{
    public class Hospital
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Camas { get; set; } = 0;
        public int Cola { get; set; } = 0;
        public TablaHash<Paciente> EstadoCamas = new TablaHash<Paciente>();
        public ColaPrioridad<Paciente> EstadoCola = new ColaPrioridad<Paciente>();

        public void Add(Paciente nuevo)
        {
            EstadoCola.Add(nuevo, Paciente.CompararPrioridad);
            Cola++;
        }
    }
}