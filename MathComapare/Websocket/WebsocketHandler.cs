using MathComapare.Entities;
using MathComapare.Interfaces;
using MathComapare.Models;
using MathComapare.Repositories.Interfaces;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;

namespace MathComapare.Websocket
{
    public class WebsocketHandler 
    { 
        private readonly WebSocketConnectionManager _connectionManager;
        public WebsocketHandler (WebSocketConnectionManager webSocketConnection)
        {
            _connectionManager = webSocketConnection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task HandlerWebsocket(HttpContext httpContext)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                string clientid = Guid.NewGuid().ToString();
                _connectionManager.Addsocket(clientid, socket);

                await ProcessMessage(clientid, socket);

                _connectionManager.RemoveSocket(clientid);
            }
            else
            {
                httpContext.Response.StatusCode = 400;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientid"></param>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        public async Task ProcessMessage(string clientid, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            while(webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close) {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,"Closed by client", CancellationToken.None);
                    break;
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await _connectionManager.BroadCastMessage($"ClientId: {clientid}: {message} ");
                }
            }
        }
    }
}
