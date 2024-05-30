using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Web.Helpers
{
    public static class FileHelper
    {
        public static bool ValidarArchivo(HttpPostedFile archivo, out string mensaje)
        {
            mensaje = string.Empty;
            var nombreArchivo = Path.GetFileNameWithoutExtension(archivo.FileName);
            var extensionArchivo = Path.GetExtension(archivo.FileName);

            if (ContieneCaracteresInvalidos(nombreArchivo) || !EsNombreValido(nombreArchivo))
            {
                mensaje = "El nombre de archivo contiene caracteres inválidos";
                return false;
            }

            if (!EsExtensionValida(extensionArchivo))
            {
                mensaje = "La extensión del archivo no es válida";
                return false;
            }

            return true;
        }

        private static bool ContieneCaracteresInvalidos(string nombreArchivo)
        {
            char[] caracteresInvalidos = Path.GetInvalidFileNameChars();
            return nombreArchivo.Any(c => caracteresInvalidos.Contains(c));
        }

        private static bool EsNombreValido(string nombreArchivo)
        {
            // Verifica si el nombre del archivo contiene solo letras, números, guion y guion bajo
            return Regex.IsMatch(nombreArchivo, "^[a-zA-Z0-9-_]+$");
        }

        private static bool EsExtensionValida(string extension)
        {
            // Verifica si la extensión del archivo contiene solo letras y números
            return Regex.IsMatch(extension, "^\\.[a-zA-Z0-9]+$");
        }
    }
}