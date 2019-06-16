namespace NCommons.Collections.Tests.WeakReferenceCollection
{
    using Xunit;

    public class ClearTests : WeakReferenceCollectionTestBase
    {

        [Fact]
        public void Clear_Removes_All_Items()
        {
            var collection = new WeakReferenceCollection<object>();
            FillCollection(collection);

            collection.Clear();

            Assert.Empty(collection);
        }

    }

}
