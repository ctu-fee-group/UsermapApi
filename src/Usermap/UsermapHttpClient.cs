using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Usermap
{
    public class UsermapHttpClient
    {
        private readonly IRestClient _client;
        private readonly ILogger _logger;
        private readonly UsermapApiOptions _options;
        
        public UsermapHttpClient(IRestClient client, ILogger<UsermapHttpClient> logger, IOptionsSnapshot<UsermapApiOptions> options)
        {
            _client = client;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<TEntity?> GetAsync<TEntity>(string url, CancellationToken token = default)
        {
            IRestRequest request = new RestRequest(url, Method.GET);

            IRestResponse<TEntity?>? response = await _client.ExecuteAsync<TEntity?>(request, token);

            if (!(response?.IsSuccessful ?? false) || response.Data is null)
            {
                if (_options.ThrowOnError && response?.StatusCode != HttpStatusCode.NotFound)
                {
                    if (response?.ErrorException != null)
                    {
                        throw response.ErrorException;
                    }
                    
                    throw new InvalidOperationException(
                        $"Could not obtain response from the server {response?.StatusCode} {response?.ErrorMessage} {response?.Content}");                }
            }

            if (response is null)
            {
                return default;
            }

            return response.Data;
        }
    }
}