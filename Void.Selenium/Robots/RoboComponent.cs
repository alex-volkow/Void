using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    class RoboComponent : IWrapsDriver
    {
        public IRobot Robot { get; }

        public IWebDriver WrappedDriver => this.Robot.WrappedDriver;


        public RoboComponent(IRobot robot) {
            this.Robot = robot ?? throw new ArgumentNullException(nameof(robot));
        }
    }
}
