using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium
{
    class RoboPages : RoboComponent, IRoboPages
    {
        public RoboPages(IRobot robot) 
            : base(robot) {
        }



        public Task<IWebPage> FindAsync(Type type) {
            return FindAsync(type, this.Robot.PageSearchingTimeout);
        }

        public Task<IWebPage> FindAsync(Type type, TimeSpan timeout) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            var page = (IWebPage)CreateGenericPage(type);
            var wait = this.Robot.Wait();
            if (timeout != this.Robot.PageSearchingTimeout) {
                wait.WithTimeout(timeout);
            }
            return wait.UntilAsync(() => {
                return page.Match().Success ? page : null;
            });
        }

        public async Task<IWebPage<T>> FindAsync<T>() where T : class {
            var page = await FindAsync(typeof(T));
            return (IWebPage<T>)page;
        }

        public async Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout) where T : class {
            var page = await FindAsync(typeof(T), timeout);
            return (IWebPage<T>)page;
        }

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

        public bool IsMatch(Type type) {
            return CreateGenericPage(type).Match().Success;
        }

        public bool IsMatch<T>() where T : class {
            return IsMatch(typeof(T));
        }

        public Task<bool> IsMatchAsync(Type type) {
            return IsMatchAsync(type, this.Robot.PageSearchingTimeout);
        }

        public Task<bool> IsMatchAsync(Type type, TimeSpan timeout) {
            var page = CreateGenericPage(type);
            var wait = this.Robot.Wait();
            if (timeout != this.Robot.PageSearchingTimeout) {
                wait.WithTimeout(timeout);
            }
            return wait.UntilAsync(() => page.Match().Success);
        }

        public Task<bool> IsMatchAsync<T>() where T : class {
            return IsMatchAsync<T>(this.Robot.PageSearchingTimeout);
        }

        public Task<bool> IsMatchAsync<T>(TimeSpan timeout) where T : class {
            return IsMatchAsync(typeof(T), timeout);
        }

        public Task<IWebPage> TryFindAsync(Type type) {
            throw new NotImplementedException();
        }

        public Task<IWebPage> TryFindAsync(Type type, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<IWebPage<T>> TryFindAsync<T>() where T : class {
            throw new NotImplementedException();
        }

        public Task<IWebPage<T>> TryFindAsync<T>(TimeSpan timeout) where T : class {
            throw new NotImplementedException();
        }

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

        private WebPage CreateGenericPage(Type type) {
            var page = typeof(WebPage<>).MakeGenericType(type);
            return (WebPage)Activator.CreateInstance(page, this.WrappedDriver);
        }
    }
}
