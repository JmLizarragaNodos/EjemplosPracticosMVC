
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Web.SignalR
{
    public class ChatHub : Hub
    {
        public static string _atributo;

        [HubMethodName("change_weather")]
        public void ChangeWeather(string temperature)
        {
            _atributo = temperature;
            Clients.Others.NotifyUser(temperature);
        }

        [HubMethodName("get_weather")]
        public void GetWeather()
        {
            Clients.Caller.NotifyUser(_atributo);
        }

    }
}