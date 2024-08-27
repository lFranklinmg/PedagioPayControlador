using PedagioPayApiControlador.Data.Entidades;
using System.Reflection.PortableExecutable;

namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public class ResponseAutorizacaoDto : BaseAutorizacaoDto<ResponseAutorizacaoDto.ResponseAutorizacaoPayloadDTO>
    {

        public ResponseAutorizacaoDto()
        {
            Header.EventType = 131;
        }
        public class ResponseAutorizacaoPayloadDTO
        {
            public string processingCode { get; set; }
            public string processingMessage { get; set; }
            public ResponseAutorizacaoPayloadTransactionDTO transaction { get; set; }
            public ResponseAutorizacaoPayloadPaymentDTO payment { get; set; }
        }

        public class ResponseAutorizacaoPayloadTransactionDTO
        {
            public string transactionId { get; set; }
            public string sgaRequestId { get; set; }
        }

        public class ResponseAutorizacaoPayloadPaymentDTO
        {
            public string paymentType { get; set; }
            public string nsu { get; set; }
            public int amount { get; set; }
            public object paymentId { get; set; }
            public object orderId { get; set; }
            public ResponseAutorizacaoPayloadCarddetailsDTO cardDetails { get; set; }
        }

        public class ResponseAutorizacaoPayloadCarddetailsDTO
        {
            public string paymentMethodToken { get; set; }
            public string paymentMethodTokenExpiration { get; set; }
            public string paymentMethodType { get; set; }
            public ResponseAutorizacaoPayloadCardDTO card { get; set; }
        }

        public class ResponseAutorizacaoPayloadCardDTO
        {
            public string brand { get; set; }
            public string number { get; set; }
        }

    }
}