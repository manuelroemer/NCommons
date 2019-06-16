namespace NCommons.Monads.Tests.Either
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using Xunit;

    public class SerializationTests
    {

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

    }

}
