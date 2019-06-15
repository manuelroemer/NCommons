namespace NCommons.Collections
{
    using System.Collections.Specialized;
    
    /// <summary>
    ///     Represents the method that handles the <see cref="INotifyCollectionChanging.CollectionChanging"/>
    ///     event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Information about the event.</param>
    public delegate void NotifyCollectionChangingEventHandler(object sender, NotifyCollectionChangedEventArgs e);

    /// <summary>
    ///     Notifies listeners about when a collection is about to change, but before the change
    ///     is actually going to happen.
    /// </summary>
    /// <remarks>
    ///     This interface can be treated as the counterpart of the 
    ///     <see cref="INotifyCollectionChanged"/> interface and should be implemented together
    ///     with it.
    ///     
    ///     The <see cref="PreviewingObservableCollection{T}"/> is a finished collection which
    ///     implements both of these interfaces.
    /// </remarks>
    /// <seealso cref="INotifyCollectionChanged"/>
    public interface INotifyCollectionChanging
    {

        /// <summary>
        ///     Occurs when the collection is about to change, but before the change has actually
        ///     happened.
        /// </summary>
        event NotifyCollectionChangingEventHandler CollectionChanging;

    }

}
