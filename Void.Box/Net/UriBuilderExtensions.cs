using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Net
{
    public static class UriBuilderExtensions
    {
        public static void AppendPath(this UriBuilder builder, string path) {
            if ((builder.Path?.EndsWith("/") ?? false) && path.StartsWith("/")) {
                path = path.RemoveFirst("/");
            }
            builder.Path += path;
        }

        public static void SetQuery(this UriBuilder builder, string query) {
            if (query?.StartsWith("?") ?? false) {
                query = query.RemoveFirst("?");
            }
            builder.Query = query;
        }

        public static void AppendQuery(this UriBuilder builder, string query) {
            if (query?.StartsWith("?") ?? false) {
                query = query.RemoveFirst("?");
            }
            var baseStartWith = builder.Query?.EndsWith("&") ?? false;
            var queryStartWith = query?.StartsWith("&") ?? false;
            if (baseStartWith && queryStartWith) {
                query = query.RemoveFirst("&");
            }
            builder.Query += query;
        }
    }
}
