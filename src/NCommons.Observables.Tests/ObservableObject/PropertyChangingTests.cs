namespace NCommons.Observables.Tests.ObservableObject
{
    using System;
    using System.ComponentModel;
    using Xunit;

    public class PropertyChangingTests : ObservableObjectTestBase
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

}
