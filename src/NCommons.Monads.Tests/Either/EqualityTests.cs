namespace NCommons.Monads.Tests.Either
{
    using System;
    using Xunit;

    public class EqualityTests
    {

        [Fact]
        public void Correctly_Compares_Equal_Eithers()
        {
            var val = new Left();
            var e1 = new Either<Left, Right>(val);
            var e2 = new Either<Left, Right>(val);

            AssertEquality(e1, e2);
        }

        [Fact]
        public void Equality_Respects_Either_Type()
        {
            // Same value, but on different sides.
            var val = new object();
            var e1 = Either<object, object>.Left(val);
            var e2 = Either<object, object>.Right(val);

            AssertEquality(e1, e2, areEqual: false);
        }

        [Fact]
        public void Equality_Respects_Value()
        {
            // Different value (but same type) on same side.
            var e1 = new Either<Left, Right>(new Left());
            var e2 = new Either<Left, Right>(new Left());

            AssertEquality(e1, e2, areEqual: false);
        }

        [Fact]
        public void Equality_Correctly_Compares_Reference_Types()
        {
            var obj = new object();
            var e1 = new Either<object, int>(obj);
            var e2 = new Either<object, int>(obj);

            AssertEquality(e1, e2);
        }

        [Fact]
        public void Equality_Correctly_Compares_Value_Types()
        {
            var e1 = new Either<object, int>(123);
            var e2 = new Either<object, int>(123);

            AssertEquality(e1, e2);
        }

        [Fact]
        public void Equality_Correctly_Compares_Null()
        {
            var e1 = new Either<object?, int>(null);
            var e2 = new Either<object?, int>(null);

            AssertEquality(e1, e2);
        }

        private void AssertEquality<TL, TR>(Either<TL, TR> e1, Either<TL, TR> e2, bool areEqual = true)
        {
            if (areEqual)
            {
                Assert.True(e1.Equals(e2));
                Assert.True(e1.Equals((object)e2));
                Assert.True(((IEquatable<Either<TL, TR>>)e1).Equals(e2));
                Assert.True(e1 == e2);
                Assert.False(e1 != e2);
            }
            else
            {
                Assert.False(e1.Equals(e2));
                Assert.False(e1.Equals((object)e2));
                Assert.False(((IEquatable<Either<TL, TR>>)e1).Equals(e2));
                Assert.False(e1 == e2);
                Assert.True(e1 != e2);
            }
        }

    }

}
