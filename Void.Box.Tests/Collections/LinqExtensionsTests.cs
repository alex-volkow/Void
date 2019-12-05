using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Void.Collections
{
    [Parallelizable]
    public class LinqExtensionsTests
    {
        private static int[] GetRandomUniqIntArray(int length) {
            var array = new int[length];
            var random = new Random();
            for (var i = 0; i < length; i++) {
                while (true) {
                    var value = random.Next();
                    if (!array.Contains(value)) {
                        array[i] = value;
                        break;
                    }
                }
            }
            return array;
        }
        private static string[] GetRandomStringArray(int length) {
            var array = new string[length];
            var random = new RandomGenerator();
            for (var i = 0; i < length; i++) {
                while (true) {
                    var value = random.NextString(length, "abcdefghijklmnoqprstuvwxyz");
                    if (!array.Contains(value)) {
                        array[i] = value;
                        break;
                    }
                }
            }
            return array;
        }


        [Test]
        public void ToQueue() {
            var array = GetRandomUniqIntArray(5);
            var queue = array.ToQueue();
            foreach (var item in array) {
                Assert.AreEqual(item, queue.Dequeue());
            }
        }

        [Test]
        public void IndexOf() {
            var array = GetRandomUniqIntArray(5);
            Assert.AreEqual(1, array.IndexOf(array[1]));
        }

        [Test]
        public void NextReguler() {
            var array = GetRandomUniqIntArray(5);
            Assert.AreEqual(array[1], array.Next(array[0]));
        }

        [Test]
        public void NextLast() {
            var array = GetRandomUniqIntArray(5);
            Assert.Throws<ArgumentException>(() => {
                array.Next(array[array.Length - 1]);
            });
        }

        [Test]
        public void NextEmpty() {
            var array = new int[] { };
            Assert.Throws<ArgumentException>(() => {
                array.Next(default);
            });
        }

        [Test]
        public void NextObjectExist() {
            var array = GetRandomStringArray(5);
            Assert.AreEqual(array[2], array.NextObject(array[1]));
        }

        [Test]
        public void NextObjectNull() {
            var array = GetRandomStringArray(5);
            Assert.Null(array.NextObject("123"));
        }

        [Test]
        public void NextValueExist() {
            var array = GetRandomUniqIntArray(5);
            Assert.AreEqual(array[2], array.NextValue(array[1]));
        }

        [Test]
        public void NextValueNull() {
            var array = new int[] { 1, 2, 3 };
            Assert.Null(array.NextValue(4));
        }

        [Test]
        public void Shuffle() {
            var array = GetRandomUniqIntArray(100);
            var heap = array.Shuffle();
            Assert.AreEqual(array.Length, heap.Count);
            foreach (var item in heap) {
                Assert.True(array.Contains(item));
            }
            var ordered = true;
            for (var i = 0; i < array.Length; i++) {
                if (array[i] != heap[i]) {
                    ordered = false;
                    break;
                }
            }
            if (ordered) {
                Assert.Fail("Collections have similar order");
            }
        }

        [Test]
        public void Circle() {
            using var cancel = new CancellationTokenSource();
            var array = GetRandomUniqIntArray(5);
            var limit = array.Length * 10;
            var counter = 0;
            foreach (var item in array.Circle(cancel.Token)) {
                var index = counter % array.Length;
                Assert.AreEqual(array[index], item);
                if (counter == limit) {
                    cancel.Cancel();
                }
                counter++;
            }
        }

        [Test]
        public void Count() {
            var array = GetRandomUniqIntArray(5);
            Assert.AreEqual(array.Length, ((IEnumerable)array).Count());
        }
    }
}
