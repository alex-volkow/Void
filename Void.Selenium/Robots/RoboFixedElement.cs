using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    class RoboFixedElement : RoboElement
    {
        public RoboFixedElement(IRobot robot, IWebElement element) 
            : base(robot) {
        }
    }
}
