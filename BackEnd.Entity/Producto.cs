using System.Text.Json.Serialization;

namespace BackEnd.Entity
{
    public class Producto
    {
        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("marca")]
        public string? Marca { get; set; }

        [JsonPropertyName("precio")]
        public decimal? Precio { get; set; }

        [JsonPropertyName("unidades")]
        public string? Unidades { get; set; }
    }
}