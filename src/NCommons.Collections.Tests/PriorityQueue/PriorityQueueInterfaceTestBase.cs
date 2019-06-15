namespace NCommons.Collections.Tests.PriorityQueue
{
    using System.Collections.Generic;
    using NCommons.Collections.Tests.IPriorityQueue;

    public class PriorityQueueInterfaceTests : PriorityQueueInterfaceTestBase
    {

        protected override IPriorityQueue<T> CreateQueue<T>(IComparer<T> comparer = null)
        {
            // The base class already covers a lot of tests for this class.
            // We must only provide instances, with and without a comparer.
            //
            // This covers the cases where PriorityQueue<T> uses a default underlying queue.
            // This doesn't have to be tested in this class.
            return new PriorityQueue<T>(comparer);
        }

    }

}
