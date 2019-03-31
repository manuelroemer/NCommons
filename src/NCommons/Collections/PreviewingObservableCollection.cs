using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

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
