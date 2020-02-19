using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.IO
{
    public class LineReader : IDisposable
    {
        private static readonly int DEFAULT_BUFFER_SIZE = 16 * 1024;


        private readonly Queue<string> lines;
        private readonly StringBuilder line;
        private readonly byte[] buffer;
        private readonly Stream stream;



        public int BufferSize => this.buffer.Length;

        public Encoding Encoding { get; }



        public LineReader(Stream stream) 
            : this(stream, Encoding.Default, DEFAULT_BUFFER_SIZE) {
        }

        public LineReader(Stream stream, Encoding encoding, int bufferSize) {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
            this.Encoding = encoding ?? Encoding.Default;
            if (bufferSize < 1) {
                throw new ArgumentException(
                    "Positive value required",
                    nameof(bufferSize)
                    );
            }
            this.buffer = new byte[bufferSize];
            this.lines = new Queue<string>();
            this.line = new StringBuilder();
        }


        public async Task<string> ReadLineAsync(CancellationToken token = default) {
            while (true) {
                token.ThrowIfCancellationRequested();
                if (this.lines.Count > 0) {
                    return this.lines.Dequeue();
                }
                var count = await this.stream.ReadAsync(this.buffer, 0, this.buffer.Length, token);
                if (count > 0 ) {
                    var text = this.Encoding.GetString(this.buffer, 0, count);
                    foreach (var c in text) {
                        if (c == '\n') {
                            this.line.TrimEnd('\r');
                            this.lines.Enqueue(line.ToString());
                            this.line.Clear();
                        }
                        else {
                            this.line.Append(c);
                        }
                    }
                }
                else {
                    return this.line.Length > 0
                        ? this.line.ToString()
                        : default;
                }
            }
        }

        public void Dispose() {
            this.stream.Dispose();
        }
    }
}
