using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace MathComapare.Models
{
    public class WebSocketMessage : SnakeCaseNamingStrategy
    {
        public string Type { get; set; }
        public object Data { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
