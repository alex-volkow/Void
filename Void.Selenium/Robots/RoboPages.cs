using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Selenium
{
    class RoboPages : RoboComponent, IRoboPages
    {
        public RoboPages(IRobot robot) 
            : base(robot) {
        }


        #region FindAsync

        public Task<IWebPage> FindAsync(Type type) {
            return FindAsync(type, CancellationToken.None);
        }

        public Task<IWebPage> FindAsync(Type type, CancellationToken token) {
            return FindAsync(type, this.Robot.PageSearchingTimeout, token);
        }

        public Task<IWebPage> FindAsync(Type type, TimeSpan timeout) {
            return FindAsync(type, timeout, CancellationToken.None);
        }

        public Task<IWebPage> FindAsync(Type type, TimeSpan timeout, CancellationToken token) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            var page = (IWebPage)CreateGenericPage(type);
            var wait = this.Robot.Wait()
                .UsingCancellationToken(token);
            if (timeout != this.Robot.PageSearchingTimeout) {
                wait.WithTimeout(timeout);
            }
            return wait.UntilAsync(() => {
                return page.Match().Success ? page : null;
            });
        }

        public Task<IWebPage<T>> FindAsync<T>() where T : class {
            return FindAsync<T>(CancellationToken.None);
        }

        public async Task<IWebPage<T>> FindAsync<T>(CancellationToken token) where T : class {
            var page = await FindAsync(typeof(T), token);
            return (IWebPage<T>)page;
        }

        public Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout) where T : class {
            return FindAsync<T>(timeout, CancellationToken.None);
        }

        public async Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout, CancellationToken token) where T : class {
            var page = await FindAsync(typeof(T), timeout, token);
            return (IWebPage<T>)page;
        }

        #endregion

        #region FindFirstPage

        public Task<IWebPage> FindFistPage(params Type[] types) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(params IWebPage[] pages) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(IEnumerable<Type> types) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(IEnumerable<Type> types, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(IEnumerable<Type> types, CancellationToken token) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages, CancellationToken token) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(IEnumerable<Type> types, TimeSpan timeout, CancellationToken token) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout, CancellationToken token) {
            throw new NotImplementedException();
        }

#endregion

        #region IsMatch

        public bool IsMatch(Type type) {
            return CreateGenericPage(type).Match().Success;
        }

        public bool IsMatch<T>() where T : class {
            return IsMatch(typeof(T));
        }

        public Task<bool> IsMatchAsync(Type type) {
            return IsMatchAsync(type, CancellationToken.None);
        }

        public Task<bool> IsMatchAsync(Type type, CancellationToken token) {
            return IsMatchAsync(type, this.Robot.PageSearchingTimeout, token);
        }

        public Task<bool> IsMatchAsync(Type type, TimeSpan timeout) {
            return IsMatchAsync(type, timeout, CancellationToken.None);
        }

        public Task<bool> IsMatchAsync(Type type, TimeSpan timeout, CancellationToken token) {
            var page = CreateGenericPage(type);
            var wait = this.Robot.Wait()
                .UsingCancellationToken(token)
                .WithTimeout(timeout)
                .UsingExceptionHandler(e => true)
                .IgnoreConditionExceptions()
                .NotThrowTimeoutException();
            return wait.UntilAsync(() => page.Match().Success);
        }

        public Task<bool> IsMatchAsync<T>() where T : class {
            return IsMatchAsync<T>(CancellationToken.None);
        }

        public Task<bool> IsMatchAsync<T>(CancellationToken token) where T : class {
            return IsMatchAsync<T>(this.Robot.PageSearchingTimeout, token);
        }

        public Task<bool> IsMatchAsync<T>(TimeSpan timeout) where T : class {
            return IsMatchAsync<T>(timeout, CancellationToken.None);
        }

        public Task<bool> IsMatchAsync<T>(TimeSpan timeout, CancellationToken token) where T : class {
            return IsMatchAsync(typeof(T), timeout, token);
        }

        #endregion

        #region TryFind

        public Task<IWebPage> TryFindAsync(Type type) {
            return TryFindAsync(type, this.Robot.PageSearchingTimeout);
        }

        public Task<IWebPage> TryFindAsync(Type type, TimeSpan timeout) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            var page = (IWebPage)CreateGenericPage(type);
            var wait = this.Robot.Wait()
                .UsingExceptionHandler(e => true)
                .IgnoreConditionExceptions()
                .NotThrowTimeoutException();
            if (timeout != this.Robot.PageSearchingTimeout) {
                wait.WithTimeout(timeout);
            }
            return wait.UntilAsync(() => {
                return page.Match().Success ? page : null;
            });
        }

        public Task<IWebPage<T>> TryFindAsync<T>() where T : class {
            return TryFindAsync<T>(this.Robot.PageSearchingTimeout);
        }

        public async Task<IWebPage<T>> TryFindAsync<T>(TimeSpan timeout) where T : class {
            var page = await TryFindAsync(typeof(T), timeout);
            return (IWebPage<T>)page;
        }

        public Task<IWebPage> TryFindAsync(Type type, CancellationToken token) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindAsync(Type type, TimeSpan timeout, CancellationToken token) {
            throw new NotImplementedException();
        }

        public Task<IWebPage<T>> TryFindAsync<T>(CancellationToken token) where T : class {
            throw new NotImplementedException();
        }

        public Task<IWebPage<T>> TryFindAsync<T>(TimeSpan timeout, CancellationToken token) where T : class {
            throw new NotImplementedException();
        }

        #endregion

        #region TryFindFirst

        public Task<IWebPage> TryFindFistPage(params Type[] types) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(params IWebPage[] pages) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(IEnumerable<Type> types) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(IEnumerable<Type> types, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(IEnumerable<Type> types, CancellationToken token) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages, CancellationToken token) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(IEnumerable<Type> types, TimeSpan timeout, CancellationToken token) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindFistPage(IEnumerable<IWebPage> pages, TimeSpan timeout, CancellationToken token) {
            throw new NotImplementedException();
        }

        #endregion

        private WebPage CreateGenericPage(Type type) {
            var page = typeof(WebPage<>).MakeGenericType(type);
            return (WebPage)Activator.CreateInstance(page, this.WrappedDriver);
        }
    }
}
