using System.Web.Mvc;

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
    }
}