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
    }
}
