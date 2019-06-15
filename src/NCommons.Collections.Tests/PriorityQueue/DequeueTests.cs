namespace NCommons.Collections.Tests.PriorityQueue
{
    using Xunit;

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

}
