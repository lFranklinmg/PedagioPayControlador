using PedagioPayApiControlador.Data.Entidades;
using System.Reflection.PortableExecutable;

namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public class RequestAutorizacaoDTO : BaseAutorizacaoDto<RequestAutorizacaoDTO.RequestAutorizacaoPayloadDTO>
    {
        public RequestAutorizacaoDTO()
        {
            Header.EventType = 131;
        }
        public class RequestAutorizacaoPayloadDTO
        {
            public RequestAutorizacaoLocalDTO local { get; set; }
            public RequestAutorizacaoRequestDTO request { get; set; }
        }

        public class RequestAutorizacaoLocalDTO
        {
            public int concessionaireId { get; set; }
            public int terminalId { get; set; }
        }

        public class RequestAutorizacaoRequestDTO
        {
            public Guid transactionId { get; set; }
            public string sgaRequestId { get; set; }
            public string paymentType { get; set; }
            public int debitAmount { get; set; }
            public string plate { get; set; }
            public DateTime transactionDateTime { get; set; }
            public string paymentMethodNonce { get; set; }
            public string? paymentMethodToken { get; set; }
            public DateTime? cancellationDateTime { get; set; }
            public string? justification { get; set; }
            public string? cancellationUser { get; set; }
        }

    }
}

