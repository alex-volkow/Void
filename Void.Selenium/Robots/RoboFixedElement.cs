using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    class RoboFixedElement : RoboElement
    {
        public override IWebElement WrappedElement { get; }


        public RoboFixedElement(IRobot robot, ISearchContext context, IWebElement element) 
            : base(robot, context) {
            this.WrappedElement = element;
        }


        public override IWebElement Match() {
            return this.WrappedElement;
        }

        public override IWebElement Required() {
            if (this.WrappedElement == null) {
                throw new NotFoundException(
                    $"Element is not exist"
                    );
            }
            if (this.IsStaled) {
                throw new NotFoundException(
                    "Element is staled"
                    );
            }
            return this.WrappedElement;
        }
    }
}
