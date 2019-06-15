namespace NCommons.Monads.Tests.Either
{
    using System;
    using Xunit;

    public class LeftOrRightOrTests
    {

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
            var either = new Either<Left, Right>(new Left());
            Assert.Throws<ArgumentNullException>(() => either.LeftOr((Func<Left>)null));
        }

        [Fact]
        public void Func_RightOr_Throws_For_Null()
        {
            var either = new Either<Left, Right>(new Right());
            Assert.Throws<ArgumentNullException>(() => either.RightOr((Func<Right>)null));
        }

    }

}
