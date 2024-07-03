using System;
using System.Web.Mvc;
using Web.Dto;
using Web.Helpers;

namespace Web.Controllers
{
    public class EncriptarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EncriptarTexto(string texto)
        {
            var res = new RespuestaBackend();

            try
            {
                var textoEncriptado = CryptoHelper.Encriptar(texto);
                res.objeto = new { textoEncriptado };
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult DesencriptarTexto(string textoEncriptado)
        {
            var res = new RespuestaBackend();

            try
            {
                bool estaEncriptado = CryptoHelper.TryDesencriptar(textoEncriptado, out var textoDesencriptado);   // Validar si está encriptado

                if (estaEncriptado)
                    res.objeto = new { textoDesencriptado };
                else
                    res.AgregarBadRequest("texto no tiene el formato correcto");
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult NumberToText(int numero)
        {
            var res = new RespuestaBackend();

            try
            {
                res.objeto = numero.ObtenerCadenaDeTexto();
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }
    }
}