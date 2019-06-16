namespace NCommons.Monads.Tests.Either
{
    using Xunit;

    public class ImplicitOperatorTests
    {

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

    }

}
