namespace NCommons.Collections.Tests.WeakReferenceCollection
{
    using Xunit;

    public class ContainsTests : WeakReferenceCollectionTestBase
    {

        [Fact]
        public void Contains_Returns_True_For_Available_Null()
        {
            var collection = new WeakReferenceCollection<object>();
            collection.Add(null);

            Assert.True(collection.Contains(null));
        }

        [Fact]
        public void Contains_Returns_False_For_Unavailable_Null()
        {
            var collection = new WeakReferenceCollection<object>();
            Assert.False(collection.Contains(null));
        }

        [Fact]
        public void Contains_Returns_True_For_Available_Object()
        {
            var obj = new object();
            var collection = new WeakReferenceCollection<object>();
            collection.Add(obj);

            Assert.True(collection.Contains(obj));
        }

        [Fact]
        public void Contains_Returns_False_For_Unavailable_Object()
        {
            var obj = new object();
            var collection = new WeakReferenceCollection<object>();
            Assert.False(collection.Contains(obj));
        }

    }

}
