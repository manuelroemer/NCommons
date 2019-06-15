namespace NCommons.Collections.Tests.WeakReferenceCollection
{
    using Xunit;

    public class AddTests : WeakReferenceCollectionTestBase
    {

        [Fact]
        public void Can_Add_Objects()
        {
            var collection = new WeakReferenceCollection<object>();
            var obj = new object();
            collection.Add(obj);

            Assert.Contains(obj, collection);
        }

        [Fact]
        public void Can_Add_Null()
        {
            var collection = new WeakReferenceCollection<object>();
            collection.Add(null);
            Assert.Contains(null, collection);
        }

    }

}
