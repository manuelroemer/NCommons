namespace NCommons.Collections
{
    using System;
    using System.Linq;


#pragma warning disable CA1812

    /// <summary>
    ///     Provides useful debug values for an <see cref="IPriorityQueue{T}"/>.
    /// </summary>
    internal sealed class PriorityQueueDebugView<T>
    {

        private readonly IPriorityQueue<T> _queue;

        public T Next
        {
            get
            {
                _queue.TryPeek(out var result);
                return result;
            }
        }

        public T[] Items => _queue.ToArray();

        public PriorityQueueDebugView(IPriorityQueue<T> queue)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

    }

#pragma warning restore CA1812

}
