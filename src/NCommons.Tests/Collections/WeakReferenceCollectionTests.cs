using System.Linq;
using NCommons.Collections;
using Xunit;

namespace NCommons.Tests.Collections
{

    public class WeakReferenceCollectionTests
    {

        [Fact]
        public void Can_Add_Items()
        {
            var collection = new WeakReferenceCollection<object>();
            var obj = new object();
            collection.Add(obj);

            Assert.Contains(obj, collection);
        }

        [Fact]
        public void Can_Remove_Items()
        {
            var collection = new WeakReferenceCollection<object>();
            var obj = new object();
            collection.Add(obj);
            collection.Remove(obj);

            Assert.DoesNotContain(obj, collection);
        }

        [Fact]
        public void Can_Clear_Collection()
        {
            var collection = new WeakReferenceCollection<object>();
            var objects = new object[]
            {
                new object(), new object(), new object()
            };

            foreach (var obj in objects)
            {
                collection.Add(obj);
            }
            collection.Clear();

            foreach (var obj in objects)
            {
                Assert.DoesNotContain(obj, collection);
            }
        }

        [Fact]
        public void Contains_Returns_Correct_Result()
        {
            var collection = new WeakReferenceCollection<object>();
            var obj = new object();
            collection.Add(obj);

            bool contains = collection.Contains(obj); // Store in variable to avoid Compiler warning.
            bool shouldNotContain = collection.Contains(new object());
            Assert.True(contains);
            Assert.False(shouldNotContain);
        }

        [Fact]
        public void Enumeration_Removes_Dead_References()
        {
            var collection = new WeakReferenceCollection<object>();
            var objects = new object?[]
            {
                new object(),
                new object(),
                null,
                null,
                new object()
            };

            foreach (var obj in objects)
            {
#nullable disable // We want to add nulls
                collection.Add(obj);
#nullable enable
            }

            // A normal enumeration is supposed to purge any dead reference from the list.
            // null simulates a dead-reference, so we expect a certain number to remain in
            // the list.
            // We can manually count this in a foreach loop.
            int count = 0;
            foreach (var item in collection)
            {
                count++;
            }

            int expectedCount = objects.Count(obj => obj != null);
            Assert.Equal(expectedCount, count);
        }
        
    }

}
