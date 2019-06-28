using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Void.Collections
{
    public class PriorityQueue<T> : IReadOnlyCollection<T>
    {
        private readonly LinkedList<Entry<T>> queue;



        public int Count => this.queue.Count;



        public PriorityQueue() {
            this.queue = new LinkedList<Entry<T>>();
        }



        public void Enqueue(T item) {
            this.Enqueue(item, default(int));
        }

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

        public virtual T Dequeue() {
            var item = GetFirst();
            this.queue.Remove(item);
            return item.Value.Value;
        }

        public T Peek() {
            return GetFirst().Value.Value;
        }

        public bool Remove(T item) {
            var entry = this.queue.FirstOrDefault(e => e.Value?.Equals(item) ?? false);
            if (entry != null) {
                return this.queue.Remove(entry);
            }
            return false;
        }

        public void Clear() {
            this.queue.Clear();
        }

        public IEnumerator<T> GetEnumerator() {
            if (this.Count > 0) {
                for (var node = this.queue.First; node != null; node = node.Next) {
                    yield return node.Value.Value;
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
