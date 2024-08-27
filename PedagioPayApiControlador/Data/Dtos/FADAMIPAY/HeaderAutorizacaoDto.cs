using PedagioPayApiControlador.Data.Entidades;

namespace PedagioPayApiControlador.Data.Dtos.FADAMIPAY
{
    public class HeaderAutorizacaoDto
    {
        private Guid? _eventId;

        public Guid EventId { get; set; }
        

        private string _eventDateTime;
        public DateTime EventDateTime { get; set; }
       

        //public TipoEventoEnum EventType { get; set; }
        public int EventType { get; set; }

    }
}
