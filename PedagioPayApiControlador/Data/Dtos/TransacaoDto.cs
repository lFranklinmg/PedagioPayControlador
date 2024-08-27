namespace PedagioPayApiControlador.Data.Dtos
{
    public class TransacaoDto
    {
        public int Amount { get; set; }
        public string Placa { get; set; }
        public string Nonce  { get; set; }
        public string? TokenCard { get; set; }
        public DateTime? DataTransacao { get; set; }
        public bool SaveCard { get; set; }
        public bool? Bl_Principal { get; set; }
        public List<int>? Transaction { get; set; }

    }
}
