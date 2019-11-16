namespace NCommons.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using NCommons.Collections.Resources;

    /// <summary>
    ///     Stores and prioritizes elements based on a max binary heap.
    ///     The contained elements are prioritized by an <see cref="IComparer{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the heap.</typeparam>
    public class BinaryHeap<T> : Heap<T>
    {

        private const int DefaultCapacity = 4;

        // Since we require a dynamically growing array, we might aswell use a List<T> as a backing
        // collection, because it is most likely going to be faster than any custom growing algorithm.
        // If this turns out to be a bottleneck, it can still be swapped out later.
        private readonly List<T> _items;

        /// <summary>
        ///     Gets the number of elements in the heap.
        /// </summary>
        public sealed override int Count => _items.Count;

        /// <summary>
        ///     Gets or sets the total number of elements the internal data structure can hold
        ///     without resizing.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="Capacity"/> is set to a value that is less than <see cref="Count"/>.
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///     There is not enough memory available on the system.
        /// </exception>
        public int Capacity
        {
            get { return _items.Capacity; }
            set
            {
                // List<T>.Capacity does the range validation of value.
                _items.Capacity = value;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryHeap{T}"/> class.
        /// </summary>
        public BinaryHeap()
            : this(null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryHeap{T}"/> class with the
        ///     specified <paramref name="comparer"/> for determining the priority of items
        ///     during insertion and retrieval.
        /// </summary>
        /// <param name="comparer">
        ///     An <see cref="IComparer{T}"/> instance which is used for for determining the
        ///     priority of items during insertion and retrieval.
        ///     
        ///     If <see langword="null"/>, <see cref="Comparer{T}.Default"/> is used.
        /// </param>
        public BinaryHeap(IComparer<T>? comparer)
            : this(DefaultCapacity, comparer) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryHeap{T}"/> class.
        /// </summary>
        /// <param name="capacity">
        ///     The number of elements that the binary heap can initially store.
        /// </param>
        public BinaryHeap(int capacity)
            : this(capacity, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BinaryHeap{T}"/> class.
        /// </summary>
        /// <param name="capacity">
        ///     The number of elements that the binary heap can initially store.
        /// </param>
        /// <param name="comparer">
        ///     An <see cref="IComparer{T}"/> instance which is used for for determining the
        ///     priority of items during insertion and retrieval.
        ///     
        ///     If <see langword="null"/>, <see cref="Comparer{T}.Default"/> is used.
        /// </param>
        public BinaryHeap(int capacity, IComparer<T>? comparer)
            : base(comparer)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity),
                    capacity,
                    ExceptionStrings.Generic_NegativeCapacity
                );
            }

            _items = new List<T>(capacity);
        }

        /// <summary>
        ///     Inserts the specified <paramref name="item"/> at the correct position into the heap.
        /// </summary>
        /// <param name="item">The item to be inserted into the heap.</param>
        public override void Push(T item)
        {
            // Heapify-Up.
            // 1) Insert the element at the end of the heap/array.
            _items.Add(item);

            int currentIndex = Count - 1;
            int parentIndex;

            // 2) As long as the element has a higher priority than its parents, swap it with them.
            //    This ensures that the items with the high priorities are on top of the heap.
            while (!IsRoot(currentIndex) &&
                   HasHigherPriority(item, /* parent: */ _items[parentIndex = GetParentPos(currentIndex)]))
            {
                SwapAt(currentIndex, parentIndex);
                currentIndex = parentIndex;
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
        public override bool TryPeek(out T result)
        {
            if (Count > 0)
            {
                result = _items[0];
                return true;
            }
            else
            {
                result = default!;
                return false;
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
        public override bool TryPop(out T result)
        {
            if (Count > 0)
            {
                // The first element has the highest priority. Store it in the return parameter.
                result = _items[0];

                // Heapify-Down.
                // 1) Remove the highest-priority element and replace it with the last element in the heap.
                _items[0] = _items[Count - 1];
                _items.RemoveAt(Count - 1);

                int currentIndex = 0;
                int leftIndex;
                int rightIndex;
                int largestIndex;

                // 2) Rearrange the heap, until the element with the highest priority is on top again.
                while (true)
                {
                    leftIndex = GetLeftChildPos(currentIndex);
                    rightIndex = GetRightChildPos(currentIndex);
                    largestIndex = currentIndex;

                    // 2.1) Check if the left child has a higher priority than the current element.
                    //      If so, consider swapping.
                    if (leftIndex < Count && HasHigherPriority(_items[leftIndex], _items[largestIndex]))
                    {
                        largestIndex = leftIndex;
                    }

                    // 2.2) Check if the right child has a higher priority than the current element.
                    //      If so, it gets precedence over the left child.
                    if (rightIndex < Count && HasHigherPriority(_items[rightIndex], _items[largestIndex]))
                    {
                        largestIndex = rightIndex;
                    }

                    // 2.3) Do the swap, or return if the heap is in valid order.
                    if (largestIndex != currentIndex)
                    {
                        SwapAt(largestIndex, currentIndex);
                        currentIndex = largestIndex;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                result = default!;
                return false;
            }
        }

        /// <summary>
        ///     Removes all items contained in the heap.
        /// </summary>
        public override void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        ///     Returns an enumerator which iterates over the elements in the queue.
        ///     The iterator does not necessarily iterate over the items in the prioritized order.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerator{T}"/> which allows iterating over the heap's
        ///     elements.
        /// </returns>
        public override IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetParentPos(int childIndex)
        {
            return (childIndex - 1) / 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLeftChildPos(int parentIndex)
        {
            return (2 * parentIndex) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetRightChildPos(int parentIndex)
        {
            return (2 * parentIndex) + 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsRoot(int index)
        {
            return index == 0;
        }

        /// <summary>Returns whether a has a higher priority than b.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasHigherPriority(T a, T b)
        {
            return Comparer.Compare(a, b) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SwapAt(int indexA, int indexB)
        {
            T tmp = _items[indexA];
            _items[indexA] = _items[indexB];
            _items[indexB] = tmp;
        }

    }

}
