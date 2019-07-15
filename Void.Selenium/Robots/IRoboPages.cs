using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboPages
    {
        bool IsMatch(Type Type);
        bool IsMatch<T>() where T : class;

        Task<bool> IsMatchAsync(Type type);
        Task<bool> IsMatchAsync(Type type, TimeSpan timeout);
        Task<bool> IsMatchAsync<T>() where T : class;
        Task<bool> IsMatchAsync<T>(TimeSpan timeout) where T : class;

        Task<IWebPage> FindAsync(Type type);
        Task<IWebPage> FindAsync(Type type, TimeSpan timeout);
        Task<IWebPage<T>> FindAsync<T>() where T : class;
        Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout) where T : class;

        Task<IWebPage> TryFindAsync(Type type);
        Task<IWebPage> TryFindAsync(Type type, TimeSpan timeout);
        Task<IWebPage<T>> TryFindAsync<T>() where T : class;
        Task<IWebPage<T>> TryFindAsync<T>(TimeSpan timeout) where T : class;

        Task<IWebPage> FindFistPage(params Type[] types);
        Task<IWebPage> FindFistPage(params IWebPage[] pages);
        Task<IWebPage> FindFistPage(IEnumerable<Type> types);
        Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages);
        Task<IWebPage> FindFistPage(IEnumerable<Type> types, TimeSpan timeout);
        Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout);

        Task<IWebPage> TryFindFistPage(params Type[] types);
        Task<IWebPage> TryFindFistPage(params IWebPage[] pages);
        Task<IWebPage> TryFindFistPage(IEnumerable<Type> types);
        Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages);
        Task<IWebPage> TryFindFistPage(IEnumerable<Type> types, TimeSpan timeout);
        Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout);
    }
}
