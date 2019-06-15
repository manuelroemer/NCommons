namespace NCommons.Collections.Tests.WeakReferenceCollection
{
    using Xunit;

    public class RemoveTests : WeakReferenceCollectionTestBase
    {

        [Fact]
        public void Remove_Removes_Null()
        {
            var collection = new WeakReferenceCollection<object>();
            collection.Add(null);

            collection.Remove(null);

            Assert.Empty(collection);
        }

        [Fact]
        public void Remove_Removes_Object()
        {
            var obj = new object();
            var collection = new WeakReferenceCollection<object>();
            collection.Add(obj);

            collection.Remove(obj);

            Assert.Empty(collection);
        }

        [Fact]
        public void Remove_Only_Removes_Single_Item()
        {
            var collection = new WeakReferenceCollection<object>();
            collection.Add(null);
            collection.Add(null);

            collection.Remove(null);

            Assert.Single(collection);
        }

        [Fact]
        public void Remove_Returns_True_If_Something_Was_Removed()
        {
            var collection = new WeakReferenceCollection<object>();
            collection.Add(null);
            Assert.True(collection.Remove(null));
        }

        [Fact]
        public void Remove_Returns_False_If_Nothing_Was_Removed()
        {
            var collection = new WeakReferenceCollection<object>();
            Assert.False(collection.Remove(null));
        }

    }

}
