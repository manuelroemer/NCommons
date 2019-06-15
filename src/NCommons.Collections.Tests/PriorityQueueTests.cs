namespace NCommons.Collections.Tests
{
    using System.Collections.Generic;
    using NCommons.Collections;
    using Xunit;


    public sealed class PriorityQueueTests : IPriorityQueueTests
    {

        protected override IPriorityQueue<T> CreateQueue<T>(IComparer<T>? comparer = null)
        {
            // The base class already covers a lot of tests for this class.
            // We must only provide instances, with and without a comparer.
            //
            // This covers the cases where PriorityQueue<T> uses a default underlying queue.
            // This doesn't have to be tested in this class.
            return new PriorityQueue<T>(comparer);
        }

        [Fact]
        public void Ctor_Accepts_Null()
        {
            // Should not throw.
            new PriorityQueue<int>((IComparer<int>?)null);
            new PriorityQueue<int>((IPriorityQueue<int>?)null);
        }

        [Fact]
        public void Enqueue_Uses_Provided_Underlying_Queue()
        {
            var underlyingHeap = new BinaryHeap<int>();
            var queue = new PriorityQueue<int>(underlyingHeap);

            queue.Enqueue(1);

            Assert.Equal(1, underlyingHeap.Count);
        }

        [Fact]
        public void Dequeue_Uses_Provided_Underlying_Queue()
        {
            var underlyingHeap = new BinaryHeap<int>();
            var queue = new PriorityQueue<int>(underlyingHeap);

            queue.Enqueue(1); queue.Enqueue(2); queue.Enqueue(3);
            queue.Dequeue();

            Assert.Equal(2, underlyingHeap.Count);
        }
        
        [Fact]
        public void TryDequeue_Uses_Provided_Underlying_Queue()
        {
            var underlyingHeap = new BinaryHeap<int>();
            var queue = new PriorityQueue<int>(underlyingHeap);

            queue.Enqueue(1); queue.Enqueue(2); queue.Enqueue(3);
            queue.TryDequeue(out _);

            Assert.Equal(2, underlyingHeap.Count);
        }
        
        [Fact]
        public void Peek_Uses_Provided_Underlying_Queue()
        {
            var underlyingHeap = new BinaryHeap<int>();
            var queue = new PriorityQueue<int>(underlyingHeap);

            underlyingHeap.Push(1);

            Assert.Equal(1, queue.Peek());
        }
        
        [Fact]
        public void TryPeek_Uses_Provided_Underlying_Queue()
        {
            var underlyingHeap = new BinaryHeap<int>();
            var queue = new PriorityQueue<int>(underlyingHeap);

            underlyingHeap.Push(1);

            queue.TryPeek(out var peekResult);
            Assert.Equal(1, peekResult);
        }
        
        [Fact]
        public void Clear_Uses_Provided_Underlying_Queue()
        {
            var underlyingHeap = new BinaryHeap<int>();
            var queue = new PriorityQueue<int>(underlyingHeap);

            underlyingHeap.Push(1);

            queue.Clear();
            Assert.Equal(0, underlyingHeap.Count);
        }

    }

}
