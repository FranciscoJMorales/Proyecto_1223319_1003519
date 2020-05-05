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
        public ActionResult Cola()
        {
            return View("cola");
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
                    Storage.Instance.Hospitales[paciente.HospitalMasCercano()].EstadoCamas.Add(paciente, Paciente => Paciente.ToLlavePaciente().Nombre);
                }
                return RedirectToAction("Index");
            }
            catch
            {

                return View("Index");
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

        public ActionResult BuscarNombre(string text)
        {
            Paciente p1 = new Paciente( text.ToLower(), "", 0, 0, "", "", "", "") {
                Nombre = text
            };
            return Buscar(Paciente.CompararNombre, 1,p1);
        }

        public ActionResult BuscarApellido(string text)
        {
            Paciente p1 = new Paciente("", text.ToLower(), 0, 0, "", "", "", "") {
                Apellido = text
            };
            return Buscar(Paciente.CompararNombre, 2,p1);
        }
        public ActionResult BuscarDPI(int text)
        {
            Paciente p1 = new Paciente("", "", text, 0, "", "", "", "")
            {
                DPI = text
            };
            return Buscar(Paciente.CompararNombre, 3, p1);
        }
        public ActionResult Buscar(Comparison<Paciente> parametro,int arbol, Paciente p1)
        {
            switch (arbol)
            {
                case 1:
                    if(Storage.Instance.AVLNombre.Search(p1, Paciente.CompararNombre).Equals(p1.Nombre))
                    {
                       
                        return RedirectToAction("Resultado");
                    }
                    break;

                case 2:
                    if (Storage.Instance.AVLApellido.Search(p1, Paciente.CompararApellido).Equals(p1.Apellido))
                    {
                        
                        return RedirectToAction("Resultado");
                    }
                    break;

                case 3:
                    if (Storage.Instance.AVLDPI.Search(p1, Paciente.CompararDpi).Equals(p1.DPI))
                    {
                        return RedirectToAction("Resultado");
                    }
                    
                    break;
            }
            return RedirectToAction("Listas");
    }
    }
}
