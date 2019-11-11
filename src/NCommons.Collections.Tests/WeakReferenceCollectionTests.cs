namespace NCommons.Collections.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public abstract class WeakReferenceCollectionTests
    {

        /// <summary>
        ///     Fills the specified collection with a set of alive objects, objects to be GC'ed
        ///     and with nulls.
        ///     It returns an int which represents the number of expected elements after every
        ///     element has been purged from the list.
        /// </summary>
        protected static int FillCollection(
            WeakReferenceCollection<object?> collection,
            int collectableObjects = 10,
            int aliveObjects = 10,
            int nulls = 5)
        {
            for (int i = 0; i < collectableObjects; i++)
            {
                collection.Add(new object());
            }

            for (int i = 0; i < aliveObjects; i++)
            {
                collection.Add(GetAliveObject(i));
            }

            for (int i = 0; i < nulls; i++)
            {
                collection.Add(null);
            }

            return aliveObjects + nulls;
        }

        protected static object GetAliveObject(int index)
        {
            if (s_aliveObjectHolder.Count > index)
            {
                return s_aliveObjectHolder[index];
            }
            else
            {
                // Add objects until we arrive at the index.
                while (s_aliveObjectHolder.Count <= index)
                {
                    s_aliveObjectHolder.Add(new object());
                }
                return s_aliveObjectHolder[index];
            }
        }

        // References objects, so that they don't get GC'ed. Should only be used with GetAliveObject.
        private static List<object> s_aliveObjectHolder = new List<object>();

        protected static void CollectGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public class EnumeratorTests : WeakReferenceCollectionTests
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
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(null);

                var enumerator = collection.GetEnumerator();
                collection.Remove(null);

                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }

            [Fact]
            public void Enumerator_Throws_If_Collection_Gets_Cleared_During_Enumeration()
            {
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(null);

                var enumerator = collection.GetEnumerator();
                collection.Clear();

                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }

            [Fact]
            public void Enumerator_MoveNext_Returns_True_If_Item_Is_Available()
            {
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(new object());

                Assert.True(collection.GetEnumerator().MoveNext());
            }

            [Fact]
            public void Enumerator_MoveNext_Returns_False_If_No_Item_Is_Available()
            {
                var collection = new WeakReferenceCollection<object?>();
                Assert.False(collection.GetEnumerator().MoveNext());
            }

            [Fact]
            public void Enumerator_Current_Cannot_Be_Garbage_Collected()
            {
                var collection = new WeakReferenceCollection<object?>();
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

        public class DeadReferenceClearingTests : WeakReferenceCollectionTests
        {

            [Fact]
            public void ClearDeadReferences_Removes_Dead_References()
            {
                var collection = new WeakReferenceCollection<object?>();
                FillCollection(collection, collectableObjects: 5, aliveObjects: 0, nulls: 0);

                CollectGarbage();
                collection.ClearDeadReferences();

                Assert.Empty(collection);
            }

            [Fact]
            public void ClearDeadReferences_Keeps_Alive_References()
            {
                var collection = new WeakReferenceCollection<object?>();
                FillCollection(collection, collectableObjects: 0, aliveObjects: 5, nulls: 0);

                CollectGarbage();
                collection.ClearDeadReferences();

                Assert.Equal(5, collection.Count());
            }

            [Fact]
            public void ClearDeadReferences_Keeps_Nulls()
            {
                var collection = new WeakReferenceCollection<object?>();
                FillCollection(collection, collectableObjects: 0, aliveObjects: 0, nulls: 5);

                CollectGarbage();
                collection.ClearDeadReferences();

                Assert.Equal(5, collection.Count());
            }

            [Fact]
            public void Enumeration_Removes_Dead_References()
            {
                var collection = new WeakReferenceCollection<object?>();
                FillCollection(collection, collectableObjects: 5, aliveObjects: 0, nulls: 0);

                CollectGarbage();
                foreach (var item in collection) { }

                Assert.Empty(collection);
            }

            [Fact]
            public void Enumeration_Keeps_Alive_References()
            {
                var collection = new WeakReferenceCollection<object?>();
                FillCollection(collection, collectableObjects: 0, aliveObjects: 5, nulls: 0);

                CollectGarbage();
                foreach (var item in collection) { }

                Assert.Equal(5, collection.Count());
            }

            [Fact]
            public void Enumeration_Keeps_Nulls()
            {
                var collection = new WeakReferenceCollection<object?>();
                FillCollection(collection, collectableObjects: 0, aliveObjects: 0, nulls: 5);

                CollectGarbage();
                foreach (var item in collection) { }

                Assert.Equal(5, collection.Count());
            }

        }

        public class AddTests : WeakReferenceCollectionTests
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
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(null);
                Assert.Contains(null, collection);
            }

        }

        public class RemoveTests : WeakReferenceCollectionTests
        {

            [Fact]
            public void Remove_Removes_Null()
            {
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(null);

                collection.Remove(null);

                Assert.Empty(collection);
            }

            [Fact]
            public void Remove_Removes_Object()
            {
                var obj = new object();
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(obj);

                collection.Remove(obj);

                Assert.Empty(collection);
            }

            [Fact]
            public void Remove_Only_Removes_Single_Item()
            {
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(null);
                collection.Add(null);

                collection.Remove(null);

                Assert.Single(collection);
            }

            [Fact]
            public void Remove_Returns_True_If_Something_Was_Removed()
            {
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(null);
                Assert.True(collection.Remove(null));
            }

            [Fact]
            public void Remove_Returns_False_If_Nothing_Was_Removed()
            {
                var collection = new WeakReferenceCollection<object?>();
                Assert.False(collection.Remove(null));
            }

        }

        public class ClearTests : WeakReferenceCollectionTests
        {

            [Fact]
            public void Clear_Removes_All_Items()
            {
                var collection = new WeakReferenceCollection<object?>();
                FillCollection(collection);

                collection.Clear();

                Assert.Empty(collection);
            }

        }

        public class ContainsTests : WeakReferenceCollectionTests
        {

            [Fact]
            public void Contains_Returns_True_For_Available_Null()
            {
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(null);

                Assert.True(collection.Contains(null));
            }

            [Fact]
            public void Contains_Returns_False_For_Unavailable_Null()
            {
                var collection = new WeakReferenceCollection<object?>();
                Assert.False(collection.Contains(null));
            }

            [Fact]
            public void Contains_Returns_True_For_Available_Object()
            {
                var obj = new object();
                var collection = new WeakReferenceCollection<object?>();
                collection.Add(obj);

                Assert.True(collection.Contains(obj));
            }

            [Fact]
            public void Contains_Returns_False_For_Unavailable_Object()
            {
                var obj = new object();
                var collection = new WeakReferenceCollection<object?>();
                Assert.False(collection.Contains(obj));
            }

        }

    }

}
