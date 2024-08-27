using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using System.Text;
namespace TestePedagioPayApiControlador
{
    public class UnitServiceTests
    {
        private readonly WebApplicationFactory<Program> _webApplicationFactory;
        public UnitServiceTests(WebApplicationFactory<Program> webApplication)
        {
            _webApplicationFactory = webApplication;
        }
        [Fact]
        public async Task ValidaDados()
        {
            var client = _webApplicationFactory.CreateClient();
            var token = "isto é um token";
            var stringContent = new StringContent(token, Encoding.UTF8);

            var response = await client.PostAsync("Cartao/usuario", stringContent);

        }
    }
}