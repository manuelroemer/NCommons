namespace NCommons.Observables.Tests
{
    using System;
    using System.ComponentModel;
    using NCommons.Observables;
    using NCommons.Tests;
    using Xunit;


    // Instead of creating a separate class, we can trick around the abstract restrictions
    // by simply deriving from ObservableObject.
    // Not very clean, but that's the most simple way to get to the protected methods.
    public class ObservableObjectTests : ObservableObject
    {

        private int _number;
        public int Number
        {
            get => _number;
            set => Set(ref _number, value);
        }

        #region PropertyChanging Raising

        [Fact]
        public void OnPropertyChanging_Raises_PropertyChanging_With_Args()
        {
            VerifyPropertyChangingGetsRaised(
                () => OnPropertyChanging(new PropertyChangingEventArgs(nameof(Number)))
            );
        }
        
        [Fact]
        public void OnPropertyChanging_Raises_PropertyChanging_With_String()
        {
            VerifyPropertyChangingGetsRaised(
                () => OnPropertyChanging(nameof(Number))
            );
        }

        [Fact]
        public void OnPropertyChanging_Correctly_Passes_Name_With_Args()
        {
            var ev = VerifyPropertyChangingGetsRaised(
                () => OnPropertyChanging(new PropertyChangingEventArgs(nameof(Number)))
            );
            Assert.Equal(nameof(Number), ev.Arguments.PropertyName);
        }
        
        [Fact]
        public void OnPropertyChanging_Correctly_Passes_Name_With_String()
        {
            var ev = VerifyPropertyChangingGetsRaised(
                () => OnPropertyChanging(nameof(Number))
            );
            Assert.Equal(nameof(Number), ev.Arguments.PropertyName);
        }

        #endregion
        
        #region PropertyChanged Raising

        [Fact]
        public void OnPropertyChanged_Raises_PropertyChanged_With_Args()
        {
            VerifyPropertyChangedGetsRaised(
                () => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Number)))
            );
        }
        
        [Fact]
        public void OnPropertyChanged_Raises_PropertyChanged_With_Stred()
        {
            VerifyPropertyChangedGetsRaised(
                () => OnPropertyChanged(nameof(Number))
            );
        }

        [Fact]
        public void OnPropertyChanged_Correctly_Passes_Name_With_Args()
        {
            var ev = VerifyPropertyChangedGetsRaised(
                () => OnPropertyChanged(new PropertyChangedEventArgs(nameof(Number)))
            );
            Assert.Equal(nameof(Number), ev.Arguments.PropertyName);
        }
        
        [Fact]
        public void OnPropertyChanged_Correctly_Passes_Name_With_Stred()
        {
            var ev = VerifyPropertyChangedGetsRaised(
                () => OnPropertyChanged(nameof(Number))
            );
            Assert.Equal(nameof(Number), ev.Arguments.PropertyName);
        }

        #endregion

        #region No events for null and empty strings

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PropertyChanging_Doesnt_Get_Raised_For_Certain_Strings(string? propertyName)
        {
            PropertyChanging += (sender, e) => throw new Exception();
            OnPropertyChanging(propertyName);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PropertyChanged_Doesnt_Get_Raised_For_Certain_Strings(string? propertyName)
        {
            PropertyChanged += (sender, e) => throw new Exception();
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region Set Raising

        [Fact]
        public void Set_With_Ref_Property_Raises_PropertyChanging()
        {
            VerifyPropertyChangingGetsRaised(
                () => Set(ref _number, 123)
            );
        }

        [Fact]
        public void Set_With_Ref_Property_Raises_PropertyChanged()
        {
            VerifyPropertyChangedGetsRaised(
                () => Set(ref _number, 123)
            );
        }
        
        [Fact]
        public void Set_With_Action_Property_Raises_PropertyChanging()
        {
            VerifyPropertyChangingGetsRaised(
                () => Set(() => { })
            );
        }

        [Fact]
        public void Set_With_Action_Property_Raises_PropertyChanged()
        {
            VerifyPropertyChangedGetsRaised(
                () => Set(() => { })
            );
        }

        #endregion

        #region Set<T> Setting

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
            VerifyPropertyChangingGetsRaised(
                () => Set(ref _number, 123, equalityComparer: null)
            );
            VerifyPropertyChangedGetsRaised(
                () => Set(ref _number, 123, equalityComparer: null)
            );
        }

        #endregion

        private Assert.RaisedEvent<PropertyChangingEventArgs> VerifyPropertyChangingGetsRaised(Action testCode)
        {
            return AssertEx.Raises<PropertyChangingEventHandler, PropertyChangingEventArgs>(
                handler => new PropertyChangingEventHandler(handler),
                handler => PropertyChanging += handler,
                handler => PropertyChanging -= handler,
                testCode
            );
        }
        
        private Assert.RaisedEvent<PropertyChangedEventArgs> VerifyPropertyChangedGetsRaised(Action testCode)
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
