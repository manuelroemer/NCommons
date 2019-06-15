namespace NCommons.Monads.Tests.Either
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Xunit;

    public class TransformTests
    {

        [Fact]
        public void TransformLeft_Throws_For_Null()
        {
            var either = new Either<Left, Right>(new Left());
            Assert.Throws<ArgumentNullException>(() => either.TransformLeft<object>(null));
        }

        [Fact]
        public void TransformRight_Throws_For_Null()
        {
            var either = new Either<Left, Right>(new Right());
            Assert.Throws<ArgumentNullException>(() => either.TransformRight<object>(null));
        }

        [Fact]
        public void TransformLeft_Holds_Transformed_Value_For_Left_Either()
        {
            var either = new Either<double, object>(100.0);
            var trans = either.TransformLeft(dbl => (int)dbl + 5);

            Assert.Equal(105, trans.LeftOrThrow());
        }

        [Fact]
        public void TransformRight_Holds_Transformed_Value_For_Right_Either()
        {
            var either = new Either<object, double>(100.0);
            var trans = either.TransformRight(dbl => (int)dbl + 5);

            Assert.Equal(105, trans.RightOrThrow());
        }

        [Fact]
        public void TransformLeft_Holds_Previous_Right_Value_For_Right_Either()
        {
            var val = new object();
            var either = new Either<double, object>(val);
            var trans = either.TransformLeft(dbl => (int)dbl + 5);

            Assert.Same(val, trans.RightOrThrow());
        }

        [Fact]
        public void TransformRight_Holds_Previous_Left_Value_For_Left_Either()
        {
            var val = new object();
            var either = new Either<object, double>(val);
            var trans = either.TransformRight(dbl => (int)dbl + 5);

            Assert.Same(val, trans.LeftOrThrow());
        }

        [Fact]
        public void Transform_Throws_For_Null()
        {
            var either = new Either<Left, Right>();
            Assert.Throws<ArgumentNullException>(() => either.Transform<Left, Right>(null, r => r));
            Assert.Throws<ArgumentNullException>(() => either.Transform<Left, Right>(l => l, null));
        }

        [Fact]
        public void Transform_Transforms_Left_Either()
        {
            var either = new Either<int, string>(100);
            var trans = either.Transform(
                l => l + 5,
                r => r + "Transformed"
            );

            Assert.Equal(105, trans.LeftOrThrow());
        }

        [Fact]
        public void Transform_Transforms_Right_Either()
        {
            var either = new Either<int, string>("");
            var trans = either.Transform(
                l => l + 5,
                r => r + "Transformed"
            );

            Assert.Equal("Transformed", trans.RightOrThrow());
        }

    }

}
