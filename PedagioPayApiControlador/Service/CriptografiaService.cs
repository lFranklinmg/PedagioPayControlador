using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PedagioPayApiControlador.Service.Interface;
using System.Security.Cryptography;
using System.Text;

namespace PedagioPayApiControlador.Service
{
    public class CriptograService : ICriptografiaService
    {
        private const string KeyExtension = ".pem";
        private readonly string _pathKeys;
        private readonly string _privateKeyFile;
        private readonly string _publicKeyFile;
        public CriptograService()
        {
            _pathKeys = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "keys");
            _privateKeyFile = Path.Combine(_pathKeys, $"private-key{KeyExtension}");
            _publicKeyFile = Path.Combine(_pathKeys, $"public-key{KeyExtension}");
        }


        public string Encriptar(string text)
        {
            var rsa = CreateRSA(_publicKeyFile);

            var buffer = Encoding.UTF8.GetBytes(text);
            var cript = rsa.Encrypt(buffer, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(cript);
        }

        public string Decriptar(string text)
        {
            var rsa = CreateRSA(_privateKeyFile);

            var buffer = Convert.FromBase64String(text);
            var cript = rsa.Decrypt(buffer, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(cript);
        }

        private RSA CreateRSA(string fileKey)
        {
            var rsa = RSA.Create();
            char[] key = null;

            using (var reader = File.OpenText(fileKey))
                key = reader.ReadToEnd().ToCharArray();

            rsa.ImportFromPem(key);

            return rsa;
        }
    }
}
