//
//  UsermapHttpClient.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
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
        private readonly ILogger _logger;
        private readonly UsermapApiOptions _options;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenProvider _tokenProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsermapHttpClient"/> class.
        /// </summary>
        /// <param name="tokenProvider">The token provider.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="options">The options.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        /// <param name="httpClientFactory">The http client factory.</param>
        public UsermapHttpClient
        (
            TokenProvider tokenProvider,
            ILogger<UsermapHttpClient> logger,
            IOptionsSnapshot<UsermapApiOptions> options,
            IOptions<JsonSerializerOptions> serializerOptions,
            IHttpClientFactory httpClientFactory
        )
        {
            _tokenProvider = tokenProvider;
            _httpClientFactory = httpClientFactory;
            _serializerOptions = serializerOptions.Value;
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
            (string path, Action<HttpRequestMessageBuilder>? configureRequest = null, CancellationToken token = default)
        {
            using var requestMessage = ConfigureRequest(path, HttpMethod.Get, configureRequest);
            using var response = await ExecuteRequestAsync(requestMessage, token);

            if (response is null)
            {
                return default;
            }

            return await ParseResponseEntity<TEntity>(response, token);
        }

        /// <summary>
        /// Calls HEAD on the specified url. Returns true for successful response, false for unsuccessful.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="configureRequest">The action that will be called to configure the request.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>Whether successful response was obtained.</returns>
        /// <exception cref="Exception">If the response could not be retrieved from the server and <see cref="UsermapApiOptions.ThrowOnError"/> is true.</exception>
        /// <exception cref="InvalidOperationException">If the response could not be retrieved from the server and <see cref="UsermapApiOptions.ThrowOnError"/> is true..</exception>
        public async Task<bool> HeadAsync
            (string path, Action<HttpRequestMessageBuilder>? configureRequest = null, CancellationToken token = default)
        {
            using var requestMessage = ConfigureRequest(path, HttpMethod.Head, configureRequest);
            using var response = await ExecuteRequestAsync(requestMessage, token);

            if (response is null)
            {
                return false;
            }

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Calls GET on the specified url, returns an image.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="configureRequest">The action that will be called to configure the request.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>The retrieved image or null if it was not found.</returns>
        /// <exception cref="Exception">If the response could not be retrieved from the server and <see cref="UsermapApiOptions.ThrowOnError"/> is true.</exception>
        /// <exception cref="InvalidOperationException">If the response could not be retrieved from the server and <see cref="UsermapApiOptions.ThrowOnError"/> is true..</exception>
        public async Task<Image?> GetImageAsync
            (string path, Action<HttpRequestMessageBuilder>? configureRequest = null, CancellationToken token = default)
        {
            using var requestMessage = ConfigureRequest
            (
                path,
                HttpMethod.Get,
                (builder) =>
                {
                    builder.WithContentAccept("image/png");
                    configureRequest?.Invoke(builder);
                }
            );
            using var response = await ExecuteRequestAsync(requestMessage, token);

            if (response is null)
            {
                return null;
            }

            if (response.IsSuccessStatusCode)
            {
                return Image.FromStream(await response.Content.ReadAsStreamAsync(token));
            }

            await HandleErrorResponse(response, token);
            return null;
        }

        private async Task<HttpResponseMessage?> ExecuteRequestAsync
            (HttpRequestMessage request, CancellationToken token)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Usermap");
                return await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
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

        private HttpRequestMessage ConfigureRequest
            (string path, HttpMethod method, Action<HttpRequestMessageBuilder>? configureAction)
        {
            var requestMessageBuilder = new HttpRequestMessageBuilder(method, path);
            configureAction?.Invoke(requestMessageBuilder);
            var requestMessage = requestMessageBuilder.Build();

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue
            (
                "Bearer",
                _tokenProvider.AccessToken
            );

            return requestMessage;
        }

        private async Task HandleErrorResponse(HttpResponseMessage response, CancellationToken token)
        {
            try
            {
                using var content = await response.Content.ReadAsStreamAsync(token);
                var reader = new StreamReader(content);
                var contentString = await reader.ReadToEndAsync();

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return;
                }

                string errorMessage =
                    $"There was an error with calling usermap api: {response.StatusCode} {response.ReasonPhrase} {contentString}";
                if (_options.ThrowOnError)
                {
                    throw new Exception(errorMessage);
                }

                _logger.LogError(errorMessage);
            }
            catch (Exception e)
            {
                if (_options.ThrowOnError)
                {
                    throw;
                }

                _logger.LogError
                (
                    e,
                    $"There was an error while processing the http response {response.StatusCode} {response.ReasonPhrase}"
                );
            }
        }

        private async Task<TEntity?> ParseResponseEntity<TEntity>(HttpResponseMessage response, CancellationToken token)
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

            await HandleErrorResponse(response, token);
            return default;
        }
    }
}