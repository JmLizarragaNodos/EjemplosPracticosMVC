using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;

namespace Web.Controllers
{
    public class EjemploDescargaCsvController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public class Datos
        {
            public string Periodo { get; set; }
            public string Codigo { get; set; }
            public string Rut { get; set; }
            public string NombreCompleto { get; set; }
        }

        [HttpPost]
        public ActionResult Exportar(string fecha)
        {
            string nombreArchivo = $"PruebaCsv__{fecha}.csv";

            try
            {
                var titulos = new List<string>() { "Periodo", "Codigo", "Rut", "NombreCompleto" };

                List<Datos> lista = new List<Datos>()
                {
                    new Datos { Periodo = "1", Codigo = "1", Rut = "24697282-6", NombreCompleto = "José Ñandú" },
                    new Datos { Periodo = "2", Codigo = "2", Rut = "24956294-7", NombreCompleto = "Juana" },
                    new Datos { Periodo = "3", Codigo = "3", Rut = "8443630-5", NombreCompleto = "Andres" },
                    new Datos { Periodo = "4", Codigo = "4", Rut = "7319506-3", NombreCompleto = "Sofía" },
                };

                StringBuilder builder = new StringBuilder();

                builder.Append(string.Join(";", titulos));
                builder.Append("\r\n");

                foreach (Datos x in lista)
                {
                    var fila = new List<string>() {
                        x.Periodo,
                        x.Codigo,
                        x.Rut,
                        x.NombreCompleto
                    };

                    builder.Append(string.Join(";", fila));
                    builder.Append("\r\n");
                }

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo);
                Response.ContentEncoding = Encoding.GetEncoding("Windows-1252");
                Response.ContentType = "application/text";
                Response.Output.Write(builder.ToString());
                Response.Flush();

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return Json(new { errores = new List<string>() { ex.Message } });
            }
        }

    }
}