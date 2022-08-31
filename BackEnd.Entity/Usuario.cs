using System.Text.Json.Serialization;

namespace BackEnd.Entity
{
    public class Usuario
    {
        [JsonPropertyName("apellidos")]
        public string? Apellidos { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombres { get; set; }

        [JsonPropertyName("num_docum")]
        public string? NumDocum { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("telefono")]
        public string? Telefono { get; set; }

        [JsonPropertyName("tipo_docum")]
        public int? TipoDocum { get; set; } = 1; //1: DNI, 6: RUC

        [JsonPropertyName("username")]
        public string? Username { get; set; }
    }
}