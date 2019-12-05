using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Collections
{
    /// <summary>
    /// Represents fixed size collection with auto-removing of elements according to the principle of first-in, first-out.
    /// </summary>
    public class Tube<T> : IReadOnlyList<T>
    {
        private readonly T[] buffer;
        private int shift;
        private int count;



        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public virtual int Count => this.count;

        /// <summary>
        /// Get the maximum possible count of items in the collection.
        /// </summary>
        public int Capacity => this.buffer.Length;

        /// <summary>
        /// Get item from the collection by index.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Index is out of range.</exception>

        public virtual T this[int index] {
            get {
                index = this.count - index - 1;
                var shift = this.count + this.shift - 1;
                var corrector = this.count / this.Capacity;
                var pointer = (corrector * shift + index) % this.Capacity;
                return this.buffer[pointer];
            }
        }


        /// <summary>
        /// Initialize a new instance with the capacity.
        /// </summary>
        /// <exception cref="ArgumentException">Capacity must have positive value.</exception>
        public Tube(int capacity) {
            if (capacity < 1) {
                throw new ArgumentException(
                    $"Capacity must have positive value",
                    nameof(capacity)
                    );
            }
            this.buffer = new T[capacity];
        }


        /// <summary>
        /// Add the element to the beginning of the collection. 
        /// If the collection size is larger than the capacity, then the last item will be deleted.
        /// </summary>
        public virtual void Add(T value) {
            this.count += 1 - (this.count / this.Capacity);
            var shift = this.count + this.shift + this.Capacity - 1;
            var index = shift % this.Capacity;
            this.buffer[index] = value;
            this.shift = (this.shift + this.count / this.Capacity) % this.Capacity;
        }
        
        /// <summary>
        /// Removes all objects from the collection.
        /// </summary>
        public virtual void Clear() {
            if (!typeof(T).IsValueType) {
                for (var i = 0; i < this.Capacity; i++) {
                    this.buffer[i] = default(T);
                }
            }
            this.shift = 0;
            this.count = 0;
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the queue.
        /// </summary>
        public virtual IEnumerator<T> GetEnumerator() {
            for (var i = 0; i < this.count; i++) {
                yield return this[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
