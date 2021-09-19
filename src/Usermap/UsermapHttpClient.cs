//
//  UsermapHttpClient.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Usermap
{
    /// <summary>
    /// Client for calling usermap http api.
    /// </summary>
    public class UsermapHttpClient
    {
        private readonly IRestClient _client;
        private readonly ILogger _logger;
        private readonly UsermapApiOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsermapHttpClient"/> class.
        /// </summary>
        /// <param name="client">The rest client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="options">The options.</param>
        public UsermapHttpClient
            (IRestClient client, ILogger<UsermapHttpClient> logger, IOptionsSnapshot<UsermapApiOptions> options)
        {
            _client = client;
            _logger = logger;
            _options = options.Value;
        }

        /// <summary>
        /// Calls GET on the specified url.
        /// </summary>
        /// <param name="url">The path.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
        /// <returns>The retrieved entity or null if it was not found.</returns>
        /// <exception cref="Exception">If the response could not be retrieved from the server and <see cref="UsermapApiOptions.ThrowOnError"/> is true.</exception>
        /// <exception cref="InvalidOperationException">If the response could not be retrieved from the server and <see cref="UsermapApiOptions.ThrowOnError"/> is true..</exception>
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

                    throw new InvalidOperationException
                    (
                        $"Could not obtain response from the server {response?.StatusCode} {response?.ErrorMessage} {response?.Content}"
                    );
                }
            }

            if (response is null)
            {
                return default;
            }

            return response.Data;
        }
    }
}