using System.Text.Json.Serialization;

namespace BackEnd.Entity
{
    public class Usuario
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }
    }
}