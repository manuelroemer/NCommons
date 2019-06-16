namespace NCommons.Collections.Tests.PriorityQueue
{
    using Xunit;


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

}
