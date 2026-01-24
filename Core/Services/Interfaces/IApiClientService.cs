using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IApiClientService
    {
        public Task<ApiClient> GetApiClient(string apiKey);
    }
}
