namespace PedagioPayApiControlador.Data.Dtos
{
    public class UsuarioCartaoDto
    {
        public int IdUsuarioCartao { get; set; }
        public byte?  ID_TIPO_CARTAO { get; set; }
        public string NumeroCartao { get; set; }
        public string CODIGO_SEGURANCA { get; set; }
        public string MetodoPagamentoToken { get; set; }
        public string MetodoTipoPagamento { get; set; }
        public string Bandeira {  get; set; }
        public DateTime? DH_VALIDADE { get; set; }
        public string? NOME_CARTAO { get; set; }
        public string? DOCUMENTO_USUARIO { get; set; }
        public bool? BlPrincipal { get; set; }
        public string? IdPagamentoEstorno { get; set; } 
        


    }
}
