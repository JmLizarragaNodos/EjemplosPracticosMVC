using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class DataTableController : Controller
    {
        private static List<Probando> lista;

        static DataTableController()
        {
            // Así, la inicialización de datos se realiza solo una sola vez durante el ciclo de vida de la aplicación
            lista = new List<Probando>();

            Enumerable.Range(1, 50).ToList().ForEach(numero => {
                lista.Add(new Probando { codigo = numero, nombre = $"Nombre {numero}" });
            });
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerDatos(int start, int length)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            int page = (start / length) + 1;    // Calcular la página actual
            int pageSize = length;              // Tamaño de la página

            string filtro = Request.Form.GetValues("search[value]").FirstOrDefault();  // Obtener el filtro de búsqueda por nombre

            var datosFiltrados = string.IsNullOrEmpty(filtro)
                ? lista                                                     // Si no hay filtro, usar todos los datos
                : lista.Where(d => d.nombre.Contains(filtro)).ToList();     // Filtrar por nombre

            int totalRecords = datosFiltrados.Count;    // Número total de registros después de aplicar el filtro
            int offset = (page - 1) * pageSize;         // Cálculo del offset

            var retorno = datosFiltrados.Skip(offset).Take(pageSize).ToList();

            return Json(new Grilla()
            {
                draw = draw,
                recordsFiltered = totalRecords,
                recordsTotal = totalRecords,
                data = retorno
            });
        }


        public class Probando
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
        }

        public class Grilla
        {
            public string draw { get; set; }
            public decimal recordsFiltered { get; set; }
            public decimal recordsTotal { get; set; }
            public object data { get; set; }

            public Grilla()
            {
                draw = "";
                recordsFiltered = 0;
                recordsTotal = 0;
                data = new List<object>();
            }
        }
    }
}