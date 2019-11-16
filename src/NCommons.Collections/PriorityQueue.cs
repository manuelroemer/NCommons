namespace NCommons.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using NCommons.Collections.Resources;

    /// <summary>
    ///     Stores and serves items ordered by a priority.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the priority queue.</typeparam>
    /// <remarks>
    ///     The <see cref="PriorityQueue{T}"/> class is, at its heart, just a wrapper around
    ///     another <see cref="IPriorityQueue{T}"/> implementation.
    ///     This basically means that you don't have to use it at all - you could simply use
    ///     any class that implements the <see cref="IPriorityQueue{T}"/> interface directly.
    ///     
    ///     With that being said, using this class usually makes code easier to understand, because certain
    ///     <see cref="IPriorityQueue{T}"/> implementations rename methods (for instance, the
    ///     <see cref="BinaryHeap{T}"/> doesn't expose <c>Enqueue / Dequeue</c> methods; instead,
    ///     they are called <c>Push / Pop</c>).
    ///     For example, the code below uses a <see cref="PriorityQueue{T}"/> and a <see cref="BinaryHeap{T}"/>
    ///     to enqueue an integer. While the code's result is the same for the two classes, using
    ///     the <see cref="PriorityQueue{T}"/> makes the intention much clearer.
    ///     
    ///     <code>
    ///     var pq1 = new BinaryHeap&lt;int&gt;();
    ///     var pq2 = new PriorityQueue&lt;int&gt;();
    ///     
    ///     pq1.Push(1);
    ///     pq2.Enqueue(1);
    ///     </code>
    /// </remarks>
    [DebuggerTypeProxy(typeof(PriorityQueueDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        
        /// <summary>
        ///     Gets the underlying <see cref="IPriorityQueue{T}"/> instance which backs this
        ///     priority queue.
        /// </summary>
        protected IPriorityQueue<T> UnderlyingQueue { get; }

        /// <summary>
        ///     Gets the comparer which is used to determine the priority of items during
        ///     insertion and retrieval.
        /// </summary>
        public IComparer<T> Comparer => UnderlyingQueue.Comparer;

        /// <summary>
        ///     Gets the number of elements in the priority queue.
        /// </summary>
        public int Count => UnderlyingQueue.Count;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PriorityQueue{T}"/> class which
        ///     uses a <see cref="BinaryHeap{T}"/> for determining the priority of items during
        ///     insertion and retrieval.
        /// </summary>
        public PriorityQueue()
            : this(underlyingQueue: null, comparer: null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PriorityQueue{T}"/> class which
        ///     uses a <see cref="BinaryHeap{T}"/> and the specified <paramref name="comparer"/>
        ///     for determining the priority of items during insertion and retrieval.
        /// </summary>
        /// <param name="comparer">
        ///     An <see cref="IComparer{T}"/> instance which is used for for determining the
        ///     priority of items during insertion and retrieval.
        ///     
        ///     If <see langword="null"/>, <see cref="Comparer{T}.Default"/> is used.
        /// </param>
        public PriorityQueue(IComparer<T>? comparer)
            : this(underlyingQueue: null, comparer) { }

        /// <summary>
        ///     Initializes a new <see cref="PriorityQueue{T}"/> instance which
        ///     uses a given <see cref="IPriorityQueue{T}"/> instance
        ///     for determining the priority of items during insertion and retrieval.
        /// </summary>
        /// <param name="underlyingQueue">
        ///     An <see cref="IPriorityQueue{T}"/> instance which will be wrapped and thus
        ///     directly be used by this priority queue.
        ///     
        ///     If <see langword="null"/>, a newly created <see cref="BinaryHeap{T}"/> instance is
        ///     used instead.
        /// </param>
        public PriorityQueue(IPriorityQueue<T>? underlyingQueue)
            : this(underlyingQueue, underlyingQueue?.Comparer) { }

        private PriorityQueue(IPriorityQueue<T>? underlyingQueue, IComparer<T>? comparer)
        {
            UnderlyingQueue = underlyingQueue ?? new BinaryHeap<T>(comparer ?? Comparer<T>.Default);
        }

        /// <summary>
        ///     Inserts the specified <paramref name="item"/> at the correct position into the queue.
        /// </summary>
        /// <param name="item">The item to be inserted into the queue.</param>
        public virtual void Enqueue(T item) =>
            UnderlyingQueue.Enqueue(item);

        /// <summary>
        ///     Returns the item with the highest priority without removing it from the queue.
        /// </summary>
        /// <returns>The item with the highest priority.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the queue is empty, i.e. when it does not contain any items.
        ///     Use <see cref="TryPeek(out T)"/> for an exception-less version.
        /// </exception>
        /// <seealso cref="TryPeek(out T)"/>
        public virtual T Peek()
        {
            try
            {
                return UnderlyingQueue.Peek();
            }
            catch (InvalidOperationException ex)
            {
                // We want a custom exception message for this class.
                throw new InvalidOperationException(
                    ExceptionStrings.PriorityQueue_PeekDequeueOnEmptyHeap,
                    ex
                );
            }
        }

        /// <summary>
        ///     Removes and returns the item with the highest priority from the queue.
        /// </summary>
        /// <returns>The item that was removed from the queue and had the highest priority.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the queue is empty, i.e. when it does not contain any items.
        ///     Use <see cref="TryDequeue(out T)"/> for an exception-less version.
        /// </exception>
        /// <seealso cref="TryDequeue(out T)"/>
        public virtual T Dequeue()
        {
            try
            {
                return UnderlyingQueue.Dequeue();
            }
            catch (InvalidOperationException ex)
            {
                // We want a custom exception message for this class.
                throw new InvalidOperationException(
                    ExceptionStrings.PriorityQueue_PeekDequeueOnEmptyHeap,
                    ex
                );
            }
        }

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
        public virtual bool TryPeek(out T result) =>
            UnderlyingQueue.TryPeek(out result);

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
        public virtual bool TryDequeue(out T result) =>
            UnderlyingQueue.TryDequeue(out result);

        /// <summary>
        ///     Removes all items from the queue.
        /// </summary>
        public virtual void Clear() =>
            UnderlyingQueue.Clear();

        /// <summary>
        ///     Returns an enumerator which iterates over the elements in the queue.
        ///     The iterator does not necessarily iterate over the items in the prioritized order.
        ///     See remarks of <see cref="IPriorityQueue{T}"/> for details.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerator{T}"/> which allows iterating over the queue's elements.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() =>
            UnderlyingQueue.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator which iterates over the elements in the queue.
        ///     The iterator does not necessarily iterate over the items in the prioritized order.
        ///     See remarks of <see cref="IPriorityQueue{T}"/> for details.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerator{T}"/> which allows iterating over the queue's elements.
        /// </returns>
        public virtual IEnumerator<T> GetEnumerator() =>
            UnderlyingQueue.GetEnumerator();

    }

}
