namespace BackEnd.Model
{
    public class Tcompra
    {
        public string Cclirazon { get; set; } = string.Empty;
        public string Ccliruc { get; set; } = string.Empty;
        public string Ccodigo { get; set; } = string.Empty;
        public string Cestado { get; set; } = string.Empty;
        public string Cguid { get; set; } = string.Empty;
        public string Cmoneda { get; set; } = string.Empty;
        public string Cobserv { get; set; } = string.Empty;
        public string Ctipo { get; set; } = string.Empty;
        public string Cusucrea { get; set; } = string.Empty;
        public string Cusumodi { get; set; } = string.Empty;
        public DateTime Dfeccrea { get; set; }
        public DateTime Dfecha { get; set; }
        public DateTime Dfecmodi { get; set; }
        public int Nid { get; set; }
        public decimal Nimport { get; set; }
        public decimal Nimportigv { get; set; }
        public decimal Ntotaligv { get; set; }
        public decimal Nvaligv { get; set; }
    }
}