using System;
using Xunit;

namespace NCommons.Tests
{

    public class VerifierTests
    {

        [Fact]
        public static void VerifyNullThrows()
        {
            object nullObj = null;
            var ex = Record.Exception(() => nullObj.VerifyNotNull());

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

    }

}
