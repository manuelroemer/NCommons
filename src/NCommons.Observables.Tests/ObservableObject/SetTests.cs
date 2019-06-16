namespace NCommons.Observables.Tests.ObservableObject
{
    using System;
    using Xunit;

    public class SetTests : ObservableObjectTestBase
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
