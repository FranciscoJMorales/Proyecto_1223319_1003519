using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_1223319_1003519.Helpers;
using Proyecto_1223319_1003519.Models;
using ClasesGenericas.Estructuras;
using System.Web.UI.WebControls;

namespace Proyecto_1223319_1003519.Controllers
{
    public class PacienteController : Controller
    {
        // GET: Paciente
        public ActionResult Index()
        {
            if (Storage.Instance.showAlert)
                Storage.Instance.showAlert = false;
            else
                ViewBag.Message = null;
            return View("Index", Storage.Instance.Hospitales);
        }

        // GET: Paciente/Details/5
        public ActionResult Details(int id)
        {
            List<Paciente> resultados = Storage.Instance.AVLDPI.Search(new Paciente { DPI = id }, Paciente.CompararDpi);
            ViewBag.Nombre = resultados[0].Nombre + " " + resultados[0].Apellido;
            return View("Paciente", resultados[0]);
        }

        // GET: Paciente/Create
        public ActionResult Create()
        {
            if (Storage.Instance.showAlert)
                Storage.Instance.showAlert = false;
            else
                ViewBag.Message = null;
            return View("Nuevo");
        }

        // POST: Paciente/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Storage.Instance.showAlert = true;
                Paciente paciente = new Paciente(collection["Nombre"], collection["Apellido"], Int32.Parse(collection["DPI"]), Int32.Parse(collection["edad"]),
                    collection["Departamento"], collection["Municipio"], collection["Sintomas"], collection["Descripcion"]);
                if (Valido(paciente))
                {
                    Storage.Instance.AVLNombre.Add(paciente, Paciente.CompararNombre);
                    Storage.Instance.AVLApellido.Add(paciente, Paciente.CompararApellido);
                    Storage.Instance.AVLDPI.Add(paciente, Paciente.CompararDpi);
                    Storage.Instance.Hospitales[paciente.HospitalMasCercano()].Add(paciente);
                    Storage.Instance.Stats.NuevoSospechoso();
                    ViewBag.Message = "El paciente se ha creado correctamente y fue asignado a la cola del Hospital " + Storage.Instance.Hospitales[paciente.HospitalMasCercano()].Nombre;
                    return Index();
                }
                else
                {
                    Storage.Instance.showAlert = false;
                    return View("Nuevo", paciente);
                }
            }
            catch
            {
                ViewBag.Message = "Hubo un error al crear al paciente. Datos no validos";
                return RedirectToAction("Create");
            }
        }

        public bool Valido(Paciente paciente)
        {
            bool valido = true;
            ViewBag.Message = "Hubo un error al crear al paciente";
            if (paciente.DPI <= 0)
            {
                ViewBag.Message += ". DPI no valido";
                valido = false;
            }
            else
            {
                List<Paciente> results = Storage.Instance.AVLDPI.Search(paciente, Paciente.CompararDpi);
                if (results.Count > 0)
                {
                    ViewBag.Message += ". DPI ya existe";
                    valido = false;
                }
            }
            if (paciente.Edad < 0)
            {
                ViewBag.Message += ". Edad no valida";
                valido = false;
            }
            if (paciente.HospitalMasCercano() == -1)
            {
                ViewBag.Message += ". Departamento no valido";
                valido = false;
            }
            return valido;
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

        public ActionResult VolverHospital()
        {
            return Hospital(Storage.Instance.HospitalActual);
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
            return View("Hospital", Storage.Instance.Hospitales[Storage.Instance.HospitalActual]);
        }

        public ActionResult ProximoPaciente()
        {
            Paciente valor = Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCola.Get();
            if (valor != null)
            {
                if (Storage.Instance.showAlert)
                    Storage.Instance.showAlert = false;
                else
                    ViewBag.Message = null;
                ViewBag.Nombre = valor.Nombre + " " + valor.Apellido;
                return View("ProximoPaciente", valor);
            }
            else
                return Cola();
        }

        public ActionResult Cola()
        {
            if (Storage.Instance.showAlert)
                Storage.Instance.showAlert = false;
            else
                ViewBag.Message = null;
            List<LlavePaciente> cola = new List<LlavePaciente>();
            foreach (Paciente item in Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCola)
                cola.Add(item.ToLlavePaciente());
            return View("Cola", cola);
        }

        public ActionResult Cama()
        {
            if (Storage.Instance.showAlert)
                Storage.Instance.showAlert = false;
            else
                ViewBag.Message = null;
            Cama[] camas = new Cama[10];
            for (int i =0; i < Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo.Length; i++)
            {
                if (Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo[i] != null)
                    camas[i] = new Cama(i + 1, Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo[i].ToLlavePaciente());
                else
                    camas[i] = new Cama(i + 1);
            }
            return View("Cama", camas);
        }

        public ActionResult Recuperado(int id)
        {
            Paciente nuevo = Storage.Instance.Hospitales[Storage.Instance.HospitalActual].RemoveFromCamas(new Paciente { DPI = id }, paciente => paciente.DPI.ToString());
            if (nuevo != null)
            {
                nuevo.Estado = "Recuperado";
                Storage.Instance.Stats.NuevoRecuperado();
                ViewBag.Message = "El paciente se ha dado de alta y ha salido del hospital";
                Storage.Instance.showAlert = true;
            }
            return Cama();
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

        public ActionResult Prueba()
        {
            Paciente nuevo = Storage.Instance.Hospitales[Storage.Instance.HospitalActual].RemoveFromCola();
            Storage.Instance.showAlert = true;
            if (nuevo.Estado == "Sospechoso")
            {
                if (nuevo.RealizarPrueba())
                {
                    nuevo.Estado = "Contagiado";
                    Storage.Instance.Stats.NuevoContagiado();
                    Storage.Instance.Hospitales[Storage.Instance.HospitalActual].Add(nuevo);
                    ViewBag.Message = "El resultado de la prueba fue: POSITIVO";
                    return Cama();
                }
                else
                {
                    nuevo.Estado = "Sano";
                    Storage.Instance.Stats.NuevoSano();
                    ViewBag.Message = "El resultado de la prueba fue: NEGATIVO";
                    return ProximoPaciente();
                }
            }
            else
            {
                ViewBag.Message = "La prueba ya fue realizada en este paciente. Liberar camas para asignar al paciente a una.";
                return Cama();
            }
        }

        public ActionResult Estadisticas()
        {
            return View(Storage.Instance.Stats);
        }
    }
}
