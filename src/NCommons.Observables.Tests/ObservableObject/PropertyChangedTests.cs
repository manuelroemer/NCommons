namespace NCommons.Observables.Tests.ObservableObject
{
    using System;
    using System.ComponentModel;
    using Xunit;

    public class PropertyChangedTests : ObservableObjectTestBase
    {

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void PropertyChanged_Doesnt_Get_Raised_For_Certain_Strings(string? propertyName)
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

}
