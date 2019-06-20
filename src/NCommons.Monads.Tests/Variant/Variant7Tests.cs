namespace NCommons.Monads.Tests.Variant
{
    using System;
    using Xunit;

    public class Variant7Tests : Variant6Tests
    {

        internal override dynamic CreateEmptyVariant() =>
            new Variant<P1, P2, P3, P4, P5, P6, P7>();

        internal override dynamic CreateVariant(P1 value) =>
            new Variant<P1, P2, P3, P4, P5, P6, P7>(value);

        internal override dynamic CreateVariant(P2 value) =>
            new Variant<P1, P2, P3, P4, P5, P6, P7>(value);

        internal override dynamic CreateVariant(P3 value) =>
            new Variant<P1, P2, P3, P4, P5, P6, P7>(value);

        internal override dynamic CreateVariant(P4 value) =>
            new Variant<P1, P2, P3, P4, P5, P6, P7>(value);

        internal override dynamic CreateVariant(P5 value) =>
            new Variant<P1, P2, P3, P4, P5, P6, P7>(value);

        internal override dynamic CreateVariant(P6 value) =>
            new Variant<P1, P2, P3, P4, P5, P6, P7>(value);
        
        internal virtual dynamic CreateVariant(P7 value) =>
            new Variant<P1, P2, P3, P4, P5, P6, P7>(value);

        #region IsSeventh

        [Fact]
        public void IsSeventh_Is_True_For_Seventh_Variant()
        {
            var v = CreateVariant(new P7());
            Assert.True(v.IsSeventh);
        }

        #endregion

        #region Value

        [Fact]
        public void Seventh_Variant_Has_Expected_Value()
        {
            var value = new P7();
            var v = CreateVariant(value);
            Assert.Same(value, v.Value);
        }

        #endregion

        #region GetValue

        // Should be kept in sync with explicit operator tests.

        [Fact]
        public void GetValue_Retrieves_Seventh_Value()
        {
            var v = CreateVariant(new P7());
            var returnedValue = v.GetValue(out P7 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValue_Throws_InvalidOperationException_For_Seventh_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Throws<InvalidOperationException>(() => v.GetValue(out P7 _));
        }

        #endregion

        #region GetValueOr (Substitute)

        [Fact]
        public void GetValueOr_Substitute_Retrieves_Seventh_Value()
        {
            var v = CreateVariant(new P7());
            var returnedValue = v.GetValueOr(new P7(), out P7 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOr_Substitute_Seventh_Value_Returns_Alternative_Value()
        {
            var substitute = new P7();
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOr(substitute, out P7 outValue);

            Assert.Same(substitute, outValue);
            Assert.Same(substitute, returnedValue);
        }

        #endregion

        #region GetValueOr (Substitute Provider)

        [Fact]
        public void GetValueOr_SubstituteProvider_Seventh_Value_Throws_ArgumentNullException()
        {
            Func<P7> substituteProvider = null;
            var v = CreateVariant(new P7());
            Assert.Throws<ArgumentNullException>(() => v.GetValueOr(substituteProvider, out P7 _));
        }

        [Fact]
        public void GetValueOr_SubstituteProvider_Retrieves_Seventh_Value()
        {
            Func<P7> substituteProvider = () => new P7();
            var v = CreateVariant(new P7());
            var returnedValue = v.GetValueOr(substituteProvider, out P7 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOr_SubstituteProvider_Seventh_Value_Returns_Alternative_Value()
        {
            var substitute = new P7();
            Func<P7> substituteProvider = () => substitute;
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOr(substituteProvider, out P7 outValue);

            Assert.Same(substitute, outValue);
            Assert.Same(substitute, returnedValue);
        }

        #endregion

        #region GetValueOrDefault

        [Fact]
        public void GetValueOrDefault_Retrieves_Seventh_Value()
        {
            var v = CreateVariant(new P7());
            var returnedValue = v.GetValueOrDefault(out P7 outValue);

            Assert.Same(v.Value, outValue);
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void GetValueOrDefault_Seventh_Value_Returns_Alternative_Value()
        {
            var v = CreateEmptyVariant();
            var returnedValue = v.GetValueOrDefault(out P7 outValue);

            Assert.Same(default(P7), outValue);
            Assert.Same(default(P7), returnedValue);
        }

        #endregion

        #region TryGetValue

        [Fact]
        public void TryGetValue_Returns_True_If_Seventh_Value_Was_Retrieved()
        {
            var v = CreateVariant(new P7());
            Assert.True(v.TryGetValue(out P7 _));
        }

        [Fact]
        public void TryGetValue_Returns_False_If_Seventh_Value_Was_Not_Retrieved()
        {
            var v = CreateEmptyVariant();
            Assert.False(v.TryGetValue(out P7 _));
        }

        [Fact]
        public void TryGetValue_Retrieves_Seventh_Value()
        {
            var v = CreateVariant(new P7());
            v.TryGetValue(out P7 value);
            Assert.Same(v.Value, value);
        }

        #endregion

        #region Equality / GetHashCode

        [Fact]
        public void Equality_Fulfilled_For_Seventh_Variants()
        {
            var value = new P7();
            var v1 = CreateVariant(value);
            var v2 = CreateVariant(value);

            Assert.True(v1.Equals(v2));
            Assert.True(v1.Equals((object)v2));
            Assert.True(v1 == v2);
            Assert.False(v1 != v2);
            Assert.Equal(v1.GetHashCode(), v2.GetHashCode());
        }

        [Fact]
        public void Unequality_Fulfilled_For_Unequal_Seventh_Values()
        {
            var v1 = CreateVariant(new P7());
            var v2 = CreateVariant(new P7());

            Assert.False(v1.Equals(v2));
            Assert.False(v1.Equals((object)v2));
            Assert.False(v1 == v2);
            Assert.True(v1 != v2);
            Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
        }

        [Fact]
        public void Unequality_Fulfilled_For_Different_Type_With_Seventh()
        {
            var v1 = CreateVariant(new P7());
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
        public void ToString_For_Seventh_Variant(bool createValue)
        {
            var v = CreateVariant(createValue ? new P7() : null);
            Assert.Equal(ExpectedToStringFor(v.Value, position: 7), v.ToString());
        }

        #endregion

        #region Explicit Operator

        // Should be kept in sync with GetValue tests.

        [Fact]
        public void Explicit_Operator_Converts_To_Seventh_Value()
        {
            var v = CreateVariant(new P7());
            var returnedValue = (P7)v;
            Assert.Same(v.Value, returnedValue);
        }

        [Fact]
        public void Explicit_Operator_Throws_InvalidOperationException_For_Seventh_Value()
        {
            var v = CreateEmptyVariant();
            Assert.Throws<InvalidOperationException>(() => (P7)v);
        }

        #endregion

        #region Implicit Operator

        [Fact]
        public override void Implicit_Operator_Creates_First_Variant()
        {
            Variant<P1, P2, P3, P4, P5, P6, P7> v = new P1();
            Assert.True(v.IsFirst);
        }

        [Fact]
        public override void Implicit_Operator_Creates_Second_Variant()
        {
            Variant<P1, P2, P3, P4, P5, P6, P7> v = new P2();
            Assert.True(v.IsSecond);
        }

        [Fact]
        public override void Implicit_Operator_Creates_Third_Variant()
        {
            Variant<P1, P2, P3, P4, P5, P6, P7> v = new P3();
            Assert.True(v.IsThird);
        }

        [Fact]
        public override void Implicit_Operator_Creates_Fourth_Variant()
        {
            Variant<P1, P2, P3, P4, P5, P6, P7> v = new P4();
            Assert.True(v.IsFourth);
        }

        [Fact]
        public override void Implicit_Operator_Creates_Fifth_Variant()
        {
            Variant<P1, P2, P3, P4, P5, P6, P7> v = new P5();
            Assert.True(v.IsFifth);
        }

        [Fact]
        public override void Implicit_Operator_Creates_Sixth_Variant()
        {
            Variant<P1, P2, P3, P4, P5, P6, P7> v = new P6();
            Assert.True(v.IsSixth);
        }

        [Fact]
        public virtual void Implicit_Operator_Creates_Seventh_Variant()
        {
            Variant<P1, P2, P3, P4, P5, P6, P7> v = new P7();
            Assert.True(v.IsSeventh);
        }

        #endregion

        #region Match (Action)

        [Fact]
        public override void Match_Action_Matches_Empty()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>();

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P1());

            v.Match(
                v => wasCalled = true,
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P2());

            v.Match(
                v => throw new Exception(),
                v => wasCalled = true,
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P3());

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
        public override void Match_Action_Matches_Fourth_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P4());

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
        public override void Match_Action_Matches_Fifth_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P5());

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
        public override void Match_Action_Matches_Sixth_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P6());

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
        public virtual void Match_Action_Matches_Seventh_Value()
        {
            bool wasCalled = false;
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P7());

            v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>();

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P1());

            var matched = v.Match(
                v => expected,
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P2());

            var matched = v.Match(
                v => throw new Exception(),
                v => expected,
                v => throw new Exception(),
                v => throw new Exception(),
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
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P3());

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
        public override void Match_Func_Matches_Fourth_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P4());

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
        public override void Match_Func_Matches_Fifth_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P5());

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
        public override void Match_Func_Matches_Sixth_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P6());

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
        public virtual void Match_Func_Matches_Seventh_Value()
        {
            var expected = new object();
            var v = new Variant<P1, P2, P3, P4, P5, P6, P7>(new P7());

            var matched = v.Match(
                v => throw new Exception(),
                v => throw new Exception(),
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
