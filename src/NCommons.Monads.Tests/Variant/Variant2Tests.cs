﻿namespace NCommons.Monads.Tests.Variant
{
    using System;
    using Xunit;

    public class Variant2Tests : Variant1Tests
    {

        internal override dynamic CreateEmptyVariant() =>
            new Variant<P1, P2>();

        internal override dynamic CreateVariant(P1 value) =>
            new Variant<P1, P2>(value);
        
        internal virtual dynamic CreateVariant(P2 value) =>
            new Variant<P1, P2>(value);

        #region IsSecond

        [Fact]
        public void IsSecond_Is_True_For_Second_Variant()
        {
            var v = CreateVariant(new P2());
            Assert.True(v.IsSecond);
        }

        #endregion

        #region Value

        [Fact]
        public void Second_Variant_Has_Expected_Value()
        {
            var value = new P2();
            var v = CreateVariant(value);
            Assert.Same(value, v.Value);
        }

        #endregion

        #region GetValue

        // Should be kept in sync with explicit operator tests.

        [Fact]
        public void GetValue_Retrieves_Second_Value()
        {
            var v = CreateVariant(new P2());
            var returnedValue = v.GetValue(out P2 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValue_Throws_InvalidOperationException_For_Second_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Throws<InvalidOperationException>(() => v.GetValue(out P2 _));
        }

        #endregion

        #region GetValueOr (Substitute)

        [Fact]
        public void GetValueOr_Substitute_Retrieves_Second_Value()
        {
            var v = CreateVariant(new P2());
            var returnedValue = v.GetValueOr(new P2(), out P2 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOr_Substitute_Second_Value_Returns_Alternative_Value()
        {
            var substitute = new P2();
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOr(substitute, out P2 outValue);

            Assert.Same(substitute, outValue);
            Assert.Same(substitute, returnedValue);
        }

        #endregion

        #region GetValueOr (Substitute Provider)

        [Fact]
        public void GetValueOr_SubstituteProvider_Second_Value_Throws_ArgumentNullException()
        {
            Func<P2> substituteProvider = null;
            var v = CreateVariant(new P2());
            Assert.Throws<ArgumentNullException>(() => v.GetValueOr(substituteProvider, out P2 _));
        }

        [Fact]
        public void GetValueOr_SubstituteProvider_Retrieves_Second_Value()
        {
            Func<P2> substituteProvider = () => new P2();
            var v = CreateVariant(new P2());
            var returnedValue = v.GetValueOr(substituteProvider, out P2 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOr_SubstituteProvider_Second_Value_Returns_Alternative_Value()
        {
            var substitute = new P2();
            Func<P2> substituteProvider = () => substitute;
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOr(substituteProvider, out P2 outValue);

            Assert.Same(substitute, outValue);
            Assert.Same(substitute, returnedValue);
        }

        #endregion

        #region GetValueOrDefault

        [Fact]
        public void GetValueOrDefault_Retrieves_Second_Value()
        {
            var v = CreateVariant(new P2());
            var returnedValue = v.GetValueOrDefault(out P2 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOrDefault_Second_Value_Returns_Alternative_Value()
        {
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOrDefault(out P2 outValue);

            Assert.Same(default(P2), outValue);
            Assert.Same(default(P2), returnedValue);
        }

        #endregion

        #region TryGetValue

        [Fact]
        public void TryGetValue_Returns_True_If_Second_Value_Was_Retrieved()
        {
            var v = CreateVariant(new P2());
            Assert.True(v.TryGetValue(out P2 _));
        }

        [Fact]
        public void TryGetValue_Returns_False_If_Second_Value_Was_Not_Retrieved()
        {
            var v = CreateEmptyVariant();
            Assert.False(v.TryGetValue(out P2 _));
        }

        [Fact]
        public void TryGetValue_Retrieves_Second_Value()
        {
            var v = CreateVariant(new P2());
            v.TryGetValue(out P2 value);
            Assert.Same(v.Value, value);
        }

        #endregion

        #region Equality / GetHashCode

        [Fact]
        public void Equality_Fulfilled_For_Second_Variants()
        {
            var value = new P2();
            var v1 = CreateVariant(value);
            var v2 = CreateVariant(value);

            Assert.True(v1.Equals(v2));
            Assert.True(v1.Equals((object)v2));
            Assert.True(v1 == v2);
            Assert.False(v1 != v2);
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
        }

        [Fact]
        public void Unequality_Fulfilled_For_Unequal_Second_Values()
        {
            var v1 = CreateVariant(new P2());
            var v2 = CreateVariant(new P2());

            Assert.False(v1.Equals(v2));
            Assert.False(v1.Equals((object)v2));
            Assert.False(v1 == v2);
            Assert.True(v1 != v2);
            Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
        }

        [Fact]
        public void Unequality_Fulfilled_For_Different_Type_With_Second()
        {
            var v1 = CreateVariant(new P2());
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
        public void ToString_For_Second_Variant(bool createValue)
        {
            var v = CreateVariant(createValue ? new P2() : null);
            Assert.Equal(ExpectedToStringFor(v.Value, position: 2), v.ToString());
        }

        #endregion

        #region Explicit Operator

        // Should be kept in sync with GetValue tests.

        [Fact]
        public void Explicit_Operator_Converts_To_Second_Value()
        {
            var v = CreateVariant(new P2());
            var returnedValue = (P2)v;
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void Explicit_Operator_Throws_InvalidOperationException_For_Second_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Throws<InvalidOperationException>(() => (P2)v);
        }

        #endregion

        #region Match (Action)

        [Fact]
        public override void Match_Action_Matches_Empty()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2>();

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                () => wasCalled = true
            );

            Assert.True(wasCalled);
        }

        [Fact]
        public override void Match_Action_Matches_First_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2>(new P1());

            v.Match(
                v => wasCalled = true,
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.True(wasCalled);
        }
        
        [Fact]
        public virtual void Match_Action_Matches_Second_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2>(new P2());

            v.Match(
                v => throw new Exception(),
                v => wasCalled = true,
                () => throw new Exception()
            );

            Assert.True(wasCalled);
        }

        #endregion

        #region Match (Action)

        [Fact]
        public override void Match_Func_Matches_Empty()
        {
            var expected = new object();
            var v = new Variant<P1, P2>();

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                () => expected
            );

            Assert.Same(expected, matched);
        }

        [Fact]
        public override void Match_Func_Matches_First_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2>(new P1());

            var matched = v.Match(
                v => expected,
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.Same(expected, matched);
        }
        
        [Fact]
        public virtual void Match_Func_Matches_Second_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2>(new P2());

            var matched = v.Match(
                v => throw new Exception(),
                v => expected,
                () => throw new Exception()
            );

            Assert.Same(expected, matched);
        }

        #endregion

    }

}