using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NCommons.Collections
{

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
        public event NotifyCollectionChangedEventHandler CollectionChanging;

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

        /// <summary>
        ///     Inserts the <paramref name="item"/> into the collection at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index where to insert the item.</param>
        /// <param name="item">The item to be inserted.</param>
        protected override void InsertItem(int index, T item)
        {
            using (BlockReentrancy())
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
            using (BlockReentrancy())
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
            using (BlockReentrancy())
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
            using (BlockReentrancy())
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
            using (BlockReentrancy())
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

    }

}
