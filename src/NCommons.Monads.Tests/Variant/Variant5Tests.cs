namespace NCommons.Monads.Tests.Variant
{
    using System;
    using Xunit;

    public class Variant5Tests : Variant4Tests
    {

        internal override dynamic CreateEmptyVariant() =>
            new Variant<P1, P2, P3, P4, P5>();

        internal override dynamic CreateVariant(P1 value) =>
            new Variant<P1, P2, P3, P4, P5>(value);

        internal override dynamic CreateVariant(P2 value) =>
            new Variant<P1, P2, P3, P4, P5>(value);

        internal override dynamic CreateVariant(P3 value) =>
            new Variant<P1, P2, P3, P4, P5>(value);

        internal override dynamic CreateVariant(P4 value) =>
            new Variant<P1, P2, P3, P4, P5>(value);
        
        internal virtual dynamic CreateVariant(P5 value) =>
            new Variant<P1, P2, P3, P4, P5>(value);

        #region IsFifth

        [Fact]
        public void IsFifth_Is_True_For_Fifth_Variant()
        {
            var v = CreateVariant(new P5());
            Assert.True(v.IsFifth);
        }

        #endregion

        #region Value

        [Fact]
        public void Fifth_Variant_Has_Expected_Value()
        {
            var value = new P5();
            var v = CreateVariant(value);
            Assert.Same(value, v.Value);
        }

        #endregion

        #region GetValue

        // Should be kept in sync with explicit operator tests.

        [Fact]
        public void GetValue_Retrieves_Fifth_Value()
        {
            var v = CreateVariant(new P5());
            var returnedValue = v.GetValue(out P5 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValue_Throws_InvalidOperationException_For_Fifth_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Throws<InvalidOperationException>(() => v.GetValue(out P5 _));
        }

        #endregion

        #region GetValueOr (Substitute)

        [Fact]
        public void GetValueOr_Substitute_Retrieves_Fifth_Value()
        {
            var v = CreateVariant(new P5());
            var returnedValue = v.GetValueOr(new P5(), out P5 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOr_Substitute_Fifth_Value_Returns_Alternative_Value()
        {
            var substitute = new P5();
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOr(substitute, out P5 outValue);

            Assert.Same(substitute, outValue);
            Assert.Same(substitute, returnedValue);
        }

        #endregion

        #region GetValueOr (Substitute Provider)

        [Fact]
        public void GetValueOr_SubstituteProvider_Fifth_Value_Throws_ArgumentNullException()
        {
            Func<P5> substituteProvider = null;
            var v = CreateVariant(new P5());
            Assert.Throws<ArgumentNullException>(() => v.GetValueOr(substituteProvider, out P5 _));
        }

        [Fact]
        public void GetValueOr_SubstituteProvider_Retrieves_Fifth_Value()
        {
            Func<P5> substituteProvider = () => new P5();
            var v = CreateVariant(new P5());
            var returnedValue = v.GetValueOr(substituteProvider, out P5 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOr_SubstituteProvider_Fifth_Value_Returns_Alternative_Value()
        {
            var substitute = new P5();
            Func<P5> substituteProvider = () => substitute;
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOr(substituteProvider, out P5 outValue);

            Assert.Same(substitute, outValue);
            Assert.Same(substitute, returnedValue);
        }

        #endregion

        #region GetValueOrDefault

        [Fact]
        public void GetValueOrDefault_Retrieves_Fifth_Value()
        {
            var v = CreateVariant(new P5());
            var returnedValue = v.GetValueOrDefault(out P5 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOrDefault_Fifth_Value_Returns_Alternative_Value()
        {
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOrDefault(out P5 outValue);

            Assert.Same(default(P5), outValue);
            Assert.Same(default(P5), returnedValue);
        }

        #endregion

        #region TryGetValue

        [Fact]
        public void TryGetValue_Returns_True_If_Fifth_Value_Was_Retrieved()
        {
            var v = CreateVariant(new P5());
            Assert.True(v.TryGetValue(out P5 _));
        }

        [Fact]
        public void TryGetValue_Returns_False_If_Fifth_Value_Was_Not_Retrieved()
        {
            var v = CreateEmptyVariant();
            Assert.False(v.TryGetValue(out P5 _));
        }

        [Fact]
        public void TryGetValue_Retrieves_Fifth_Value()
        {
            var v = CreateVariant(new P5());
            v.TryGetValue(out P5 value);
            Assert.Same(v.Value, value);
        }

        #endregion

        #region Equality / GetHashCode

        [Fact]
        public void Equality_Fulfilled_For_Fifth_Variants()
        {
            var value = new P5();
            var v1 = CreateVariant(value);
            var v2 = CreateVariant(value);

            Assert.True(v1.Equals(v2));
            Assert.True(v1.Equals((object)v2));
            Assert.True(v1 == v2);
            Assert.False(v1 != v2);
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
        }

        [Fact]
        public void Unequality_Fulfilled_For_Unequal_Fifth_Values()
        {
            var v1 = CreateVariant(new P5());
            var v2 = CreateVariant(new P5());

            Assert.False(v1.Equals(v2));
            Assert.False(v1.Equals((object)v2));
            Assert.False(v1 == v2);
            Assert.True(v1 != v2);
            Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
        }

        [Fact]
        public void Unequality_Fulfilled_For_Different_Type_With_Fifth()
        {
            var v1 = CreateVariant(new P5());
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
        public void ToString_For_Fifth_Variant(bool createValue)
        {
            var v = CreateVariant(createValue ? new P5() : null);
            Assert.Equal(ExpectedToStringFor(v.Value, position: 5), v.ToString());
        }

        #endregion

        #region Explicit Operator

        // Should be kept in sync with GetValue tests.

        [Fact]
        public void Explicit_Operator_Converts_To_Fifth_Value()
        {
            var v = CreateVariant(new P5());
            var returnedValue = (P5)v;
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void Explicit_Operator_Throws_InvalidOperationException_For_Fifth_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Throws<InvalidOperationException>(() => (P5)v);
        }

        #endregion

        #region Implicit Operator

        [Fact]
        public override void Implicit_Operator_Creates_First_Variant()
        {
            Variant<P1, P2, P3, P4, P5> v = new P1();
            Assert.True(v.IsFirst);
        }

        [Fact]
        public override void Implicit_Operator_Creates_Second_Variant()
        {
            Variant<P1, P2, P3, P4, P5> v = new P2();
            Assert.True(v.IsSecond);
        }

        [Fact]
        public override void Implicit_Operator_Creates_Third_Variant()
        {
            Variant<P1, P2, P3, P4, P5> v = new P3();
            Assert.True(v.IsThird);
        }

        [Fact]
        public override void Implicit_Operator_Creates_Fourth_Variant()
        {
            Variant<P1, P2, P3, P4, P5> v = new P4();
            Assert.True(v.IsFourth);
        }

        [Fact]
        public virtual void Implicit_Operator_Creates_Fifth_Variant()
        {
            Variant<P1, P2, P3, P4, P5> v = new P5();
            Assert.True(v.IsFifth);
        }

        #endregion

        #region Match (Action)

        [Fact]
        public override void Match_Action_Matches_Empty()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5>();

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5>(new P1());

            v.Match(
                v => wasCalled = true,
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.True(wasCalled);
        }

        [Fact]
        public override void Match_Action_Matches_Second_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5>(new P2());

            v.Match(
                v => throw new Exception(),
                v => wasCalled = true,
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.True(wasCalled);
        }

        [Fact]
        public override void Match_Action_Matches_Third_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5>(new P3());

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                v => wasCalled = true,
                v => throw new Exception(),
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.True(wasCalled);
        }

        [Fact]
        public override void Match_Action_Matches_Fourth_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5>(new P4());

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                v => wasCalled = true,
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.True(wasCalled);
        }
        
        [Fact]
        public virtual void Match_Action_Matches_Fifth_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5>(new P5());

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5>();

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5>(new P1());

            var matched = v.Match(
                v => expected,
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.Same(expected, matched);
        }

        [Fact]
        public override void Match_Func_Matches_Second_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2, P3, P4, P5>(new P2());

            var matched = v.Match(
                v => throw new Exception(),
                v => expected,
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.Same(expected, matched);
        }

        [Fact]
        public override void Match_Func_Matches_Third_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2, P3, P4, P5>(new P3());

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                v => expected,
                v => throw new Exception(),
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.Same(expected, matched);
        }

        [Fact]
        public override void Match_Func_Matches_Fourth_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2, P3, P4, P5>(new P4());

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                v => expected,
                v => throw new Exception(),
                () => throw new Exception()
            );

            Assert.Same(expected, matched);
        }
        
        [Fact]
        public virtual void Match_Func_Matches_Fifth_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2, P3, P4, P5>(new P5());

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                v => throw new Exception(),
                v => expected,
                () => throw new Exception()
            );

            Assert.Same(expected, matched);
        }

        #endregion

    }

}
