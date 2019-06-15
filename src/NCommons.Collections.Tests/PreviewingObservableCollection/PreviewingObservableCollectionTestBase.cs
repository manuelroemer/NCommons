namespace NCommons.Collections.Tests.PreviewingObservableCollection
{
    using System;
    using System.Collections.Specialized;
    using static Xunit.Assert;
    using Assert = NCommons.Tests.AssertEx;

    public class PreviewingObservableCollectionTestBase
    {

        protected static RaisedEvent<NotifyCollectionChangedEventArgs> AssertRaisesCollectionChanging<T>(
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
