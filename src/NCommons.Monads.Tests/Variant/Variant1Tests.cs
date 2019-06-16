namespace NCommons.Monads.Tests.Variant
{
    using System;
    using Xunit;

    public class Variant1Tests : VariantTestBase
    {

        internal override dynamic CreateEmptyVariant() =>
            new Variant<P1>();

        internal virtual dynamic CreateVariant(P1 value) =>
            new Variant<P1>(value);

        #region IsFirst

        [Fact]
        public void IsFirst_Is_True_For_First_Variant()
        {
            var v = CreateVariant(new P1());
            Assert.True(v.IsFirst);
        }

        #endregion

        #region Value

        [Fact]
        public void First_Variant_Has_Expected_Value()
        {
            var value = new P1();
            var v = CreateVariant(value);
            Assert.Same(value, v.Value);
        }

        #endregion

        #region GetValue

        // Should be kept in sync with explicit operator tests.

        [Fact]
        public void GetValue_Retrieves_First_Value()
        {
            var v = CreateVariant(new P1());
            var returnedValue = v.GetValue(out P1 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValue_Throws_InvalidOperationException_For_First_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Throws<InvalidOperationException>(() => v.GetValue(out P1 _));
        }

        #endregion

        #region GetValueOr (Substitute)

        [Fact]
        public void GetValueOr_Substitute_Retrieves_First_Value()
        {
            var v = CreateVariant(new P1());
            var returnedValue = v.GetValueOr(new P1(), out P1 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOr_Substitute_Returns_Alternative_Value()
        {
            var substitute = new P1();
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOr(substitute, out P1 outValue);

            Assert.Same(substitute, outValue);
            Assert.Same(substitute, returnedValue);
        }

        #endregion

        #region GetValueOr (Substitute Provider)

        [Fact]
        public void GetValueOr_SubstituteProvider_Throws_ArgumentNullException()
        {
            Func<P1> substituteProvider = null;
            var v = CreateVariant(new P1());
            Assert.Throws<ArgumentNullException>(() => v.GetValueOr(substituteProvider, out P1 _));
        }

        [Fact]
        public void GetValueOr_SubstituteProvider_Retrieves_First_Value()
        {
            Func<P1> substituteProvider = () => new P1();
            var v = CreateVariant(new P1());
            var returnedValue = v.GetValueOr(substituteProvider, out P1 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOr_SubstituteProvider_Returns_Alternative_Value()
        {
            var substitute = new P1();
            Func<P1> substituteProvider = () => substitute;
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOr(substituteProvider, out P1 outValue);

            Assert.Same(substitute, outValue);
            Assert.Same(substitute, returnedValue);
        }

        #endregion

        #region TryGetValue

        [Fact]
        public void TryGetValue_Returns_True_If_First_Value_Was_Retrieved()
        {
            var v = CreateVariant(new P1());
            Assert.True(v.TryGetValue(out P1 _));
        }
        
        [Fact]
        public void TryGetValue_Returns_False_If_First_Value_Was_Not_Retrieved()
        {
            var v = CreateEmptyVariant();
            Assert.False(v.TryGetValue(out P1 _));
        }
        
        [Fact]
        public void TryGetValue_Retrieves_First_Value()
        {
            var v = CreateVariant(new P1());
            v.TryGetValue(out P1 value);
            Assert.Same(v.Value, value);
        }

        #endregion

        #region Equality / GetHashCode

        [Fact]
        public void Equality_Fulfilled_For_First_Variants()
        {
            var value = new P1();
            var v1 = CreateVariant(value);
            var v2 = CreateVariant(value);

            Assert.True(v1.Equals(v2));
            Assert.True(v1.Equals((object)v2));
            Assert.True(v1 == v2);
            Assert.False(v1 != v2);
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
        }

        [Fact]
        public void Unequality_Fulfilled_For_Unequal_First_Values()
        {
            var v1 = CreateVariant(new P1());
            var v2 = CreateVariant(new P1());

            Assert.False(v1.Equals(v2));
            Assert.False(v1.Equals((object)v2));
            Assert.False(v1 == v2);
            Assert.True(v1 != v2);
            Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
        }
        
        [Fact]
        public void Unequality_Fulfilled_For_Different_Type_With_First()
        {
            var v1 = CreateVariant(new P1());
            var v2 = CreateEmptyVariant();

            Assert.False(v1.Equals(v2));
            Assert.False(v1.Equals((object)v2));
            Assert.False(v1 == v2);
            Assert.True(v1 != v2);
            Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
        }

        #endregion

        #region ToString

        [Theory, InlineData(true), InlineData(false)]
        public void ToString_For_First_Variant(bool createValue)
        {
            var v = CreateVariant(createValue ? new P1() : null);
            Assert.Equal(ExpectedToStringFor(v.Value, position: 1), v.ToString());
        }

        protected static string ExpectedToStringFor(object value, int position)
        {
            return $"Value {position}: {value?.ToString() ?? "null"}";
        }

        #endregion

        #region Explicit Operator

        // Should be kept in sync with GetValue tests.

        [Fact]
        public void Explicit_Operator_Converts_To_First_Value()
        {
            var v = CreateVariant(new P1());
            var returnedValue = (P1)v;
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void Explicit_Operator_Throws_InvalidOperationException_For_First_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Throws<InvalidOperationException>(() => (P1)v);
        }

        #endregion

        #region Match (Action)

        [Fact]
        public virtual void Match_Action_Matches_Empty()
        {
            bool wasCalled = false;
            var v = new Variant<P1>();

            v.Match(
                v1 => throw new Exception(),
                () => wasCalled = true
            );

            Assert.True(wasCalled);
        }

        [Fact]
        public virtual void Match_Action_Matches_First_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1>(new P1());

            v.Match(
                v1 => wasCalled = true,
                () => throw new Exception()
            );

            Assert.True(wasCalled);
        }

        #endregion
        
        #region Match (Action)

        [Fact]
        public virtual void Match_Func_Matches_Empty()
        {
            var expected = new object();
            var v = new Variant<P1>();
            
            var matched = v.Match(
                v1 => throw new Exception(),
                () => expected
            );

            Assert.Same(expected, matched);
        }

        [Fact]
        public virtual void Match_Func_Matches_First_Value()
        {
            var expected = new object();
            var v = new Variant<P1>(new P1());

            var matched = v.Match(
                v1 => expected,
                () => throw new Exception()
            );

            Assert.Same(expected, matched);
        }

        #endregion

    }

}
