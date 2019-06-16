namespace NCommons.Collections.Tests.BinaryHeap
{
    using System;
    using Xunit;

    public class CapacityTests
    {

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

    }

}
