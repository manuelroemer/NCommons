namespace NCommons.Collections.Tests.PreviewingObservableCollection
{
    using Xunit;

    public class CollectionChangingTests : PreviewingObservableCollectionTestBase
    {

        [Fact]
        public void Add_Raises_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>();
            AssertRaisesCollectionChanging(collection, () => collection.Add(123));
        }

        [Fact]
        public void Insert_Raises_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>();
            AssertRaisesCollectionChanging(collection, () => collection.Insert(0, 123));
        }

        [Fact]
        public void Clear_Raises_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>();
            AssertRaisesCollectionChanging(collection, () => collection.Clear());
        }

        [Fact]
        public void Remove_Raises_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertRaisesCollectionChanging(collection, () => collection.Remove(123));
        }

        [Fact]
        public void RemoveAt_Raises_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertRaisesCollectionChanging(collection, () => collection.RemoveAt(0));
        }

        [Fact]
        public void Indexer_Raises_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertRaisesCollectionChanging(collection, () => collection[0] = 456);
        }

        [Fact]
        public void Move_Raises_Collection_Changing()
        {
            var collection = new PreviewingObservableCollection<int>() { 123, 456 };
            AssertRaisesCollectionChanging(collection, () => collection.Move(0, 1));
        }

    }

}
