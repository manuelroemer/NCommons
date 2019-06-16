namespace NCommons.Collections.Tests.WeakReferenceCollection
{
    using System.Linq;
    using Xunit;

    public class DeadReferenceClearingTests : WeakReferenceCollectionTestBase
    {

        [Fact]
        public void ClearDeadReferences_Removes_Dead_References()
        {
            var collection = new WeakReferenceCollection<object>();
            FillCollection(collection, collectableObjects: 5, aliveObjects: 0, nulls: 0);

            CollectGarbage();
            collection.ClearDeadReferences();

            Assert.Empty(collection);
        }

        [Fact]
        public void ClearDeadReferences_Keeps_Alive_References()
        {
            var collection = new WeakReferenceCollection<object>();
            FillCollection(collection, collectableObjects: 0, aliveObjects: 5, nulls: 0);

            CollectGarbage();
            collection.ClearDeadReferences();

            Assert.Equal(5, collection.Count());
        }

        [Fact]
        public void ClearDeadReferences_Keeps_Nulls()
        {
            var collection = new WeakReferenceCollection<object>();
            FillCollection(collection, collectableObjects: 0, aliveObjects: 0, nulls: 5);

            CollectGarbage();
            collection.ClearDeadReferences();

            Assert.Equal(5, collection.Count());
        }

        [Fact]
        public void Enumeration_Removes_Dead_References()
        {
            var collection = new WeakReferenceCollection<object>();
            FillCollection(collection, collectableObjects: 5, aliveObjects: 0, nulls: 0);

            CollectGarbage();
            foreach (var item in collection) { }

            Assert.Empty(collection);
        }

        [Fact]
        public void Enumeration_Keeps_Alive_References()
        {
            var collection = new WeakReferenceCollection<object>();
            FillCollection(collection, collectableObjects: 0, aliveObjects: 5, nulls: 0);

            CollectGarbage();
            foreach (var item in collection) { }

            Assert.Equal(5, collection.Count());
        }

        [Fact]
        public void Enumeration_Keeps_Nulls()
        {
            var collection = new WeakReferenceCollection<object>();
            FillCollection(collection, collectableObjects: 0, aliveObjects: 0, nulls: 5);

            CollectGarbage();
            foreach (var item in collection) { }

            Assert.Equal(5, collection.Count());
        }

    }

}
