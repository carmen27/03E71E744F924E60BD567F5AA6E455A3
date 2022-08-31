namespace BackEnd.Model
{
    public class Tcompradet
    {
        public string Cestado { get; set; } = string.Empty;
        public string Cprodcod { get; set; } = string.Empty;
        public string Cproddesc { get; set; } = string.Empty;
        public string Cprodmarca { get; set; } = string.Empty;
        public string Cprodunid { get; set; } = string.Empty;
        public string Cusucrea { get; set; } = string.Empty;
        public string Cusumodi { get; set; } = string.Empty;
        public DateTime Dfeccrea { get; set; }
        public DateTime Dfecmodi { get; set; }
        public decimal Ncantidad { get; set; }
        public int Ncompraid { get; set; }
        public int Nid { get; set; }
        public decimal Nimport { get; set; }
        public decimal Nprecio { get; set; }
    }
}