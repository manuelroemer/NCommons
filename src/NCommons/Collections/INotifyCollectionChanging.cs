using System.Collections.Specialized;

namespace NCommons.Collections
{

    /// <summary>
    ///     Notifies listeners about when a collection is about to change, but before the change
    ///     is actually going to happen.
    /// </summary>
    /// <remarks>
    ///     This interface can be treated as the counterpart of the 
    ///     <see cref="INotifyCollectionChanged"/> interface and should be implemented together
    ///     with it.
    ///     
    ///     The <see cref="PreviewingObservableCollection"/> is a finished collection which
    ///     implements both of these interfaces.
    /// </remarks>
    /// <seealso cref="INotifyCollectionChanged"/>
    public interface INotifyCollectionChanging
    {

        /// <summary>
        ///     Occurs when the collection is about to change, but before the change has actually
        ///     happened.
        /// </summary>
        event NotifyCollectionChangedEventHandler CollectionChanging;

    }

}
