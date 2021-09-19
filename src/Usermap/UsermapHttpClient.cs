//
//  UsermapHttpClient.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Usermap
{
    /// <summary>
    /// Client for calling usermap http api.
    /// </summary>
    public class UsermapHttpClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        private readonly UsermapApiOptions _options;
        private readonly JsonSerializerOptions _serializerOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsermapHttpClient"/> class.
        /// </summary>
        /// <param name="clientFactory">The http client factory with configured "Usermap" client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="options">The options.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        public UsermapHttpClient
        (
            IHttpClientFactory clientFactory,
            ILogger<UsermapHttpClient> logger,
            IOptionsSnapshot<UsermapApiOptions> options,
            IOptions<JsonSerializerOptions> serializerOptions
        )
        {
            _serializerOptions = serializerOptions.Value;
            _clientFactory = clientFactory;
            _logger = logger;
            _options = options.Value;
        }

        /// <summary>
        /// Calls GET on the specified url.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="configureRequest">The action that will be called to configure the request.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <typeparam name="TEntity">The type of the entity to retrieve.</typeparam>
        /// <returns>The retrieved entity or null if it was not found.</returns>
        /// <exception cref="Exception">If the response could not be retrieved from the server and <see cref="UsermapApiOptions.ThrowOnError"/> is true.</exception>
        /// <exception cref="InvalidOperationException">If the response could not be retrieved from the server and <see cref="UsermapApiOptions.ThrowOnError"/> is true..</exception>
        public async Task<TEntity?> GetAsync<TEntity>
            (string path, Action<HttpRequestMessage>? configureRequest = null, CancellationToken token = default)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, path);
            configureRequest?.Invoke(requestMessage);

            try
            {
                var client = _clientFactory.CreateClient("Usermap");
                using var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, token);
                return await ParseResponse<TEntity>(response, token);
            }
            catch (Exception e)
            {
                if (_options.ThrowOnError)
                {
                    throw;
                }

                _logger.LogError(e, "There was an error while calling the api");
                return default;
            }
        }

        private async Task<TEntity?> ParseResponse<TEntity>(HttpResponseMessage response, CancellationToken token)
        {
            if (response.IsSuccessStatusCode)
            {
                var entity = await JsonSerializer.DeserializeAsync<TEntity>
                (
                    await response.Content.ReadAsStreamAsync(token),
                    _serializerOptions,
                    token
                );

                return entity;
            }

            try
            {
                using var content = await response.Content.ReadAsStreamAsync(token);
                var reader = new StreamReader(content);
                var contentString = await reader.ReadToEndAsync();

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return default;
                }

                string errorMessage = $"There was an error with calling usermap api: {response.StatusCode} {response.ReasonPhrase} {contentString}";
                if (_options.ThrowOnError)
                {
                    throw new Exception(errorMessage);
                }
                _logger.LogError(errorMessage);
                return default;
            }
            catch (Exception e)
            {
                if (_options.ThrowOnError)
                {
                    throw;
                }

                _logger.LogError(e, $"There was an error while processing the http response {response.StatusCode} {response.ReasonPhrase}");

                return default;
            }
        }
    }
}