using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboPages
    {
        /// <summary>
        /// Check current driver content is matching to the page type.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        bool IsMatch(Type Type);

        /// <summary>
        /// Check current driver content is matching to the page type.
        /// </summary>
        bool IsMatch<T>() where T : class;




        /// <summary>
        /// Check asynchronously current driver content is matching to the page type 
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        Task<bool> IsMatchAsync(Type type);

        /// <summary>
        /// Check asynchronously current driver content is matching to the page type
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        Task<bool> IsMatchAsync(Type type, CancellationToken token);

        /// <summary>
        /// Check asynchronously current driver content is matching to the page type
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        Task<bool> IsMatchAsync(Type type, TimeSpan timeout);

        /// <summary>
        /// Check asynchronously current driver content is matching to the page type
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        Task<bool> IsMatchAsync(Type type, TimeSpan timeout, CancellationToken token);

        /// <summary>
        /// Check asynchronously current driver content is matching to the page type
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        Task<bool> IsMatchAsync<T>() where T : class;

        /// <summary>
        /// Check asynchronously current driver content is matching to the page type
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        Task<bool> IsMatchAsync<T>(CancellationToken token) where T : class;

        /// <summary>
        /// Check asynchronously current driver content is matching to the page type
        /// with the robot's condition checking interval.
        /// </summary>
        Task<bool> IsMatchAsync<T>(TimeSpan timeout) where T : class;

        /// <summary>
        /// Check asynchronously current driver content is matching to the page type
        /// with the robot's condition checking interval.
        /// </summary>
        Task<bool> IsMatchAsync<T>(TimeSpan timeout, CancellationToken token) where T : class;




        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        /// <exception cref="NotFoundException">Page is not found.</exception>
        Task<IWebPage> FindAsync(Type type);

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        /// <exception cref="NotFoundException">Page is not found.</exception>
        Task<IWebPage> FindAsync(Type type, CancellationToken token);

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        /// <exception cref="NotFoundException">Page is not found.</exception>
        Task<IWebPage> FindAsync(Type type, TimeSpan timeout);

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        /// <exception cref="NotFoundException">Page is not found.</exception>
        Task<IWebPage> FindAsync(Type type, TimeSpan timeout, CancellationToken token);

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">Page is not found.</exception>
        Task<IWebPage<T>> FindAsync<T>() where T : class;

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">Page is not found.</exception>
        Task<IWebPage<T>> FindAsync<T>(CancellationToken token) where T : class;

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">Page is not found.</exception>
        Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout) where T : class;

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">Page is not found.</exception>
        Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout, CancellationToken token) where T : class;




        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <returns>Page if found else null.</returns>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        Task<IWebPage> TryFindAsync(Type type);

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <returns>Page if found else null.</returns>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        Task<IWebPage> TryFindAsync(Type type, CancellationToken token);

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <returns>Page if found else null.</returns>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        Task<IWebPage> TryFindAsync(Type type, TimeSpan timeout);

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <returns>Page if found else null.</returns>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        Task<IWebPage> TryFindAsync(Type type, TimeSpan timeout, CancellationToken token);

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <returns>Page if found else null.</returns>
        Task<IWebPage<T>> TryFindAsync<T>() where T : class;

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <returns>Page if found else null.</returns>
        Task<IWebPage<T>> TryFindAsync<T>(CancellationToken token) where T : class;

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <returns>Page if found else null.</returns>
        Task<IWebPage<T>> TryFindAsync<T>(TimeSpan timeout) where T : class;

        /// <summary>
        /// Match asynchronously the page by type with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <returns>Page if found else null.</returns>
        Task<IWebPage<T>> TryFindAsync<T>(TimeSpan timeout, CancellationToken token) where T : class;




        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> FindFistAsync(params Type[] types);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> FindFistAsync(params IWebPage[] pages);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> FindFistAsync(IEnumerable<Type> types);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> FindFistAsync(IEnumerable<Type> types, CancellationToken token);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> FindFistAsync(IEnumerable<IWebPage> pages);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> FindFistAsync(IEnumerable<IWebPage> pages, CancellationToken token);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> FindFistAsync(IEnumerable<Type> types, TimeSpan timeout);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> FindFistAsync(IEnumerable<Type> types, TimeSpan timeout, CancellationToken token);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> FindFistAsync(IEnumerable<IWebPage> pages, TimeSpan timeout);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="NotFoundException">No page found.</exception>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> FindFistAsync(IEnumerable<IWebPage> pages, TimeSpan timeout, CancellationToken token);




        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> TryFindFistAsync(params Type[] types);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> TryFindFistAsync(params IWebPage[] pages);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> TryFindFistAsync(IEnumerable<Type> types);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> TryFindFistAsync(IEnumerable<Type> types, CancellationToken token);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> TryFindFistAsync(IEnumerable<IWebPage> pages);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's page searching timeout and condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> TryFindFistAsync(IEnumerable<IWebPage> pages, CancellationToken token);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> TryFindFistAsync(IEnumerable<Type> types, TimeSpan timeout);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Types are null.</exception>
        Task<IWebPage> TryFindFistAsync(IEnumerable<Type> types, TimeSpan timeout, CancellationToken token);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> TryFindFistAsync(IEnumerable<IWebPage> pages, TimeSpan timeout);

        /// <summary>
        /// Match asynchronously a first found page from types with current driver content
        /// with the robot's condition checking interval.
        /// </summary>
        /// <exception cref="ArgumentNullException">Pages are null.</exception>
        Task<IWebPage> TryFindFistAsync(IEnumerable<IWebPage> pages, TimeSpan timeout, CancellationToken token);
    }
}
