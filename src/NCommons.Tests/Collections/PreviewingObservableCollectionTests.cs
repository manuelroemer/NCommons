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

        #endregion

        #region BlockReentrancy in CollectionChanging handlers

        [Fact]
        public void Add_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>();
            AssertBlocksReentrancy(
                collection, 
                raiseCollectionChanging: () => collection.Add(0),
                testCode:                () => collection.Add(123)
            );
        }
        
        [Fact]
        public void Insert_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>();
            AssertBlocksReentrancy(
                collection, 
                raiseCollectionChanging: () => collection.Add(0),
                testCode:                () => collection.Insert(0, 123)
            );
        }
        
        [Fact]
        public void Clear_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertBlocksReentrancy(
                collection, 
                raiseCollectionChanging: () => collection.Add(0),
                testCode:                () => collection.Clear()
            );
        }
        
        [Fact]
        public void Remove_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertBlocksReentrancy(
                collection, 
                raiseCollectionChanging: () => collection.Add(0),
                testCode:                () => collection.Remove(123)
            );
        }
        
        [Fact]
        public void RemoveAt_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertBlocksReentrancy(
                collection, 
                raiseCollectionChanging: () => collection.Add(0),
                testCode:                () => collection.RemoveAt(0)
            );
        }
        
        [Fact]
        public void Indexer_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123 };
            AssertBlocksReentrancy(
                collection, 
                raiseCollectionChanging: () => collection.Add(0),
                testCode:                () => collection[0] = 456
            );
        }
        
        [Fact]
        public void Move_Blocks_Reentrancy_During_CollectionChanging()
        {
            var collection = new PreviewingObservableCollection<int>() { 123, 456 };
            AssertBlocksReentrancy(
                collection, 
                raiseCollectionChanging: () => collection.Add(0),
                testCode:                () => collection.Move(0, 1)
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

        #endregion

        private static RaisedEvent<NotifyCollectionChangedEventArgs> AssertRaisesCollectionChanging<T>(
            PreviewingObservableCollection<T> collection,
            Action testCode)
        {
            return Assert.Raises<NotifyCollectionChangingEventHandler, NotifyCollectionChangedEventArgs>(
                ev => new NotifyCollectionChangingEventHandler(ev),
                handler => collection.CollectionChanging += handler,
                handler => collection.CollectionChanging -= handler,
                testCode
            );
        }

    }

}
