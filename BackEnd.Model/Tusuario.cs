namespace BackEnd.Model
{
    public class Tusuario
    {
        public string Capellidos { get; set; } = string.Empty;
        public string Ccodigo { get; set; } = string.Empty;
        public string Cemail { get; set; } = string.Empty;
        public string Cestado { get; set; } = string.Empty;
        public string Cguid { get; set; } = string.Empty;
        public string Cnombres { get; set; } = string.Empty;
        public string Cnumdocum { get; set; } = string.Empty;
        public string Cnumero1 { get; set; } = string.Empty;
        public string Cnumero2 { get; set; } = string.Empty;
        public string Cnumero3 { get; set; } = string.Empty;
        public string Cusername { get; set; } = string.Empty;
        public string Cusucrea { get; set; } = string.Empty;
        public string Cusumodi { get; set; } = string.Empty;
        public DateTime Dfeccrea { get; set; }
        public DateTime Dfecmodi { get; set; }
        public int Nid { get; set; }
        public int Ntipdocum { get; set; }
        public byte[] Ypassword { get; set; } = Array.Empty<byte>();
    }
}