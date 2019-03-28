using System;
using System.Collections;
using System.Collections.Generic;

namespace NCommons.Collections
{

    /// <summary>
    ///     A special collection which only contains weak references to the elements
    ///     that are added to it.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items added to the collection.
    /// </typeparam>
    /// <remarks>
    ///     This collection only implements the <see cref="IEnumerable{T}"/> interface, because
    ///     not all members of <see cref="ICollection{T}"/> can be implemented.
    /// </remarks>
    public sealed class WeakReferenceCollection<T> : IEnumerable<T> where T : class
    {

        private readonly IList<WeakReference<T>> _underlyingCollection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeakReferenceCollection{T}"/> class.
        /// </summary>
        public WeakReferenceCollection()
        {
            _underlyingCollection = new List<WeakReference<T>>();
        }

        /// <summary>
        ///     Adds a weak reference to the <paramref name="item"/> to the collection.
        /// </summary>
        /// <param name="item">The item to be added to the collection.</param>
        public void Add(T item)
        {
            _underlyingCollection.Add(new WeakReference<T>(item));
        }

        /// <summary>
        ///     Removes the first occurence of the specified <paramref name="item"/> from the
        ///     collection.
        ///     This may purge dead references.
        /// </summary>
        /// <param name="item">The item to be removed from the collection.</param>
        /// <returns>
        ///     true if the item was removed from the collection;
        ///     false if no such item was found in the collection.
        /// </returns>
        public bool Remove(T item)
        {
            int itemIndex = -1;
            int iterationIndex = 0;
            var enumerator = GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == item)
                {
                    itemIndex = iterationIndex;
                    break;
                }
                else
                {
                    iterationIndex++;
                }
            }

            if (itemIndex >= 0)
            {
                _underlyingCollection.RemoveAt(itemIndex);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Returns a value indicating whether the collection contains an alive reference to
        ///     the specified <paramref name="item"/>.
        ///     This may purge dead references.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     true if the collection contains an alive reference to the specified 
        ///     <paramref name="item"/>; false if not.
        /// </returns>
        public bool Contains(T item)
        {
            foreach (var addedItem in this)
            {
                if (addedItem == item)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Iterates over the whole collection and removes any dead references.
        /// </summary>
        public void ClearDeadReferences()
        {
            // Enumerating over this collection takes care of removing dead references.
            // All we have to do to remove all dead references is a full enumeration.
            foreach (var item in this)
            {
            }
        }

        /// <summary>
        ///     Clears the collection.
        /// </summary>
        public void Clear()
        {
            _underlyingCollection.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator which iterates over all active weak references in the
        ///     collection.
        ///     Dead references are purged on-the-fly.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            int index = 0;
            int count = _underlyingCollection.Count;

            // When creating an enumerator, we will heavily abuse yield return to automatically
            // remove any dead reference we find during the enumeration.
            // If a reference is dead, we skip it and head over to the next element.
            // This leads to an automatic purge of dead references during every enumeration.
            while (index < count)
            {
                if (_underlyingCollection[index].TryGetTarget(out var item))
                {
                    index++;
                    yield return item;
                }
                else
                {
                    // The reference at the current position is dead. Remove it and try the next
                    // one.
                    _underlyingCollection.RemoveAt(index);
                    count--;
                    continue;
                }
            }
        }

    }

}
