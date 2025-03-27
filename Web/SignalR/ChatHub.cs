using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.SignalR
{
    public class DatoRecibido
    {
        public string rut { get; set; }
        public string operacion { get; set; }
        public string mensaje { get; set; }
        public string identificadorUnico { get; set; }
    }

    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, List<string>> _connections = new ConcurrentDictionary<string, List<string>>();
        private static ConcurrentDictionary<string, string> _atributos = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> _unreadMessage = new ConcurrentDictionary<string, string>();

        public override Task OnConnected()
        {
            var rut = Context.QueryString["rut"];
            var connectionId = Context.ConnectionId;

            if (!string.IsNullOrEmpty(rut))
            {
                _connections.AddOrUpdate(rut, new List<string> { connectionId }, (key, oldList) =>
                {
                    oldList.Add(connectionId);
                    return oldList;
                });

                if (_unreadMessage.TryRemove(rut, out string message))  // Enviar mensaje no entregado
                {
                    Clients.Client(connectionId).NotifyUser(message);
                }
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;

            // Buscar el rut asociado al connectionId
            var entry = _connections.FirstOrDefault(x => x.Value.Contains(connectionId));

            if (!string.IsNullOrEmpty(entry.Key))
            {
                var rut = entry.Key;

                _connections.AddOrUpdate(rut, new List<string>(),
                    (key, oldList) =>
                    {
                        oldList.Remove(connectionId);
                        return oldList.Any() ? oldList : null; // Retornar null para eliminar la clave
                    });

                // Si la clave quedó sin conexiones, eliminarla
                if (_connections.TryGetValue(rut, out var list) && (list == null || !list.Any()))
                {
                    _connections.TryRemove(rut, out _);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        [HubMethodName("change_weather")]
        public void ChangeWeather(string message)
        {
            var datoRecibido = JsonConvert.DeserializeObject<DatoRecibido>(message);

            DatoInterno datoInterno = LlamadasDbSignalR.GetByRut(datoRecibido.rut);

            if (datoInterno == null)
            {
                Clients.Caller.NotifyUser($"No se encontró la persona con el rut {datoRecibido.rut}");
                return;
            }

            datoInterno.identificadorUnico = datoRecibido.identificadorUnico;

            if (datoRecibido.operacion == "enviar_mensaje_privado")
                datoInterno.mensajePrivado = datoRecibido.mensaje;

            if (datoRecibido.operacion == "cerrar_otra_sesion")
                LlamadasDbSignalR.CerrarSesionByRut(datoRecibido.rut);

            if (datoRecibido.operacion == "abrir_otra_sesion")
                LlamadasDbSignalR.AbrirSesionByRut(datoRecibido.rut);

            var datoInternoString = JsonConvert.SerializeObject(datoInterno);

            if (_connections.TryGetValue(datoRecibido.rut, out List<string> connectionIds))
            {
                _atributos[datoRecibido.rut] = datoInternoString;

                foreach (var conn in connectionIds)
                {
                    if (conn != Context.ConnectionId) // Evita enviar a la misma pantalla
                    {
                        Clients.Client(conn).NotifyUser(datoInternoString);
                    }
                }
            }
            else
            {
                if (!_connections.ContainsKey(datoRecibido.rut))  // Si el usuario no está conectado
                {
                    _unreadMessage[datoRecibido.rut] = datoInternoString; // Solo guarda el último mensaje
                }
            }
        }

        [HubMethodName("get_weather")]
        public void GetWeather()
        {
            var rut = Context.QueryString["rut"];
            var connectionId = Context.ConnectionId;

            if (!string.IsNullOrEmpty(rut))
            {
                if (_atributos.TryGetValue(rut, out string data))
                {
                    Clients.Client(connectionId).NotifyUser(data);
                }

                if (_unreadMessage.TryRemove(rut, out string message))
                {
                    Clients.Client(connectionId).NotifyUser(message);
                }
            }
        }
    }
}