using System;
using System.Collections.Generic;
using System.Text;
using Void.Reflection;

namespace Void
{
    public class ErrorInfo : IError
    {
        public Type Type { get; }

        public string Message { get; }

        public string Content { get; }


        public ErrorInfo(Type type, string message, string content) {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.Content = content;
            this.Message = message;
        }

        public ErrorInfo(Exception exception) : this(
                  exception?.GetType() ?? throw new ArgumentNullException(nameof(exception)),
                  exception.Message,
                  exception.ToString()
                  ) {
        }



        public static bool Equals(IError first, IError second) {
            return first != null
                && second != null
                && first.Type == second.Type
                && first.Message == second.Message
                && first.Content == second.Content;
        }

        public bool Equals(IError other) {
            return Equals(this, other);
        }

        public override bool Equals(object obj) {
            return Equals(obj as IError);
        }

        public override int GetHashCode() {
            return HashCode.Create(
                 this.Content,
                this.Message,
                this.Type
                );
        }

        public override string ToString() {
            if (string.IsNullOrWhiteSpace(this.Content)) {
                var text = new StringBuilder();
                text.Append(this.Type);
                if (this.Message?.Length > 0) {
                    text.Append(": ");
                    text.Append(this.Message);
                }
                return text.ToString();
            }
            return this.Content;
        }
    }
}
