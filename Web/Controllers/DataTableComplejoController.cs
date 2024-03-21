using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Dto;

namespace Web.Controllers
{
    public class DataTableComplejoController : Controller
    {
        private static List<Probando> _lista;

        static DataTableComplejoController()
        {
            // Así, la inicialización de datos se realiza solo una sola vez durante el ciclo de vida de la aplicación
            _lista = new List<Probando>();

            Enumerable.Range(1, 55).ToList().ForEach(numero => {
                _lista.Add(new Probando { codigo = numero, nombre = $"Nombre {numero}" });
            });
        }

        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult ObtenerDatos(int start, int length, bool cargaInicial)   // https://localhost:44353/DataTableComplejo/ObtenerDatos?start=0&length=10?cargaInicial=true
        {
            var listaNumeros = new List<int>();

            if (cargaInicial)
            {
                listaNumeros = new List<int>() { 1, 2, 3, 4 };
            }

            var draw = (Request.Form.GetValues("draw") != null) ?
                Request.Form.GetValues("draw").FirstOrDefault() : null;

            int totalRegistros = 0;
            var datosGrilla = new List<Probando>();

            if (length != -1)  // Si es distinto de traer todo
            {
                int page = (start / length) + 1;    // Calcular la página actual
                int pageSize = length;              // Tamaño de la página

                var filtro = (Request.Form.GetValues("search[value]") != null) ? Request.Form.GetValues("search[value]").FirstOrDefault() : null;

                var datosFiltrados = string.IsNullOrEmpty(filtro)
                    ? _lista                                                     // Si no hay filtro, usar todos los datos
                    : _lista.Where(d => d.nombre.Contains(filtro)).ToList();     // Filtrar por nombre

                totalRegistros = datosFiltrados.Count;    // Número total de registros después de aplicar el filtro
                int offset = (page - 1) * pageSize;         // Cálculo del offset

                datosGrilla = datosFiltrados.Skip(offset).Take(pageSize).ToList();
            }
            else
            {
                datosGrilla = _lista;
                totalRegistros = _lista.Count;
            }

            var retorno = new { listaNumeros, INFO_TABLA = new { draw = draw, totalRegistros, data = datosGrilla } };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public class Probando
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
        }

    }
}