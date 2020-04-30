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
            Hospitales[0] = new Hospital { Nombre = "Capital" };
            Hospitales[1] = new Hospital { Nombre = "Quetzaltenango" };
            Hospitales[2] = new Hospital { Nombre = "Peten" };
            Hospitales[3] = new Hospital { Nombre = "Escuintla" };
            Hospitales[4] = new Hospital { Nombre = "Oriente" };
        }
        
        public static Storage Instance
        {
            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }

        public AVL<LlavePaciente> AVLNombre = new AVL<LlavePaciente>();
        public TablaHashGeneral<Paciente> infoTareas = new TablaHashGeneral<Paciente>();
        public Hospital[] Hospitales = new Hospital[5];
        /*public bool admin = false;
        public bool PrimeraSesion = true;
        public string name;
        public ColaPrioridad<TituloTarea> tareasUsuario = new ColaPrioridad<TituloTarea>();
        public Arbol<Farmaco> Indice = new Arbol<Farmaco>();
        public Arbol<Farmaco> SinExistencias = new Arbol<Farmaco>();
        public List<FarmacoPrecio> Pedidos = new List<FarmacoPrecio>();
        public string Farmacos;
        public string[] ListadoFarmacos;
        public string dir;*/
    }
}