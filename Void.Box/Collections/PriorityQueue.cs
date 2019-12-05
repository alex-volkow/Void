using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Void.Collections
{
    /// <summary>
    /// Represents a first-in, first-out collection of objects with priority.
    /// </summary>
    public class PriorityQueue<T> : IReadOnlyCollection<T>
    {
        private readonly LinkedList<Entry<T>> queue;


        /// <summary>
        /// Gets the number of elements contained in the queue.
        /// </summary>
        public int Count => this.queue.Count;



        public PriorityQueue() {
            this.queue = new LinkedList<Entry<T>>();
        }


        /// <summary>
        /// Adds an object to the end of the queue with default (0) priority.
        /// </summary>
        public void Enqueue(T item) {
            this.Enqueue(item, default);
        }

        /// <summary>
        /// Adds an object to the end of the queue with custom priority.
        /// </summary>
        public virtual void Enqueue(T item, int priority) {
            var entry = new Entry<T>(item, priority);
            for (var node = this.queue.First; node != null; node = node.Next) {
                if (node.Value.Priority < priority) {
                    this.queue.AddBefore(node, entry);
                    return;
                }
            }
            this.queue.AddLast(
                entry
                );
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the queue with highest priority.
        /// </summary>
        /// <exception cref="InvalidOperationException">Queue is empty.</exception>
        public virtual T Dequeue() {
            var item = GetFirst();
            this.queue.Remove(item);
            return item.Value.Value;
        }

        /// <summary>
        /// Get priority of the object in queue.
        /// </summary>
        /// <exception cref="ArgumentException">Queue does not contain element.</exception>
        public int PriorityOf(T item) {
            var match = GetNodes().FirstOrDefault(e => object.Equals(e.Value, item));
            if (match == null) {
                throw new ArgumentException(
                    $"Queue does not contain element"
                    );
            }
            return match.Priority;
        }

        /// <summary>
        /// Returns the object at the beginning of the queue without removing it.
        /// </summary>
        /// <exception cref="InvalidOperationException">Queue is empty.</exception>
        public T Peek() {
            return GetFirst().Value.Value;
        }

        /// <summary>
        /// Remove the object from the queue.
        /// </summary>
        /// <returns>True is the object has been removed, other false.</returns>
        public bool Remove(T item) {
            var entry = this.queue.FirstOrDefault(e => e.Value?.Equals(item) ?? false);
            if (entry != null) {
                return this.queue.Remove(entry);
            }
            return false;
        }

        /// <summary>
        /// Removes all objects from the queue.
        /// </summary>
        public void Clear() {
            this.queue.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the queue.
        /// </summary>
        public IEnumerator<T> GetEnumerator() {
            if (this.Count > 0) {
                for (var node = this.queue.First; node != null; node = node.Next) {
                    yield return node.Value.Value;
                }
            }
        }

        private IEnumerable<Entry<T>> GetNodes() {
            if (this.Count > 0) {
                for (var node = this.queue.First; node != null; node = node.Next) {
                    yield return node.Value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        private LinkedListNode<Entry<T>> GetFirst() {
            return this.Count > 0
                ? this.queue.First
                : throw new InvalidOperationException(
                    "Queue is empty"
                    );
        }



        private class Entry<V> : IEquatable<Entry<V>>, IEquatable<V>
        {
            public V Value { get; private set; }

            public int Priority { get; private set; }



            public Entry(V value, int priority) {
                this.Value = value;
                this.Priority = priority;
            }



            public bool Equals(V other) {
                return object.Equals(this.Value, other);
            }

            public bool Equals(Entry<V> other) {
                return other != null &&
                    other.Priority == this.Priority &&
                    object.Equals(this.Value, other.Value);
            }

            public override bool Equals(object obj) {
                if (obj is Entry<V>) {
                    return Equals(obj as Entry<V>);
                }
                if (obj is V) {
                    return Equals((V)obj);
                }
                return false;
            }

            public override int GetHashCode() {
                return HashCode.Create(
                    this.Value,
                    this.Priority
                    );
            }

            public override string ToString() {
                return this.Value?.ToString();
            }

            public static implicit operator V(Entry<V> entry) {
                return entry.Value;
            }
        }
    }
}
