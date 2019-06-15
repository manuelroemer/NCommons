namespace NCommons.Collections.Tests.WeakReferenceCollection
{
    using System;
    using Xunit;

    public class EnumeratorTests : WeakReferenceCollectionTestBase
    {

        [Fact]
        public void Enumerator_Throws_If_Not_Started()
        {
            var enumerator = new WeakReferenceCollection<object>().GetEnumerator();
            Assert.Throws<InvalidOperationException>(() => enumerator.Current);
        }

        [Fact]
        public void Enumerator_Throws_If_Finished()
        {
            var enumerator = new WeakReferenceCollection<object>().GetEnumerator();
            enumerator.MoveNext();
            Assert.Throws<InvalidOperationException>(() => enumerator.Current);
        }

        [Fact]
        public void Enumerator_Throws_If_Item_Gets_Added_During_Enumeration()
        {
            var collection = new WeakReferenceCollection<object>();
            var enumerator = collection.GetEnumerator();
            collection.Add(new object());
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
        }

        [Fact]
        public void Enumerator_Throws_If_Item_Gets_Removed_During_Enumeration()
        {
            var collection = new WeakReferenceCollection<object>();
            collection.Add(null);

            var enumerator = collection.GetEnumerator();
            collection.Remove(null);

            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
        }

        [Fact]
        public void Enumerator_Throws_If_Collection_Gets_Cleared_During_Enumeration()
        {
            var collection = new WeakReferenceCollection<object>();
            collection.Add(null);

            var enumerator = collection.GetEnumerator();
            collection.Clear();

            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
        }

        [Fact]
        public void Enumerator_MoveNext_Returns_True_If_Item_Is_Available()
        {
            var collection = new WeakReferenceCollection<object>();
            collection.Add(new object());

            Assert.True(collection.GetEnumerator().MoveNext());
        }

        [Fact]
        public void Enumerator_MoveNext_Returns_False_If_No_Item_Is_Available()
        {
            var collection = new WeakReferenceCollection<object>();
            Assert.False(collection.GetEnumerator().MoveNext());
        }

        [Fact]
        public void Enumerator_Current_Cannot_Be_Garbage_Collected()
        {
            var collection = new WeakReferenceCollection<object>();
            FillCollection(collection, collectableObjects: 1, aliveObjects: 0, nulls: 0);

            var enumerator = collection.GetEnumerator();

            // There is a change that the obj gets collected automatically. Check that.
            if (enumerator.MoveNext())
            {
                CollectGarbage();
                Assert.NotNull(enumerator.Current); // Always keep this check, otherwise, enumerator may be GCed too.
                Assert.Single(collection);
            }
        }

    }

}
