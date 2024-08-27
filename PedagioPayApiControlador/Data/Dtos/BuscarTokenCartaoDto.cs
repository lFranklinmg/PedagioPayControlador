namespace PedagioPayApiControlador.Data.Dtos
{
    public class BuscarTokenCartaoDto
    {
        public string tokenConcessao { get; set; }
        public List<string> placa { get; set;}
    }
}
