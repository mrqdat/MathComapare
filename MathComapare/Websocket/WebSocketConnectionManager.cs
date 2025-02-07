using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace MathComapare.Websocket
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _socket = new();


        // Add new socket when new client connect
        public void Addsocket(string id, WebSocket socket)
        {
            _socket.TryAdd(id, socket);
        }
        // remove socket when client disconnect
        public void RemoveSocket(string id) 
        { 
            _socket.TryRemove(id, out _);
        }

        public WebSocket GetSocketById (string id)
        {
            return _socket.TryGetValue(id, out var socket) ? socket : null;
        }

        // gửi tin tới client cụ thể
        public async Task SendMessageToClient(string clientId, string messsage) 
        {
            if(_socket.TryGetValue(clientId, out var socket) && socket.State == WebSocketState.Open)
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes(messsage);
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        // gửi tin tới tât cả client đã kết nối
        public async Task BroadCastMessage(string message) {  
        
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);
            foreach(var socket in _socket.Values.Where(a => a.State == WebSocketState.Open))
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public int GetConnectedClientCount()
        {
            return _socket.Count;
        }
    }
}
