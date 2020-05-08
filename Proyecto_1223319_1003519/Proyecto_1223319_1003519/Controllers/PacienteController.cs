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
        //Devuelve la pantalla principal y muestra la alerta si existe
        public ActionResult Index()
        {
            if (Storage.Instance.showAlert)
                Storage.Instance.showAlert = false;
            else
                ViewBag.Message = null;
            return View("Index", Storage.Instance.Hospitales);
        }

        // GET: Paciente/Details/5
        //Devuelve la vista con los detalles del paciente recibido
        public ActionResult Details(string id)
        {
            List<Paciente> resultados = Storage.Instance.AVLDPI.Search(new Paciente { DPI = id }, Paciente.CompararDpi);
            ViewBag.Nombre = resultados[0].Nombre + " " + resultados[0].Apellido;
            return View("Paciente", resultados[0]);
        }

        // GET: Paciente/Create
        //Devuelve la vista para ingresar a un nuevo paciente y muestra la alerta si existe
        public ActionResult Create()
        {
            if (Storage.Instance.showAlert)
                Storage.Instance.showAlert = false;
            else
                ViewBag.Message = null;
            return View("Nuevo");
        }

        // POST: Paciente/Create
        //Crea al paciente con los datos recibidos y lo almacena en las estructuras correspondientes
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Storage.Instance.showAlert = true;
                Paciente paciente = new Paciente(collection["Nombre"], collection["Apellido"], collection["DPI"], Int32.Parse(collection["edad"]),
                    collection["Departamento"], collection["Municipio"], collection["Sintomas"], collection["Descripcion"]);
                //Si los datos son válidos, almacena al paciente en las estructuras correspondientes
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
                    //Si no son válidos, vuelve a la vista de crear nuevo paciente
                    Storage.Instance.showAlert = false;
                    return View("Nuevo", paciente);
                }
            }
            catch
            {
                //Si no son válidos, vuelve a la vista de crear nuevo paciente
                ViewBag.Message = "Hubo un error al crear al paciente. Datos no validos";
                return RedirectToAction("Create");
            }
        }

        //Método que devuelve si los datos para el paciente ingresado son válidos
        public bool Valido(Paciente paciente)
        {
            //El método revisa si:
            bool valido = true;
            ViewBag.Message = "Hubo un error al crear al paciente";
            //No hay letras en el dpi
            if (long.TryParse(paciente.DPI, out long dpi))
            {
                //El dpi es un número válido
                if (dpi <= 0)
                {
                    ViewBag.Message += ". DPI no valido";
                    valido = false;
                }
                else
                {
                    List<Paciente> results = Storage.Instance.AVLDPI.Search(paciente, Paciente.CompararDpi);
                    //El dpi no ha sido ingresado anteriormente
                    if (results.Count > 0)
                    {
                        ViewBag.Message += ". DPI ya existe";
                        valido = false;
                    }
                }
            }
            else
            {
                ViewBag.Message += ". DPI no valido";
                valido = false;
            }
            //La edad del paciente no es negativa
            if (paciente.Edad < 0)
            {
                ViewBag.Message += ". Edad no valida";
                valido = false;
            }
            //El departamento ingresado existe
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

        //Muestra al último hospital visitado
        public ActionResult VolverHospital()
        {
            return Hospital(Storage.Instance.HospitalActual);
        }

        //Muestra la vista del hospital indicado
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

        //Muestra los datos del paciente con mayor prioridad en la cola
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
            //Si la cola está vacía, muestra la vista de la cola vacía
            else
                return Cola();
        }

        //Muestra el estado de todos los pacientes en la cola
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

        //Muestra el estado de todas las camas en el hospital
        public ActionResult Cama()
        {
            if (Storage.Instance.showAlert)
                Storage.Instance.showAlert = false;
            else
                ViewBag.Message = null;
            Cama[] camas = new Cama[10];
            //Convierte los elementos de la tabla hash de Paciente a Cama
            for (int i =0; i < Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo.Length; i++)
            {
                if (Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo[i] != null)
                    camas[i] = new Cama(i + 1, Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.Arreglo[i].ToLlavePaciente());
                else
                    camas[i] = new Cama(i + 1);
            }
            return View("Cama", camas);
        }

        //Método que cambia el estado de un paciente contagiado a recuperado y lo elimina de su cama
        public ActionResult Recuperado(string id)
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

        //Búsqueda por nombre
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

        //Búsqueda por apellido
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

        //Búsqueda por DPI
        [HttpPost]
        public ActionResult BuscarDPI(FormCollection collection)
        {
            try
            {
                List<Paciente> resultados = Storage.Instance.AVLDPI.Search(new Paciente { DPI = collection["text"] }, Paciente.CompararDpi);
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

        //Muestra la vista con los resultados de una búsqueda
        public ActionResult Resultados(List<LlavePaciente> result)
        {
            ViewBag.Resultados = result.Count;
            return View("Resultados", result);
        }

        //Método que realiza la prueba del covid-19 en un paciente, y lo mueve a la estructura correspondiente dependiendo del resultado
        public ActionResult Prueba()
        {
            Paciente nuevo = Storage.Instance.Hospitales[Storage.Instance.HospitalActual].RemoveFromCola();
            Storage.Instance.showAlert = true;
            if (nuevo.Estado == "Sospechoso")
            {
                if (nuevo.RealizarPrueba())
                {
                    //Si está contagiado, lo mueve a una cama si hay espacio y muestra el estado de las camas
                    nuevo.Estado = "Contagiado";
                    Storage.Instance.Stats.NuevoContagiado();
                    Storage.Instance.Hospitales[Storage.Instance.HospitalActual].Add(nuevo);
                    ViewBag.Message = "El resultado de la prueba fue: POSITIVO";
                    if (Storage.Instance.Hospitales[Storage.Instance.HospitalActual].EstadoCamas.isFull)
                        return ProximoPaciente();
                    else
                        return Cama();
                }
                else
                {
                    //Si está sano, únicamente lo elimina de la cola y muestra al próximo paciente
                    nuevo.Estado = "Sano";
                    Storage.Instance.Stats.NuevoSano();
                    ViewBag.Message = "El resultado de la prueba fue: NEGATIVO";
                    return ProximoPaciente();
                }
            }
            else
            {
                //Muestra este mensaje si se intenta realizar la prueba en un paciente ya confirmado
                ViewBag.Message = "La prueba ya fue realizada en este paciente. Liberar camas para asignar al paciente a una.";
                return Cama();
            }
        }

        //Muestra la vista con las estadísticas de los pacientes
        public ActionResult Estadisticas()
        {
            return View(Storage.Instance.Stats);
        }
    }
}
