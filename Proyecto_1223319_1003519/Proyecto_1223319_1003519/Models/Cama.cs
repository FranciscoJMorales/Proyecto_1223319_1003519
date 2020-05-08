using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_1223319_1003519.Models
{
    //Modelo que indica la información de una cama y del paciente que hospeda o si está vacía
    public class Cama
    {
        public int No { get; set; }
        public string DPI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Estado { get; set; }

        //Si la cama está vacía, llena los valores con datos nulos
        public Cama(int id)
        {
            No = id;
            DPI = "---";
            Nombre = "---";
            Apellido = "---";
            Edad = 0;
            Estado = "---";
        }

        public Cama(int id, LlavePaciente nuevo)
        {
            No = id;
            DPI = nuevo.DPI;
            Nombre = nuevo.Nombre;
            Apellido = nuevo.Apellido;
            Edad = nuevo.Edad;
            Estado = nuevo.Estado;
        }
    }
}