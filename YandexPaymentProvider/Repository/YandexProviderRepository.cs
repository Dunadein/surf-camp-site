using Dapper;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using YandexPaymentProvider.Interfaces;

namespace YandexPaymentProvider.Repository
{
    internal class YandexProviderRepository : IYandexProviderRepository
    {
        private readonly string _connection;

        public YandexProviderRepository(string connectionString)
        {
            _connection = connectionString;
        }

        public async Task SaveInstanceId(string instanceId)
        {
            using (var conn = new MySqlConnection(_connection))
            {
                await conn.ExecuteAsync("insert into tbl_yandex_provider (yp_InstanceId) values (@instanceId)"
                    , new { instanceId }
                );
            }
        }

        public async Task<string> GetInstanceId()
        {
            using (var conn = new MySqlConnection(_connection))
            {
                return await conn.ExecuteScalarAsync<string>(
                    "select top 1 yp_InstanceId from tbl_yandex_provider"
                );
            }
        }
    }
}
