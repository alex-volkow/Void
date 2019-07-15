using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Void.Selenium.Tests
{
    public class WebRobotTests : WebContext
    {
        [Fact]
        public async Task FindPageAsync() {
            var page = GetRobot().Pages.FindAsync<TemplatePage>();
            OpenDefaultPage();
            await page;
        }

        [Fact]
        public async Task FailFindPageAsync() {
            await Assert.ThrowsAnyAsync<Exception>(() => {
                return GetRobot().Pages.FindAsync<TemplatePage>();
            });
        }

        [Fact]
        public void IsMatch() {
            OpenDefaultPage();
            Assert.True(GetRobot().Pages.IsMatch<TemplatePage>());
        }

        [Fact]
        public void IsNotMatch() {
            Assert.False(GetRobot().Pages.IsMatch<TemplatePage>());
        }

        [Fact]
        public async Task IsMatchAsync() {
            OpenDefaultPage();
            Assert.True(await GetRobot().Pages.IsMatchAsync<TemplatePage>());
        }

        [Fact]
        public async Task IsNotMatchAsync() {
            Assert.False(await GetRobot().Pages.IsMatchAsync<TemplatePage>());
        }

        [Fact]
        public async Task TryFindPageAsync() {
            var page = await GetRobot().Pages.TryFindAsync<TemplatePage>();
            Assert.Null(page);
            OpenDefaultPage();
            page = await GetRobot().Pages.TryFindAsync<TemplatePage>();
            Assert.NotNull(page);
        }

        protected IRobot GetRobot() {
            return new WebRobot(GetDriver()) {
                PageSearchingTimeout = TimeSpan.FromSeconds(1.5),
                ElementSearchingTimeout = TimeSpan.FromSeconds(1.5),
                ConditionCheckingInterval = TimeSpan.FromSeconds(0.4)
            };
        }
    }
}
