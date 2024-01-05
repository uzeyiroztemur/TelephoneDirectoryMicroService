using Core.Utilities.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace DataAccess.DataContext.Dapper.Helper
{
    public class ConnectionHelper
    {
        private readonly string _connectionStrings;

        public ConnectionHelper()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _connectionStrings = configuration.GetConnectionString("PostgreSql");
        }

        public IDbConnection CreateSqlConnection() => new NpgsqlConnection(_connectionStrings);
    }
}