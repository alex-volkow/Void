using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Collections
{
    [Parallelizable]
    public class MapTests
    {
        [Test]
        public void AddNullValue() {
            var map = new Map<int, string>();
            for (var i = 0; i < 5; i++) {
                map[i] = i.ToString();
            }
            map.Add(5, null);
            Assert.AreEqual(5, map.Count);
            map[6] = null;
            Assert.AreEqual(5, map.Count);
        }

        [Test]
        public void SetNullValue() {
            var map = new Map<int, string>();
            for (var i = 0; i < 5; i++) {
                map[i] = i.ToString();
            }
            map[0] = null;
            Assert.AreEqual(4, map.Count);
        }
    }
}
