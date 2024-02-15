using System;
using System.Collections.Generic;
using System.Web.Mvc;

using ClosedXML.Excel;
using System.Data;
using System.Net;
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
        public ActionResult Exportar(string nombreArchivo)
        {
            nombreArchivo = $"{nombreArchivo}.xlsx";

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
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

            return new EmptyResult();
        }
      
    }
}