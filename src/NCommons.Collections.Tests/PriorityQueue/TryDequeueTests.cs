namespace NCommons.Collections.Tests.PriorityQueue
{
    using Xunit;

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

}
