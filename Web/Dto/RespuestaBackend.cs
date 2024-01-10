using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Dto
{
    public class RespuestaBackend
    {
        public object objeto { get; set; }
        public List<string> errores { get; set; } = new List<string>();
        public int status { get; set; } = 200;
        public string mensajeExito { get; set; }

        public void AgregarBadRequest(string mensaje)
        {
            this.errores.Add(mensaje);
            this.status = 400;
        }

        public void AgregarInternalServerError(string mensaje)
        {
            this.errores.Add(mensaje);
            this.status = 500;
        }

        public void NoAutorizado()
        {
            this.errores.Add("No autorizado");   // 401 Unauthorized
            this.status = 401;
        }

        public void AgregarMensajeExito(string mensaje)
        {
            mensajeExito = mensaje;
        }
    }
}