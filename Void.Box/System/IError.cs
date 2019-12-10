using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    /// <summary>
    /// Represents aggregate exception information.
    /// </summary>
    public interface IError : IEquatable<IError>
    {
        /// <summary>
        /// Top level exception type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Top level exception message.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Aggregated information about all exception.
        /// </summary>
        string Content { get; }
    }
}
