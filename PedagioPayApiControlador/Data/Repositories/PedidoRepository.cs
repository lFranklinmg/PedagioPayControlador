using PedagioPayApiControlador.Data.Interfaces;
using PedagioPayApiControlador.Models;

namespace PedagioPayApiControlador.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PEDAGIOPAYControladorContext _context;
        public PedidoRepository(PEDAGIOPAYControladorContext context)
        {
            _context = context;
        }
        
        public Task Cadastra(PassagemPedido pedido)
        {
            try
            {
                _context.Add(pedido);
                _context.SaveChanges();
                return Task.CompletedTask;
            }catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
