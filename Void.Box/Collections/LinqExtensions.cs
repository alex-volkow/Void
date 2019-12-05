using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Void.Collections
{
    /// <summary>
    /// Provides additional Linq extension methods.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Create new Queue<T> using the enumeration.
        /// </summary>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> collection) {
            return new Queue<T>(collection);
        }

        /// <summary>
        /// Determines the index of a specific item in the enumeration.
        /// </summary>
        /// <returns>The index of item if found in the enumeration; otherwise, -1.</returns>
        public static int IndexOf<T>(this IEnumerable<T> collection, T item) {
            var index = 0;
            if (collection is IList<T> list) {
                return list.IndexOf(item);
            }
            foreach (var element in collection) {
                if (item.Equals(element)) {
                    return index;
                }
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Return next element after the specified.
        /// </summary>
        /// <exception cref="ArgumentException">No matches found or the specified element is the last.</exception>
        /// <returns>Next element.</returns>
        public static T Next<T>(this IEnumerable<T> collection, T item) {
            var found = false;
            foreach (var element in collection) {
                if (found) {
                    return element;
                }
                if (item.Equals(element)) {
                    found = true;
                }
            }
            throw new ArgumentException(found
                ? "The element is the last"
                : "Collection does not contain element"
                );
        }

        /// <summary>
        /// Return next object item from the specified element from enumeration or null.
        /// </summary>
        public static T NextObject<T>(this IEnumerable<T> collection, T item) where T : class {
            return collection.Contains(item) ? collection.Next(item) : default;
        }

        /// <summary>
        /// Return next value item from the specified element from enumeration or null.
        /// </summary>
        public static T? NextValue<T>(this IEnumerable<T> collection, T item) where T : struct {
            return collection.Contains(item) ? collection.Next(item) : default(T?);
        }

        /// <summary>
        /// Shuffle items randomly.
        /// </summary>
        /// <returns>Shuffled collection.</returns>
        public static IReadOnlyList<T> Shuffle<T>(this IEnumerable<T> collection) {
            var list = collection.ToList();
            int count = list.Count;
            while (count-- > 1) {
                var k = RandomGenerator.Default.Next(count + 1);
                T value = list[k];
                list[k] = list[count];
                list[count] = value;
            }
            return list;
        }

        /// <summary>
        /// Close the enumeration on itself until canceled.
        /// </summary>
        /// <returns>Endless enumeration.</returns>
        public static IEnumerable<T> Circle<T>(this IEnumerable<T> collection, CancellationToken token) {
            while (!token.IsCancellationRequested) {
                foreach (var item in collection) {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the enumeration.
        /// </summary>
        public static int Count(this IEnumerable items) {
            if (items is ICollection collection) {
                return collection.Count;
            }
            var count = 0;
            if (items != null) {
                foreach (var item in items) {
                    count++;
                }
            }
            return count;
        }
    }
}
