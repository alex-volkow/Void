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

        Task<IWebPage> FindFistPage(params Type[] types);
        Task<IWebPage> FindFistPage(params IWebPage[] pages);
        Task<IWebPage> FindFistPage(IEnumerable<Type> types);
        Task<IWebPage> FindFistPage(IEnumerable<Type> types, CancellationToken token);
        Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages);
        Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages, CancellationToken token);
        Task<IWebPage> FindFistPage(IEnumerable<Type> types, TimeSpan timeout);
        Task<IWebPage> FindFistPage(IEnumerable<Type> types, TimeSpan timeout, CancellationToken token);
        Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout);
        Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout, CancellationToken token);

        Task<IWebPage> TryFindFistPage(params Type[] types);
        Task<IWebPage> TryFindFistPage(params IWebPage[] pages);
        Task<IWebPage> TryFindFistPage(IEnumerable<Type> types);
        Task<IWebPage> TryFindFistPage(IEnumerable<Type> types, CancellationToken token);
        Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages);
        Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages, CancellationToken token);
        Task<IWebPage> TryFindFistPage(IEnumerable<Type> types, TimeSpan timeout);
        Task<IWebPage> TryFindFistPage(IEnumerable<Type> types, TimeSpan timeout, CancellationToken token);
        Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout);
        Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout, CancellationToken token);
    }
}
