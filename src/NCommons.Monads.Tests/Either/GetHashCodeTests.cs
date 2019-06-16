namespace NCommons.Monads.Tests.Either
{
    using Xunit;

    public class GetHashCodeTests
    {

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

    }

}
