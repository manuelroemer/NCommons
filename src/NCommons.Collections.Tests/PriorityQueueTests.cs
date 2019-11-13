namespace NCommons.Collections.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Xunit;

    public class PriorityQueueTests
    {

        public class PriorityQueueInterfaceTests : PriorityQueueInterfaceTestBase
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

        }

        public class ConstructorTests
        {

            [Fact]
            public void Can_Pass_Null()
            {
                // Should not throw.
                _ = new PriorityQueue<int>((IComparer<int>?)null);
                _ = new PriorityQueue<int>((IPriorityQueue<int>?)null);
            }

        }

        public class EnqueueTests
        {

            [Fact]
            public void Enqueue_Uses_Provided_Underlying_Queue()
            {
                var underlyingHeap = new BinaryHeap<int>();
                var queue = new PriorityQueue<int>(underlyingHeap);

                queue.Enqueue(1);

                Assert.Equal(1, underlyingHeap.Count);
            }

        }

        public class DequeueTests
        {

            [Fact]
            public void Dequeue_Uses_Provided_Underlying_Queue()
            {
                var underlyingHeap = new BinaryHeap<int>();
                var queue = new PriorityQueue<int>(underlyingHeap);

                queue.Enqueue(1); queue.Enqueue(2); queue.Enqueue(3);
                queue.Dequeue();

                Assert.Equal(2, underlyingHeap.Count);
            }

        }

        public class TryDequeueTests
        {

            [Fact]
            public void TryDequeue_Uses_Provided_Underlying_Queue()
            {
                var underlyingHeap = new BinaryHeap<int>();
                var queue = new PriorityQueue<int>(underlyingHeap);

                queue.Enqueue(1); queue.Enqueue(2); queue.Enqueue(3);
                queue.TryDequeue(out _);

                Assert.Equal(2, underlyingHeap.Count);
            }

        }

        public class PeekTests
        {

            [Fact]
            public void Peek_Uses_Provided_Underlying_Queue()
            {
                var underlyingHeap = new BinaryHeap<int>();
                var queue = new PriorityQueue<int>(underlyingHeap);

                underlyingHeap.Push(1);

                Assert.Equal(1, queue.Peek());
            }

        }

        public class TryPeekTests
        {

            [Fact]
            public void TryPeek_Uses_Provided_Underlying_Queue()
            {
                var underlyingHeap = new BinaryHeap<int>();
                var queue = new PriorityQueue<int>(underlyingHeap);

                underlyingHeap.Push(1);

                queue.TryPeek(out var peekResult);
                Assert.Equal(1, peekResult);
            }

        }

        public class ClearTests
        {

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

}
