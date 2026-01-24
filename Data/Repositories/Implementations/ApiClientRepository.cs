using Core.Entities;
using Dapper;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Implementations
{
    public class ApiClientRepository : IApiClientRepository
    {
        private readonly IDbConnection _connection;

        public ApiClientRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        //searches the DB for apiClient by the ApiKeyHash
        public Task<ApiClient?> GetActiveByApiKeyAsync(string apiKeyHash)
        {
            var sql = @"SELECT * FROM ApiClients WHERE ApiKeyHash = @ApiKeyHash AND IsActive = 1";
            return _connection.QueryFirstOrDefaultAsync<ApiClient>(sql,new { ApiKeyHash = apiKeyHash});
        }

    }
}

