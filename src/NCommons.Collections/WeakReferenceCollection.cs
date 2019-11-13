namespace NCommons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using NCommons.Collections.Resources;

    /// <summary>
    ///     A special collection which only holds weak references to the elements
    ///     that are added to it.
    ///     This allows the elements that are added to this list to be garbage-collected.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of items added to the collection.
    /// </typeparam>
    /// <remarks>
    ///     This collection only implements the <see cref="IEnumerable{T}"/> interface, because
    ///     not all members of <see cref="ICollection{T}"/> can be implemented.
    /// </remarks>
    public sealed class WeakReferenceCollection<T> : IEnumerable<T> where T : class?
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<WeakReference<T>?> _underlyingCollection;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _version;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WeakReferenceCollection{T}"/> class.
        /// </summary>
        public WeakReferenceCollection()
        {
            _underlyingCollection = new List<WeakReference<T>?>();
        }

        /// <summary>
        ///     Adds a weak reference to the <paramref name="item"/> to the collection.
        /// </summary>
        /// <param name="item">The item to be added to the collection.</param>
        /// <param name="trackResurrection">
        ///     <see langword="true"/> to track the object after finalization;
        ///     <see langword="false"/> to track the object only until finalization.
        /// </param>
        public void Add(T item, bool trackResurrection = false)
        {
            IncrementVersion();
            if (item is null)
            {
                _underlyingCollection.Add(null);
            }
            else
            {
                _underlyingCollection.Add(new WeakReference<T>(item, trackResurrection));
            }
        }

        /// <summary>
        ///     Removes the first occurence of the specified <paramref name="item"/> from the
        ///     collection.
        ///     This may purge dead references.
        /// </summary>
        /// <param name="item">The item to be removed from the collection.</param>
        /// <returns>
        ///     <c>true</c> if the item was removed from the collection;
        ///     <c>false</c> if no such item was found in the collection.
        /// </returns>
        public bool Remove(T item)
        {
            int currentIndex = 0;
            int foundIndex = -1;

            foreach (var currentItem in this)
            {
                if (currentItem == item)
                {
                    foundIndex = currentIndex;
                }
                else
                {
                    currentIndex++;
                }
            }

            if (foundIndex > -1)
            {
                IncrementVersion();
                _underlyingCollection.RemoveAt(foundIndex);
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
        ///     <c>true</c> if the collection contains an alive reference to the specified 
        ///     <paramref name="item"/>; <c>false</c> if not.
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
            if (_underlyingCollection.Count > 0)
            {
                IncrementVersion();
                _underlyingCollection.Clear();
            }
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
            return new Enumerator(this, _version);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void IncrementVersion()
        {
            unchecked
            {
                _version++;
            }
        }

        private struct Enumerator : IEnumerator<T>
        {

            private const int EnumerationNotStartedYet = -1;
            private const int EnumerationFinished = -2;

            private WeakReferenceCollection<T> _collection;
            private readonly int _version;
            private int _pos;
            private T _current;

            object? IEnumerator.Current => Current;

            public T Current
            {
                get
                {
                    VerifyEnumerationStarted();
                    VerifyEnumerationNotFinished();
                    return _current;
                }
            }

            internal Enumerator(WeakReferenceCollection<T> collection, int version)
            {
#nullable disable
                _collection = collection;
                _version = version;
                _pos = EnumerationNotStartedYet;
                _current = null;
#nullable restore
            }

            public bool MoveNext()
            {
                VerifyVersion();

                while (++_pos < _collection._underlyingCollection.Count)
                {
                    var itemRef = _collection._underlyingCollection[_pos];
                    if (itemRef is null)
                    {
                        // The WeakReference<T> is null. -> null was added to the collection.
                        _current = null!;
                        return true;
                    }
                    else
                    {
                        if (itemRef.TryGetTarget(out var dereferencedItem))
                        {
                            // Valid item, all good. Store it in this enumerator so that it doesn't
                            // get collected while enumerating.
                            _current = dereferencedItem;
                            return true;
                        }
                        else
                        {
                            // The item is dead. We can remove the WeakReference from the collection now.
                            // The interesting part is that we must stay at the same pos, because the collection
                            // "loses" a member. -> _pos--
                            _collection._underlyingCollection.RemoveAt(_pos--);
                            continue;
                        }
                    }
                }

                // If we get here, nothing was found, i.e. the enumeration ended.
                _current = null!;
                _pos = EnumerationFinished;
                return false;
            }

            public void Reset()
            {
                VerifyVersion();
                _pos = EnumerationNotStartedYet;
                _current = null!;
            }

            public void Dispose() { }

            private void VerifyVersion()
            {
                if (_version != _collection._version)
                {
                    throw new InvalidOperationException(ExceptionStrings.Enumerator_MismatchingVersions);
                }
            }

            private void VerifyEnumerationStarted()
            {
                if (_pos == EnumerationNotStartedYet)
                {
                    throw new InvalidOperationException(ExceptionStrings.Enumerator_EnumerationHasNotStarted);
                }
            }

            private void VerifyEnumerationNotFinished()
            {
                if (_pos == EnumerationFinished)
                {
                    throw new InvalidOperationException(ExceptionStrings.Enumerator_EnumerationFinished);
                }
            }

        }

    }

}
