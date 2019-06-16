namespace NCommons.Monads.Tests.Either
{
    using System;
    using Xunit;

    public class MatchTests
    {

        [Fact]
        public void Action_Match_Throws_For_Null()
        {
            var either = new Either<Left, Right>();
            Assert.Throws<ArgumentNullException>(() => either.Match(null, r => { }));
            Assert.Throws<ArgumentNullException>(() => either.Match(l => { }, null));
        }

        [Fact]
        public void Func_Match_Throws_For_Null()
        {
            var either = new Either<Left, Right>();
            Assert.Throws<ArgumentNullException>(() => either.Match<object>(null, r => null));
            Assert.Throws<ArgumentNullException>(() => either.Match<object>(l => null, null));
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

    }

}
