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

        protected IRobot GetRobot() {
            return new WebRobot(GetDriver());
        }
    }
}
