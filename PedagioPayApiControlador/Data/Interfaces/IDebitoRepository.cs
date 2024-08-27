using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;

namespace PedagioPayApiControlador.Data.Interfaces
{
    public interface IDebitoRepository
    {
        public void CadastraHistorico(string responseAutorizacao, string idPassagem);
        public int SaveTransaction(RequestTransactionSaveDto.RequestTransactionSavePayloadDto transaction);

    }
}
