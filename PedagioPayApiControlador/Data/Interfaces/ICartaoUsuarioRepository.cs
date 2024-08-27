using PedagioPayApiControlador.Data.Dtos;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;
using PedagioPayApiControlador.Models;

namespace PedagioPayApiControlador.Data.Interfaces
{
    public interface ICartaoUsuarioRepository
    {
        public Task<UsuarioCartaoDto> Cadastra(UsuarioCartao cartao);
        public int IsPrincipal(AlterarPrincipalDto cartao);
        public List<UsuarioCartaoDto> Buscar(string token);
        public List<ResponseCabineAutorizacaoDto> BuscarPorPlaca(string placa);
        public Task Deletar(int IdUsuarioCartao);
    }
}
