using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Web.SignalR.StartUp))]

namespace Web.SignalR
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);  // Permitir CORS para SignalR
            app.MapSignalR();
        }
    }
}
