namespace NCommons.Collections.Tests.PriorityQueue
{
    using System.Collections.Generic;
    using Xunit;

    public class ConstructorTests
    {

        [Fact]
        public void Can_Pass_Null()
        {
            // Should not throw.
            new PriorityQueue<int>((IComparer<int>)null);
            new PriorityQueue<int>((IPriorityQueue<int>)null);
        }

    }

}
