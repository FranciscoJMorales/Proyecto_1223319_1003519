using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proyecto_1223319_1003519.Models;
using ClasesGenericas.Estructuras;

namespace Proyecto_1223319_1003519.Helpers
{
    public class Storage
    {
        private static Storage _instance;

        public Storage()
        {
            Hospitales[0] = new Hospital { ID = 0, Nombre = "Capital" };
            Hospitales[1] = new Hospital { ID = 1, Nombre = "Quetzaltenango" };
            Hospitales[2] = new Hospital { ID = 2, Nombre = "Peten" };
            Hospitales[3] = new Hospital { ID = 3, Nombre = "Escuintla" };
            Hospitales[4] = new Hospital { ID = 4, Nombre = "Oriente" };
        }
        
        public static Storage Instance
        {
            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        public AVL<Paciente> AVLNombre = new AVL<Paciente>();
        public AVL<Paciente> AVLApellido = new AVL<Paciente>();
        public AVL<Paciente> AVLDPI = new AVL<Paciente>();
        public Hospital[] Hospitales = new Hospital[5];
        public Estadisticas Stats = new Estadisticas();
        public int HospitalActual = 0;
        public bool showAlert = false;
    }
}