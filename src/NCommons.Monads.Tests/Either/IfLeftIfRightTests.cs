namespace NCommons.Monads.Tests.Either
{
    using System;
    using Xunit;

    public class IfLeftIfRightTests
    {

        [Fact]
        public void IfLeft_Throws_For_Null()
        {
            var either = new Either<Left, Right>(new Left());
            Assert.Throws<ArgumentNullException>(() => either.IfLeft(null));
        }

        [Fact]
        public void IfRight_Throws_For_Null()
        {
            var either = new Either<Left, Right>(new Right());
            Assert.Throws<ArgumentNullException>(() => either.IfRight(null));
        }

        [Fact]
        public void IfLeft_Runs_Action_For_Left_Either()
        {
            bool wasActionCalled = false;
            var either = new Either<Left, Right>(new Left());
            either.IfLeft(l => { wasActionCalled = true; });
            Assert.True(wasActionCalled);
        }

        [Fact]
        public void IfRight_Runs_Action_For_Right_Either()
        {
            bool wasActionCalled = false;
            var either = new Either<Left, Right>(new Right());
            either.IfRight(l => { wasActionCalled = true; });
            Assert.True(wasActionCalled);
        }

        [Fact]
        public void IfLeft_Doesnt_Run_Action_For_Right_Either()
        {
            bool wasActionCalled = false;
            var either = new Either<Left, Right>(new Right());
            either.IfLeft(l => { wasActionCalled = true; });
            Assert.False(wasActionCalled);
        }

        [Fact]
        public void IfRight_Doesnt_Run_Action_For_Left_Either()
        {
            bool wasActionCalled = false;
            var either = new Either<Left, Right>(new Left());
            either.IfRight(l => { wasActionCalled = true; });
            Assert.False(wasActionCalled);
        }

    }

}
