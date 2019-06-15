namespace NCommons.Collections.Tests.PriorityQueue
{
    using Xunit;

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
