using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using Web.SignalR;

// Instalar Microsoft.AspNet.SignalR        Version     2.4.2
// Instalar Microsoft.Owin.Cors             Versión     2.1.0

/*
En la clase      Global.asax.cs      en el método Application_Start declarar esto:
    AreaRegistration.RegisterAllAreas();
    RouteConfig.RegisterRoutes(RouteTable.Routes);

En el namespace     Web.SignalR     crear la clase StartUp
*/

namespace Web.Controllers
{
    public class EjemploSignalRController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProbandoEnviarMensaje()
        {
            return View();
        }

        public ActionResult ProbandoRecibirMensaje()
        {
            return View();
        }

        public ActionResult IntentoEnviarMensaje(string rut, string mensaje)  // https://localhost:44353/EjemploSignalR/IntentoEnviarMensaje?rut=5678&mensaje=probando
        {
            try
            {
                // Crear el objeto mensaje
                string message = JsonConvert.SerializeObject(new { rut = rut, mensaje = mensaje, operacion = "enviar_mensaje_privado" });

                // Obtener el contexto del hub
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                // Llamar al método ChangeWeather del hub directamente usando el contexto
                hubContext.Clients.All.change_weather(message);

                return Json(new { exito = "Mensaje enviado exitosamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        /*
        public ActionResult IntentoEnviarMensaje(string rut, string mensaje)  // https://localhost:44353/EjemploSignalR/IntentoEnviarMensaje?rut=5678&mensaje=probando
        {
            try
            {
                // Crear el objeto mensaje
                string message = JsonConvert.SerializeObject(new { rut = rut, mensaje = mensaje, operacion = "enviar_mensaje_privado" });

                // Obtener el contexto del hub
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

                // Llamar al método ChangeWeather del hub directamente
                var chatHub = new ChatHub();
                chatHub.ChangeWeather(message);

                return Json(new { exito = "Mensaje enviado exitosamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        */

    }
}