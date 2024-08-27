namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public class RequestCancelamentoDto : BaseAutorizacaoDto<RequestCancelamentoDto.RequestCancelamentoPayloadDto>
    {
        public RequestCancelamentoDto()
        {
            Header.EventType = 131;
        }

        public class RequestCancelamentoPayloadDto
        {
            public string sgaRequestId { get; set; }
            public int amount { get; set; }
            public string cancellationUser { get; set; }
            public DateTime cancellationDateTime { get; set; }
           public string justification { get; set; }
        }
    }
}
