using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Web.SignalR
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> _atributos = new ConcurrentDictionary<string, string>();

        public override Task OnConnected()
        {
            var rut = Context.QueryString["rut"];
            if (!string.IsNullOrEmpty(rut))
            {
                _connections[rut] = Context.ConnectionId;
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var rut = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(rut))
            {
                _connections.TryRemove(rut, out _);
                _atributos.TryRemove(rut, out _);
            }
            return base.OnDisconnected(stopCalled);
        }

        [HubMethodName("change_weather")]
        public void ChangeWeather(string message)
        {
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(message);
            if (_connections.TryGetValue(data.rut, out string connectionId))
            {
                _atributos[data.rut] = data.data;
                Clients.Client(connectionId).NotifyUser(message); // Enviar el mensaje original como JSON
            }
        }

        [HubMethodName("get_weather")]
        public void GetWeather()
        {
            var rut = Context.QueryString["rut"];
            if (!string.IsNullOrEmpty(rut) && _atributos.TryGetValue(rut, out string data))
            {
                var message = new Message { rut = rut, data = data };
                var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                Clients.Caller.NotifyUser(jsonMessage); // Enviar el mensaje como JSON
            }
        }

        public class Message
        {
            public string rut { get; set; }
            public string data { get; set; }
        }
    }
}