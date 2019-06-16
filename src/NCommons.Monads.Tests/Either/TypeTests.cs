namespace NCommons.Monads.Tests.Either
{
    using Xunit;

    public class TypeTests
    {

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

    }

}
