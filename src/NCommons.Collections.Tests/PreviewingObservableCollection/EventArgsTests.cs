namespace NCommons.Collections.Tests.PreviewingObservableCollection
{
    using System.Collections.Specialized;
    using Xunit;

    public class EventArgsTests : PreviewingObservableCollectionTestBase
    {

        [Fact]
        public void Add_EventArgs_Contain_Correct_Data()
        {
            var objToAdd = new object();
            var collection = new PreviewingObservableCollection<object>()
            {
                new object(), // 1 item for StartingIndex asserts.
            };

            var evData = AssertRaisesCollectionChanging(collection, () => collection.Add(objToAdd));
            var args = evData.Arguments;


            Assert.NotNull(args);
            Assert.Equal(NotifyCollectionChangedAction.Add, args.Action);

            Assert.Null(args.OldItems);
            Assert.Equal(-1, args.OldStartingIndex);

            Assert.Equal(1, args.NewItems.Count);
            Assert.Equal(1, args.NewStartingIndex);

            Assert.Same(objToAdd, args.NewItems[0]);
        }

        [Fact]
        public void Insert_EventArgs_Contain_Correct_Data()
        {
            var objToInsert = new object();
            var collection = new PreviewingObservableCollection<object>()
            {
                new object(),
                new object(), // 2 items so that we can insert in between.
            };

            var evData = AssertRaisesCollectionChanging(collection, () => collection.Insert(1, objToInsert));
            var args = evData.Arguments;


            Assert.NotNull(args);
            Assert.Equal(NotifyCollectionChangedAction.Add, args.Action);

            Assert.Null(args.OldItems);
            Assert.Equal(-1, args.OldStartingIndex);

            Assert.Equal(1, args.NewItems.Count);
            Assert.Equal(1, args.NewStartingIndex);

            Assert.Same(objToInsert, args.NewItems[0]);
        }

        [Fact]
        public void Clear_EventArgs_Contain_Correct_Data()
        {
            var collection = new PreviewingObservableCollection<object>()
            {
                new object(), // 2 items for multiple items
                new object(),
            };

            var evData = AssertRaisesCollectionChanging(collection, () => collection.Clear());
            var args = evData.Arguments;


            // We want to mirror the NotifyCollectionChanged event for Clears.
            // This means that no Old/NewItems get passed to the event args.
            // The only indicator that the collection gets cleared is the Reset flag.
            Assert.NotNull(args);
            Assert.Equal(NotifyCollectionChangedAction.Reset, args.Action);

            Assert.Null(args.OldItems);
            Assert.Equal(-1, args.OldStartingIndex);

            Assert.Null(args.NewItems);
            Assert.Equal(-1, args.NewStartingIndex);
        }

        [Fact]
        public void Remove_EventArgs_Contain_Correct_Data()
        {
            var objToRemove = new object();
            var collection = new PreviewingObservableCollection<object>()
            {
                new object(), // 2 items for StartingIndex asserts.
                objToRemove,
                new object(),
            };

            var evData = AssertRaisesCollectionChanging(collection, () => collection.Remove(objToRemove));
            var args = evData.Arguments;


            Assert.NotNull(args);
            Assert.Equal(NotifyCollectionChangedAction.Remove, args.Action);

            Assert.Equal(1, args.OldItems.Count);
            Assert.Equal(1, args.OldStartingIndex);

            Assert.Null(args.NewItems);
            Assert.Equal(-1, args.NewStartingIndex);

            Assert.Same(objToRemove, args.OldItems[0]);
        }

        [Fact]
        public void RemoveAt_EventArgs_Contain_Correct_Data()
        {
            var objToRemove = new object();
            var collection = new PreviewingObservableCollection<object>()
            {
                new object(), // 3 items for StartingIndex asserts.
                objToRemove,
                new object(),
            };

            var evData = AssertRaisesCollectionChanging(collection, () => collection.RemoveAt(1));
            var args = evData.Arguments;


            Assert.NotNull(args);
            Assert.Equal(NotifyCollectionChangedAction.Remove, args.Action);

            Assert.Equal(1, args.OldItems.Count);
            Assert.Equal(1, args.OldStartingIndex);

            Assert.Null(args.NewItems);
            Assert.Equal(-1, args.NewStartingIndex);

            Assert.Same(objToRemove, args.OldItems[0]);
        }

        [Fact]
        public void Indexer_EventArgs_Contain_Correct_Data()
        {
            var objToBeReplaced = new object();
            var replacingObj = new object();
            var collection = new PreviewingObservableCollection<object>()
            {
                new object(), // 3 items for StartingIndex asserts.
                objToBeReplaced,
                new object(),
            };

            var evData = AssertRaisesCollectionChanging(collection, () => collection[1] = replacingObj);
            var args = evData.Arguments;


            Assert.NotNull(args);
            Assert.Equal(NotifyCollectionChangedAction.Replace, args.Action);

            Assert.Equal(1, args.OldItems.Count);
            Assert.Equal(1, args.OldStartingIndex);

            Assert.Equal(1, args.NewItems.Count);
            Assert.Equal(1, args.NewStartingIndex);

            Assert.Same(objToBeReplaced, args.OldItems[0]);
            Assert.Same(replacingObj, args.NewItems[0]);
        }

        [Fact]
        public void Move_EventArgs_Contain_Correct_Data()
        {
            var toBeMoved = new object();
            var willBeMoved = new object(); // The item that will switch places with toBeMoved.
            var collection = new PreviewingObservableCollection<object>()
            {
                new object(), // 3 items for StartingIndex asserts.
                toBeMoved,
                willBeMoved,
            };

            var evData = AssertRaisesCollectionChanging(collection, () => collection.Move(1, 2));
            var args = evData.Arguments;


            Assert.NotNull(args);
            Assert.Equal(NotifyCollectionChangedAction.Move, args.Action);

            Assert.Equal(1, args.OldItems.Count);
            Assert.Equal(1, args.OldStartingIndex);

            Assert.Equal(1, args.NewItems.Count);
            Assert.Equal(2, args.NewStartingIndex);

            // Move should put the item that appeared at the index both in OldItems and NewItems.
            // A little unintuitive, but that comes from the framework. Stay consistent with that.
            Assert.Same(toBeMoved, args.OldItems[0]);
            Assert.Same(toBeMoved, args.NewItems[0]);
        }

    }

}
