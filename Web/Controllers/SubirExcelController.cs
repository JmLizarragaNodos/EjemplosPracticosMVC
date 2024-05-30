using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Controllers
{
    public class SubirExcelController : Controller
    {
        private List<INFO_CSV_ENT> _lista = new List<INFO_CSV_ENT>
        {
            new INFO_CSV_ENT { campo = "PROBANDO_FIELD", nombre = "Ejemplo Probando" },
            new INFO_CSV_ENT { campo = "TRATANDO_FIELD", nombre = "Ejemplo Tratando" },
            new INFO_CSV_ENT { campo = "INTENTANDO_FIELD", nombre = "Ejemplo Intentando" },
            new INFO_CSV_ENT { campo = "EJEMPLIFICANDO_FIELD", nombre = "Ejemplo Ejemplificando" }
        };

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Subir()
        {
            object res = new { };

            try
            {
                HttpRequest httpRequest = System.Web.HttpContext.Current.Request;
                HttpPostedFile archivo = httpRequest.Files["archivo"];

                if (
                    archivo != null && (
                        archivo.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ||   // Es xlsx
                        archivo.ContentType == "text/csv" ||                                                            // Es csv en Edge
                        archivo.ContentType == "application/vnd.ms-excel"                                               // Es csv en Firefox
                    )
                )
                {
                    if (!ValidarColumnasArchivo(archivo, out List<ValidacionColumnas> columnasValidadas))
                    {
                        res = new { status = 400, informacionExtra = columnasValidadas };
                    }
                    else
                    {
                        if (!FileHelper.ValidarArchivo(archivo, out string mensajeArchivo))
                        {
                            res = new { status = 400, errores = new List<string>() { mensajeArchivo } };
                        }

                        string p_nombre_archivo = Path.GetFileName(archivo.FileName);  // Incluye la extensión

                        using (Stream stream = archivo.InputStream)
                        {
                            string p_path = string.Empty;
                            string p_columns = string.Empty;
                            string p_data = ExcelHelper.ExcelAsXml(stream, archivo.ContentType, ref p_path, ref p_columns);

                            res = new { status = 200, objeto = new { p_data, p_path, p_columns } };

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                }
                else
                {
                    res = new { status = 400, errores = new List<string>() { "El archivo debe ser en formato xlsx o csv" } };
                }

            }
            catch (Exception ex)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();

                res = new { status = 500, errores = new List<string>() { ex.Message } };
            }

            return Json(res);
        }

        public class ValidacionColumnas
        {
            public string nombreEsperado { get; set; }
            public string nombreRecibido { get; set; }
            public bool esValido { get; set; }
        }

        public class INFO_CSV_ENT
        {
            public string campo { get; set; }
            public string nombre { get; set; }
        }

        private bool ValidarColumnasArchivo(HttpPostedFile archivo, out List<ValidacionColumnas> columnasValidadas)
        {
            columnasValidadas = new List<ValidacionColumnas>();

            List<string> listaCabecera = ExcelHelper.ObtenerListaCabecera(archivo);

            if (listaCabecera.Count > 0)
            {
                var listaComparar = _lista.Select(x => x.campo.ToUpper()).ToList();

                if (!listaCabecera.Select(x => x.ToUpper()).ToList().SequenceEqual(listaComparar))
                {
                    int cont = 0;

                    foreach (var nombreEsperado in listaComparar)
                    {
                        var nombreRecibido = (cont < listaCabecera.Count) ?
                            listaCabecera[cont] : null;

                        var item = new ValidacionColumnas
                        {
                            nombreEsperado = nombreEsperado,
                            nombreRecibido = nombreRecibido,
                            esValido = (nombreEsperado == nombreRecibido)
                        };

                        columnasValidadas.Add(item);
                        cont++;
                    }

                    return false;
                }
            }

            return true;
        }

        [HttpPost]
        public ActionResult DescargarEjemplo(string fecha)
        {
            string nombreArchivo = $"Ejemplo__{fecha}.csv";

            try
            {
                List<string> titulos = _lista.Select(x => x.campo).ToList();
                StringBuilder builder = new StringBuilder();

                builder.Append(string.Join(";", titulos));
                builder.Append("\r\n");

                for (int i = 0; i < 10; i++)
                {
                    var fila = _lista.Select(x => $"{x.nombre} {i}").ToList();
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