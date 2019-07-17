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
            using (var client = new WindowsSshClient("5.199.163.29", "Profiler2", "EpSL43&#J")) {
                Terminal.Write("Connecting...");
                client.Connect();
                Terminal.WriteLine("\tOK");
                //Terminal.WriteLine($"Is admin: {client.IsAdmin}");
                //Terminal.WriteLine($"User folder: {client.UserFolder}");
                //Terminal.WriteLine(await client.ExecuteAsync("ping google.com -n 1"));
                ////await client.OpenInTcpPortAsync(7000);
                ////Terminal.WriteLine($"Port 7000 has been opened");
                //var file = client.UserFolder + "Desktop" + "tmp" + "text.txt";
                //await client.WriteAsync(file, "pew");
                //if (client.Exists(file.Parent)) {
                //    if (client.Delete(file.Parent)) {
                //        Terminal.WriteLine($"Removed: {file}");
                //    }
                //    else {
                //        Terminal.WriteLine($"Not found: {file}");
                //    }
                //}
                //await client.WriteAsync(file, "pew pew");
                //await client.WriteAsync(file, "pew pew pew");
                //Terminal.Write("Content: ");
                //Terminal.WriteLine(await client.ReadTextAsync(file));
                //Terminal.Write("SHA256: ");
                //Terminal.WriteLine(await client.GetSha256Async(file));
                //Terminal.Write("SHA512: ");
                //Terminal.WriteLine(await client.GetSha512Async(file));
                //if (client.Delete(file.Parent)) {
                //    Terminal.WriteLine($"Removed: {file}");
                //}
                //else {
                //    Terminal.WriteLine($"Not found: {file}");
                //}
                //if (client.Delete(file.Parent)) {
                //    Terminal.WriteLine($"Removed: {file}");
                //}
                //else {
                //    Terminal.WriteLine($"Not found: {file}");
                //}
                var start = DateTime.Now;
                foreach (var item in client.GetFilesRecursive(client.UserFolder + "Desktop").OrderBy(e => e)) {
                    Terminal.WriteLine(item);
                }
                Terminal.WriteLine((int)(DateTime.Now - start).TotalSeconds);
                //for (var i = 0; i < 4; i++) {
                //    var local = new FilePath($@"C:\Users\Sandbox\Desktop\{i + 1}.txt");
                //    var remote = client.UserFolder + "Desktop" + $"{i + 1}.txt";
                //    Terminal.Write($"File: {i + 1}.txt\t");
                //    if (await client.IsDifferent(local, remote)) {
                //        Terminal.WriteLine("different");
                //        if (File.Exists(local)) {
                //            Terminal.WriteLine("Sync remote file");
                //            await client.WriteAsync(remote, File.ReadAllText(local));
                //        }
                //    }
                //    else {
                //        Terminal.WriteLine("equal");
                //    }
                //}
            }
        }
    }
}
