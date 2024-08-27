using PedagioPayApiControlador.Models;

namespace PedagioPayApiControlador.Data.Interfaces
{
    public interface IPedidoRepository
    {
        public Task Cadastra(PassagemPedido pedido);
    }
}
