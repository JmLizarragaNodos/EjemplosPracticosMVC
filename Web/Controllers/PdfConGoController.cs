using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Controllers
{
    public class PdfConGoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DescargarPdfConExe(string tipoPapel, string contenidoPDF, string nombreArchivo, bool esHorizontal)
        {
            try
            {
                // 1. Mantener tu lógica de desencriptación y logo
                contenidoPDF = ParametrosHelper.DesencriptarHtml(contenidoPDF);
                string logoInacapBase64 = "";
                string ruta = Server.MapPath("~/Img/foto_pdf.png");

                if (System.IO.File.Exists(ruta))
                {
                    byte[] imagenByte = System.IO.File.ReadAllBytes(ruta);
                    logoInacapBase64 = $"data:image/png;base64,{Convert.ToBase64String(imagenByte)}";
                }

                string html = @"<html>
                <head>
                    <meta charset='utf-8'>
                    <title>" + nombreArchivo + @"</title>
                    <style>" + ObtenerEstilosReporte_Exe_Go() + @"</style>
                </head>
                <body>";

                html += $"<img src='{logoInacapBase64}' style='width: 200px; display: block; margin-left: auto;' />";
                html += contenidoPDF;
                html += $"</body></html>";

                byte[] buffer = GenerarPdfConGo(html, tipoPapel, esHorizontal);

                MemoryStream ms = new MemoryStream(buffer, 0, 0, true, true);
                Response.AddHeader("Content-Disposition", $"attachment; filename={nombreArchivo}.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();
                return new FileStreamResult(Response.OutputStream, "application/pdf");
            }
            catch (Exception ex)
            {
                return Content("Error al generar PDF: " + ex.Message);
            }
        }

        private string ObtenerEstilosReporte_Exe_Go()
        {
            return @"
            p { word-break: break-word; }

            /* ======================================================================== */
            /* Organizar tablas en caso de que sobrepasen la página */

            thead {
                /* ANTES: display: table-row-group; */
                /* EXPLICACIÓN: 'table-row-group' trata al encabezado como una fila común. 
                   Cambiamos a 'table-header-group' para que Chrome repita el encabezado 
                   automáticamente en cada nueva página si la tabla es muy larga. */
                display: table-header-group; 
            }

            tr {
                /* ANTES: page-break-before: always; */
                /* ANTES: page-break-after: always; */
                /* EXPLICACIÓN: 'always' obligaba a saltar de página en CADA fila (una fila por hoja).
                   Lo comentamos para que las filas fluyan seguidas y solo salten cuando no quepa más texto. */
                /* page-break-before: always; */
                /* page-break-after: always; */
    
                page-break-inside: avoid;
                /* NUEVO: 'break-inside' es la versión moderna y más compatible para motores Chromium */
                break-inside: avoid; 
            }

            table {
                word-break: break-word;
                /* NUEVO: Asegura que la tabla no intente forzar saltos extraños y respete el flujo */
                page-break-inside: auto; 
            }

            table td {
                word-break: break-word;
                min-width: 80px;
            }

            /* ======================================================================== */

            .encabezado { 
                page-break-inside: avoid !important; 
            }

            .tabla-info {
              width:100%;
              border-collapse: collapse;
            }

            .tabla-info th, td {
              border: 1px solid rgb(104, 103, 103);
            }

            .tabla-info td {
              padding: 5px;
            }

            .tabla-info th {
              padding: 3px;
            }

            .tabla-info thead {
              /* Mantenemos tus estilos visuales intactos */
              background-color: gray; color: white;
            }

            .sin-borde {
              border:none !important;
            }
            ";
        }

        private byte[] GenerarPdfConGo(string html, string tipoPapel, bool esHorizontal, double margen = 0.4)
        {
            double ancho = 8.5;
            double alto = 11.0;

            if (tipoPapel == "carta") { ancho = 8.5; alto = 11.0; }
            else if (tipoPapel == "oficio") { ancho = 8.5; alto = 13.0; }
            else if (tipoPapel == "A4") { ancho = 8.27; alto = 11.69; }

            string exePath = Server.MapPath("~/App_Data/pdf-worker.exe");

            // Pasamos 4 argumentos: ancho, alto, orientacion, margen
            string args = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} {1} {2} {3}",
                          ancho, alto, esHorizontal.ToString().ToLower(), margen);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process proceso = Process.Start(psi))
            {
                // LA CLAVE: Usar UTF8Encoding(false) para que no envíe caracteres basura al inicio
                using (StreamWriter sw = new StreamWriter(proceso.StandardInput.BaseStream, new System.Text.UTF8Encoding(false)))
                {
                    sw.Write(html);
                    sw.Flush();
                    sw.Close();
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    proceso.StandardOutput.BaseStream.CopyTo(ms);
                    proceso.WaitForExit();

                    if (ms.Length == 0)
                    {
                        string errorMsg = proceso.StandardError.ReadToEnd();
                        throw new Exception("Error motor PDF: " + errorMsg);
                    }

                    return ms.ToArray();
                }
            }
        }
    }
}