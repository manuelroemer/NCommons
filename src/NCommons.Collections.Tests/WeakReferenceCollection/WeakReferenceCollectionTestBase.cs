namespace NCommons.Collections.Tests.WeakReferenceCollection
{
    using System;
    using System.Collections.Generic;

    public abstract class WeakReferenceCollectionTestBase
    {

        /// <summary>
        ///     Fills the specified collection with a set of alive objects, objects to be GC'ed
        ///     and with nulls.
        ///     It returns an int which represents the number of expected elements after every
        ///     element has been purged from the list.
        /// </summary>
        protected static int FillCollection(
            WeakReferenceCollection<object> collection,
            int collectableObjects = 10,
            int aliveObjects = 10,
            int nulls = 5)
        {
            for (int i = 0; i < collectableObjects; i++)
            {
                collection.Add(new object());
            }

            for (int i = 0; i < aliveObjects; i++)
            {
                collection.Add(GetAliveObject(i));
            }

            for (int i = 0; i < nulls; i++)
            {
                collection.Add(null);
            }

            return aliveObjects + nulls;
        }

        protected static object GetAliveObject(int index)
        {
            if (s_aliveObjectHolder.Count > index)
            {
                return s_aliveObjectHolder[index];
            }
            else
            {
                // Add objects until we arrive at the index.
                while (s_aliveObjectHolder.Count <= index)
                {
                    s_aliveObjectHolder.Add(new object());
                }
                return s_aliveObjectHolder[index];
            }
        }

        // References objects, so that they don't get GC'ed. Should only be used with GetAliveObject.
        protected static List<object> s_aliveObjectHolder = new List<object>();

        protected static void CollectGarbage()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

    }

}
