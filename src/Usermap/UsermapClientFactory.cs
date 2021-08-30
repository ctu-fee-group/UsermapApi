using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;

namespace Usermap
{
    public class UsermapClientFactory
    {
        private readonly ILogger<UsermapHttpClient> _logger;
        private readonly IOptionsSnapshot<UsermapApiOptions> _options;

        public UsermapClientFactory(ILogger<UsermapHttpClient> logger, IOptionsSnapshot<UsermapApiOptions> options)
        {
            _options = options;
            _logger = logger;
            _options = options;
        }

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