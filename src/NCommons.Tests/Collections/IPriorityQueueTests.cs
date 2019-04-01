using System;
using System.Collections.Generic;
using System.Linq;
using NCommons.Collections;
using Xunit;

namespace NCommons.Tests.Collections
{

    /// <summary>
    ///     Base tests which any <see cref="IPriorityQueue{T}"/> implementer should pass.
    /// </summary>
    public abstract class IPriorityQueueTests
    {

        /// <summary>Creates a new instance of the priority queue type under test.</summary>
        protected abstract IPriorityQueue<T> CreateQueue<T>(IComparer<T>? comparer = null);

        public static TheoryData<IEnumerable<int>> NumberData => new TheoryData<IEnumerable<int>>()
        {
            Enumerable.Range(0, 100),
            Enumerable.Range(0, 100).Concat(Enumerable.Range(0, 1000)),
            Enumerable.Range(-100_000, 200_000).Where(i => i % 2 != 0),
            Enumerable.Range(-100_000, 200_000).Where(i => i % 2 == 0),
            Enumerable.Range(0, 100).Concat(Enumerable.Range(0, 100)).Concat(Enumerable.Range(0, 100)),
        };

        #region Comparer

        [Fact]
        public void Uses_Specified_Comparer()
        {
            var myComparer = Comparer<object>.Create((o1, o2) => 0);
            var queue = CreateQueue(myComparer);
            Assert.Same(myComparer, queue.Comparer);
        }

        [Fact]
        public void Uses_Default_Comparer_By_Default()
        {
            var queue = CreateQueue<int>(comparer: null);
            Assert.Same(Comparer<int>.Default, queue.Comparer);
        }

        #endregion

        #region Count

        [Fact]
        public void Count_Starts_At_Zero()
        {
            var queue = CreateQueue<int>();
            Assert.Equal(0, queue.Count);
        }

        [Fact]
        public void Count_Increases_With_Enqueues()
        {
            const int itemsToEnqueue = 100;
            var queue = CreateQueue<int>();

            for (int i = 0; i < itemsToEnqueue; i++)
            {
                queue.Enqueue(i);
            }

            Assert.Equal(itemsToEnqueue, queue.Count);
        }

        [Fact]
        public void Count_Decreases_With_Dequeues()
        {
            const int itemsToEnqueue = 100;
            const int itemsToDequeue = 13;
            var queue = CreateQueue<int>();

            for (int i = 0; i < itemsToEnqueue; i++)
            {
                queue.Enqueue(i);
            }

            for (int i = 0; i < itemsToDequeue; i++)
            {
                queue.Dequeue();
            }
            
            Assert.Equal(itemsToEnqueue - itemsToDequeue, queue.Count);
        }

        #endregion

        #region Prioritized Enqueue / Dequeue

        [Theory]
        [MemberData(nameof(NumberData))]
        public void Items_Are_Correctly_Prioritized(IEnumerable<int> itemsToEnqueue)
        {
            var queue = CreateQueue<int>();

            foreach (var item in itemsToEnqueue)
            {
                queue.Enqueue(item);
            }

            AssertItemPriorityOrder(queue);
        }

        [Theory]
        [MemberData(nameof(NumberData))]
        public void Correctly_Prioritizes_Items_With_Custom_Comparer(IEnumerable<int> itemsToEnqueue)
        {
            var comparer = Comparer<int>.Create((a, b) =>
            {
                if (a == b)
                    return 0;
                
                // Just make up some non-normal rules now.
                if (a == 13)
                    return 1;

                if (a == 7)
                    return 1;

                // Reverse ordering now. Smaller values are prioritized more highly.
                if (a < b)
                    return 1;
                return -1;
            });
            var queue = CreateQueue<int>(comparer);

            foreach (var item in itemsToEnqueue)
            {
                queue.Enqueue(item);
            }

            AssertItemPriorityOrder(queue, comparer);
        }

        /// <summary>
        ///     Ensures that each item in the queue fulfills the priority constraint given
        ///     by the comparer.
        ///     This method has the side-effect of de-queueing every element in the queue.
        ///     Call it as the last assert, as it will modify the queue.
        /// </summary>
        private static void AssertItemPriorityOrder<T>(IPriorityQueue<T> queue, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;

            // Basically just go through the whole queue and ensure that each item which gets
            // dequeued is >= the item after it (priority-wise).
            if (queue.Count >= 2)
            {
                T previousItem;
                T currentItem = queue.Dequeue();

                do
                {
                    previousItem = currentItem;
                    currentItem = queue.Dequeue();

                    var cmpRes = comparer.Compare(previousItem, currentItem);
                    Assert.True(cmpRes >= 0);
                } while (queue.Count > 0);
            }
        }

