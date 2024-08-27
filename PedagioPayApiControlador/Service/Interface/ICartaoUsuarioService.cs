using PedagioPayApiControlador.Data.Dtos;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;

namespace PedagioPayApiControlador.Service.Interface
{
    public interface ICartaoUsuarioService
    {
        public Task<UsuarioCartaoDto> Adicionar(TransacaoDto transacao, string token);
        public Task Desativar(int idCartaoUsuario);
        public bool IsPrincipal(AlterarPrincipalDto cartao);
        public List<UsuarioCartaoDto> Buscar(string token);
        public ResponseCabineAutorizacaoDto BuscarPorPlaca(string mac, RequestCabineAutorizacaoDto request);
        public bool Validar(UsuarioCartaoDto usuarioCartao);
    }
}
