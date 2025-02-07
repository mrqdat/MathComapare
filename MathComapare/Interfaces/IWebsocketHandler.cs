using System.Net.WebSockets;

namespace MathComapare.Interfaces
{
    public interface IWebsocketHandler
    {
        public Task HandlerWebsocket(WebSocket websocket);
        public Task HandlerMessageAsync(WebSocket webSocket, string message);
    }
}
