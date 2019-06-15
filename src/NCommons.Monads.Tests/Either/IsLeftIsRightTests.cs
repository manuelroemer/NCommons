namespace NCommons.Monads.Tests.Either
{
    using Xunit;

    public class IsLeftIsRightTests
    {

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

    }

}
