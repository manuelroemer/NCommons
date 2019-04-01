using System;
using System.Collections.Generic;
using System.Text;
using NCommons.Collections;

namespace NCommons.Tests.Collections
{

    public sealed class BinaryHeapTests : IPriorityQueueTests
    {

        protected override IPriorityQueue<T> CreateQueue<T>(IComparer<T>? comparer = null) =>
            new BinaryHeap<T>(comparer);

    }

}
