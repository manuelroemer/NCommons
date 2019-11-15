namespace NCommons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using NCommons.Collections.Resources;

    /// <summary>
    ///     An abstract base class for any <see cref="PriorityQueue{T}"/> implementation that
    ///     uses a heap of any kind for managing the data.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the heap.</typeparam>
    [DebuggerTypeProxy(typeof(PriorityQueueDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public abstract class Heap<T> : IPriorityQueue<T>
    {

        /// <summary>
        ///     Gets the comparer which is used to determine the priority of items during
        ///     insertion and retrieval.
        /// </summary>
        public IComparer<T> Comparer { get; }

        /// <summary>
        ///     Gets the number of elements in the heap.
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        ///     Initializes a new <see cref="Heap{T}"/> instance with the specified
        ///     <paramref name="comparer"/>.
        /// </summary>
        /// <param name="comparer">
        ///     An <see cref="IComparer{T}"/> instance which is used for for determining the
        ///     priority of items during insertion and retrieval.
        ///     
        ///     If <see langword="null"/>, <see cref="Comparer{T}.Default"/> is used.
        /// </param>
        public Heap(IComparer<T>? comparer)
        {
            Comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        ///     Inserts the specified <paramref name="item"/> at the correct position into the heap.
        /// </summary>
        /// <param name="item">The item to be inserted into the heap.</param>
        public abstract void Push(T item);

        /// <summary>
        ///     Removes and returns the item with the highest priority from the heap.
        /// </summary>
        /// <returns>The item that was removed from the heap and had the highest priority.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the heap is empty, i.e. when it does not contain any items.
        ///     Use <see cref="TryPop(out T)"/> for an exception-less version.
        /// </exception>
        /// <seealso cref="TryPop(out T)"/>
        public virtual T Pop()
        {
            if (TryPop(out var result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException(ExceptionStrings.Heap_PeekPopOnEmptyHeap);
            }
        }

        /// <summary>
        ///     Attempts to remove and return the item with the highest priority from the heap.
        /// </summary>
        /// <param name="result">
        ///     When this method succeeded, this parameter holds the result of the operation, i.e.
        ///     the item with the highest priority.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the operation succeeded, i.e. the heap contained one or more item(s).
        ///     <c>false</c> if the heap was empty.
        /// </returns>
        public abstract bool TryPop(out T result);

        /// <summary>
        ///     Returns the item with the highest priority without removing it from the heap.
        /// </summary>
        /// <returns>The item with the highest priority.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the heap is empty, i.e. when it does not contain any items.
        ///     Use <see cref="TryPeek(out T)"/> for an exception-less version.
        /// </exception>
        /// <seealso cref="TryPeek(out T)"/>
        public virtual T Peek()
        {
            if (TryPeek(out var result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException(ExceptionStrings.Heap_PeekPopOnEmptyHeap);
            }
        }

        /// <summary>
        ///     Attempts to return the item with the highest priority without removing it from the heap.
        /// </summary>
        /// <param name="result">
        ///     When this method succeeded, this parameter holds the result of the operation, i.e.
        ///     the item with the highest priority.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the operation succeeded, i.e. the heap contained one or more item(s).
        ///     <c>false</c> if the heap was empty.
        /// </returns>
        /// <seealso cref="Peek"/>
        public abstract bool TryPeek(out T result);

        /// <summary>
        ///     Removes all items contained in the heap.
        /// </summary>
        public abstract void Clear();

        /// <seealso cref="Push(T)"/>
        void IPriorityQueue<T>.Enqueue(T item)
        {
            Push(item);
        }

        /// <seealso cref="Pop"/>
        T IPriorityQueue<T>.Dequeue()
        {
            return Pop();
        }

        /// <seealso cref="TryPop(out T)"/>
        bool IPriorityQueue<T>.TryDequeue(out T result)
        {
            return TryPop(out result);
        }

        /// <summary>
        ///     Returns an enumerator which iterates over the elements in the heap.
        ///     The iterator does not necessarily iterate over the items in the prioritized order.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerator{T}"/> which allows iterating over the heap's
        ///     elements.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator which iterates over the elements in the heap.
        ///     The iterator does not necessarily iterate over the items in the prioritized order.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerator{T}"/> which allows iterating over the heap's
        ///     elements.
        /// </returns>
        public abstract IEnumerator<T> GetEnumerator();

    }

}
