using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClasesGenericas.Estructuras;

namespace Proyecto_1223319_1003519.Models
{
    //Clase que almacena toda la información de un hospital
    public class Hospital
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Camas { get; set; } = 0;
        public int Cola { get; set; } = 0;
        public TablaHash<Paciente> EstadoCamas = new TablaHash<Paciente>();
        public ColaPrioridad<Paciente> EstadoCola = new ColaPrioridad<Paciente>();

        //Agrega a los nuevos pacientes al lugar correspondiente
        public void Add(Paciente nuevo)
        {
            //Si ya están contagiados, los inserta en una cama si hay espacio, o en la cola si las camas están llenas
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
                //Si es sospechoso se ingresa directamente a la cola
                EstadoCola.Add(nuevo, Paciente.CompararPrioridad);
                Cola++;
            }
        }

        //Elimina a un paciente de la cola y lo devuelve
        public Paciente RemoveFromCola()
        {
            Paciente valor = EstadoCola.Remove(Paciente.CompararPrioridad);
            if (valor != null)
                Cola--;
            return valor;
        }

        //Elimina y devuelve a un paciente de las camas
        public Paciente RemoveFromCamas(Paciente value, Func<Paciente,string> llave)
        {
            if (value.DPI != "---")
            {
                Paciente valor = EstadoCamas.Remove(value, llave);
                if (valor != null)
                {
                    Camas--;
                    //Revisa si el próximo paciente en la cola está confirmado para trasladarlo a la cama
                    if (EstadoCola.Get() != null)
                    {
                        if (EstadoCola.Get().Estado == "Contagiado")
                        {
                            EstadoCamas.Add(EstadoCola.Remove(Paciente.CompararPrioridad), llave);
                            Camas++;
                            Cola--;
                        }
                    }
                }
                return valor;
            }
            else
                return null;
        }
    }
}