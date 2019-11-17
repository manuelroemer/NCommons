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

        public class GetNonNullValueTests
        {

            [Fact]
            public void Value_Type_Returns_Value()
            {
                var opt = new Optional<int?>(123);
                Assert.Equal(123, opt.GetNonNullValue());
            }

            [Fact]
            public void Value_Type_Throws_InvalidOperationException_For_Null_Value()
            {
                var opt = new Optional<int?>(null);
                Assert.Throws<InvalidOperationException>(() => opt.GetNonNullValue());
            }

            [Fact]
            public void Value_Type_Throws_InvalidOperationException_For_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                Assert.Throws<InvalidOperationException>(() => opt.GetNonNullValue());
            }
            
            [Fact]
            public void ReferenceType_Returns_Value()
            {
                var opt = new Optional<string?>("123");
                Assert.Equal("123", opt.GetNonNullValue());
            }

            [Fact]
            public void ReferenceType_Throws_InvalidOperationException_For_Null_Value()
            {
                var opt = new Optional<string?>(null);
                Assert.Throws<InvalidOperationException>(() => opt.GetNonNullValue());
            }

            [Fact]
            public void ReferenceType_Throws_InvalidOperationException_For_Empty_Optional()
            {
                var opt = Optional<string?>.Empty;
                Assert.Throws<InvalidOperationException>(() => opt.GetNonNullValue());
            }

        }

        public class GetNonNullValueOrDefaultTests
        {

            [Fact]
            public void Value_Type_Returns_Value()
            {
                var opt = new Optional<int?>(123);
                Assert.Equal(123, opt.GetNonNullValueOrDefault());
            }

            [Fact]
            public void Value_Type_Returns_Default_Value_For_Null_Value()
            {
                var opt = new Optional<int?>(null);
                Assert.Equal(default, opt.GetNonNullValueOrDefault());
            }
            
            [Fact]
            public void Value_Type_Returns_Default_Value_For_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                Assert.Equal(default, opt.GetNonNullValueOrDefault());
            }

        }

        public class GetNonNullValueOrTests
        {

            [Fact]
            public void Value_Type_Substitute_Returns_Value()
            {
                var opt = new Optional<int?>(123);
                Assert.Equal(123, opt.GetNonNullValueOr(456));
            }

            [Fact]
            public void Value_Type_Substitute_Returns_Substitute_For_Null_Value()
            {
                var opt = new Optional<int?>(null);
                Assert.Equal(456, opt.GetNonNullValueOr(456));
            }

            [Fact]
            public void Value_Type_Substitute_Returns_Substitute_For_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                Assert.Equal(456, opt.GetNonNullValueOr(456));
            }
            
            [Fact]
            public void Value_Type_SubstituteProvider_Returns_Value()
            {
                var opt = new Optional<int?>(123);
                Assert.Equal(123, opt.GetNonNullValueOr(() => 456));
            }

            [Fact]
            public void Value_Type_SubstituteProvider_Returns_Substitute_For_Null_Value()
            {
                var opt = new Optional<int?>(null);
                Assert.Equal(456, opt.GetNonNullValueOr(() => 456));
            }

            [Fact]
            public void Value_Type_SubstituteProvider_Returns_Substitute_For_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                Assert.Equal(456, opt.GetNonNullValueOr(() => 456));
            }
            
            [Fact]
            public void Value_Type_SubstituteProvider_Throws_ArgumentNullException()
            {
                var opt = Optional<int?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.GetNonNullValueOr((Func<int>?)null!));
            }
            
            [Fact]
            public void Reference_Type_Substitute_Returns_Value()
            {
                var opt = new Optional<string?>("123");
                Assert.Equal("123", opt.GetNonNullValueOr("456"));
            }

            [Fact]
            public void Reference_Type_Substitute_Returns_Substitute_For_Null_Value()
            {
                var opt = new Optional<string?>(null);
                Assert.Equal("456", opt.GetNonNullValueOr("456"));
            }

            [Fact]
            public void Reference_Type_Substitute_Returns_Substitute_For_Empty_Optional()
            {
                var opt = Optional<string?>.Empty;
                Assert.Equal("456", opt.GetNonNullValueOr("456"));
            }
            
            [Fact]
            public void Reference_Type_SubstituteProvider_Returns_Value()
            {
                var opt = new Optional<string?>("123");
                Assert.Equal("123", opt.GetNonNullValueOr(() => "456"));
            }

            [Fact]
            public void Reference_Type_SubstituteProvider_Returns_Substitute_For_Null_Value()
            {
                var opt = new Optional<string?>(null);
                Assert.Equal("456", opt.GetNonNullValueOr(() => "456"));
            }

            [Fact]
            public void Reference_Type_SubstituteProvider_Returns_Substitute_For_Empty_Optional()
            {
                var opt = Optional<string?>.Empty;
                Assert.Equal("456", opt.GetNonNullValueOr(() => "456"));
            }

            [Fact]
            public void Reference_Type_SubstituteProvider_Throws_ArgumentNullException()
            {
                var opt = Optional<string?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.GetNonNullValueOr((Func<string>?)null!));
            }

        }

        public class TryGetNonNullValueTests
        {

            [Fact]
            public void Value_Type_Returns_True_And_Value()
            {
                var opt = new Optional<int?>(123);
                var result = opt.TryGetNonNullValue(out var value);
                Assert.True(result);
                Assert.Equal(123, value);
            }

            [Fact]
            public void Value_Type_Returns_False_And_Default_Value_For_Null_Value()
            {
                var opt = new Optional<int?>(null);
                var result = opt.TryGetNonNullValue(out var value);
                Assert.False(result);
                Assert.Equal(default, value);
            }
            
            [Fact]
            public void Value_Type_Returns_False_And_Default_Value_For_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                var result = opt.TryGetNonNullValue(out var value);
                Assert.False(result);
                Assert.Equal(default, value);
            }
            
            [Fact]
            public void Reference_Type_Returns_True_And_Value()
            {
                var opt = new Optional<string?>("123");
                var result = opt.TryGetNonNullValue(out var value);
                Assert.True(result);
                Assert.Equal("123", value);
            }

            [Fact]
            public void Reference_Type_Returns_False_And_Default_Value_For_Null_Value()
            {
                var opt = new Optional<string?>(null);
                var result = opt.TryGetNonNullValue(out var value);
                Assert.False(result);
                Assert.Equal(default, value);
            }
            
            [Fact]
            public void Reference_Type_Returns_False_And_Default_Value_For_Empty_Optional()
            {
                var opt = Optional<string?>.Empty;
                var result = opt.TryGetNonNullValue(out var value);
                Assert.False(result);
                Assert.Equal(default, value);
            }

        }

        public class MatchNonNullTests
        {

            [Fact]
            public void Value_Type_Void_Matches_Value()
            {
                var wasCalled = true;
                var opt = new Optional<int?>(123);
                opt.MatchNonNull(
                    v => { wasCalled = true; Assert.Equal(123, v); },
                    () => throw new Exception()
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Value_Type_Void_Matches_Null_Value()
            {
                var wasCalled = true;
                var opt = new Optional<int?>(null);
                opt.MatchNonNull(
                    v => throw new Exception(),
                    () => { wasCalled = true; }
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Value_Type_Void_Matches_Empty_Optional()
            {
                var wasCalled = true;
                var opt = Optional<int?>.Empty;
                opt.MatchNonNull(
                    v => throw new Exception(),
                    () => { wasCalled = true; }
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Value_Type_Void_Throws_ArgumentNullException()
            {
                var opt = Optional<int?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.MatchNonNull(null!, () => { }));
                Assert.Throws<ArgumentNullException>(() => opt.MatchNonNull(_ => { }, null!));
            }
            
            [Fact]
            public void Reference_Type_Void_Matches_Value()
            {
                var wasCalled = true;
                var opt = new Optional<string?>("123");
                opt.MatchNonNull(
                    v => { wasCalled = true; Assert.Equal("123", v); },
                    () => throw new Exception()
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Reference_Type_Void_Matches_Null_Value()
            {
                var wasCalled = true;
                var opt = new Optional<string?>(null);
                opt.MatchNonNull(
                    v => throw new Exception(),
                    () => { wasCalled = true; }
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Reference_Type_Void_Matches_Empty_Optional()
            {
                var wasCalled = true;
                var opt = Optional<string?>.Empty;
                opt.MatchNonNull(
                    v => throw new Exception(),
                    () => { wasCalled = true; }
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Reference_Type_Void_Throws_ArgumentNullException()
            {
                var opt = Optional<string?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.MatchNonNull(null!, () => { }));
                Assert.Throws<ArgumentNullException>(() => opt.MatchNonNull(_ => { }, null!));
            }
            
            [Fact]
            public void Value_Type_TResult_Matches_Value()
            {
                var opt = new Optional<int?>(123);
                var wasCalled = opt.MatchNonNull(
                    v => { Assert.Equal(123, v); return true; },
                    () => throw new Exception()
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Value_Type_TResult_Matches_Null_Value()
            {
                var opt = new Optional<int?>(null);
                var wasCalled = opt.MatchNonNull(
                    v => throw new Exception(),
                    () => true
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Value_Type_TResult_Matches_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                var wasCalled = opt.MatchNonNull(
                    v => throw new Exception(),
                    () => true
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Value_Type_TResult_Throws_ArgumentNullException()
            {
                var opt = Optional<int?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.MatchNonNull(null!, () => true));
                Assert.Throws<ArgumentNullException>(() => opt.MatchNonNull(_ => true, null!));
            }
            
            [Fact]
            public void Reference_Type_TResult_Matches_Value()
            {
                var opt = new Optional<string?>("123");
                var wasCalled = opt.MatchNonNull(
                    v => { Assert.Equal("123", v); return true; },
                    () => throw new Exception()
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Reference_Type_TResult_Matches_Null_Value()
            {
                var opt = new Optional<string?>(null);
                var wasCalled = opt.MatchNonNull(
                    v => throw new Exception(),
                    () => true
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Reference_Type_TResult_Matches_Empty_Optional()
            {
                var opt = Optional<string?>.Empty;
                var wasCalled = opt.MatchNonNull(
                    v => throw new Exception(),
                    () => true
                );
                Assert.True(wasCalled);
            }
            
            [Fact]
            public void Reference_Type_TResult_Throws_ArgumentNullException()
            {
                var opt = Optional<string?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.MatchNonNull(null!, () => true));
                Assert.Throws<ArgumentNullException>(() => opt.MatchNonNull(_ => true, null!));
            }

        }

        public class MapNonNullTests
        {

            [Fact]
            public void Value_Type_Returns_Mapping_Result_For_Value()
            {
                var opt = new Optional<int?>(123);
                var res = opt.MapNonNull(val => val.ToString());
                Assert.Equal("123", res.GetValue());
            }

            [Fact]
            public void Value_Type_Returns_Empty_Optional_For_Null_Value()
            {
                var opt = new Optional<int?>(null);
                var res = opt.MapNonNull<int, string>(val => throw new Exception());
                Assert.False(res.HasValue);
            }
            
            [Fact]
            public void Value_Type_Returns_Empty_Optional_For_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                var res = opt.MapNonNull<int, string>(val => throw new Exception());
                Assert.False(res.HasValue);
            }
            
            [Fact]
            public void Value_Type_Throws_ArgumentNullException()
            {
                var opt = Optional<int?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.MapNonNull<int, string>(null!));
            }
            
            [Fact]
            public void Reference_Type_Returns_Mapping_Result_For_Value()
            {
                var opt = new Optional<string?>("123");
                var res = opt.MapNonNull(val => 123);
                Assert.Equal(123, res.GetValue());
            }

            [Fact]
            public void Reference_Type_Returns_Empty_Optional_For_Null_Value()
            {
                var opt = new Optional<string?>(null);
                var res = opt.MapNonNull<string, int>(val => throw new Exception());
                Assert.False(res.HasValue);
            }
            
            [Fact]
            public void Reference_Type_Returns_Empty_Optional_For_Empty_Optional()
            {
                var opt = Optional<string?>.Empty;
                var res = opt.MapNonNull<string, int>(val => throw new Exception());
                Assert.False(res.HasValue);
            }
            
            [Fact]
            public void Reference_Type_Throws_ArgumentNullException()
            {
                var opt = Optional<string?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.MapNonNull<string, int>(null!));
            }

        }

        public class FlatMapNonNullTests
        {

            [Fact]
            public void Value_Type_Returns_Mapping_Result_For_Value()
            {
                var opt = new Optional<int?>(123);
                var res = opt.FlatMapNonNull(val => new Optional<string>(val.ToString()));
                Assert.Equal("123", res.GetValue());
            }

            [Fact]
            public void Value_Type_Returns_Empty_Optional_For_Null_Value()
            {
                var opt = new Optional<int?>(null);
                var res = opt.FlatMapNonNull<int, string>(val => throw new Exception());
                Assert.False(res.HasValue);
            }

            [Fact]
            public void Value_Type_Returns_Empty_Optional_For_Empty_Optional()
            {
                var opt = Optional<int?>.Empty;
                var res = opt.FlatMapNonNull<int, string>(val => throw new Exception());
                Assert.False(res.HasValue);
            }

            [Fact]
            public void Value_Type_Throws_ArgumentNullException()
            {
                var opt = Optional<int?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.FlatMapNonNull<int, string>(null!));
            }
            
            [Fact]
            public void Reference_Type_Returns_Mapping_Result_For_Value()
            {
                var opt = new Optional<string?>("123");
                var res = opt.FlatMapNonNull(val => new Optional<int>(123));
                Assert.Equal(123, res.GetValue());
            }

            [Fact]
            public void Reference_Type_Returns_Empty_Optional_For_Null_Value()
            {
                var opt = new Optional<string?>(null);
                var res = opt.FlatMapNonNull<string, int>(val => throw new Exception());
                Assert.False(res.HasValue);
            }

            [Fact]
            public void Reference_Type_Returns_Empty_Optional_For_Empty_Optional()
            {
                var opt = Optional<string?>.Empty;
                var res = opt.FlatMapNonNull<string, int>(val => throw new Exception());
                Assert.False(res.HasValue);
            }

            [Fact]
            public void Reference_Type_Throws_ArgumentNullException()
            {
                var opt = Optional<string?>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.FlatMapNonNull<string, int>(null!));
            }

        }

    }

}
