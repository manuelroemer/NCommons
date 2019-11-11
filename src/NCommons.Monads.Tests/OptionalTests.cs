namespace NCommons.Monads.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Xunit;

    public class OptionalTests
    {

        public class FromNullableTests
        {

            [Fact]
            public void Value_Type_Returns_Empty_Optional_For_Null()
            {
                int? value = null;
                var opt = Optional.FromNullable(value);
                Assert.False(opt.HasValue);
            }

            [Fact]
            public void Value_Type_Returns_Non_Empty_Optional_For_Value()
            {
                int? value = 123;
                var opt = Optional.FromNullable(value);
                Assert.True(opt.HasValue);
                Assert.Equal(123, opt.GetValue());
            }
            
            [Fact]
            public void Reference_Type_Returns_Empty_Optional_For_Null()
            {
                string? value = null;
                var opt = Optional.FromNullable(value);
                Assert.False(opt.HasValue);
            }

            [Fact]
            public void Reference_Type_Returns_Non_Empty_Optional_For_Value()
            {
                string? value = "123";
                var opt = Optional.FromNullable(value);
                Assert.True(opt.HasValue);
                Assert.Equal("123", opt.GetValue());
            }

        }

        public class ToNullableTests
        {

            [Fact]
            public void Returns_Null_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                var value = opt.ToNullable();
                Assert.False(value.HasValue);
            }

            [Fact]
            public void Returns_Value_For_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                var value = opt.ToNullable();
                Assert.True(value.HasValue);
            }

        }

        public class ToEmptyIfNullTests
        {

            [Fact]
            public void Value_Type_Returns_Empty_Optional_If_Null()
            {
                var opt = new Optional<int?>(null);
                var res = opt.ToEmptyIfNull();
                Assert.False(res.HasValue);
            }

            [Fact]
            public void Value_Type_Returns_Empty_Optional_For_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                var res = opt.ToEmptyIfNull();
                Assert.False(res.HasValue);
            }

            [Fact]
            public void Value_Type_Returns_Non_Empty_Optional_If_Not_Null()
            {
                var opt = new Optional<int?>(123);
                var res = opt.ToEmptyIfNull();
                Assert.True(res.HasValue);
                Assert.Equal(123, res.GetValue());
            }
            
            [Fact]
            public void Reference_Type_Returns_Empty_Optional_If_Null()
            {
                var opt = new Optional<string?>(null);
                var res = opt.ToEmptyIfNull();
                Assert.False(res.HasValue);
            }

            [Fact]
            public void Reference_Type_Returns_Empty_Optional_For_Empty_Optional()
            {
                var opt = Optional<string?>.Empty;
                var res = opt.ToEmptyIfNull();
                Assert.False(res.HasValue);
            }

            [Fact]
            public void Reference_Type_Returns_Non_Empty_Optional_If_Not_Null()
            {
                var opt = new Optional<string?>("123");
                var res = opt.ToEmptyIfNull();
                Assert.True(res.HasValue);
                Assert.Equal("123", res.GetValue());
            }

        }

    }

}
