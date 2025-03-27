
using System.Collections.Generic;
using System.Linq;

namespace Web.SignalR
{
    public class DatoInterno
    {
        public string rut { get; set; }
        public bool sesionActiva { get; set; } = true;
        public string mensajePrivado { get; set; }
        public string blablabla { get; set; } = "nada";
        public string jajajajaa { get; set; } = "nada";
        public string identificadorUnico { get; set; }
    }

    public static class LlamadasDbSignalR
    {
        private static List<DatoInterno> lista = new List<DatoInterno>()
        {
            new DatoInterno { rut = "1234" }, new DatoInterno { rut = "5678" }
        };

        public static DatoInterno GetByRut(string rut)
        {
            return lista.FirstOrDefault(x => x.rut == rut);
        }

        public static void CerrarSesionByRut(string rut)
        {
            var elemento = lista.FirstOrDefault(x => x.rut == rut);
            elemento.sesionActiva = false;
        }

        public static void AbrirSesionByRut(string rut)
        {
            var elemento = lista.FirstOrDefault(x => x.rut == rut);
            elemento.sesionActiva = true;
        }
    }
}