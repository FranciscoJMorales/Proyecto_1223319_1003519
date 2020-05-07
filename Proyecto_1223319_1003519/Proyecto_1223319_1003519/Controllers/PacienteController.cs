using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_1223319_1003519.Helpers;
using Proyecto_1223319_1003519.Models;
using ClasesGenericas.Estructuras;

namespace Proyecto_1223319_1003519.Controllers
{
    public class PacienteController : Controller
    {
        // GET: Paciente
        public ActionResult Index()
        {
            return View(Storage.Instance.Hospitales);
        }

        // GET: Paciente/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Paciente/Create
        public ActionResult Create()
        {
            return View("Nuevo");
        }
       
        // POST: Paciente/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {

                Paciente paciente = new Paciente(collection["Nombre"], collection["Apellido"], Int32.Parse(collection["DPI"]), Int32.Parse(collection["edad"]),
                    collection["Departamento"], collection["Municipio"], collection["Sintomas"], collection["Descripcion"])
                {
                
                };
                paciente.ToLlavePaciente();
                Storage.Instance.AVLNombre.Add(paciente, Paciente.CompararNombre);
                Storage.Instance.AVLApellido.Add(paciente, Paciente.CompararApellido);
                Storage.Instance.AVLDPI.Add(paciente, Paciente.CompararDpi);
                Storage.Instance.Hospitales[paciente.HospitalMasCercano()].EstadoCola.Add(paciente, Paciente.CompararNombre);
                if (paciente.RealizarPrueba())
                {
                    if (Storage.Instance.Hospitales[1].EstadoCamas.isFull)
                    {
                        Storage.Instance.Hospitales[paciente.HospitalMasCercano()].Cola++;
                        Storage.Instance.Hospitales[paciente.HospitalMasCercano()].EstadoCola.Add(paciente, Paciente.CompararDpi);                     
                    }
                    else
                    {
                       // Storage.Instance.Hospitales[paciente.HospitalMasCercano()].EstadoCamas.Add(paciente, p1 => Convert.ToString(p1.DPI));
                        Storage.Instance.Hospitales[paciente.HospitalMasCercano()].Camas++;
                    }
                  


                }
                return RedirectToAction("Index");
            }
            catch
           {

                return View("Nuevo");
            }
        }

        // GET: Paciente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Paciente/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Paciente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Paciente/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        public ActionResult Hospital(int id)
        {
            Storage.Instance.HospitalActual = id;
            return View(Storage.Instance.Hospitales[id]);
        }
        public ActionResult Pacientee(int id)
        {
            Storage.Instance.HospitalActual = id;
            return View(Storage.Instance.Hospitales[id]);
        }
        public ActionResult Cama()
        {


            return View(Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCola.Get().ToLlavePaciente());
        }
        public ActionResult Cola()
        {

            //if (Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCola.Get() != null)
            //{
            //    ColaPrioridad<LlavePaciente> info = new ColaPrioridad<LlavePaciente>();
            //    ColaPrioridad<Paciente> cola = Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCola;
            //    foreach (Paciente infopaciente in cola)
            //    {
            //        info.Add(infopaciente.ToLlavePaciente(), Paciente.CompararNombre);
            //    }
            //    return View(cola);
            //}

            //else
               return RedirectToAction("Index");   
        }
        public ActionResult PacienteTH()
        {
            return View(Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas);
        }


        public ActionResult BuscarNombre(string text)
        {
            Paciente p1 = new Paciente(text.ToLower(), "", 0, 0, "", "", "", "") {
            
            };
            return BuscarAVL(Paciente.CompararNombre, 1,p1);
        }
       
        public ActionResult BuscarApellido(string text)
        {
            Paciente p1 = new Paciente("", text.ToLower(), 0, 0, "", "", "", "") {
               
            };
            return BuscarAVL(Paciente.CompararNombre, 2,p1);
        }
       
        public ActionResult BuscarDPI(string text)
        {
            Paciente p1 = new Paciente("", "", Int32.Parse(text), 0, "", "", "", "")
            {
               
            };
            return BuscarAVL(Paciente.CompararNombre, 3, p1);
        }
        
        //public ActionResult BuscarTH(Comparison<Paciente> parametro, int hospital, Paciente p1)
        //{
        //    if (Storage.Instance.Hospitales[hospital]. (p1, Paciente.CompararDpi).Equals(p1.DPI))
        //    {

        //        return RedirectToAction("Resultado");
        //    }

        //    return RedirectToAction("Listas");
        //}
        public ActionResult BuscarAVL(Comparison<Paciente> parametro,int arbol, Paciente p1)
        {
            List<LlavePaciente> info = new List<LlavePaciente>();
            List<Paciente> pacientes = new List<Paciente>();
            switch (arbol)
            {
                case 1:
                    
                    pacientes = Storage.Instance.AVLNombre.Search(p1, Paciente.CompararNombre);
                    
                    foreach (Paciente infopaciente in pacientes)
                    {
                        info.Add(infopaciente.ToLlavePaciente());
                    }
                    return View("Resultados",info);
                case 2:
                    pacientes = Storage.Instance.AVLApellido.Search(p1, Paciente.CompararApellido);
                    foreach (Paciente infopaciente in pacientes)
                    {
                        info.Add(infopaciente.ToLlavePaciente());
                    }

                    return View("Resultados", info);
                  
                   
                    
                case 3:
                    pacientes = Storage.Instance.AVLDPI.Search(p1, Paciente.CompararDpi);
                    foreach (Paciente infopaciente in pacientes)
                    {
                        info.Add(infopaciente.ToLlavePaciente());
                    }

                    return View("Resultados", info);

            }
            return RedirectToAction("Index");
        }

        


    }
}
