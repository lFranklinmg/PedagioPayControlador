using Microsoft.AspNetCore.Mvc;
using PedagioPayApiControlador.Data.Dtos;
using System.Net.Http.Json;
using System.Net.Http;
using Azure.Core;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;
using static PedagioPayApiControlador.Data.Dtos.FADAMIPAY.ResponseAutorizacaoDto;
using PedagioPayApiControlador.Data.Entidades;
using PedagioPayApiControlador.Models;
using PedagioPayApiControlador.Service.Interface;

namespace PedagioPayApiControlador.Controllers;

[ApiController]
[Route("[Controller]")]

public class PagamentoController : ControllerBase
{
    private readonly PEDAGIOPAYControladorContext _context;
    private readonly IPagamentoService _pagamentoService;
    public PagamentoController(PEDAGIOPAYControladorContext context,
        IPagamentoService pagamentoService)
    {
        _context = context;
        _pagamentoService = pagamentoService;
    }

    [HttpPost("Processar")]
    public ActionResult ProcessarTransacao(TransacaoDto transacao)
    {

        try
        {
            var adicionaCartao = _pagamentoService.ProcessarTransacao(transacao);
            if (adicionaCartao.IsCompleted)
            {
                return Ok(new { code = StatusCode(200), cardNumber = "", bandeira = "", isPrincipal = true });
            }
            return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(400), menssagem = "Ação não permitida." });
        }
    }

    [HttpPost("CancelarTransacao")]
    public ActionResult CancelarTransacao(CancelamentoDto cancelamento)
    {
        try
        {
            if (cancelamento == null)
            {
                return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida."});
            }
            try
            {
                var authorize = _pagamentoService.CancelarTransacao(cancelamento);
                return Ok(new { code = StatusCode(200) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });
            }

        }
        catch (Exception ex)
        {
            return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });
        }
    }

}
