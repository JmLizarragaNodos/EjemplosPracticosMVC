using OpenHtmlToPdf;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class OpenHtmlToPdfController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DescargarPDF(string contenidoPDF, string nombreArchivo)
        {
            try
            {
                contenidoPDF = DesencriptarHtml(contenidoPDF);

                string logoInacapBase64 = "";
                string ruta = Server.MapPath("~/Img/foto_pdf.png");

                if (System.IO.File.Exists(ruta))
                {
                    byte[] imagenByte = System.IO.File.ReadAllBytes(ruta);
                    string imageBase64Data = Convert.ToBase64String(imagenByte);
                    logoInacapBase64 = $"data:image/png;base64,{imageBase64Data}";
                }

                string html = @"<html>
                <head>
                    <style>" + ObtenerEstilosReporte() + @"</style>
                </head>
                <body>";

                html += $"<img src='{logoInacapBase64}' style='width: 200px; display: block; margin-left: auto;' />";
                html += contenidoPDF;
                html += $"</body></html>";

                PaperSize tamañoCarta = new PaperSize(Length.Millimeters(216), Length.Millimeters(279));

                byte[] buffer = new byte[0];

                buffer = Pdf.From(html)
                    .OfSize(tamañoCarta)
                    .WithObjectSetting("web.defaultEncoding", "utf-8")
                    .Content();

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
                return Content(ex.Message);
            }
        }

        public string DesencriptarHtml(string cadena)
        {
            if (!string.IsNullOrEmpty(cadena))
            {
                cadena = cadena.Replace("|amp;", "&");
                cadena = cadena.Replace("|lt;", "<");
                cadena = cadena.Replace("|gt;", ">");

                cadena = Regex.Replace(cadena, "<!--.*?-->", String.Empty, RegexOptions.Singleline);   // Remover comentarios <!-- -->     
            }

            return cadena;
        }

        private string ObtenerEstilosReporte()
        {
            return @"
            p { word-break: break-word; } /* Impedir textos enormes que desbordan la página */    

		    /* ======================================================================== */
		    /* Organizar tablas en caso de que sobrepasen la página */

		    thead {
			    display: table-row-group;
		    }
		    tr {
			    page-break-before: always;
			    page-break-after: always;
			    page-break-inside: avoid;
		    }
		    table {
			    word-break: break-word;  /* Impedir que textos enormes dentro de la celda desbordan la página */
		    }
		    table td {
			    word-break: break-word;  /* Impedir que textos enormes dentro de la celda desbordan la página */
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
		      background-color: gray; color: white;
		    }

		    .sin-borde {
		      border:none !important;
		    }
            ";
        }
    }
}