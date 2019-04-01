using System;
using System.Collections.Generic;
using System.Text;
using NCommons.Collections;

namespace NCommons.Tests.Collections
{

    public sealed class PriorityQueueTests : IPriorityQueueTests
    {

        protected override IPriorityQueue<T> CreateQueue<T>(IComparer<T>? comparer = null) =>
            new PriorityQueue<T>(comparer);

    }

}
