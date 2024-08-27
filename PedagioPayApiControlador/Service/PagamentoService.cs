using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PedagioPayApiControlador.Data.Dtos;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;
using PedagioPayApiControlador.Data.Entidades;
using PedagioPayApiControlador.Service.Interface;
using System.Net;
using System;
using static PedagioPayApiControlador.Data.Dtos.FADAMIPAY.ResponseAutorizacaoDto;
using System.Reflection.Metadata;
using System.Text;
using PedagioPayApiControlador.Models;
using Newtonsoft.Json;
using System.Text.Json;
using RestSharp;
using Azure.Core;
using static PedagioPayApiControlador.Data.Dtos.FADAMIPAY.RequestAutorizacaoDTO;
using Serilog;
using PedagioPayApiControlador.Data.Interfaces;
using PedagioPayApiControlador.Data.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq;
using static PedagioPayApiControlador.Data.Dtos.FADAMIPAY.RequestCancelamentoDto;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

namespace PedagioPayApiControlador.Service
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IConfiguration _configuration;
        private readonly ICartaoUsuarioService _cartaoUsuarioService;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDebitoRepository _debitoRepository;
        private readonly ICriptografiaService _criptografiaService;

        public PagamentoService(IDebitoRepository debitoRepository)
        {
           _debitoRepository = debitoRepository;
        }
        public async Task<ResponseAutorizacaoDto> ProcessarTransacao(TransacaoDto transacao)
        {
            
            try
            {
                //Realiza Pagamento
                var eventId = Guid.NewGuid();
                var transactionId = Guid.NewGuid();
                var request = new RequestAutorizacaoDTO()
                {
                    Header = new HeaderAutorizacaoDto()
                    {
                        EventId = eventId,
                        EventDateTime = DateTime.Now,
                        EventType = 131,
                    },
                    Payload = new RequestAutorizacaoPayloadDTO
                    {
                        local = new RequestAutorizacaoLocalDTO
                        {
                            concessionaireId = 1,
                            terminalId = 2586001
                        },
                        request = new RequestAutorizacaoRequestDTO
                        {
                            transactionId = transactionId,
                            sgaRequestId = transactionId.ToString(),
                            paymentType = "pedagioPay",
                            debitAmount = transacao.Amount,

                            plate = transacao.Placa,
                            transactionDateTime = DateTime.Now,
                            paymentMethodNonce = transacao.Nonce,
                            paymentMethodToken = transacao.TokenCard,
                        }
                    }
                };

                var requestHeader = EncriptCode(eventId.ToString()).Result;
                var client = new RestClient("https://fadamipay-autorizador1-online.com/fadamipay-autorizador-api-pos/api/PedagioPay/600D0F16-8161-49C3-B106-D263990A9C5E/authorize");
                var requestNewCard = new RestRequest("https://fadamipay-autorizador1-online.com/fadamipay-autorizador-api-pos/api/PedagioPay/600D0F16-8161-49C3-B106-D263990A9C5E/authorize", Method.Post);
                requestNewCard.AddHeader("content-type", "application/json");
                requestNewCard.AddHeader("mac", $"{requestHeader}");
                requestNewCard.AddJsonBody(request);

                var response = client.Execute<object>(requestNewCard);
                var resposta = response.Content;

                if (response.IsSuccessStatusCode && transacao.SaveCard == false) 
                {
                    Log.Information(resposta);

                    var teste = JsonConvert.DeserializeObject<ResponseAutorizacaoDto>(resposta);
                    string payload = JsonConvert.SerializeObject(teste.Payload);
                        Log.Information(payload);

                    if (teste.Payload.processingCode == "001")
                    {
                        Log.Information("REQUEST FADAMIPAY :" + JsonConvert.SerializeObject(request));
                        try
                        {
                            var teste1 = transacao.Transaction;
                            _debitoRepository.CadastraHistorico(payload, string.Join(",", transacao.Transaction));
                            //return Task.CompletedTask;
                            return teste;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Erro ao realizar transação", ex);
                        }
                    }
                    else
                    {
                        //return Task.FromException(new Exception("Operação não realizada."));
                        throw new Exception("Erro ao realizar transação");
                    }
                }
                else if(response.IsSuccessStatusCode && transacao.SaveCard == true)
                {
                    var teste = JsonConvert.DeserializeObject<ResponseAutorizacaoDto>(resposta);
                    if (teste.Payload.processingCode == "001")
                    {
                        return teste;
                    }
                    //return Task.CompletedTask;
                    throw new Exception("Erro ao realizar transação");
                }
                else
                {
                    //return Task.FromException(new Exception("Operação não realizada."));
                    return null;
                }
            }
            catch (Exception ex)
            {
                //return Task.FromException(new Exception("Operação não realizada."));
                throw new Exception("Erro ao realizar transação", ex);
            }
            //return Task.FromException(new Exception("Operação não realizada."));
        }

        public async Task CancelarTransacao(CancelamentoDto cancelamento)
         {
            var eventId = Guid.NewGuid();
            var transactionId = Guid.NewGuid();
            var request = new RequestCancelamentoDto()
            {
                Header = new HeaderAutorizacaoDto()
                {
                    EventId = eventId,
                    EventDateTime = DateTime.Now,
                    EventType = 132,
                },
                Payload = new RequestCancelamentoPayloadDto
                {
                    amount = cancelamento.Amount,
                    cancellationDateTime = DateTime.Now,
                    cancellationUser = cancelamento.Usuario,
                    justification = @"Dívida não reconhecida pelo usuário",
                    sgaRequestId = cancelamento.PaymentId
                }
            };

            var requestHeader = EncriptCode(eventId.ToString()).Result;
            var client = new RestClient("https://fadamipay-autorizador1-online.com/fadamipay-autorizador-api-pos/api/PedagioPay/600D0F16-8161-49C3-B106-D263990A9C5E/cancel");
            var requestNewCard = new RestRequest("https://fadamipay-autorizador1-online.com/fadamipay-autorizador-api-pos/api/PedagioPay/600D0F16-8161-49C3-B106-D263990A9C5E/cancel", Method.Post);
            requestNewCard.AddHeader("content-type", "application/json");
            requestNewCard.AddHeader("mac", $"{requestHeader}");
            requestNewCard.AddJsonBody(request);

            var response = client.Execute<object>(requestNewCard);
            var resposta = response.Content;
            if (response.IsSuccessStatusCode)
            {
                Log.Information(resposta);
                var teste = JsonConvert.DeserializeObject<ResponseAutorizacaoDto>(resposta);
                if (teste.Payload.processingCode != "001")
                {
                    throw new Exception();
                }
            }
        }

        public ResponseAutorizacaoDto SaveTransaction(RequestTransactionSaveDto.RequestTransactionSavePayloadDto transaction, string mac)
        {
            try
            {
                var eventId = Guid.NewGuid();
                if (!DecriptMac(mac, eventId))
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

                var historicoCabine = _debitoRepository.SaveTransaction(transaction);
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
                        processingCode = "001",
                        processingMessage = "Sucesso"
                    }
                };
                if(historicoCabine != 0)
                {
                    return response;
                }
                throw new Exception("Ação não permitida");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> EncriptCode(string code)
        {
            
            var httpClient = new HttpClient();
            using HttpResponseMessage responseGet = await httpClient.GetAsync($"http://fadami.tmp.br/fadami-autorizador-api-qrcodeativo/helper/CriptografarTexto/{code}");
            var codeFinal = responseGet.Content.ReadAsStringAsync();
            return codeFinal.Result.ToString();
        }
        public bool DecriptMac(string mac, Guid eventId)
        {
            var decriptMacResult = _criptografiaService.Decriptar(mac);
            if (decriptMacResult == eventId.ToString())
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
