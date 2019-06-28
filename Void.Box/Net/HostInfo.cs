using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Void.Net
{
    public static class HostInfo
    {
        public static IPAddress ParseAddress(string value) {
            switch (value?.ToLower()?.Trim()) {
                case "localhost": return IPAddress.Loopback;
                case "*":
                case "0.0.0.0": return IPAddress.Any;
                default: return IPAddress.Parse(value);
            }
        }

        public static IPEndPoint ParseEndpoint(string value) {
            var parts = value?.Trim()?.Split(':') ?? 
                throw new ArgumentNullException(nameof(value)); ;
            if (parts.Length != 2) {
                throw new FormatException();
            }
            return new IPEndPoint(
                ParseAddress(parts[0]), 
                int.Parse(parts[1])
                );
        }
    }
}
