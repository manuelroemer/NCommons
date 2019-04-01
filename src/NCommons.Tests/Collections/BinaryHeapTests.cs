using System;
using System.Collections.Generic;
using NCommons.Collections;
using Xunit;

namespace NCommons.Tests.Collections
{

    public sealed class BinaryHeapTests : IPriorityQueueTests
    {

        protected override IPriorityQueue<T> CreateQueue<T>(IComparer<T>? comparer = null)
        {
            // The base class already covers a lot of tests for this class.
            // We must only provide instances, with and without a comparer.
            return new BinaryHeap<T>(comparer);
        }

        #region Capacity

        [Fact]
        public void Throws_If_Initializing_With_Negative_Capacity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new BinaryHeap<int>(capacity: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new BinaryHeap<int>(capacity: -1, comparer: null));
        }

        [Fact]
        public void Throws_If_Capacity_Is_Set_To_Negative_Value()
        {
            var heap = new BinaryHeap<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => heap.Capacity = -1);
        }

        [Fact]
        public void Throws_If_Capacity_Is_Set_To_Value_Less_Than_Count()
        {
            var heap = new BinaryHeap<int>();
            heap.Push(1);
            Assert.Throws<ArgumentOutOfRangeException>(() => heap.Capacity = 0);
        }

        [Fact]
        public void Automatically_Increases_Capacity_If_Not_Sufficient()
        {
            var heap = new BinaryHeap<int>(capacity: 0);
            heap.Push(1);
            Assert.True(heap.Capacity > 0);
        }

        #endregion

        #region IPriorityQueue Aliases

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
        
        #endregion

    }

}
