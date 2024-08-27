namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public abstract class BaseAutorizacaoDto<T> where T : class
    {
        public HeaderAutorizacaoDto Header { get; set; }
        public T Payload { get; set; }
        public BaseAutorizacaoDto()
        {
            Header = new HeaderAutorizacaoDto();
        }
    }
}
