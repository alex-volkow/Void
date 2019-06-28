using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    public partial class JOption
    {
        private class JElement : JOption
        {
            public string Name { get; }

            public JElement(JOption parent, string name)
                : base(parent) {
                this.Name = name;
            }

            public override object Clone() {
                lock (this.locker) {
                    return new JElement(this.parent, this.Name);
                }
            }

            public override bool Equals(JOption other) {
                return other != null 
                    && other is JElement element 
                    && this.Name == element.Name 
                    && base.Equals(other);
            }
        }

        private class JIndex : JOption
        {
            public int Index { get; }

            public JIndex(JOption parent, int index)
                : base(parent) {
                //if (index < 0) {
                //    throw new IndexOutOfRangeException();
                //}
                this.Index = index;
            }

            public override object Clone() {
                lock (this.locker) {
                    return new JIndex(this.parent, this.Index);
                }
            }

            public override bool Equals(JOption other) {
                return other != null
                    && other is JIndex item
                    && this.Index == item.Index
                    && base.Equals(other);
            }
        }
    }
}
