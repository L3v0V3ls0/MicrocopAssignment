using Core.DTOs.Response;
using Core.Entities;
using Core.Helpers;
using Core.Services.Interfaces;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Implementations
{
    public class ApiClientService : IApiClientService
    {
        private readonly IApiClientRepository _apiClientRepo;

        public ApiClientService(IApiClientRepository apiClientRepository) 
        {
            _apiClientRepo = apiClientRepository;
        }
        //Get Api client by apiKey
        public async Task<ApiClient?> GetApiClient(string apiKey)
        {
            var apiClientHash = ApiKeyHelper.HashApiKey(apiKey);

            var apiClient = await _apiClientRepo.GetActiveByApiKeyAsync(apiClientHash);

            return apiClient;
        }
    }
}
