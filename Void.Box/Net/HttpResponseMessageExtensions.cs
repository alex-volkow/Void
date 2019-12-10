using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Void.Net
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task EnsureStatusCodeAsync(this HttpResponseMessage response, params HttpStatusCode[] codes) {
            if (codes == null || codes.Length == 0) {
                codes = new HttpStatusCode[] {
                    HttpStatusCode.OK,
                    HttpStatusCode.NoContent
                };
            }
            if (!codes.Contains(response.StatusCode)) {
                var content = response.Content?.Headers?.ContentLength > 0
                    ? await response.Content.ReadAsStringAsync()
                    : default;
                var message = new StringBuilder();
                message.Append($"Received code ");
                message.Append((int)response.StatusCode);
                message.Append(" - ");
                message.Append(response.StatusCode);
                message.Append(". Avaialble codes:");
                var first = true;
                foreach (var code in codes) {
                    if (first) {
                        first = false;
                    }
                    else {
                        message.Append(",");
                    }
                    message.Append(" ");
                    message.Append((int)code);
                    message.Append(" - ");
                    message.Append(code);
                }
                message.Append(".\r\n");
                message.Append(response.RequestMessage.RequestUri);
                message.Append("\r\n");
                if (string.IsNullOrEmpty(content)) {
                    message.Append("No content.");
                }
                else {
                    message.Append("Content: ");
                    message.Append(content);
                }
                throw new HttpRequestException(message.ToString());
            }
        }
    }
}
