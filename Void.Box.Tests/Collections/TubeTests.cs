using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Collections
{
    [Parallelizable]
    public class TubeTests
    {
        [Test]
        public void AddElements() {
            var tube = new Tube<int>(3) {
                1
            };
            Assert.True(tube.SequenceEqual(new int[] { 1 }));
            tube.Add(2);
            Assert.True(tube.SequenceEqual(new int[] { 2, 1 }));
            tube.Add(3);
            Assert.True(tube.SequenceEqual(new int[] { 3, 2, 1 }));
            tube.Add(4);
            Assert.True(tube.SequenceEqual(new int[] { 4, 3, 2 }));
        }

        [Test]
        public void GetByIndex() {
            var tube = new Tube<int>(4) { 
                1, 2, 3
            };
            Assert.AreEqual(3, tube[0]);
            Assert.AreEqual(1, tube[2]);
            tube.Add(4);
            tube.Add(5);
            Assert.AreEqual(5, tube[0]);
            Assert.AreEqual(2, tube[3]);
        }
    }
}
