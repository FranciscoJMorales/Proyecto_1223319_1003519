using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_1223319_1003519.Models
{
    public class Estadisticas
    {
        public int Contagiados { get; set; } = 0;
        public int Sospechosos { get; set; } = 0;
        public double Porcentaje { get; set; } = 0;
        public int Recuperados { get; set; } = 0;
        public int Sanos { get; set; } = 0;

        public void NuevoSospechoso()
        {
            Sospechosos++;
        }

        public void NuevoContagiado()
        {
            Contagiados++;
            Sospechosos--;
            Porcentaje = 100 * ((Contagiados + Recuperados) / (Contagiados + Recuperados + Sanos));
        }

        public void NuevoSano()
        {
            Sospechosos--;
            Sanos++;
        }

        public void NuevoRecuperado()
        {
            Recuperados++;
            Contagiados--;
        }
    }
}