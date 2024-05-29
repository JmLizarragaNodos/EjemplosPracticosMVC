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

            Enumerable.Range(1, 15).ToList().ForEach(numero => {
                _lista.Add(new Probando { codigo = numero, nombre = $"Nombre {numero}" });
            });
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerDatos(int start, int length, string pruebaSelect, bool cargaInicial)
        {
            var cboProbando = new List<object>();

            if (cargaInicial)
            {
                cboProbando = new List<int> { 1, 2, 3, 4 }.Select(x => new { codigo = x, descripcion = x }).Cast<object>().ToList();
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

                var query = _lista.AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                    query = query.Where(d => d.nombre.Contains(filtro));

                if (!string.IsNullOrEmpty(pruebaSelect))
                    query = query.Where(d => d.codigo == int.Parse(pruebaSelect));

                totalRegistros = query.Count();      // Número total de registros después de aplicar el filtro
                int offset = (page - 1) * pageSize;         // Cálculo del offset

                datosGrilla = query.Skip(offset).Take(pageSize).ToList();
            }
            else
            {
                datosGrilla = _lista;
                totalRegistros = _lista.Count;
            }

            return Json(new { cboProbando, INFO_TABLA = new { draw = draw, totalRegistros, data = datosGrilla } });
        }

        public ActionResult TraerSeleccionados(bool p_todas, string p_codigos) 
        {
            var res = new RespuestaBackend();
            var elementosSeleccionados = new List<Probando>();
            var cadenaSQL = "";

            try
            {
                if (p_todas && string.IsNullOrEmpty(p_codigos))  // Si seleccionó todo
                {
                    elementosSeleccionados = _lista;
                    cadenaSQL = "UPDATE tu_tabla SET seleccionado = 1; <br/>";
                }
                else
                {
                    cadenaSQL = "UPDATE tu_tabla SET seleccionado = 0; <br/><br/>";

                    List<int> listaNumeros = p_codigos.Split(',').Select(int.Parse).ToList();

                    if (p_todas && !string.IsNullOrEmpty(p_codigos))  // Si seleccionó todo, pero se removió algunos
                    {
                        elementosSeleccionados = _lista.Where(x => !listaNumeros.Contains(x.codigo)).ToList();
                        cadenaSQL += $"UPDATE tu_tabla SET seleccionado = 1 WHERE codigo NOT IN ({p_codigos}); <br/>";
                    }
                    else if (!p_todas && !string.IsNullOrEmpty(p_codigos))  // Si seleccionó solo algunas
                    {
                        elementosSeleccionados = _lista.Where(x => listaNumeros.Contains(x.codigo)).ToList();
                        cadenaSQL += $"UPDATE tu_tabla SET seleccionado = 1 WHERE codigo IN ({p_codigos}); <br/>";
                    }
                }

                res.objeto = new { elementosSeleccionados, cadenaSQL };
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public class Probando
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
        }

    }
}