using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PedagioPayApiControlador.Data.Dtos.FADAMIPAY;
using PedagioPayApiControlador.Data.Interfaces;
using PedagioPayApiControlador.Models;
using Serilog;
using System.Data;
using System.Data.Common;

namespace PedagioPayApiControlador.Data.Repositories
{
    public class DebitoRepository : IDebitoRepository
    { 
        private readonly PEDAGIOPAYControladorContext _context;
        private readonly IConfiguration _configuration;
        public DebitoRepository(PEDAGIOPAYControladorContext context, IConfiguration configuration)
        {
            _context = context;
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            _configuration = configuration;
            _configuration = configuration;
        }
        public void CadastraHistorico(string responseAutorizacao, string idPassagem)
        {
            try
            {
                Log.Information("ID PASSAGEM :"+idPassagem);

                Log.Information("PAYLOAD:" + responseAutorizacao);

                using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                    _dbConnection.Execute(
                        "SPI_PEDIDO",
                        new
                        {
                            JSON = responseAutorizacao,
                            ID_PASSAGEM = idPassagem,
                        },
                        commandType: CommandType.StoredProcedure
                    );
            }
            catch (Exception ex)
            {
                Log.Error("erro ao cadastrar debitos no banco", ex);
                throw new Exception("Erro ao adicionar débitos.", ex);
            }
        }

        public int SaveTransaction(RequestTransactionSaveDto.RequestTransactionSavePayloadDto transaction)
        {
            using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
            return _dbConnection.Execute(
                       "SPI_PASSAGEM_PEDIDO",
                       new
                       {
                           PAYLOAD = transaction
                       },
                       commandType: CommandType.StoredProcedure
                   );
        }

       /* public bool EstornoPagamento()
        {
            try 
            {
                using (var _dbConnection = new SqlConnection(_configuration.GetConnectionString("DevelopmentDB")))
                    if (_dbConnection.Execute(
                        "SPA_PEDIDO",
                        new
                        {
                            JSON = ,
                            ID_PASSAGEM = ,
                            ID_PEDIDO =,

                        },
                        commandType: CommandType.StoredProcedure
                    ) == 1)
                    {
                        return true;
                    }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar débitos.", ex);
            }
        }*/
    }
}
