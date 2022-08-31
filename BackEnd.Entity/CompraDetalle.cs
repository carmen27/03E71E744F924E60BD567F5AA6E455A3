using System.Text.Json.Serialization;

namespace BackEnd.Entity
{
    public class CompraDetalle
    {
        [JsonPropertyName("cantidad")]
        public decimal? Cantidad { get; set; }

        [JsonPropertyName("cod_articulo")]
        public string? CodArticulo { get; set; }

        [JsonPropertyName("desc_articulo")]
        public string? DescArticulo { get; set; }

        [JsonPropertyName("marca")]
        public string? Marca { get; set; }

        [JsonPropertyName("precio")]
        public decimal? Precio { get; set; }

        [JsonPropertyName("total")]
        public decimal? Total { get; set; }
    }
}