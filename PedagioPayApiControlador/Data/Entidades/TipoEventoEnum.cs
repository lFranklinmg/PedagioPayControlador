namespace PedagioPayApiControlador.Data.Entidades
{
    public enum TipoEventoEnum : ushort
    {
        RequisicaoAutorizacaoPassagem = 101,
        RespostaAutorizacaoPassagem = 102,
        RespostaAutorizacaoOsa = 121,
        RespostaCartiraDigitalQRCode = 141,
        RequisicaoCarteiraDigitalStatusPagamento = 142,
        RespostaCarteiraDigitalStatusPagamento = 143,
        RequisicaoEstornoPassagem = 201,
        RespostaEstornoPassagem = 202,
        RequisicaoAutorizacaoAME = 998,
        RespostaAutorizacaoAME = 999,
        RequisicaoRegistrarQrCodeAtivo = 301,
        RespostaRegistrarQrCodeAtivo = 302,
        RequisicaoObterStatusQrCodeAtivo = 303,
        RespostaObterStatusQrCodeAtivo = 304,
        RequisicaoSolicitacaoQrCodeActive = 305,
        RespostaSolicitacaoQrCodeActive = 306,
        HardCoded = 131
    }
}
