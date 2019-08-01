using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.IO;

namespace Void.Net
{
    class Program
    {
        public static void Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
                Terminal.WriteLine("!!! FATAL EXCEPTION !!!");
                Terminal.WriteLine(e.ExceptionObject, ConsoleColor.Red);
                Console.ReadLine();
            };
            try {
                Console.OutputEncoding = Encoding.UTF8;
                MainAsync(args)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex) {
                Terminal.WriteLine();
                Terminal.WriteLine(ex, ConsoleColor.Red);
            }
            Terminal.WriteLine("Press <Enter> to exit ...");
            Console.ReadLine();
        }

        public static async Task MainAsync(string[] args) {
            using (var client = new WindowsAdapter(new SshClient("5.199.163.29", "Profiler2", "EpSL43&#J"))) {
                client.Client.Connect();
                //Terminal.WriteLine(await client.ExecuteAsync(@"cd Automation.Node & for /R %F in (*) do @certutil -hashfile ""%F"" SHA512"));
                var hashes = await client.GetSha512RecursiveAsync(@"C:\Users\Profiler2\Automation.Node");
                foreach (var hash in hashes.OrderBy(e => e.Key)) {
                    Terminal.WriteLine($"{hash.Key}\r\n{hash.Value}\r\n");
                }
            }
        }
    }
}