        #endregion

        #region Peek / TryPeek

        [Fact]
        public void Peek_Doesnt_Remove_Item()
        {
            var queue = CreateQueue<int>();
            queue.Enqueue(1);
            queue.Peek();
            Assert.Equal(1, queue.Count);
        }
        
        [Fact]
        public void TryPeek_Doesnt_Remove_Item()
        {
            var queue = CreateQueue<int>();
            queue.Enqueue(1);
            queue.TryPeek(out _);
            Assert.Equal(1, queue.Count);
        }

        [Theory]
        [MemberData(nameof(NumberData))]
        public void Peek_Returns_Item_With_Highest_Priority(IEnumerable<int> itemsToEnqueue)
        {
            var queue = CreateQueue<int>();
            int? currentHighest = null;

            foreach (var item in itemsToEnqueue)
            {
                queue.Enqueue(item);

                if (currentHighest is null || currentHighest < item)
                    currentHighest = item;

                Assert.Equal(currentHighest, queue.Peek());
            }
        }
        
        [Theory]
        [MemberData(nameof(NumberData))]
        public void TryPeek_Returns_Item_With_Highest_Priority(IEnumerable<int> itemsToEnqueue)
        {
            var queue = CreateQueue<int>();
            int? currentHighest = null;

            foreach (var item in itemsToEnqueue)
            {
                queue.Enqueue(item);

                if (currentHighest is null || currentHighest < item)
                    currentHighest = item;

                queue.TryPeek(out var peekResult);
                Assert.Equal(currentHighest, peekResult);
            }
        }

        [Fact]
        public void Peek_Throws_If_Empty()
        {
            var queue = CreateQueue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        [Fact]
        public void TryPeek_Returns_True_If_Not_Empty()
        {
            var queue = CreateQueue<int>();
            queue.Enqueue(1);
            Assert.True(queue.TryPeek(out _));
        }
        
        [Fact]
        public void TryPeek_Returns_False_If_Empty()
        {
            var queue = CreateQueue<int>();
            Assert.False(queue.TryPeek(out _));
        }

        #endregion

        #region Dequeue / TryDequeue

        [Fact]
        public void Dequeue_Throws_If_Empty()
        {
            var queue = CreateQueue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Fact]
        public void TryDequeue_Returns_True_If_Not_Empty()
        {
            var queue = CreateQueue<int>();
            queue.Enqueue(1);
            Assert.True(queue.TryDequeue(out _));
        }

        [Fact]
        public void TryDequeue_Returns_False_If_Empty()
        {
            var queue = CreateQueue<int>();
            Assert.False(queue.TryDequeue(out _));
        }

        [Theory]
        [MemberData(nameof(NumberData))]
        public void Dequeue_Returns_Item_With_Highest_Priority(IEnumerable<int> itemsToEnqueue)
        {
            var queue = CreateQueue<int>();
            int? currentHighest = null;

            foreach (var item in itemsToEnqueue)
            {
                queue.Enqueue(item);

                if (currentHighest is null || currentHighest < item)
                    currentHighest = item;

                var dequeueResult = queue.Dequeue();
                queue.Enqueue(dequeueResult);
                Assert.Equal(currentHighest, dequeueResult);
            }
        }
        
        [Theory]
        [MemberData(nameof(NumberData))]
        public void TryDequeue_Returns_Item_With_Highest_Priority(IEnumerable<int> itemsToEnqueue)
        {
            var queue = CreateQueue<int>();
            int? currentHighest = null;

            foreach (var item in itemsToEnqueue)
            {
                queue.Enqueue(item);

                if (currentHighest is null || currentHighest < item)
                    currentHighest = item;

                queue.TryDequeue(out var dequeueResult);
                queue.Enqueue(currentHighest.Value);
                Assert.Equal(currentHighest, dequeueResult);
            }
        }

        #endregion

        #region Clear

        [Fact]
        public void Clear_Removes_All_Items()
        {
            var queue = CreateQueue<int>();

            queue.Enqueue(1);
            queue.Clear();

            Assert.Empty(queue);
            Assert.Equal(0, queue.Count);
        }

        #endregion

        #region Enumeration

        [Fact]
        public void Enumerates_Over_All_Elements()
        {
            // IPriorityQueue does not specify any conditions about the order in which the elements
            // are returned during enumeration. They must merely all be in the queue.
            //
            // Don't use NumberSource, because otherwise Contains() takes too much time.
            var items = Enumerable.Range(0, 1234).ToList();
            var queue = CreateQueue<int>();

            foreach (var item in items)
            {
                queue.Enqueue(item);
            }

            foreach (var item in queue)
            {
                Assert.Contains(item, items);
            }
        }

        #endregion

    }



}
