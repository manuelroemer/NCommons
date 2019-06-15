namespace NCommons.Observables.Tests.ObservableObject
{
    using System;
    using System.ComponentModel;
    using NCommons.Observables;
    using NCommons.Tests;
    using Xunit;

    // Instead of creating a separate class, we can trick around the abstract restrictions
    // by simply deriving from ObservableObject.
    // Not very clean, but that's the most simple way to get to the protected methods.
    public class ObservableObjectTestBase : ObservableObject
    {

        protected int _number;

        /// <summary>
        ///     Gets or sets a number which will invoke the changing events.
        /// </summary>
        public int Number
        {
            get => _number;
            set => Set(ref _number, value);
        }

        protected Assert.RaisedEvent<PropertyChangingEventArgs> AssertPropertyChangingGetsRaised(Action testCode)
        {
            return AssertEx.Raises<PropertyChangingEventHandler, PropertyChangingEventArgs>(
                handler => new PropertyChangingEventHandler(handler),
                handler => PropertyChanging += handler,
                handler => PropertyChanging -= handler,
                testCode
            );
        }

        protected Assert.RaisedEvent<PropertyChangedEventArgs> AssertPropertyChangedGetsRaised(Action testCode)
        {
            return AssertEx.Raises<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => new PropertyChangedEventHandler(handler),
                handler => PropertyChanged += handler,
                handler => PropertyChanged -= handler,
                testCode
            );
        }

    }

}
