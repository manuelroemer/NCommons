namespace NCommons.Monads.Tests.Either
{
    using Xunit;

    public class ToStringTests
    {

        [Fact]
        public void Default_ToString_Follows_Format()
        {
            var either = new Either<Left, Right>(new Left());
            Assert.Equal($"Left({new Left().ToString()})", either.ToString());
        }

        [Fact]
        public void Default_ToString_Includes_null_Part()
        {
            var either = Either<Left, Right>.Right(null);
            Assert.Equal($"Right(null)", either.ToString());
        }

    }

}
