namespace NCommons.Monads.Tests.Either
{
    using Xunit;

    public class LeftRightTests
    {

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

    }

}
