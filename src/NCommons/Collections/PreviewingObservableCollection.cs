using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using NCommons.Collections.Resources;

namespace NCommons.Collections
{

#pragma warning disable CA1001 // _reentrancyMonitor doesn't have to be disposed

    /// <summary>
    ///     A dynamic data collection which provides notifications about when items are
    ///     going to change and when they have actually changed.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class PreviewingObservableCollection<T> :
        ObservableCollection<T>, INotifyCollectionChanging
    {

        /// <summary>
        ///     Occurs when the collection is about to change, but before the change has actually
        ///     happened.
        /// </summary>
        public event NotifyCollectionChangingEventHandler CollectionChanging;

        private readonly ReentrancyMonitor _reentrancyMonitor = new ReentrancyMonitor();

        /// <summary>
        ///     Initializes a new instance of the <see cref="PreviewingObservableCollection{T}"/> class.
        /// </summary>
        public PreviewingObservableCollection() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PreviewingObservableCollection{T}"/>
        ///     class that contains elements copied from the specified <paramref name="collection"/>.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="collection"/>
        /// </exception>
        public PreviewingObservableCollection(IEnumerable<T> collection)
            : base(collection) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PreviewingObservableCollection{T}"/> 
        ///     class that contains elements copied from the specified <paramref name="list"/>.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="list"/>
        /// </exception>
        public PreviewingObservableCollection(List<T> list)
            : base(list) { }



        //
        // Note: In the following, calls to BlockCollectionChangingReentrancy are made.
        //       Calling this method has the same effect as BlockReentrancy which, for perf.
        //       reasons, does not have to be called by us.
        //

        /// <summary>
        ///     Inserts the <paramref name="item"/> into the collection at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index where to insert the item.</param>
        /// <param name="item">The item to be inserted.</param>
        protected override void InsertItem(int index, T item)
        {
            CheckCollectionChangingReentrancy();
            using (BlockCollectionChangingReentrancy())
            {
                OnCollectionChanging(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    item,
                    index
                ));
                base.InsertItem(index, item);
            }
        }

        /// <summary>
        ///     Removes all items from the collection.
        /// </summary>
        protected override void ClearItems()
        {
            CheckCollectionChangingReentrancy();
            using (BlockCollectionChangingReentrancy())
            {
                OnCollectionChanging(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset
                ));
                base.ClearItems();
            }
        }

        /// <summary>
        ///     Removes the item at the specified <paramref name="index"/> of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the item to be removed.</param>
        protected override void RemoveItem(int index)
        {
            CheckCollectionChangingReentrancy();
            using (BlockCollectionChangingReentrancy())
            {
                OnCollectionChanging(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    this[index],
                    index
                ));
                base.RemoveItem(index); 
            }
        }

        /// <summary>
        ///     Replaces the item at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the item to be replaced.</param>
        /// <param name="item">The item with which to replace the old item.</param>
        protected override void SetItem(int index, T item)
        {
            CheckCollectionChangingReentrancy();
            using (BlockCollectionChangingReentrancy())
            {
                OnCollectionChanging(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    newItem: item,
                    oldItem: this[index],
                    index:   index
                ));
                base.SetItem(index, item);
            }
        }

        /// <summary>
        ///     Moves the item at the specified index to a new location in the collection.
        /// </summary>
        /// <param name="oldIndex">The zero-based index of the item to be moved.</param>
        /// <param name="newIndex">The zero-based index to which the item should be moved.</param>
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            CheckCollectionChangingReentrancy();
            using (BlockCollectionChangingReentrancy())
            {
                OnCollectionChanging(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move,
                    this[oldIndex],
                    newIndex,
                    oldIndex
                ));
                base.MoveItem(oldIndex, newIndex); 
            }
        }

        /// <summary>
        ///     Raises the <see cref="INotifyCollectionChanging"/> event with the provided
        ///     event args.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnCollectionChanging(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanging?.Invoke(this, e);
        }

        /// <summary>
        ///     Ensures that any action which modifies this collection throws an
        ///     <see cref="InvalidOperationException"/> while the returned 
        ///     <see cref="IDisposable"/> has not been disposed.
        ///     
        ///     See remarks for details.
        /// </summary>
        /// <returns>
        ///     An <see cref="IDisposable"/> which is used to scope the lifetime of the
        ///     reentrancy block.
        /// </returns>
        /// <remarks>
        ///     The only exception to this rule is if <see cref="CollectionChanging"/> has 1 or less
        ///     subscribers.
        ///     
        ///     This method has the same functionality as
        ///     <see cref="ObservableCollection{T}.BlockReentrancy"/>, but only for the
        ///     <see cref="CollectionChanging"/> event.
        ///     
        ///     Deriving classes must call <see cref="CheckCollectionChangingReentrancy"/> for 
        ///     throwing the exception.
        /// </remarks>
        /// <see cref="CheckCollectionChangingReentrancy"/>
        /// <seealso cref="ObservableCollection{T}.BlockReentrancy"/>
        protected IDisposable BlockCollectionChangingReentrancy()
        {
            _reentrancyMonitor.Enter();
            return _reentrancyMonitor;
        }

        /// <summary>
        ///     Throws an <see cref="InvalidOperationException"/> if the conditions of
        ///     <see cref="BlockCollectionChangingReentrancy"/> are met.
        /// </summary>
        /// <seealso cref="BlockCollectionChangingReentrancy"/>
        /// <seealso cref="ObservableCollection{T}.CheckReentrancy"/>
        protected void CheckCollectionChangingReentrancy()
        {
            if (_reentrancyMonitor.IsBusy &&
                CollectionChanging != null &&
                CollectionChanging.GetInvocationList().Length > 1)
            {
                throw new InvalidOperationException(
                    ExceptionStrings.PreviewingObservableCollection_CollectionChangingReentrancy
                );
            }
        }

        /// <summary>
        ///     Used for reentrancy checks. Follows the implementation of the
        ///     ObservableCollection{T}. See the reference source for details.
        /// </summary>
        private class ReentrancyMonitor : IDisposable
        {
            private int _busyCount;
            public bool IsBusy => _busyCount > 0;
            public void Enter() => _busyCount++;
            public void Dispose() => _busyCount--;
        }

    }

#pragma warning restore CA1001 // _reentrancyMonitor doesn'T have to be disposed

}
