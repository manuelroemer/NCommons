using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NCommons.Collections;
using Xunit;
using static Xunit.Assert;
using Assert = NCommons.Tests.AssertEx;

namespace NCommons.Tests.Collections
{

    public sealed class PreviewingObservableCollectionTests
    {

        #region Initialization

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

        #endregion

        #region Raises CollectionChanging

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

        #endregion

        #region EventArgs Data

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

        #endregion

        private static RaisedEvent<NotifyCollectionChangedEventArgs> AssertRaisesCollectionChanging<T>(
            PreviewingObservableCollection<T> collection,
            Action testCode)
        {
            return Assert.Raises<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                ev => new NotifyCollectionChangedEventHandler(ev),
                handler => collection.CollectionChanging += handler,
                handler => collection.CollectionChanging -= handler,
                testCode
            );
        }

    }

}
