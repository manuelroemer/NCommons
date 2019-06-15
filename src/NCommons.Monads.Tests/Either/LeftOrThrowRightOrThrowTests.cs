namespace NCommons.Monads.Tests.Either
{
    using Xunit;

    public class LeftOrThrowRightOrThrowTests
    {

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

    }

}
