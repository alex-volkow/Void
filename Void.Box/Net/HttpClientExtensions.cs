using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Net
{
    public static class HttpClientExtensions
    {
        public static AuthenticationHeaderValue SetBasicAuth(
            this HttpClient client,
            string username,
            string password
            ) {
            var secret = $"{username}:{password}";
            var value = Strings.ToBase64(secret, Encoding.ASCII);
            var header = new AuthenticationHeaderValue("Basic", value);
            client.DefaultRequestHeaders.Authorization = header;
            return header;
        }

        public static NetworkCredential GetBasicAuth(this HttpClient client) {
            if (client.DefaultRequestHeaders?.Authorization?.Scheme == "Basic") {
                var secret = Strings.FromBase64(client.DefaultRequestHeaders.Authorization.Parameter);
                var parameters = secret.Split(':');
                return new NetworkCredential(parameters[0], parameters[1]);
            }
            return null;
        }

        public static Task<HttpResponseMessage> GetRelative(this HttpClient client, string uri, CancellationToken token) {
            return client.BaseAddress != null
                && !string.IsNullOrEmpty(client.BaseAddress.PathAndQuery)
                && client.BaseAddress.PathAndQuery != "/"
                ? client.GetAsync(client.CreateRelative(uri).Uri, token)
                : client.GetAsync(uri, token);
        }

        public static Task<HttpResponseMessage> PostRelative(this HttpClient client, string uri, HttpContent content,
            CancellationToken token) {
            return client.BaseAddress != null
                && !string.IsNullOrEmpty(client.BaseAddress.PathAndQuery)
                && client.BaseAddress.PathAndQuery != "/"
                ? client.PostAsync(client.CreateRelative(uri).Uri, content, token)
                : client.PostAsync(uri, content, token);
        }

        private static UriBuilder CreateRelative(this HttpClient client, string uri) {
            var builder = new UriBuilder(client.BaseAddress);
            var parameters = uri.Split('?');
            if (!string.IsNullOrEmpty(parameters[0])) {
                builder.AppendPath(parameters[0]);
            }
            if (parameters.Length == 2 && !string.IsNullOrEmpty(parameters[1])) {
                builder.AppendQuery(parameters[1]);
            }
            return builder;
        }
    }
}
