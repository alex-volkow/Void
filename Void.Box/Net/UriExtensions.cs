using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Net
{
    public static class UriExtensions
    {
        public static bool HasRoot(this Uri uri, Uri other) {
            return uri != null && other != null
                && string.Equals(uri.Scheme, other.Scheme, StringComparison.OrdinalIgnoreCase)
                && string.Equals(uri.Host, other.Host, StringComparison.OrdinalIgnoreCase)
                && uri.Port == other.Port;
        }

        public static UriBuilder Combine(this Uri uri, string path) {
            return uri.Combine(path, null);
        }

        public static UriBuilder Combine(this Uri uri, string path, string query) {
            var address = new UriBuilder(uri);
            if (!string.IsNullOrWhiteSpace(path)) {
                var rootHasSlash = address.ToString().EndsWith("/");
                var pathHasSlash = path.StartsWith("/");
                if (rootHasSlash && pathHasSlash) {
                    address.Path = path.RemoveFirst("/");
                }
                else if (!(rootHasSlash && pathHasSlash)) {
                    address.Path = $"/{path}";
                }
                else {
                    address.Path = path;
                }
            }
            if (!string.IsNullOrWhiteSpace(query)) {
                if (query.StartsWith("?")) {
                    query = query.RemoveFirst("?");
                }
                if (address.Query == "?") {
                    address.Query = null;
                }
                if (string.IsNullOrWhiteSpace(address.Query)) {
                    address.Query = query;
                }
                else {
                    var rootHasAmp = address.Query.EndsWith("&");
                    var queryHasAmp = query.StartsWith("&");
                    if (rootHasAmp && queryHasAmp) {
                        address.Query += query.RemoveFirst("&");
                    }
                    else if (!(rootHasAmp && queryHasAmp)) {
                        address.Query += $"&{query}";
                    }
                    else {
                        address.Query = query;
                    }
                }
            }
            return address;
        }
    }
}
