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
            throw new NotImplementedException();
        }

        public Task<IWebPage> FindAsync(Type type, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<IWebPage<T>> FindAsync<T>() where T : class {
            throw new NotImplementedException();
        }

        public Task<IWebPage<T>> FindAsync<T>(TimeSpan timeout) where T : class {
            throw new NotImplementedException();
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

        public bool IsMatch(Type Type) {
            throw new NotImplementedException();
        }

        public bool IsMatch<T>() where T : class {
            throw new NotImplementedException();
        }

        public Task<bool> IsMatchAsync(Type Type) {
            throw new NotImplementedException();
        }

        public Task<bool> IsMatchAsync(Type Type, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<bool> IsMatchAsync<T>() where T : class {
            throw new NotImplementedException();
        }

        public Task<bool> IsMatchAsync<T>(TimeSpan timeout) where T : class {
            throw new NotImplementedException();
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
    }
}
