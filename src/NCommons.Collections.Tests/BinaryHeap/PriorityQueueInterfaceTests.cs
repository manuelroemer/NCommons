namespace NCommons.Collections.Tests.BinaryHeap
{
    using System.Collections.Generic;
    using NCommons.Collections.Tests.IPriorityQueue;

    public class PriorityQueueInterfaceTests : PriorityQueueInterfaceTestBase
    {

        // BinaryHeap implements IPriorityQueue.
        // As such, it should pass the predefined tests.

        protected override IPriorityQueue<T> CreateQueue<T>(IComparer<T> comparer = null)
        {
            return new BinaryHeap<T>(comparer);
        }

    }

}
