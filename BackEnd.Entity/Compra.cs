using System.Text.Json.Serialization;

namespace BackEnd.Entity
{
    public class Compra
    {
        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("detalles")]
        public List<CompraDetalle>? Detalles { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("fecha")]
        public string? Fecha { get; set; }

        [JsonPropertyName("guid")]
        public string? Guid { get; set; }

        [JsonPropertyName("importe_con_igv")]
        public decimal? ImporteConIgv { get; set; }

        [JsonPropertyName("importe_sin_igv")]
        public decimal? ImporteSinIgv { get; set; }

        [JsonPropertyName("moneda")]
        public string? Moneda { get; set; }

        [JsonPropertyName("razon_cliente")]
        public string? RazonCliente { get; set; }

        [JsonPropertyName("ruc_cliente")]
        public string? RucCliente { get; set; }

        [JsonPropertyName("tasa_igv")]
        public decimal? TasaIgv { get; set; }

        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        [JsonPropertyName("total_igv")]
        public decimal? TotalIgv { get; set; }
    }
}