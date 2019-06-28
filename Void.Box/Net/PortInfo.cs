using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Void.Net
{
    public static class PortInfo
    {
        public static int MaxValue { get; } = 65535;


        public static bool IsOpen(int port) {
            return IsOpen("localhost", port);
        }

        public static bool IsOpen(string host, int port) {
            using (var client = new TcpClient()) {
                try {
                    client.Connect(host, port);
                    return true;
                }
                catch {
                    return false;
                }
            }
        }

        public static int NextTcp() {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public static IReadOnlyList<int> GetActive() {
            var ports = new List<int>();
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            ports.AddRange(properties.GetActiveTcpConnections().Select(e => e.LocalEndPoint.Port));
            ports.AddRange(properties.GetActiveTcpListeners().Select(e => e.Port));
            ports.AddRange(properties.GetActiveUdpListeners().Select(e => e.Port));
            return new HashSet<int>(ports)
                .OrderBy(e => e)
                .ToArray();
        }

        public static IReadOnlyList<int> GetFree() {
            return GetFreeLazy().ToArray();
        }

        private static IEnumerable<int> GetFreeLazy() {
            var ports = GetActive();
            for (var i = 1; i < short.MaxValue; i++) {
                if (!ports.Contains(i)) {
                    yield return i;
                }
            }
        }
    }
}
