using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PedagioPayApiControlador.Data.Dtos;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;
using PedagioPayApiControlador.Data.Entidades;
using PedagioPayApiControlador.Data.Interfaces;
using PedagioPayApiControlador.Models;
using PedagioPayApiControlador.Service.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace PedagioPayApiControlador.Service
{
    public class CartaoUsuarioService : ICartaoUsuarioService
    {
        private readonly ICartaoUsuarioRepository _cartaoUsuarioRepository;
        private readonly IPagamentoService _pagamentoService;
        private readonly ICriptografiaService _criptografiaService;

        public CartaoUsuarioService(ICartaoUsuarioRepository cartaoUsuario,
            IPagamentoService pagamentoService,
            ICriptografiaService criptografiaService)
        {
            _cartaoUsuarioRepository = cartaoUsuario;
            _pagamentoService = pagamentoService;
            _criptografiaService = criptografiaService;
        }
        public async Task<UsuarioCartaoDto> Adicionar(TransacaoDto transacao, string token)
        {
            try
            {
                var pagamentoTeste = await _pagamentoService.ProcessarTransacao(transacao);
                var cardDetails = pagamentoTeste.Payload.payment.cardDetails;
                var card = pagamentoTeste.Payload.payment.cardDetails.card;
                var cardNumber = card.number;


                UsuarioCartao novoCartao = new UsuarioCartao()
                {
                    NumeroCartao = cardNumber.Substring(Math.Max(0, cardNumber.Length - 4)),
                    MetodoPagamentoToken = cardDetails.paymentMethodToken,
                    MetodoTipoPagamento = cardDetails.paymentMethodType,
                    Bandeira = card.brand,
                    TokenUsuario = token,
                    BlPrincipal = transacao.Bl_Principal,
                    IdPagamentoEstorno = pagamentoTeste.Payload.transaction.transactionId
                };
                var cadastraNovoCartao = await _cartaoUsuarioRepository.Cadastra(novoCartao);
                return cadastraNovoCartao;
                
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao cadastrar cartão.", ex);
            }
        }
        public List<UsuarioCartaoDto> Buscar(string token)
        {
            try
            {
                var cartaoUsuario = _cartaoUsuarioRepository.Buscar(token);
                return cartaoUsuario.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public bool Validar(UsuarioCartaoDto usuarioCartao)
        {
            return true;
        }

        public ResponseCabineAutorizacaoDto BuscarPorPlaca(string mac, RequestCabineAutorizacaoDto request)
        {
            try
            {
                var eventId = Guid.NewGuid();
                var transactionId = Guid.NewGuid();

                var dadosCartao = _cartaoUsuarioRepository.BuscarPorPlaca(request.placa);
                if (!DecriptMac(mac, request) && dadosCartao == null)
                {

                    var ex = new UnauthorizedAccessException("Falha ao autenticar requisição!");

                    ex.Data["response"] = new ResponseCabineAutorizacaoDto()

                    {
                        Header = new HeaderAutorizacaoDto()
                        {
                            EventId = eventId,
                            EventDateTime = DateTime.Now,
                            EventType = 132,
                        },

                        Payload = new ResponseCabineAutorizacaoDto.ResponseCabineAutorizacaoPayloadDto()
                        {
                            processingCode = "800",
                            processingMessage = "Não Autenticado",
                        }
                    };

                    throw ex;
                }

                var tokenPagamento = _cartaoUsuarioRepository.BuscarPorPlaca(request.placa).FirstOrDefault();
                var retorno = new ResponseCabineAutorizacaoDto
                {
                    Header = new HeaderAutorizacaoDto()
                    {
                        EventId = eventId,
                        EventDateTime = DateTime.Now,
                        EventType = 132,
                    },
                    Payload = new ResponseCabineAutorizacaoDto.ResponseCabineAutorizacaoPayloadDto()
                    {
                        processingCode = "001",
                        processingMessage = "Sucesso",
                        
                        cardDatails = new ResponseCabineAutorizacaoDto.ResponseCabineAutorizacaoPayloadCarddetailsDTO()
                        {
                            paymentMethodToken = tokenPagamento.Payload.cardDatails.paymentMethodToken,
                            paymentMethodTokenExpiration = tokenPagamento.Payload.cardDatails.paymentMethodTokenExpiration,
                            paymentMethodType = tokenPagamento.Payload.cardDatails.paymentMethodType,
                            card =
                            {
                                brand = tokenPagamento.Payload.cardDatails.card.brand,
                                number = tokenPagamento.Payload.cardDatails.card.number,
                            }
                        }
                    }
                };
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsPrincipal(AlterarPrincipalDto cartao)
        {
            try
            {
                var teste = _cartaoUsuarioRepository.IsPrincipal(cartao);
                if (_cartaoUsuarioRepository.IsPrincipal(cartao) == 9)
                {
                    return true;
                }
                return false;
            }catch(Exception ex)
            {
                return false;
            }
        }
        public Task Desativar(int idCartaoUsuario)
        {
            try
            {
                _cartaoUsuarioRepository.Deletar(idCartaoUsuario);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public bool DecriptMac(string mac, RequestCabineAutorizacaoDto request)
        {
            var decriptMacResult = _criptografiaService.Decriptar(mac);
            if (decriptMacResult == request.tokenConcessao + request.placa)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
