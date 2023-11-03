using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Dto;

namespace Web.Controllers
{
    public class Select2Controller : Controller
    {
        private static List<Probando> datos;

        static Select2Controller()
        {
            // Así, la inicialización de datos se realiza solo una sola vez durante el ciclo de vida de la aplicación
            datos = new List<Probando>();

            Enumerable.Range(1, 15).ToList().ForEach(numero => {
                datos.Add(new Probando { codigo = numero, nombre = $"Nombre {numero}" });
            });
        }

        public Select2Controller()
        {
           
        }

        public ActionResult Index()
        {
            return View();
        }

        public class Probando
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
        }

        [HttpPost]
        public ActionResult ObtenerDatosSelect2(string search, int pageSize, int page)
        {
            int offset = (page - 1) * pageSize;       // Cálculo del offset
            var dt = datos.Skip(offset).Take(pageSize).ToList();

            int cantidadDatosCursor = dt.Count;
            bool traerMasRegistros = (cantidadDatosCursor > 0 && cantidadDatosCursor == pageSize);
            var retorno = new Select2(traerMasRegistros);

            foreach (Probando x in dt)
            {
                retorno.AgregarOption(x.codigo.ToString(), x.nombre);
            }

            return Json(retorno);
        }
    }
}