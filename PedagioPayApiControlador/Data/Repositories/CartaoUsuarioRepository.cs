using Dapper;
using Microsoft.Data.SqlClient;
using PedagioPayApiControlador.Data.Dtos;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;
using PedagioPayApiControlador.Data.Interfaces;
using PedagioPayApiControlador.Models;
using System.Data;

namespace PedagioPayApiControlador.Data.Repositories
{
    public class CartaoUsuarioRepository : ICartaoUsuarioRepository
    {
        private readonly PEDAGIOPAYControladorContext _context;
        private readonly IConfiguration _configuration;

        public CartaoUsuarioRepository(PEDAGIOPAYControladorContext context, IConfiguration configuration)
        {
            _context = context;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _configuration = configuration;
        }

        public async Task<UsuarioCartaoDto> Cadastra(UsuarioCartao cartao)
        {
            try
            {
                _context.Add(cartao);
                await _context.SaveChangesAsync();

                var retorno = new UsuarioCartaoDto
                {
                    NumeroCartao = cartao.NumeroCartao,
                    Bandeira = cartao.Bandeira,
                    BlPrincipal = cartao.BlPrincipal,
                    MetodoPagamentoToken = cartao.MetodoPagamentoToken,
                    IdPagamentoEstorno = cartao.IdPagamentoEstorno,
                };

                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro durante o cadastro do cartão.", ex);
            }
        }

        public int IsPrincipal(AlterarPrincipalDto cartao)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                   return _dbConnection.Execute(
                        "SPA_CARTAO_PRINCIPAL",
                        new
                        {
                            ID_USUARIO_CARTAO = cartao.IdUsuarioCartao
                        },
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<UsuarioCartaoDto> Buscar(string token)
        {
            try
            {
                using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                    return _dbConnection.Query<UsuarioCartaoDto>(
                        "SPC_CARTAO_USUARIO",
                        new
                        {
                            TOKEN_USUARIO = token
                        },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

            }catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public List<ResponseCabineAutorizacaoDto> BuscarPorPlaca(string placa)
        {
             using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                   return _dbConnection.Query<ResponseCabineAutorizacaoDto>(
                       "SPC_TOKEN_CARTAO_PLACA",
                       new
                       {
                           PLACA = placa
                       },
                       commandType: CommandType.StoredProcedure
                   ).ToList();
        }
        public Task Deletar(int IdUsuarioCartao)
        {
            try
            {
                var usuarioCartao = _context.UsuarioCartao.FirstOrDefault(C => C.IdUsuarioCartao == IdUsuarioCartao);
                _context.Remove(usuarioCartao);
                _context.SaveChanges();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
