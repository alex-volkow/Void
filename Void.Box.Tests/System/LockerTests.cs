using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Void
{
    public class LockerTests
    {
        public void Do() {
            var list = new List<int>();
            using var locker = new Locker();
            var bgThread = new Thread(() => {
                using (locker.Lock()) {
                    list.Add(3);
                }
            });
            using (locker.Lock()) {
                bgThread.Start();
                list.Add(1);
                using (locker.Lock()) {
                    list.Add(2);
                }
            }
            bgThread.Join();
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }
    }
}
