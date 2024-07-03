using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.SignalR
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> _atributos = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, List<string>> _unreadMessages = new ConcurrentDictionary<string, List<string>>();

        public override Task OnConnected()
        {
            var rut = Context.QueryString["rut"];

            if (!string.IsNullOrEmpty(rut))
            {
                _connections[rut] = Context.ConnectionId;

                // Enviar mensajes no entregados
                if (_unreadMessages.TryGetValue(rut, out List<string> messages))
                {
                    foreach (var message in messages)
                    {
                        Clients.Client(Context.ConnectionId).NotifyUser(message);
                    }
                    _unreadMessages.TryRemove(rut, out _);
                }
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
            var datoRecibido = JsonConvert.DeserializeObject<DatoRecibido>(message);

            //========================================>>>>

            DatoInterno datoInterno = LlamadasDbSignalR.GetByRut(datoRecibido.rut);

            if (datoRecibido.operacion == "enviar_mensaje_privado")
                datoInterno.mensajePrivado = datoRecibido.mensaje;

            if (datoRecibido.operacion == "cerrar_otra_sesion")
                LlamadasDbSignalR.CerrarSesionByRut(datoRecibido.rut);

            if (datoRecibido.operacion == "abrir_otra_sesion")
                LlamadasDbSignalR.AbrirSesionByRut(datoRecibido.rut);

            //========================================>>>>

            var datoInternoString = JsonConvert.SerializeObject(datoInterno);

            if (_connections.TryGetValue(datoRecibido.rut, out string connectionId))
            {
                _atributos[datoRecibido.rut] = datoInternoString;
                Clients.Client(connectionId).NotifyUser(datoInternoString);
            }
            else  // Si el usuario no está conectado, almacenar el mensaje
            {
                if (_unreadMessages.ContainsKey(datoRecibido.rut))
                    _unreadMessages[datoRecibido.rut].Add(datoInternoString);
                else
                    _unreadMessages[datoRecibido.rut] = new List<string> { datoInternoString };
            }
        }

        [HubMethodName("get_weather")]
        public void GetWeather()
        {
            var rut = Context.QueryString["rut"];
            if (!string.IsNullOrEmpty(rut) && _atributos.TryGetValue(rut, out string data))
            {
                var message = JsonConvert.DeserializeObject<DatoInterno>(data);
                Clients.Caller.NotifyUser(JsonConvert.SerializeObject(message));
            }

            // Enviar mensajes no entregados
            if (_unreadMessages.TryGetValue(rut, out List<string> messages))
            {
                foreach (var message in messages)
                {
                    Clients.Caller.NotifyUser(message);
                }
                _unreadMessages.TryRemove(rut, out _);
            }
        }

        public class DatoRecibido
        {
            public string rut { get; set; }
            public string operacion { get; set; }
            public string mensaje { get; set; }
        }

    }
}