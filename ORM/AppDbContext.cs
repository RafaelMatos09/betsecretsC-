using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace betsecrets.ORM
{
    public class AppDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("Postgres") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException(
                    "Connection string 'Postgres' não configurada. " +
                    "No Render, defina ConnectionStrings__Postgres nas variáveis de ambiente.");
        }

        public async Task TestConnectionAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
        }

        private IDbConnection Connection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        // SELECT * ...
        public async Task<IEnumerable<T>> GetAllAsync<T>(string sql, object? parametros = null)
        {
            using var connection = Connection();
            return await connection.QueryAsync<T>(sql, parametros);
        }

        // SELECT WHERE ...
        //public async Task<T?> GetAsync<T>(string sql, object? parametros = null)
        //{
        //    using var connection = Connection();
        //    return await connection.QueryFirstOrDefaultAsync<T>(sql, parametros);
        //}

        // INSERT, UPDATE e DELETE
        public async Task<int> ExecuteAsync(string sql, object? parametros = null)
        {
            using var connection = Connection();
            return await connection.ExecuteAsync(sql, parametros);
        }

        // COUNT, SUM, MAX...
        //public async Task<T> ExecuteScalarAsync<T>(string sql, object? parametros = null)
        //{
        //    using var connection = Connection();
        //    return await connection.ExecuteScalarAsync<T>(sql, parametros);
        //}
    }
}
