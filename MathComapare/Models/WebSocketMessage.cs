using Newtonsoft.Json.Serialization;

namespace MathComapare.Models
{
    public class WebSocketMessage : SnakeCaseNamingStrategy
    {
        public string Type { get; set; }
        public object Data { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
