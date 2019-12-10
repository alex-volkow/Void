using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Void.IO;

namespace Void.Data
{
    /// <summary>
    /// Represents SQLite connectin string.
    /// </summary>
    public class SQLiteConnectionString
    {
        private static readonly Regex DATA_SOURCE_PARSER = new Regex($@"^Data Source\s*=\s*(?<PATH>[^;]+)");
        private static readonly string DATA_SOURCE_MEMORY = ":memory:";


        /// <summary>
        /// Path or name of DataSource parameter
        /// </summary>
        public string DataSource { get; }

        /// <summary>
        /// Indicates the database in memory.
        /// </summary>
        public bool InMemory => this.DataSource.Equals(
            DATA_SOURCE_MEMORY,
            StringComparison.OrdinalIgnoreCase
            );


        /// <summary>
        /// Create new connection string with in-memory data source.
        /// </summary>
        public SQLiteConnectionString()
            : this("Data Source=:memory:") {
        }

        /// <summary>
        /// Create new connection string with custom data source.
        /// </summary>
        /// <exception cref="ArgumentException">Argument must not be empty or null.</exception>
        public SQLiteConnectionString(string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(
                    "Value must not be empty or null"
                    );
            }
            this.DataSource = value.ToLower().Contains("data source")
                ? ExtractDataSource(value)
                : ToAbsolutePath(value);
        }

        /// <summary>
        /// Create data source directory if it does not exist.
        /// </summary>
        public DirectoryInfo InitializeDirectory() {
            if (this.InMemory) {
                return null;
            }
            var path = Path.GetDirectoryName(this.DataSource);
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) {
                directory.Create();
            }
            return directory;
        }

        public override bool Equals(object obj) {
            if (!(obj is SQLiteConnectionString other) || obj == null) {
                return false;
            }
            if (other.InMemory == this.InMemory) {
                return true;
            }
            return this.DataSource.Equals(other.DataSource);
        }

        public override int GetHashCode() {
            return this.DataSource.GetHashCode();
        }

        public override string ToString() {
            return $"Data Source={this.DataSource}";
        }

        private string ToAbsolutePath(string value) {
            if (!value.Equals(DATA_SOURCE_MEMORY, StringComparison.OrdinalIgnoreCase) &&
                !Path.IsPathRooted(value)) {
                value = Path.Combine(Files.EntryPoint.DirectoryName, value);
            }
            return value;
        }

        private string ExtractDataSource(string value) {
            var parameters = value.Split(';')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToArray();
            for (var i = 0; i < parameters.Length; i++) {
                var match = DATA_SOURCE_PARSER.Match(parameters[i]);
                if (match.Success) {
                    var path = match.Groups["PATH"].Value;
                    return ToAbsolutePath(path);
                }
            }
            throw new ArgumentException(
                $"Invalid data source path"
                );
        }
    }
}
