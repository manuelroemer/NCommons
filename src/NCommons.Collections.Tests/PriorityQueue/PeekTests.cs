namespace NCommons.Collections.Tests.PriorityQueue
{
    using Xunit;

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

}
