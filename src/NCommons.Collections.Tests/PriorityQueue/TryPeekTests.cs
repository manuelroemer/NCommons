namespace NCommons.Collections.Tests.PriorityQueue
{
    using Xunit;

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

}
