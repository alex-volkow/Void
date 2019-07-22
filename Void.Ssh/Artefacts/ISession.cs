using Renci.SshNet;
using System;
using System.Threading.Tasks;

namespace Void.Net
{
    public interface ISession<T> : IDisposable where T : BaseClient
    {
        T Client { get; }
        bool IsActive { get; }

        Task GetAwaiter();
    }
}