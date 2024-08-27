namespace PedagioPayApiControlador.Data.Dtos
{
    public class CancelamentoDto
    {
        public int Amount { get; set; }
        public string Usuario { get; set; }
        public string Justificativa { get; set; }
        public string PaymentId { get; set; }
    }
}
