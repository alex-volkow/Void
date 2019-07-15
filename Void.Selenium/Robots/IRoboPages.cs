using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboPages
    {
        bool IsMatch(Type Type);
        bool IsMatch<T>() where T : class;

        Task<bool> IsMatchAsync(Type type);
        Task<bool> IsMatchAsync(Type type, CancellationToken token);
        Task<bool> IsMatchAsync(Type type, TimeSpan timeout);
        Task<bool> IsMatchAsync(Type type, TimeSpan timeout, CancellationToken token);
        Task<bool> IsMatchAsync<T>() where T : class;
        Task<bool> IsMatchAsync<T>(CancellationToken token) where T : class;
        Task<bool> IsMatchAsync<T>(TimeSpan timeout) where T : class;
        Task<bool> IsMatchAsync<T>(TimeSpan timeout, CancellationToken token) where T : class;

        Task<IWebPage> FindAsync(Type type);
        Task<IWebPage> FindAsync(Type type, CancellationToken token);
        Task<IWebPage> FindAsync(Type type, TimeSpan timeout);
        Task<IWebPage> FindAsync(Type type, TimeSpan timeout, CancellationToken token);
        Task<IWebPage<T>> FindAsync<T>() where T : class;
        Task<IWebPage<T>> FindAsync<T>(CancellationToken token) where T : class;
        Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout) where T : class;
        Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout, CancellationToken token) where T : class;

        Task<IWebPage> TryFindAsync(Type type);
        Task<IWebPage> TryFindAsync(Type type, CancellationToken token);
        Task<IWebPage> TryFindAsync(Type type, TimeSpan timeout);
        Task<IWebPage> TryFindAsync(Type type, TimeSpan timeout, CancellationToken token);
        Task<IWebPage<T>> TryFindAsync<T>() where T : class;
        Task<IWebPage<T>> TryFindAsync<T>(CancellationToken token) where T : class;
        Task<IWebPage<T>> TryFindAsync<T>(TimeSpan timeout) where T : class;
        Task<IWebPage<T>> TryFindAsync<T>(TimeSpan timeout, CancellationToken token) where T : class;

        Task<IWebPage> FindFistAsync(params Type[] types);
        Task<IWebPage> FindFistAsync(params IWebPage[] pages);
        Task<IWebPage> FindFistAsync(IEnumerable<Type> types);
        Task<IWebPage> FindFistAsync(IEnumerable<Type> types, CancellationToken token);
        Task<IWebPage> FindFistAsync(IEnumerable<IWebPage> pages);
        Task<IWebPage> FindFistAsync(IEnumerable<IWebPage> pages, CancellationToken token);
        Task<IWebPage> FindFistAsync(IEnumerable<Type> types, TimeSpan timeout);
        Task<IWebPage> FindFistAsync(IEnumerable<Type> types, TimeSpan timeout, CancellationToken token);
        Task<IWebPage> FindFistAsync(IEnumerable<IWebPage> pages, TimeSpan timeout);
        Task<IWebPage> FindFistAsync(IEnumerable<IWebPage> pages, TimeSpan timeout, CancellationToken token);

        Task<IWebPage> TryFindFistAsync(params Type[] types);
        Task<IWebPage> TryFindFistAsync(params IWebPage[] pages);
        Task<IWebPage> TryFindFistAsync(IEnumerable<Type> types);
        Task<IWebPage> TryFindFistAsync(IEnumerable<Type> types, CancellationToken token);
        Task<IWebPage> TryFindFistAsync(IEnumerable<IWebPage> pages);
        Task<IWebPage> TryFindFistAsync(IEnumerable<IWebPage> pages, CancellationToken token);
        Task<IWebPage> TryFindFistAsync(IEnumerable<Type> types, TimeSpan timeout);
        Task<IWebPage> TryFindFistAsync(IEnumerable<Type> types, TimeSpan timeout, CancellationToken token);
        Task<IWebPage> TryFindFistAsync(IEnumerable<IWebPage> pages, TimeSpan timeout);
        Task<IWebPage> TryFindFistAsync(IEnumerable<IWebPage> pages, TimeSpan timeout, CancellationToken token);
    }
}
