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
            List<Paciente> resultados = Storage.Instance.AVLDPI.Search(new Paciente { DPI = id }, Paciente.CompararDpi);
            return View("Paciente", resultados[0]);
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
                    collection["Departamento"], collection["Municipio"], collection["Sintomas"], collection["Descripcion"]);
                paciente.ToLlavePaciente();
                Storage.Instance.AVLNombre.Add(paciente, Paciente.CompararNombre);
                Storage.Instance.AVLApellido.Add(paciente, Paciente.CompararApellido);
                Storage.Instance.AVLDPI.Add(paciente, Paciente.CompararDpi);
                Storage.Instance.Hospitales[paciente.HospitalMasCercano()].Add(paciente);
                Storage.Instance.Stats.NuevoSospechoso();
                /*if (paciente.RealizarPrueba())
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
                  


                }*/
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Create");
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
            switch (id)
            {
                case 0:
                    ViewBag.Hospital = "Capital";
                    break;
                case 1:
                    ViewBag.Hospital = "Quetzaltenango";
                    break;
                case 2:
                    ViewBag.Hospital = "Peten";
                    break;
                case 3:
                    ViewBag.Hospital = "Escuintla";
                    break;
                default:
                    ViewBag.Hospital = "Oriente";
                    break;
            }
            return View(Storage.Instance.Hospitales[Storage.Instance.HospitalActual]);
        }

        public ActionResult ProximoPaciente()
        {
            Paciente valor = Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCola.Get();
            if (valor != null)
                return View("ProximoPaciente", valor);
            else
                return RedirectToAction("Cola");
        }

        public ActionResult Cola()
        {
            List<LlavePaciente> cola = new List<LlavePaciente>();
            foreach (Paciente item in Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCola)
                cola.Add(item.ToLlavePaciente());
            return View("Cola", cola);
        }

        public ActionResult Cama()
        {
            Cama[] camas = new Cama[10];
            for (int i =0; i < Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo.Length; i++)
            {
                if (Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo[i] != null)
                    camas[i] = new Cama(i, Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo[i].ToLlavePaciente());
                else
                    camas[i] = new Cama(i);
            }
            return View(camas);
        }

        public ActionResult Recuperado(int id)
        {
            Paciente nuevo = Storage.Instance.Hospitales[Storage.Instance.HospitalActual].RemoveFromCamas(new Paciente { DPI = id }, paciente => paciente.DPI.ToString());
            if (nuevo != null)
            {
                nuevo.Estado = "Recuperado";
                Storage.Instance.Stats.NuevoRecuperado();
            }
            return RedirectToAction("Cama");
        }

        [HttpPost]
        public ActionResult BuscarNombre(FormCollection collection)
        {
            try
            {
                List<Paciente> resultados = Storage.Instance.AVLNombre.Search(new Paciente { Nombre = collection["text"] }, Paciente.CompararNombre);
                List<LlavePaciente> result = new List<LlavePaciente>();
                foreach (Paciente item in resultados)
                    result.Add(item.ToLlavePaciente());
                return Resultados(result);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult BuscarApellido(FormCollection collection)
        {
            try
            {
                List<Paciente> resultados = Storage.Instance.AVLApellido.Search(new Paciente { Apellido = collection["text"] }, Paciente.CompararApellido);
                List<LlavePaciente> result = new List<LlavePaciente>();
                foreach (Paciente item in resultados)
                    result.Add(item.ToLlavePaciente());
                return Resultados(result);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult BuscarDPI(FormCollection collection)
        {
            try
            {
                List<Paciente> resultados = Storage.Instance.AVLDPI.Search(new Paciente { DPI = int.Parse(collection["text"]) }, Paciente.CompararDpi);
                List<LlavePaciente> result = new List<LlavePaciente>();
                foreach (Paciente item in resultados)
                    result.Add(item.ToLlavePaciente());
                return Resultados(result);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Resultados(List<LlavePaciente> result)
        {
            ViewBag.Resultados = result.Count;
            return View("Resultados", result);
        }

        //public ActionResult BuscarTH(Comparison<Paciente> parametro, int hospital, Paciente p1)
        //{
        //    if (Storage.Instance.Hospitales[hospital]. (p1, Paciente.CompararDpi).Equals(p1.DPI))
        //    {

        //        return RedirectToAction("Resultado");
        //    }

        //    return RedirectToAction("Listas");
        //}

        public ActionResult Prueba()
        {
            Paciente nuevo = Storage.Instance.Hospitales[Storage.Instance.HospitalActual].RemoveFromCola();
            if (nuevo.RealizarPrueba())
            {
                nuevo.Estado = "Contagiado";
                Storage.Instance.Stats.NuevoContagiado();
                Storage.Instance.Hospitales[Storage.Instance.HospitalActual].Add(nuevo);
                return RedirectToAction("");
            }
            else
            {
                nuevo.Estado = "Sano";
                Storage.Instance.Stats.NuevoSano();
                return RedirectToAction("ProximoPaciente");
            }
        }

        public ActionResult Estadisticas()
        {
            return View(Storage.Instance.Stats);
        }
    }
}
