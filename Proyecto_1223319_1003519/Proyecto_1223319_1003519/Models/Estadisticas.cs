using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_1223319_1003519.Models
{
    //Clase donde se almacenan todas las estadísticas de los pacientes
    public class Estadisticas
    {
        public int Contagiados { get; set; } = 0;
        public int Sospechosos { get; set; } = 0;
        public double Porcentaje { get; set; } = 0;
        public int Recuperados { get; set; } = 0;
        public int Sanos { get; set; } = 0;

        //Métodos utilizados cada vez que se ingresa un paciente o cambia su estado
        public void NuevoSospechoso()
        {
            Sospechosos++;
        }

        public void NuevoContagiado()
        {
            Contagiados++;
            Sospechosos--;
            CambiarPorcentaje();
        }

        public void NuevoSano()
        {
            Sospechosos--;
            Sanos++;
            CambiarPorcentaje();
        }

        public void NuevoRecuperado()
        {
            Recuperados++;
            Contagiados--;
        }

        private void CambiarPorcentaje()
        {
            Porcentaje = Math.Round(100.00 * ((Convert.ToDouble(Contagiados) + Convert.ToDouble(Recuperados)) / (Convert.ToDouble(Contagiados) + Convert.ToDouble(Recuperados) + Convert.ToDouble(Sanos))), 2);
        }
    }
}