using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Collections
{
    public class Tube<T> : IReadOnlyList<T>
    {
        private readonly T[] buffer;
        private int shift;
        private int count;



        public virtual int Count => this.count;

        public int Capacity => this.buffer.Length;



        public virtual T this[int index] {
            get {
                index = this.count - index - 1;
                var shift = this.count + this.shift - 1;
                var corrector = this.count / this.Capacity;
                var pointer = (corrector * shift + index) % this.Capacity;
                return this.buffer[pointer];
            }
        }



        public Tube(int capacity) {
            if (capacity < 1) {
                throw new ArgumentException(
                    $"Capacity must have positive value",
                    nameof(capacity)
                    );
            }
            this.buffer = new T[capacity];
        }



        public virtual void Add(T value) {
            this.count += 1 - (this.count / this.Capacity);
            var shift = this.count + this.shift + this.Capacity - 1;
            var index = shift % this.Capacity;
            this.buffer[index] = value;
            this.shift = (this.shift + this.count / this.Capacity) % this.Capacity;
        }

        public virtual void Clear() {
            if (!typeof(T).IsValueType) {
                for (var i = 0; i < this.Capacity; i++) {
                    this.buffer[i] = default(T);
                }
            }
            this.shift = 0;
            this.count = 0;
        }

        public virtual IEnumerator<T> GetEnumerator() {
            for (var i = 0; i < this.count; i++) {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
