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
            if (nuevo.Estado == "Contagiado")
            {
                if (EstadoCamas.isFull)
                {
                    EstadoCola.Add(nuevo, Paciente.CompararPrioridad);
                    Cola++;
                }
                else
                {
                    EstadoCamas.Add(nuevo, paciente => paciente.DPI.ToString());
                    Camas++;
                }
            }
            else
            {
                EstadoCola.Add(nuevo, Paciente.CompararPrioridad);
                Cola++;
            }
        }

        public Paciente RemoveFromCola()
        {
            Paciente valor = EstadoCola.Remove(Paciente.CompararPrioridad);
            if (valor != null)
                Cola--;
            return valor;
        }

        public Paciente RemoveFromCamas(Paciente value, Func<Paciente,string> llave)
        {
            if (value.DPI != 0)
            {
                Paciente valor = EstadoCamas.Remove(value, llave);
                if (valor != null)
                    Camas--;
                return valor;
            }
            else
                return null;
        }
    }
}