using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PedagioPayApiControlador.Data.Dtos;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;
using PedagioPayApiControlador.Models;
using PedagioPayApiControlador.Service.Interface;
using Serilog;
using System.Net.NetworkInformation;

namespace PedagioPayApiControlador.Controllers;

[ApiController]
[Route("api/[controller]/{token}")]
public class FadamiPayCabineController : ControllerBase
{
    private readonly PEDAGIOPAYControladorContext _context;
    private readonly IPagamentoService _pagamentoService;
    private readonly ICartaoUsuarioService _cartaoUsuarioService;

    public FadamiPayCabineController(PEDAGIOPAYControladorContext context,
        IPagamentoService pagamentoService,
        ICartaoUsuarioService cartaoUsuarioService)
    {
        _context = context;
        _pagamentoService = pagamentoService;
        _cartaoUsuarioService = cartaoUsuarioService;
    }

    [HttpPost("getCardToken")]
    public ActionResult BuscarPorPlaca([FromRoute] Guid token, [FromHeader] string mac, RequestCabineAutorizacaoDto transacao)
    {
        try
        {
           var retorno = _cartaoUsuarioService.BuscarPorPlaca(mac, transacao);
            return Ok(new { code = StatusCode(200), body = retorno});
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("health")]
    public IActionResult health([FromRoute] Guid token)
    {
        var retorno = new
        {
            data = DateTime.Now, // DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            status = "ok",
            token = token
        };

        return Ok(retorno);
    }


    [HttpPost("SaveTransaction")]
    public ActionResult SaveTransaction ([FromRoute] Guid token, [FromHeader] string mac, RequestTransactionSaveDto.RequestTransactionSavePayloadDto transaction)
    {
        try
        {
            var retorno = _pagamentoService.SaveTransaction(transaction, mac);
            return Ok(new { code = StatusCode(200) });
        }
        catch
        {
            var eventId = Guid.NewGuid();
            var response = new ResponseAutorizacaoDto()
            {
                Header =
                {
                    EventId = eventId,
                    EventDateTime = DateTime.Now,
                    EventType = 132,
                },
                Payload =
                {
                    processingCode = "800",
                    processingMessage = "Não Autenticado",
                }
            };
            return BadRequest(new { code = StatusCode(401), menssagem = "Ação não permitida." });
        }
    }
}
