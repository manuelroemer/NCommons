using System;
using Xunit;

namespace NCommons.Tests
{

    public class VerifierTests
    {

        [Fact]
        public static void VerifyNullThrows()
        {
            var obj = new object();
            var ex = Record.Exception(() => obj.VerifyNull());

            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public static void VerifyNotNullThrows()
        {
            object nullObj = null;
            var ex = Record.Exception(() => nullObj.VerifyNotNull());

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

    }

}
