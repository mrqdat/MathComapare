using MathComapare.Models;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace MathComapare.Websocket
{
    public static class WebsocketHandler
    {
        public static async Task HandlerWebsocket( WebSocket  websocket )
        {
            var buffer = new byte[1024*4];
            WebSocketReceiveResult result;

            do
            {
                result = await websocket.ReceiveAsync( new ArraySegment<byte>(buffer), CancellationToken.None );
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                Console.WriteLine($" Recieved: {message}");

                var response = Encoding.UTF8.GetBytes("message recieved");
                await websocket.SendAsync(new ArraySegment<byte>(response),result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
            while (!result.CloseStatus.HasValue);

            await websocket.CloseAsync(result.CloseStatus.Value, websocket.CloseStatusDescription, CancellationToken.None);
        }


        //public async Task HandlerMessageAsync(WebSocket webSocket, string message)
        //{
        //    var wsMessage = JsonSerializer.Deserialize<WebSocketMessage>(message);

        //    switch (wsMessage.Type)
        //    {
        //        case "register_user":
        //            {

        //            }
        //    }
        //}
    }
}
