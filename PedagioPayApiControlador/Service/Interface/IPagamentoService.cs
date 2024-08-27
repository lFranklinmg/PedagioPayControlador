using PedagioPayApiControlador.Data.Dtos;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;

namespace PedagioPayApiControlador.Service.Interface
{
    public interface IPagamentoService
    {
        public Task<ResponseAutorizacaoDto> ProcessarTransacao(TransacaoDto transacao);
        public Task CancelarTransacao(CancelamentoDto cancelamento);
        public ResponseAutorizacaoDto SaveTransaction(RequestTransactionSaveDto.RequestTransactionSavePayloadDto transaction, string mac);
    }
}
