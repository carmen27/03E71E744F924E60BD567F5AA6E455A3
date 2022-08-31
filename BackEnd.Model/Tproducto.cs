namespace BackEnd.Model
{
    public class Tproducto
    {
        public string Ccodigo { get; set; } = string.Empty;
        public string Cdescripcion { get; set; } = string.Empty;
        public string Cestado { get; set; } = string.Empty;
        public string Cguid { get; set; } = string.Empty;
        public string Cmarca { get; set; } = string.Empty;
        public string Cunidades { get; set; } = string.Empty;
        public string Cusucrea { get; set; } = string.Empty;
        public string Cusumodi { get; set; } = string.Empty;
        public DateTime Dfeccrea { get; set; }
        public DateTime Dfecmodi { get; set; }
        public int Nid { get; set; }
        public decimal Nprecio { get; set; }
    }
}