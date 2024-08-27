using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedagioPayApiControlador.Data.Dtos;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;
using PedagioPayApiControlador.Models;
using PedagioPayApiControlador.Service.Interface;
using Serilog;

namespace PedagioPayApiControlador.Controllers;

[ApiController]
[Route("[Controller]")]
public class CartaoUsuarioController : ControllerBase
{
    private readonly PEDAGIOPAYControladorContext _context;
    private readonly IPagamentoService _pagamentoService;
    private readonly ICartaoUsuarioService _cartaoUsuarioService;

    public CartaoUsuarioController(PEDAGIOPAYControladorContext context, 
        IPagamentoService pagamentoService ,
        ICartaoUsuarioService cartaoUsuarioService)
    {
        _context = context;
        _pagamentoService = pagamentoService;
        _cartaoUsuarioService = cartaoUsuarioService;
    }

    [HttpPost("AdicionarNovoCartao")]
    public async Task<ActionResult> Cadastro(TransacaoDto cartaoUsuario, [FromHeader] string token)
    {
        try
        {
           var adicionaCartao = await _cartaoUsuarioService.Adicionar(cartaoUsuario, token);

            if (adicionaCartao != null)
            {
                return Ok(new { code = StatusCode(200), 
                    cardNumber = adicionaCartao.NumeroCartao,
                    bandeira = adicionaCartao.Bandeira, 
                    metodoPagamentoToken = adicionaCartao.MetodoPagamentoToken, 
                    isPrincipal = adicionaCartao.BlPrincipal, 
                    IdPagamentoEstorno = adicionaCartao.IdPagamentoEstorno });
            }
            return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), menssagem = "Ação não permitida." });
        }
    }

    [HttpPost("IsPrincipal")]
    public ActionResult IsPrincipalCard(AlterarPrincipalDto cartao)
    {
        try
        {
            if(!_cartaoUsuarioService.IsPrincipal(cartao))
            {
                return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });

            }
            return Ok(new { code = StatusCode(200), mensagem = "Alteração realizada com sucesso."});
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });
        }
    }


    [HttpPost("MeusCartoes")]
    public ActionResult BuscarCartaoUsuario([FromHeader] string token)
    {
        try
        {
            var cartoes = _cartaoUsuarioService.Buscar(token);
            return Ok(new { code = StatusCode(200), cartoes = cartoes });

        }
        catch (Exception ex)
        {
            Log.Information("Teste de serilog");
            return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });
        }

    }

    [HttpPost("DesativarCartao")]
    public ActionResult Desativar([FromHeader] int idUsuarioCartao)
    {
        try
        {
            var cartao = _cartaoUsuarioService.Desativar(idUsuarioCartao);
            return Ok(new { code = StatusCode(200), menssagem = "Cartão Desativado" });

        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });
        }
    }
}
