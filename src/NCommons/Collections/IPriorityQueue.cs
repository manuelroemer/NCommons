using System;
using System.Collections.Generic;

namespace NCommons.Collections
{

    /// <summary>
    ///     Represents a data structure which stores and serves items ordered by a priority.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the priority queue.</typeparam>
    /// <remarks>
    ///     **Important:**
    ///     
    ///     <see cref="IPriorityQueue{T}"/> extends <see cref="IEnumerable{T}"/>, but care needs to
    ///     be taken when iterating over a queue. The internal order of elements added to the queue
    ///     will usually not equal the logical, priority-based ordering of the elements.
    ///     This means that when iterating over a queue, the elements will most likely be returned
    ///     in a "random" (not random at all, but not prioritized) order.
    ///     
    ///     The actual behavior does, of course, depend on the actual implementation.
    /// </remarks>
    public interface IPriorityQueue<T> : IEnumerable<T>, IReadOnlyCollection<T>
    {

        /// <summary>
        ///     Gets the comparer which is used to determine the priority of items during
        ///     insertion and retrieval.
        /// </summary>
        IComparer<T> Comparer { get; }
        
        /// <summary>
        ///     Inserts the specified <paramref name="item"/> at the correct position into the queue.
        /// </summary>
        /// <param name="item">The item to be inserted into the queue.</param>
        void Enqueue(T item);

        /// <summary>
        ///     Returns the item with the highest priority without removing it from the queue.
        /// </summary>
        /// <returns>The item with the highest priority.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the queue is empty, i.e. when it does not contain any items.
        ///     Use <see cref="TryPeek(out T)"/> for an exception-less version.
        /// </exception>
        /// <seealso cref="TryPeek(out T)"/>
        T Peek();

        /// <summary>
        ///     Removes and returns the item with the highest priority from the queue.
        /// </summary>
        /// <returns>The item that was removed from the queue and had the highest priority.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the queue is empty, i.e. when it does not contain any items.
        ///     Use <see cref="TryDequeue(out T)"/> for an exception-less version.
        /// </exception>
        /// <seealso cref="TryDequeue(out T)"/>
        T Dequeue();

        /// <summary>
        ///     Attempts to return the item with the highest priority without removing it from the queue.
        /// </summary>
        /// <param name="result">
        ///     When this method succeeded, this parameter holds the result of the operation, i.e.
        ///     the item with the highest priority.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the operation succeeded, i.e. the queue contained one or more item(s).
        ///     <c>false</c> if the queue was empty.
        /// </returns>
        /// <seealso cref="Peek"/>
        bool TryPeek(out T result);

        /// <summary>
        ///     Attempts to remove and return the item with the highest priority from the queue.
        /// </summary>
        /// <param name="result">
        ///     When this method succeeded, this parameter holds the result of the operation, i.e.
        ///     the item with the highest priority.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the operation succeeded, i.e. the queue contained one or more item(s).
        ///     <c>false</c> if the queue was empty.
        /// </returns>
        bool TryDequeue(out T result);

        /// <summary>
        ///     Removes all items from the queue.
        /// </summary>
        void Clear();

    }

}
