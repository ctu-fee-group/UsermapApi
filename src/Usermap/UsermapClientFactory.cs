//
//  UsermapClientFactory.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace Usermap
{
    /// <summary>
    /// The factory of <see cref="UsermapHttpClient"/>.
    /// </summary>
    public class UsermapClientFactory
    {
        private readonly ILogger<UsermapHttpClient> _logger;
        private readonly IOptionsSnapshot<UsermapApiOptions> _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsermapClientFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="options">The options.</param>
        public UsermapClientFactory(ILogger<UsermapHttpClient> logger, IOptionsSnapshot<UsermapApiOptions> options)
        {
            _options = options;
            _logger = logger;
            _options = options;
        }

        /// <summary>
        /// Creates instance of <see cref="UsermapHttpClient"/> with the given <paramref name="accessToken"/>.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns>The instantiated client.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the base url of the api is not set in the options.</exception>
        public UsermapHttpClient CreateClient(string accessToken)
        {
            var client = new RestClient(_options.Value.BaseUrl ??
                                        throw new InvalidOperationException("Base url must be set"))
            {
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(accessToken, "Bearer")
            };

            client.UseNewtonsoftJson();

            return new UsermapHttpClient(client, _logger, _options);
        }
    }
}