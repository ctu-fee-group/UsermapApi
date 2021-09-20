//
//   HttpRequestMessageBuilder.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;

namespace Usermap
{
    /// <summary>
    /// Builder for <see cref="HttpRequestMessage"/>.
    /// </summary>
    public class HttpRequestMessageBuilder
    {
        private readonly NameValueCollection _query;
        private string _path;
        private HttpMethod _method;
        private string? _acceptContent;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestMessageBuilder"/> class.
        /// </summary>
        /// <param name="method">The http method.</param>
        /// <param name="path">The base path.</param>
        public HttpRequestMessageBuilder(HttpMethod method, string path)
        {
            _method = method;
            _path = path;
            _query = HttpUtility.ParseQueryString(string.Empty);
        }

        /// <summary>
        /// Sets the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The builder.</returns>
        public HttpRequestMessageBuilder WithPath(string path)
        {
            _path = path;
            return this;
        }

        /// <summary>
        /// Sets the given http method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The builder.</returns>
        public HttpRequestMessageBuilder WithMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }

        /// <summary>
        /// Adds the given item to the query parameters.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The builder.</returns>
        public HttpRequestMessageBuilder AddQuery(string name, string value)
        {
            _query.Add(name, value);
            return this;
        }

        /// <summary>
        /// Sets the Accept header to the specified value.
        /// </summary>
        /// <param name="accept">The content-type that should be accepted.</param>
        /// <returns>The builder.</returns>
        public HttpRequestMessageBuilder WithContentAccept(string accept)
        {
            _acceptContent = accept;
            return this;
        }

        /// <summary>
        /// Builds the request message.
        /// </summary>
        /// <returns>The built message.</returns>
        public HttpRequestMessage Build()
        {
            string urlPath = _path;
            if (_query.Count > 0)
            {
                urlPath += "?" + _query;
            }

            var message = new HttpRequestMessage(_method, urlPath);
            if (_acceptContent is not null)
            {
                message.Headers.Add("Accept", _acceptContent);
            }

            return message;
        }
    }
}