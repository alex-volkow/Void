using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    class RoboWebPointer : RoboElement
    {
        private readonly IWebPointer pointer;

        public override IWebElement WrappedElement => this.pointer.WrappedElement;


        public RoboWebPointer(IRobot robot, ISearchContext context, IWebPointer pointer) 
            : base(robot, context) {
            this.pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
        }


        public override IWebElement Match() => this.pointer.Match();

        public override IWebElement Required() => this.pointer.Required();
    }
}
