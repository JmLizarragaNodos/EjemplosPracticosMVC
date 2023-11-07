using System.Collections.Generic;
using System.Data;
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

        [HttpPost]
        public ActionResult ObtenerDatosSelect2_ORACLE(string search, int pageSize, int page)
        {
            int offset = (page - 1) * pageSize;   // Cálculo del offset

            var sql = @"
            SELECT * FROM 
            (
                SELECT tu_tabla.id, tu_tabla.nombre, ROWNUM AS rn
                FROM tu_tabla
                WHERE 
                    UPPER(nombre) LIKE UPPER(COALESCE('%" + search + @"%', nombre)) AND
                    ROWNUM <= (
                        " + pageSize + @" -- registrosPorPagina 
                        * 
                        " + page + @"  -- numeroPagina
                    )
            )
            WHERE rn >= (
	            " + offset + @" -- offset 
                + 
                1
            )
            ";

            DataTable dt = null;
            // Ejecutar la consulta sql y el resultado ponerlo en el dt

            int cantidadDatosCursor = dt.Rows.Count;
            bool traerMasRegistros = (cantidadDatosCursor > 0 && pageSize == cantidadDatosCursor);
            var retorno = new Select2(traerMasRegistros);

            foreach (DataRow x in dt.Rows)
            {
                retorno.AgregarOption(
                    x["id"].ToString(),
                    x["ucl_nombre"].ToString()
                );
            }

            return Json(retorno);
        }

    }
}