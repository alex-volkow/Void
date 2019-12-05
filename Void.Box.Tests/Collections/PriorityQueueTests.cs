using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Collections
{
    [Parallelizable]
    public class PriorityQueueTests
    {
        [Test]
        public void EnqueueRegular() {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual(2, queue.Dequeue());
        }

        [Test]
        public void EnqueuePriority() {
            var queue = new PriorityQueue<int>();
            for (var i = 0; i < 5; i++) {
                queue.Enqueue(i, 5 - i);
            }
            for (var i = 0; i < 5; i++) {
                Assert.AreEqual(i, queue.Dequeue());
            }
        }

        [Test]
        public void NoElementToDequeue() {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1);
            queue.Dequeue();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Test]
        public void PeekRegular() {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            Assert.AreEqual(1, queue.Peek());
        }

        [Test]
        public void PeekPriority() {
            var queue = new PriorityQueue<int>();
            for (var i = 0; i < 5; i++) {
                queue.Enqueue(i, 5 - i);
            }
            Assert.AreEqual(0, queue.Peek());
        }

        [Test]
        public void NoElementToPeek() {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1);
            queue.Dequeue();
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        [Test]
        public void PriorityOfExistingElement() {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1, 1);
            queue.Enqueue(2, 2);
            queue.Enqueue(3, 3);
            Assert.AreEqual(2, queue.PriorityOf(2));
        }

        [Test]
        public void PriorityOfNonExistingElement() {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1, 1);
            queue.Enqueue(2, 2);
            queue.Enqueue(3, 3);
            Assert.Throws<ArgumentException>(() => {
                queue.PriorityOf(4);
            });
        }

        [Test]
        public void RemoveExitingElement() {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1, 1);
            queue.Enqueue(2, 2);
            queue.Enqueue(3, 3);
            Assert.True(queue.Remove(2));
        }

        [Test]
        public void RemoveNonExitingElement() {
            var queue = new PriorityQueue<int>();
            queue.Enqueue(1, 1);
            queue.Enqueue(2, 2);
            queue.Enqueue(3, 3);
            Assert.False(queue.Remove(4));
        }

        [Test]
        public void EnumerateRegularQueue() {
            var array = new int[] { 1, 2, 3, 4, 5 };
            var queue = new PriorityQueue<int>();
            foreach (var item in array) {
                queue.Enqueue(item);
            }
            foreach (var item in queue) {
                var index = queue.IndexOf(item);
                Assert.AreEqual(array[index], item);
            }
        }

        [Test]
        public void EnumeratePriorityQueue() {
            var valuePriority = new Dictionary<string, int> { 
                ["1"] = 1,
                ["2"] = 2,
                ["3"] = 3
            };
            var queue = new PriorityQueue<string>();
            foreach (var item in valuePriority) {
                queue.Enqueue(item.Key, item.Value);
            }
            Assert.AreEqual("3", queue.Dequeue());
            Assert.AreEqual("2", queue.Dequeue());
            Assert.AreEqual("1", queue.Dequeue());
        }
    }
}
