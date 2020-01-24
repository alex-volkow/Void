using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Void
{
    public static class Terminal
    {
        private static readonly object locker;


        static Terminal() {
            locker = new object();
        }



        //public static IEnumerable<Match> SelectMatches(string pattern) {
        //    return Regex.Matches(Console.ReadLine(), pattern).Cast<Match>();
        //}

        //public static IEnumerable<T> SelectOptions<T>(IEnumerable<T> options) {
        //    return SelectOptions(options, e => e?.ToString());
        //}

        //public static IEnumerable<T> SelectOptions<T>(IEnumerable<T> options, Func<T, string> format) {
        //    var items = options?.ToArray() ?? new T[] { };
        //    if (format == null) {
        //        format = e => e?.ToString();
        //    }
        //    for (var i = 0; i < items.Length; i++) {
        //        Console.WriteLine($"{i + 1}. {format(items[i])}");
        //    }
        //    while (true) {
        //        Console.Write(" > ");
        //        var indexes = Regex.Split(Console.ReadLine(), @"\s");
        //        if (indexes.Length == 0) {
        //            return new T[] { };
        //        }
        //        if (!indexes.All(e => Regex.IsMatch(e, @"^\d+$"))) {
        //            Console.WriteLine("Only integer numbers available");
        //            continue;
        //        }
        //        return indexes
        //            .Select(e => items[int.Parse(e) - 1])
        //            .ToArray();
        //    }
        //}

        public static IConsoleSpinner CreateSpinner(bool clockwise = true, TimeSpan interval = default) {
            return new ConsoleSpinner { 
                Interval = interval == default ? TimeSpan.FromMilliseconds(250) : interval,
                Clockwise = clockwise
            };
        }

        public static string ReadPassword() {
            return ReadPassword(null);
        }

        public static string ReadPassword(string mask) {
            var password = string.Empty;
            do {
                var key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter) {
                    password += key.KeyChar;
                    Console.Write(mask);
                }
                else {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0) {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter) {
                        return password;
                    }
                }
            } while (true);
        }

        public static bool ReadYesNo(string title) {
            while (true) {
                Terminal.Write(title);
                switch (Console.ReadLine().Trim().ToLower()) {
                    case "y": return true;
                    case "n": return false;
                    default: continue;
                }
            }
        }

        public static string ReadPassword(char mask = '*') {
            var value = "";
            do {
                var key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter) {
                    value += key.KeyChar;
                    Console.Write(mask);
                }
                else {
                    if (key.Key == ConsoleKey.Backspace && value.Length > 0) {
                        value = value.Substring(0, (value.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter) {
                        return value;
                    }
                }
            } while (true);
        }

        public static T Read<T>(string title, Func<string, T> value) {
            return Read(title, value, ex => ex.ToString());
        }

        public static T Read<T>(string title, Func<string, T> value, Func<Exception, string> error) {
            var line = default(string);
            //var separator = title != null ? " " : string.Empty;
            while (true) {
                Console.Write(title);
                line = Console.ReadLine();
                try {
                    return value(line);
                }
                catch (Exception ex) {
                    var message = error(ex);
                    if (!string.IsNullOrWhiteSpace(message)) {
                        Console.WriteLine(message);
                    }
                }
            }
        }

        public static void WriteLine() {
            lock (locker) {
                Console.WriteLine();
            }
        }

        public static void WriteLine(object value) {
            lock (locker) {
                Console.WriteLine(value);
            }
        }

        public static void Write(object value) {
            lock (locker) {
                Console.Write(value);
            }
        }

        public static void WriteLine(object value, ConsoleColor? foreground) {
            WriteLine(value, foreground, default(ConsoleColor?));
        }

        public static void WriteLine(object value, ConsoleColor? foreground, ConsoleColor? background) {
            lock (locker) {
                if (background != null) {
                    Console.BackgroundColor = background.Value;
                }
                if (foreground != null) {
                    Console.ForegroundColor = foreground.Value;
                }
                Console.WriteLine(value);
                Console.ResetColor();
            }
        }

        public static void Write(object value, ConsoleColor? foreground) {
            Write(value, foreground, default(ConsoleColor?));
        }

        public static void Write(object value, ConsoleColor? foreground, ConsoleColor? background) {
            lock (locker) {
                if (background != null) {
                    Console.BackgroundColor = background.Value;
                }
                if (foreground != null) {
                    Console.ForegroundColor = foreground.Value;
                }
                Console.Write(value);
                Console.ResetColor();
            }
        }
    }
}
