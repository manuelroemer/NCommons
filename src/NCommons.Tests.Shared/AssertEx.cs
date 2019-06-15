namespace NCommons.Tests
{
    using System;
    using Xunit;

    public class AssertEx : Assert
    {

        /// <summary>
        ///     Same as XUnit's Assert.Raises, but with support for specific delegates.
        /// </summary>
        /// <param name="makeDelegate">
        ///     A function which creates a new Delegate that wraps the passed EventHandler{T}.
        ///     E.g. `handler => new MyDelegate(handler)`
        /// </param>
        public static RaisedEvent<TArgs> Raises<TEventDelegate, TArgs>(
            Func<EventHandler<TArgs>, TEventDelegate> makeDelegate,
            Action<TEventDelegate> attach,
            Action<TEventDelegate> detach,
            Action testCode)
            where TArgs : EventArgs
        {
            TEventDelegate handler = default;

            return Assert.Raises<TArgs>(
                h =>
                {
                    handler = makeDelegate(h);
                    attach(handler);
                },
                h =>
                {
                    detach(handler);
                },
                testCode
            );
        }

    }

}
