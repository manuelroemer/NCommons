namespace NCommons.Collections.Tests.PreviewingObservableCollection
{
    using System;
    using System.Collections.Specialized;
    using Xunit;

    public class BlockReentrancyTests : PreviewingObservableCollectionTestBase
    {

        [Fact]
        public void Add_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>();
            AssertBlocksReentrancy(
                collection,
                raiseCollectionChanging: () => collection.Add(0),
                testCode: () => collection.Add(123)
            );
        }

        [Fact]
        public void Insert_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>();
            AssertBlocksReentrancy(
                collection,
                raiseCollectionChanging: () => collection.Add(0),
                testCode: () => collection.Insert(0, 123)
            );
        }

        [Fact]
        public void Clear_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertBlocksReentrancy(
                collection,
                raiseCollectionChanging: () => collection.Add(0),
                testCode: () => collection.Clear()
            );
        }

        [Fact]
        public void Remove_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertBlocksReentrancy(
                collection,
                raiseCollectionChanging: () => collection.Add(0),
                testCode: () => collection.Remove(123)
            );
        }

        [Fact]
        public void RemoveAt_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertBlocksReentrancy(
                collection,
                raiseCollectionChanging: () => collection.Add(0),
                testCode: () => collection.RemoveAt(0)
            );
        }

        [Fact]
        public void Indexer_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertBlocksReentrancy(
                collection,
                raiseCollectionChanging: () => collection.Add(0),
                testCode: () => collection[0] = 456
            );
        }

        [Fact]
        public void Move_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123, 456 };
            AssertBlocksReentrancy(
                collection,
                raiseCollectionChanging: () => collection.Add(0),
                testCode: () => collection.Move(0, 1)
            );
        }

        private static void AssertBlocksReentrancy<T>(
            PreviewingObservableCollection<T> collection,
            Action raiseCollectionChanging,
            Action testCode)
        {
            // wasTestCodeCalled is necessary to prevent infinite loops, because an Add()
            // might, for example, trigger the event again.
            bool wasTestCodeCalled = false;
            void CollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (!wasTestCodeCalled)
                {
                    wasTestCodeCalled = true;
                    Assert.Throws<InvalidOperationException>(testCode);
                }
            }

            void Dummy_CollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
            {
                // Only exists so that there are two event listeners.
            }

            // Reentrancy means that the collection throws, if we try to modify it while handling
            // the CollectionChanging event.
            // There must be two event listeners, otherwise, the ObservableCollection<T> will not
            // throw.
            //
            // For testing this, we can somehow modify the collection and then, in the event, check
            // that it throws an IOE when executing testCode().
            try
            {
                collection.CollectionChanging += CollectionChanging;
                collection.CollectionChanging += Dummy_CollectionChanging;
                raiseCollectionChanging(); // Should raise the event.
            }
            finally
            {
                collection.CollectionChanging -= CollectionChanging;
                collection.CollectionChanging -= Dummy_CollectionChanging;
            }
        }

    }

}
