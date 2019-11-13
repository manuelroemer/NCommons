namespace NCommons.Observables.Tests
{
    using System;
    using System.ComponentModel;
    using NCommons.Tests;
    using Xunit;

    public abstract class ObservableObjectTests : ObservableObject
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

        public class PropertyChangedTests : ObservableObjectTests
        {

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void PropertyChanged_Doesnt_Get_Raised_For_Certain_Strings(string propertyName)
            {
                PropertyChanged += (sender, e) => throw new Exception();
                OnPropertyChanged(propertyName);
            }

            [Fact]
            public void OnPropertyChanged_Raises_PropertyChanged_With_Args()
            {
                AssertPropertyChangedGetsRaised(
                    () => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Number)))
                );
            }

            [Fact]
            public void OnPropertyChanged_Raises_PropertyChanged_With_String()
            {
                AssertPropertyChangedGetsRaised(
                    () => OnPropertyChanged(nameof(Number))
                );
            }

            [Fact]
            public void OnPropertyChanged_Correctly_Passes_Name_With_Args()
            {
                var ev = AssertPropertyChangedGetsRaised(
                    () => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Number)))
                );
                Assert.Equal(nameof(Number), ev.Arguments.PropertyName);
            }

            [Fact]
            public void OnPropertyChanged_Correctly_Passes_Name_With_String()
            {
                var ev = AssertPropertyChangedGetsRaised(
                    () => OnPropertyChanged(nameof(Number))
                );
                Assert.Equal(nameof(Number), ev.Arguments.PropertyName);
            }

        }

        public class PropertyChangingTests : ObservableObjectTests
        {

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            public void PropertyChanging_Doesnt_Get_Raised_For_Certain_Strings(string propertyName)
            {
                PropertyChanging += (sender, e) => throw new Exception();
                OnPropertyChanging(propertyName);
            }

            [Fact]
            public void OnPropertyChanging_Raises_PropertyChanging_With_Args()
            {
                AssertPropertyChangingGetsRaised(
                    () => OnPropertyChanging(new PropertyChangingEventArgs(nameof(Number)))
                );
            }

            [Fact]
            public void OnPropertyChanging_Raises_PropertyChanging_With_String()
            {
                AssertPropertyChangingGetsRaised(
                    () => OnPropertyChanging(nameof(Number))
                );
            }

            [Fact]
            public void OnPropertyChanging_Correctly_Passes_Name_With_Args()
            {
                var ev = AssertPropertyChangingGetsRaised(
                    () => OnPropertyChanging(new PropertyChangingEventArgs(nameof(Number)))
                );
                Assert.Equal(nameof(Number), ev.Arguments.PropertyName);
            }

            [Fact]
            public void OnPropertyChanging_Correctly_Passes_Name_With_String()
            {
                var ev = AssertPropertyChangingGetsRaised(
                    () => OnPropertyChanging(nameof(Number))
                );
                Assert.Equal(nameof(Number), ev.Arguments.PropertyName);
            }

        }

        public class SetTests : ObservableObjectTests
        {

            [Fact]
            public void Set_With_Ref_Property_Sets_Property()
            {
                Set(ref _number, 123);
                Assert.Equal(123, _number);
            }

            [Fact]
            public void Set_With_Ref_Property_Doesnt_Set_Property_With_Same_Value()
            {
                Number = 123;
                PropertyChanging += (sender, e) => throw new Exception();
                PropertyChanged += (sender, e) => throw new Exception();

                Set(ref _number, 123);
            }

            [Fact]
            public void Set_With_Ref_Property_Doesnt_Sets_Property_With_Same_Value_If_EqualityComparer_Is_Null()
            {
                Number = 123;
                AssertPropertyChangingGetsRaised(
                    () => Set(ref _number, 123, equalityComparer: null)
                );
                AssertPropertyChangedGetsRaised(
                    () => Set(ref _number, 123, equalityComparer: null)
                );
            }

            [Fact]
            public void Set_With_Ref_Property_Raises_PropertyChanging()
            {
                AssertPropertyChangingGetsRaised(
                    () => Set(ref _number, 123)
                );
            }

            [Fact]
            public void Set_With_Ref_Property_Raises_PropertyChanged()
            {
                AssertPropertyChangedGetsRaised(
                    () => Set(ref _number, 123)
                );
            }

            [Fact]
            public void Set_With_Action_Property_Raises_PropertyChanging()
            {
                AssertPropertyChangingGetsRaised(
                    () => Set(() => { })
                );
            }

            [Fact]
            public void Set_With_Action_Property_Raises_PropertyChanged()
            {
                AssertPropertyChangedGetsRaised(
                    () => Set(() => { })
                );
            }

        }

    }

}
