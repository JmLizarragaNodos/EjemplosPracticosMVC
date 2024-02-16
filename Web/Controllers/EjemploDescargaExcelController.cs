using System;
using System.Collections.Generic;
using System.Web.Mvc;

using ClosedXML.Excel;
using System.Data;
using System.IO;

/*
Es necesario instalar la siguiente librería: 
    ClosedXML versión 0.102.2
*/

namespace Web.Controllers
{
    public class EjemploDescargaExcelController : Controller
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
            string nombreArchivo = $"Probando__{fecha}.xlsx";

            try
            {
                List<Datos> lista = new List<Datos>()
                {
                    new Datos { Periodo = "1", Codigo = "1", Rut = "24697282-6", NombreCompleto = "José Ñandú" },
                    new Datos { Periodo = "2", Codigo = "2", Rut = "24956294-7", NombreCompleto = "Juana" },
                    new Datos { Periodo = "3", Codigo = "3", Rut = "8443630-5", NombreCompleto = "Andres" },
                    new Datos { Periodo = "4", Codigo = "4", Rut = "7319506-3", NombreCompleto = "Sofía" },
                };

                DataTable dt = new DataTable();
                dt.Columns.Add("Periodo");
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Rut");
                dt.Columns.Add("Nombre Completo");

                foreach (var x in lista)
                {
                    dt.Rows.Add(
                        x.Periodo,
                        x.Codigo,
                        x.Rut,
                        x.NombreCompleto
                    );
                }

                XLWorkbook wb = new XLWorkbook();
                var ws = wb.Worksheets.Add(dt, "Hoja 1");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                return Json(new { errores = new List<string>() { ex.Message } });
            }

            return new EmptyResult();
        }


        /*
        [HttpPost]
        public HttpResponseMessage Exportar(string fecha)
        {
            string nombreArchivo = $"Probando__{fecha}.xlsx";

            try
            {
                List<Datos> lista = new List<Datos>()
                {
                    new Datos { Periodo = "1", Codigo = "1", Rut = "24697282-6", NombreCompleto = "Jose" },
                    new Datos { Periodo = "2", Codigo = "2", Rut = "24956294-7", NombreCompleto = "Juana" },
                    new Datos { Periodo = "3", Codigo = "3", Rut = "8443630-5", NombreCompleto = "Andres" },
                    new Datos { Periodo = "4", Codigo = "4", Rut = "7319506-3", NombreCompleto = "Sofía" },
                };

                DataTable dt = new DataTable();
                dt.Columns.Add("Periodo");
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Rut");
                dt.Columns.Add("Nombre Completo");

                foreach (var x in lista)
                {
                    dt.Rows.Add(
                        x.Periodo,
                        x.Codigo,
                        x.Rut,
                        x.NombreCompleto
                    );
                }

                XLWorkbook wb = new XLWorkbook();
                var ws = wb.Worksheets.Add(dt, "Datos");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new ByteArrayContent(memoryStream.ToArray());
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = nombreArchivo;
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message),
                    ReasonPhrase = "Error"
                };
            }
        }
        */
    }
}