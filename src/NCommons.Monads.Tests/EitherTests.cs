namespace NCommons.Monads.Tests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Xunit;


    public sealed class EitherTests
    {
        
        /// <summary>Used in the tests as a left value.</summary>
        [Serializable] private sealed class Left
        {
        }

        /// <summary>Used in the tests as a right value.</summary>
        [Serializable] private sealed class Right
        {
        }

        #region Either.Type / Either.IsX

        [Fact]
        public void Type_Is_Left_For_Left_Either()
        {
            var either = new Either<Left, Right>(new Left());
            Assert.Equal(EitherType.Left, either.Type);
        }

        [Fact]
        public void Type_Is_Right_For_Right_Either()
        {
            var either = new Either<Left, Right>(new Right());
            Assert.Equal(EitherType.Right, either.Type);
        }

        [Fact]
        public void IsLeft_Is_True_For_Left_Either()
        {
            var either = new Either<Left, Right>(new Left());
            Assert.True(either.IsLeft);
        }

        [Fact]
        public void IsLeft_Is_False_For_Right_Either()
        {
            var either = new Either<Left, Right>(new Right());
            Assert.False(either.IsLeft);
        }

        [Fact]
        public void IsRight_Is_True_For_Right_Either()
        {
            var either = new Either<Left, Right>(new Right());
            Assert.True(either.IsRight);
        }

        [Fact]
        public void IsRight_Is_False_For_Left_Either()
        {
            var either = new Either<Left, Right>(new Left());
            Assert.False(either.IsRight);
        }

        #endregion

        #region Either.Left() / Either.Right()

        [Fact]
        public void Left_Creates_Correct_Either()
        {
            // Use other types than Left/Right, so that the test is more clear.
            var either = Either<string, int>.Left("");
            Assert.Equal(EitherType.Left, either.Type);
        }

        [Fact]
        public void Right_Creates_Correct_Either()
        {
            // Use other types than Left/Right, so that the test is more clear.
            var either = Either<string, int>.Right(0);
            Assert.Equal(EitherType.Right, either.Type);
        }

        #endregion

        #region Either.Match()

        [Fact]
        public void Action_Match_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>();
            Assert.Throws<ArgumentNullException>(() => either.Match(null, r => { }));
            Assert.Throws<ArgumentNullException>(() => either.Match(l => { }, null));
#nullable enable
        }

        [Fact]
        public void Func_Match_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>();
            Assert.Throws<ArgumentNullException>(() => either.Match<object>(null, r => null));
            Assert.Throws<ArgumentNullException>(() => either.Match<object>(l => null, null));
