using System;
using System.Text.RegularExpressions;

namespace Web.Helpers
{
    public static class ParametrosHelper
    {
        public static string EncriptarHtml(string cadena)
        {
            if (string.IsNullOrEmpty(cadena))
                return cadena;

            cadena = cadena.Replace("&", "/amp/");   // Reemplazar &
            cadena = cadena.Replace("<", "/lt/");    // Reemplazar <
            cadena = cadena.Replace(">", "/gt/");    // Reemplazar >
            cadena = cadena.Replace("=", "/equals/");// Reemplazar =

            return cadena;
        }

        public static string DesencriptarHtml(string cadena)
        {
            if (!string.IsNullOrEmpty(cadena))
            {
                cadena = cadena.Replace("/amp/", "&");
                cadena = cadena.Replace("/lt/", "<");
                cadena = cadena.Replace("/gt/", ">");

                cadena = cadena.Replace("/equals/", "=");  // Por mientras está aca

                //cadena = cadena.Replace("|amp;", "&");
                //cadena = cadena.Replace("|lt;", "<");
                //cadena = cadena.Replace("|gt;", ">");

                cadena = Regex.Replace(cadena, "<!--.*?-->", String.Empty, RegexOptions.Singleline);   // Remover comentarios <!-- -->     
            }

            return cadena;
        }
    }
}