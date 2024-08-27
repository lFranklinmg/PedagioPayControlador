namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public class ResponseCabineAutorizacaoDto : BaseAutorizacaoDto<ResponseCabineAutorizacaoDto.ResponseCabineAutorizacaoPayloadDto>
    {
        public ResponseCabineAutorizacaoDto()
        {
            Header.EventType = 132;
        }

        public class ResponseCabineAutorizacaoPayloadDto
        {
            public string processingCode { get; set; }
            public string processingMessage { get; set; }
            public ResponseCabineAutorizacaoPayloadCarddetailsDTO cardDatails {  get; set; }
            public ResponseCabineAutorizacaoPayloadCardDto payment { get; set; }
        }

        public class ResponseCabineAutorizacaoPayloadCarddetailsDTO
        {
            public string paymentMethodToken { get; set; }
            public string paymentMethodTokenExpiration { get; set; }
            public string paymentMethodType { get; set; }
            public ResponseCabineAutorizacaoPayloadCardDto card { get; set; }
        }

        public class ResponseCabineAutorizacaoPayloadCardDto
        {
            public string brand { get; set; }
            public string number { get; set; }
        }

    }
}
