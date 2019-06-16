namespace NCommons.Monads.Tests.Variant
{
    using Xunit;

    public abstract class VariantTestBase
    {

        internal abstract dynamic CreateEmptyVariant();

        #region IsEmpty

        [Fact]
        public void IsEmpty_Is_True_For_Empty_Variant()
        {
            var v = CreateEmptyVariant();
            Assert.True(v.IsEmpty);
        }

        #endregion

        #region Value

        [Fact]
        public void Empty_Variant_Has_Expected_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Null(v.Value);
        }

        #endregion

        #region Equality

        [Fact]
        public void Equality_Fulfilled_For_Empty_Variants()
        {
            var v1 = CreateEmptyVariant();
            var v2 = CreateEmptyVariant();

            Assert.True(v1.Equals(v2));
            Assert.True(v1.Equals((object)v2));
            Assert.True(v1 == v2);
            Assert.False(v1 != v2);
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
        }

        #endregion

        #region ToString

        [Fact]
        public void ToString_For_Empty_Variant()
        {
            var v = CreateEmptyVariant();
            Assert.Equal("Empty", v.ToString());
        }

        #endregion

    }

}
