namespace PedagioPayApiControlador.Service.Interface
{

    public interface ICriptografiaService
    {
        string Encriptar(string text);
        string Decriptar(string text);
    }
}
