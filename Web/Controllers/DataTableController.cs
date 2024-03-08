using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Dto;

namespace Web.Controllers
{
    public class DataTableController : Controller
    {
        private static List<Probando> lista;

        static DataTableController()
        {
            // Así, la inicialización de datos se realiza solo una sola vez durante el ciclo de vida de la aplicación
            lista = new List<Probando>();

            Enumerable.Range(1, 55).ToList().ForEach(numero => {
                lista.Add(new Probando { codigo = numero, nombre = $"Nombre {numero}" });
            });
        }

        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult ObtenerDatos(int start, int length)   // https://localhost:44353/DataTable/ObtenerDatos?start=0&length=10
        {
            var draw = (Request.Form.GetValues("draw") != null) ?
                Request.Form.GetValues("draw").FirstOrDefault() : null;

            int totalRecords = 0;
            var retorno = new List<Probando>();

            if (length != -1)  // Si es distinto de traer todo
            {
                int page = (start / length) + 1;    // Calcular la página actual
                int pageSize = length;              // Tamaño de la página

                var filtro = (Request.Form.GetValues("search[value]") != null) ? Request.Form.GetValues("search[value]").FirstOrDefault() : null;

                var datosFiltrados = string.IsNullOrEmpty(filtro)
                    ? lista                                                     // Si no hay filtro, usar todos los datos
                    : lista.Where(d => d.nombre.Contains(filtro)).ToList();     // Filtrar por nombre

                totalRecords = datosFiltrados.Count;    // Número total de registros después de aplicar el filtro
                int offset = (page - 1) * pageSize;         // Cálculo del offset

                retorno = datosFiltrados.Skip(offset).Take(pageSize).ToList();
            }
            else
            {
                retorno = lista;
                totalRecords = lista.Count;
            }

            return Json(new Grilla()
            {
                draw = draw,
                recordsFiltered = totalRecords,
                recordsTotal = totalRecords,
                data = retorno
            }, JsonRequestBehavior.AllowGet);
        }

        public class Probando
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
        }

    }
}