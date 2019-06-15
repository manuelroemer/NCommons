namespace NCommons.Collections.Tests.BinaryHeap
{
    using System;
    using Xunit;

    public class PriorityQueueMethodAliases
    {

        [Fact]
        public void Push_Does_Enqueue()
        {
            var heap = new BinaryHeap<int>();
            heap.Push(1);
            Assert.Equal(1, heap.Peek());
        }

        [Fact]
        public void Pop_Throws_If_Empty()
        {
            var heap = new BinaryHeap<int>();
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
        }

        [Fact]
        public void Pop_Dequeues()
        {
            var heap = new BinaryHeap<int>();
            heap.Push(1);
            Assert.Equal(1, heap.Pop());
            Assert.Empty(heap);
        }

        [Fact]
        public void TryPop_Dequeues()
        {
            var heap = new BinaryHeap<int>();
            heap.Push(1);
            heap.TryPop(out var popResult);

            Assert.Equal(1, popResult);
            Assert.Empty(heap);
        }

        [Fact]
        public void TryPop_Doesnt_Remove_Item()
        {
            var heap = new BinaryHeap<int>();
            heap.Push(1);
            heap.TryPeek(out _);
            Assert.Equal(1, heap.Count);
        }

        [Fact]
        public void TryPop_Returns_True_If_Not_Empty()
        {
            var heap = new BinaryHeap<int>();
            heap.Push(1);
            Assert.True(heap.TryPeek(out _));
        }

        [Fact]
        public void TryPop_Returns_False_If_Empty()
        {
            var heap = new BinaryHeap<int>();
            Assert.False(heap.TryPeek(out _));
        }

    }

}
