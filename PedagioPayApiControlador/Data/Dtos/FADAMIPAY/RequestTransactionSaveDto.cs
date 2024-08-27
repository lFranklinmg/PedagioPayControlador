namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public class RequestTransactionSaveDto : BaseAutorizacaoDto<RequestTransactionSaveDto.RequestTransactionSavePayloadDto>
    {
        public RequestTransactionSaveDto()
        {
            Header.EventType = 132;
        }

        public class RequestTransactionSavePayloadDto
        {
            public RequestTransactionSaveTransationDetailsDto transaction { get; set; }
            public RequestTransactionSavePaymentDto paymentDto { get; set; }
            public RequestTransactionSaveStatusIdDto statusId { get; set; }
        }
        public class RequestTransactionSaveTransationDetailsDto
        {
            public string? transactionId { get; set;}
            public string? sgaRequestId { get; set; }
            public string? cob { get; set; }
            public string? dac { get; set; }
            public string? val { get; set; }
            public string? local { get; set; }
            public string? direction { get; set; }
            public string? plate { get; set; }
            public string? tag { get; set; }
            public string? codCupom { get; set; }


        }
        public class RequestTransactionSavePaymentDto
        {
            public int paymentType { get; set; }
            public string nsu { get; set; }
            public int amount { get; set; }
            public RequestTransactionSaveCardDetailsDto cardDetails { get; set; }
        }
        public class RequestTransactionSaveStatusIdDto
        {
            public int statusId {  get; set; }
        }

        public class RequestTransactionSaveCardDetailsDto
        {
            public string? brand { get; set; }
            public int number { get; set; }
        }
    }
}