#nullable enable
        }

        [Fact]
        public void Action_Match_Only_Calls_Left_For_Left_Either()
        {
            bool wasLeftCalled = false;
            bool wasRightCalled = false;
            var either = new Either<Left, Right>(new Left());

            either.Match(
                l => wasLeftCalled = true,
                r => wasRightCalled = true
            );

            Assert.True(wasLeftCalled);
            Assert.False(wasRightCalled);
        }


        [Fact]
        public void Action_Match_Only_Calls_Right_For_Right_Either()
        {
            bool wasLeftCalled = false;
            bool wasRightCalled = false;
            var either = new Either<Left, Right>(new Right());

            either.Match(
                l => wasLeftCalled = true,
                r => wasRightCalled = true
            );

            Assert.False(wasLeftCalled);
            Assert.True(wasRightCalled);
        }

        [Fact]
        public void Func_Match_Only_Calls_Left_For_Left_Either()
        {
            bool wasLeftCalled = false;
            bool wasRightCalled = false;
            var either = new Either<Left, Right>(new Left());

            either.Match<object>(
                l => { wasLeftCalled = true; return l; },
                r => { wasRightCalled = true; return r; }
            );

            Assert.True(wasLeftCalled);
            Assert.False(wasRightCalled);
        }
        
        [Fact]
        public void Func_Match_Only_Calls_Right_For_Right_Either()
        {
            bool wasLeftCalled = false;
            bool wasRightCalled = false;
            var either = new Either<Left, Right>(new Right());

            either.Match<object>(
                l => { wasLeftCalled = true; return l; },
                r => { wasRightCalled = true; return r; }
            );

            Assert.False(wasLeftCalled);
            Assert.True(wasRightCalled);
        }

        [Fact]
        public void Func_Match_Returns_OnLefts_Result()
        {
            var either = new Either<Left, Right>(new Left());
            var expectedResult = new object();

            var matchRes = either.Match(
                l => expectedResult,
                r => "Wrong"
            );

            Assert.Same(expectedResult, matchRes);
        }
        
        [Fact]
        public void Func_Match_Returns_OnRights_Result()
        {
            var either = new Either<Left, Right>(new Right());
            var expectedResult = new object();

            var matchRes = either.Match(
                l => "Wrong",
                r => expectedResult
            );

            Assert.Same(expectedResult, matchRes);
        }

        #endregion

        #region Either.LeftOr() / Either.RightOr()

        [Fact]
        public void Value_LeftOr_Returns_Left_Value_For_Left_Either()
        {
            var expectedValue = new Left();
            var either = new Either<Left, Right>(expectedValue);

            var retrievedValue = either.LeftOr(new Left());

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void Value_LeftOr_Returns_Substitute_For_Right_Either()
        {
            var either = new Either<Left, Right>(new Right());

            var expectedValue = new Left();
            var retrievedValue = either.LeftOr(expectedValue);

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void Value_RightOr_Returns_Right_Value_For_Right_Either()
        {
            var expectedValue = new Right();
            var either = new Either<Left, Right>(expectedValue);

            var retrievedValue = either.RightOr(new Right());

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void Value_RightOr_Returns_Substitute_For_Left_Either()
        {
            var either = new Either<Left, Right>(new Left());

            var expectedValue = new Right();
            var retrievedValue = either.RightOr(expectedValue);

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void Func_LeftOr_Returns_Left_Value_For_Left_Either()
        {
            var expectedValue = new Left();
            var either = new Either<Left, Right>(expectedValue);

            var retrievedValue = either.LeftOr(() => new Left());

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void Func_LeftOr_Returns_Substitute_For_Right_Either()
        {
            var either = new Either<Left, Right>(new Right());

            var expectedValue = new Left();
            var retrievedValue = either.LeftOr(() => expectedValue);

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void Func_RightOr_Returns_Right_Value_For_Right_Either()
        {
            var expectedValue = new Right();
            var either = new Either<Left, Right>(expectedValue);

            var retrievedValue = either.RightOr(() => new Right());

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void Func_RightOr_Returns_Substitute_For_Left_Either()
        {
            var either = new Either<Left, Right>(new Left());

            var expectedValue = new Right();
            var retrievedValue = either.RightOr(() => expectedValue);

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void Func_LeftOr_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>(new Left());
            Assert.Throws<ArgumentNullException>(() => either.LeftOr((Func<Left>)null));
#nullable enable
        }
        
        [Fact]
        public void Func_RightOr_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>(new Right());
            Assert.Throws<ArgumentNullException>(() => either.RightOr((Func<Right>)null));
#nullable enable
        }

        #endregion

        #region Either.LeftOrThrow() / Either.RightOrThrow()

        [Fact]
        public void LeftOrThrow_Returns_Left_Value_For_Left_Either()
        {
            var expectedValue = new Left();
            var either = new Either<Left, Right>(expectedValue);

            var retrievedValue = either.LeftOrThrow();

            Assert.Same(expectedValue, retrievedValue);
        }
        
        [Fact]
        public void RightOrThrow_Returns_Right_Value_For_Right_Either()
        {
            var expectedValue = new Right();
            var either = new Either<Left, Right>(expectedValue);

            var retrievedValue = either.RightOrThrow();

            Assert.Same(expectedValue, retrievedValue);
        }

        [Fact]
        public void LeftOrThrow_Throws_For_Right_Either()
        {
            var either = new Either<Left, Right>(new Right());
            Assert.Throws<UnexpectedEitherTypeException>(() => either.LeftOrThrow());
        }
        
        [Fact]
        public void RightOrThrow_Throws_For_Left_Either()
        {
            var either = new Either<Left, Right>(new Left());
            Assert.Throws<UnexpectedEitherTypeException>(() => either.RightOrThrow());
        }

        [Fact]
        public void LeftOrThrow_Throws_Exception_With_Correct_ActualType()
        {
            var either = new Either<Left, Right>(new Right());
            var ex = (UnexpectedEitherTypeException)Record.Exception(() => either.LeftOrThrow());
            Assert.Equal(EitherType.Right, ex.ActualType);
        }

        [Fact]
        public void RightOrThrow_Throws_Exception_With_Correct_ActualType()
        {
            var either = new Either<Left, Right>(new Left());
            var ex = (UnexpectedEitherTypeException)Record.Exception(() => either.RightOrThrow());
            Assert.Equal(EitherType.Left, ex.ActualType);
        }

        #endregion

        #region Either.IsLeft() / Either.IsRight()

        [Fact]
        public void IfLeft_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>(new Left());
            Assert.Throws<ArgumentNullException>(() => either.IfLeft(null));
#nullable enable
        }
        
        [Fact]
        public void IfRight_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>(new Right());
            Assert.Throws<ArgumentNullException>(() => either.IfRight(null));
#nullable enable
        }

        [Fact]
        public void IfLeft_Runs_Action_For_Left_Either()
        {
            bool wasActionCalled = false;
            var either = new Either<Left, Right>(new Left());
            either.IfLeft(l => { wasActionCalled = true; });
            Assert.True(wasActionCalled);
        }
        
        [Fact]
        public void IfRight_Runs_Action_For_Right_Either()
        {
            bool wasActionCalled = false;
            var either = new Either<Left, Right>(new Right());
            either.IfRight(l => { wasActionCalled = true; });
            Assert.True(wasActionCalled);
        }
        
        [Fact]
        public void IfLeft_Doesnt_Run_Action_For_Right_Either()
        {
            bool wasActionCalled = false;
            var either = new Either<Left, Right>(new Right());
            either.IfLeft(l => { wasActionCalled = true; });
            Assert.False(wasActionCalled);
        }
        
        [Fact]
        public void IfRight_Doesnt_Run_Action_For_Left_Either()
        {
            bool wasActionCalled = false;
            var either = new Either<Left, Right>(new Left());
            either.IfRight(l => { wasActionCalled = true; });
            Assert.False(wasActionCalled);
        }

        #endregion

        #region Either.TransformLeft() / Either.TransformRight() / Either.Transform()

        [Fact]
        public void TransformLeft_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>(new Left());
            Assert.Throws<ArgumentNullException>(() => either.TransformLeft<object>(null));
#nullable enable
        }
        
        [Fact]
        public void TransformRight_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>(new Right());
            Assert.Throws<ArgumentNullException>(() => either.TransformRight<object>(null));
#nullable enable
        }

        [Fact]
        public void TransformLeft_Holds_Transformed_Value_For_Left_Either()
        {
            var either = new Either<double, object>(100.0);
            var trans = either.TransformLeft(dbl => (int)dbl + 5);

            Assert.Equal(105, trans.LeftOrThrow());
        }
        
        [Fact]
        public void TransformRight_Holds_Transformed_Value_For_Right_Either()
        {
            var either = new Either<object, double>(100.0);
            var trans = either.TransformRight(dbl => (int)dbl + 5);

            Assert.Equal(105, trans.RightOrThrow());
        }

        [Fact]
        public void TransformLeft_Holds_Previous_Right_Value_For_Right_Either()
        {
            var val = new object();
            var either = new Either<double, object>(val);
            var trans = either.TransformLeft(dbl => (int)dbl + 5);

            Assert.Same(val, trans.RightOrThrow());
        }
        
        [Fact]
        public void TransformRight_Holds_Previous_Left_Value_For_Left_Either()
        {
            var val = new object();
            var either = new Either<object, double>(val);
            var trans = either.TransformRight(dbl => (int)dbl + 5);

            Assert.Same(val, trans.LeftOrThrow());
        }

        [Fact]
        public void Transform_Throws_For_Null()
        {
#nullable disable
            var either = new Either<Left, Right>();
            Assert.Throws<ArgumentNullException>(() => either.Transform<Left, Right>(null, r => r));
            Assert.Throws<ArgumentNullException>(() => either.Transform<Left, Right>(l => l, null));
#nullable enable
        }

        [Fact]
        public void Transform_Transforms_Left_Either()
        {
            var either = new Either<int, string>(100);
            var trans = either.Transform(
                l => l + 5,
                r => r + "Transformed"
            );

            Assert.Equal(105, trans.LeftOrThrow());
        }

        [Fact]
        public void Transform_Transforms_Right_Either()
        {
            var either = new Either<int, string>("");
            var trans = either.Transform(
                l => l + 5,
                r => r + "Transformed"
            );

            Assert.Equal("Transformed", trans.RightOrThrow());
        }

        #endregion

        #region Equality 

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

        #endregion

        #region Either.GetHashCode()

        [Fact]
        public void GetHashCode_Returns_Same_Code_For_Equal_Eithers()
        {
            var val = new Left();
            var e1 = new Either<Left, Right>(val);
            var e2 = new Either<Left, Right>(val);

            Assert.Equal(e1.GetHashCode(), e2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Returns_Different_Code_For_Different_Either_Types()
        {
            var val = new object();
            var e1 = Either<object, object>.Left(val);
            var e2 = Either<object, object>.Right(val);

            Assert.NotEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Returns_Different_Code_For_Different_Values()
        {
            var e1 = new Either<Left, Right>(new Left());
            var e2 = new Either<Left, Right>(new Left());

            Assert.NotEqual(e1.GetHashCode(), e2.GetHashCode());
        }

        #endregion

        #region Either.ToString()

        [Fact]
        public void Default_ToString_Follows_Format()
        {
            var either = new Either<Left, Right>(new Left());
            Assert.Equal($"Left({new Left().ToString()})", either.ToString());
        }
        
        [Fact]
        public void Default_ToString_Includes_null_Part()
        {
            var either = Either<Left?, Right?>.Right(null);
            Assert.Equal($"Right(null)", either.ToString());
        }

        #endregion

        #region Serialization

        [Fact]
        public void Correctly_Serializes_Left_Either()
        {
            var either = new Either<Left, Right>(new Left());
            var serializedEither = DoSerializationPass(either);

            Assert.True(serializedEither.IsLeft);
            Assert.IsType<Left>(serializedEither.LeftOrThrow());
        }
        
        [Fact]
        public void Correctly_Serializes_Right_Either()
        {
            var either = new Either<Left, Right>(new Right());
            var serializedEither = DoSerializationPass(either);

            Assert.True(serializedEither.IsRight);
            Assert.IsType<Right>(serializedEither.RightOrThrow());
        }

        private Either<TL, TR> DoSerializationPass<TL, TR>(Either<TL, TR> either)
        {
            // Simply serialize and deserialize the either. The formatter doesn't really matter.
            var formatter = new BinaryFormatter();
            using var ms = new MemoryStream();

            formatter.Serialize(ms, either);
            ms.Position = 0;
            return (Either<TL, TR>)formatter.Deserialize(ms);
        }

        #endregion

        #region Implicit Creation

        [Fact]
        public void Implicit_Operator_Creates_Left_Either()
        {
            Either<Left, Right> either = new Left();
            Assert.True(either.IsLeft);
        }
        
        [Fact]
        public void Implicit_Operator_Creates_Right_Either()
        {
            Either<Left, Right> either = new Right();
            Assert.True(either.IsRight);
        }

        #endregion

    }

}
