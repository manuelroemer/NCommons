namespace NCommons.Collections.Tests.PreviewingObservableCollection
{
    using System.Collections.Generic;
    using Xunit;

    public class ConstructorTests
    {

        [Fact]
        public void Copies_From_List_During_Initialization()
        {
            var lst = new List<object>() { new object(), new object(), new object() };
            var collection = new PreviewingObservableCollection<object>(lst);

            Assert.Equal(lst, collection);
        }

        [Fact]
        public void Copies_From_Enumerable_During_Initialization()
        {
            var collection = new PreviewingObservableCollection<int>(Items());
            Assert.Equal(Items(), collection);

            IEnumerable<int> Items()
            {
                for (int i = 0; i < 3; i++) yield return i;
            }
        }

        [Fact]
        public void Doesnt_Reuse_Same_Collection_During_Initialization()
        {
            var items = new List<object>() { new object(), new object(), new object() };

            // Just test two ctors at once, the implementation calls a .NET ctor anyways.
            var col1 = new PreviewingObservableCollection<object>(items);
            var col2 = new PreviewingObservableCollection<object>((IEnumerable<object>)items);

            items.Clear();

            Assert.NotEmpty(col1);
            Assert.NotEmpty(col2);
        }

    }

}
