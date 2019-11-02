namespace NCommons.Monads.Tests
{
    using System;
    using Xunit;

    public class OptionalTTests
    {

        public class EmptyTests
        {

            [Fact]
            public void Is_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                Assert.False(opt.HasValue);
            }

        }

        public class ConstructorTests
        {

            [Fact]
            public void Default_Initializes_Empty_Optional()
            {
                var opt = new Optional<int>();
                Assert.False(opt.HasValue);
            }

            [Fact]
            public void Value_Initializes_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                Assert.True(opt.HasValue);
                Assert.Equal(123, opt.GetValue());
            }

        }

        public class HasValueTests
        {

            [Fact]
            public void Returns_True_For_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                Assert.True(opt.HasValue);
            }

            [Fact]
            public void Returns_False_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                Assert.False(opt.HasValue);
            }

        }

        public class GetValueTests
        {

            [Fact]
            public void Returns_Value_For_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                var value = opt.GetValue();
                Assert.Equal(123, value);
            }

            [Fact]
            public void Throws_InvalidOperationException_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                Assert.Throws<InvalidOperationException>(() => opt.GetValue());
            }

        }

        public class GetValueOrDefaultTests
        {

            [Fact]
            public void Returns_Value_For_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                var value = opt.GetValueOrDefault();
                Assert.Equal(123, value);
            }

            [Fact]
            public void Returns_Default_Value_For_Non_Empty_Optional_And_Value_Type()
            {
                var opt = Optional<int>.Empty;
                var value = opt.GetValueOrDefault();
                Assert.Equal(default, value);
            }
            
            [Fact]
            public void Returns_Default_Value_For_Non_Empty_Optional_And_Reference_Type()
            {
                var opt = Optional<string>.Empty;
                var value = opt.GetValueOrDefault();
                Assert.Equal(default, value);
            }

        }

        public class GetValueOrTests
        {

            [Fact]
            public void Substitute_Returns_Value_For_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                var value = opt.GetValueOr(0xDEAD);
                Assert.Equal(123, value);
            }

            [Fact]
            public void Substitute_Returns_Substitute_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                var value = opt.GetValueOr(0xDEAD);
                Assert.Equal(0xDEAD, value);
            }
            
            [Fact]
            public void SubstituteProvider_Returns_Value_For_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                var value = opt.GetValueOr(() => 0xDEAD);
                Assert.Equal(123, value);
            }

            [Fact]
            public void SubstituteProvider_Returns_Substitute_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                var value = opt.GetValueOr(() => 0xDEAD);
                Assert.Equal(0xDEAD, value);
            }

            [Fact]
            public void SubstituteProvider_Throws_ArgumentNullException()
            {
                var opt = Optional<int>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.GetValueOr((Func<int>)null!));
            }

        }

        public class TryGetValueTests
        {

            [Fact]
            public void Returns_True_And_Value_For_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                var result = opt.TryGetValue(out var value);
                Assert.True(result);
                Assert.Equal(123, value);
            }

            [Fact]
            public void Returns_False_And_Default_Value_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                var result = opt.TryGetValue(out var value);
                Assert.False(result);
                Assert.Equal(default, value);
            }

        }

        public class IfValueTests
        {

            [Fact]
            public void Invokes_Function_For_Non_Empty_Optional()
            {
                var wasCalled = false;
                var opt = new Optional<int>(123);
                opt.IfValue(_ => wasCalled = true);
                Assert.True(wasCalled);
            }

            [Fact]
            public void Doesnt_Invoke_Function_For_Empty_Optional()
            {
                var wasCalled = false;
                var opt = Optional<int>.Empty;
                opt.IfValue(_ => wasCalled = true);
                Assert.False(wasCalled);
            }

            [Fact]
            public void Passes_Value_To_Function()
            {
                var opt = new Optional<int>(123);
                opt.IfValue(value => Assert.Equal(123, value));
            }

        }
        
        public class IfEmptyTests
        {

            [Fact]
            public void Doesnt_Invoke_Function_For_Non_Empty_Optional()
            {
                var wasCalled = false;
                var opt = new Optional<int>(123);
                opt.IfEmpty(() => wasCalled = true);
                Assert.False(wasCalled);
            }

            [Fact]
            public void Invokes_Function_For_Empty_Optional()
            {
                var wasCalled = false;
                var opt = Optional<int>.Empty;
                opt.IfEmpty(() => wasCalled = true);
                Assert.True(wasCalled);
            }

        }

        public class MatchTests
        {

            [Fact]
            public void Void_Throws_ArgumentNullException()
            {
                var opt = Optional<int>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.Match(null!, () => { }));
                Assert.Throws<ArgumentNullException>(() => opt.Match(_ => { }, null!));
            }
            
            [Fact]
            public void TResult_Throws_ArgumentNullException()
            {
                var opt = Optional<int>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.Match<string>(null!, () => ""));
                Assert.Throws<ArgumentNullException>(() => opt.Match<string>(_ => "", null!));
            }

            [Fact]
            public void Void_Matches_Non_Empty_Optional()
            {
                var matchedValue = false;
                var matchedEmpty = false;
                var opt = new Optional<int>(123);
                
                opt.Match(_ => { matchedValue = true; }, () => { matchedEmpty = true; });
                Assert.True(matchedValue);
                Assert.False(matchedEmpty);
            }
            
            [Fact]
            public void TResult_Matches_Non_Empty_Optional()
            {
                var matchedValue = false;
                var matchedEmpty = false;
                var opt = new Optional<int>(123);
                
                opt.Match<bool>(_ => matchedValue = true, () => matchedEmpty = true);
                Assert.True(matchedValue);
                Assert.False(matchedEmpty);
            }

            [Fact]
            public void Void_Matches_Empty_Optional()
            {
                var matchedValue = false;
                var matchedEmpty = false;
                var opt = Optional<int>.Empty;

                opt.Match(_ => { matchedValue = true; }, () => { matchedEmpty = true; });
                Assert.False(matchedValue);
                Assert.True(matchedEmpty);
            }
            
            [Fact]
            public void TResult_Matches_Empty_Optional()
            {
                var matchedValue = false;
                var matchedEmpty = false;
                var opt = Optional<int>.Empty;

                opt.Match<bool>(_ => matchedValue = true, () => matchedEmpty = true);
                Assert.False(matchedValue);
                Assert.True(matchedEmpty);
            }
            
        }

        public class MapTests
        {

            [Fact]
            public void Throws_ArgumentNullException()
            {
                var opt = Optional<int>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.Map<object>(null!));
            }

            [Fact]
            public void Returns_Mapping_Result_As_Optional()
            {
                var opt = new Optional<int>(123);
                var res = opt.Map(val => val.ToString());
                Assert.Equal("123", res.GetValue());
            }

            [Fact]
            public void Returns_Empty_Optional_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                var res = opt.Map(val => val.ToString());
                Assert.False(res.HasValue);
            }

        }
        
        public class FlatMapTests
        {

            [Fact]
            public void Throws_ArgumentNullException()
            {
                var opt = Optional<int>.Empty;
                Assert.Throws<ArgumentNullException>(() => opt.FlatMap<object>(null!));
            }

            [Fact]
            public void Returns_Mapping_Result_As_Optional()
            {
                var opt = new Optional<int>(123);
                var res = opt.FlatMap(val => new Optional<string>(val.ToString()));
                Assert.Equal("123", res.GetValue());
            }

            [Fact]
            public void Returns_Empty_Optional_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                var res = opt.FlatMap(val => new Optional<string>(val.ToString()));
                Assert.False(res.HasValue);
            }

        }

        public class EqualityTests
        {

            public static TheoryData<Optional<int?>, Optional<int?>> EqualOptionalsData => new TheoryData<Optional<int?>, Optional<int?>>()
            {
                { Optional<int?>.Empty, Optional<int?>.Empty },
                { 123, 123 }
            };

            public static TheoryData<Optional<int?>, Optional<int?>> UnequalOptionalsData => new TheoryData<Optional<int?>, Optional<int?>>()
            {
                { Optional<int?>.Empty, new Optional<int?>(123) },
                { new Optional<int?>(123), Optional<int?>.Empty },
                { Optional<int?>.Empty, null },
                { null, Optional<int?>.Empty },
                { Optional<int?>.Empty, new Optional<int?>(null) },
                { new Optional<int?>(null), Optional<int?>.Empty },
                { new Optional<int?>(123), new Optional<int?>(456) },
            };

            [Theory]
            [MemberData(nameof(EqualOptionalsData))]
            public void Optional_Equality(Optional<int?> left, Optional<int?> right)
            {
                Assert.True(left.Equals((object)right));
                Assert.True(left.Equals(right));
                Assert.True(left == right);
                Assert.False(left != right);
                Assert.Equal(left.GetHashCode(), right.GetHashCode());
            }
            
            [Theory]
            [MemberData(nameof(UnequalOptionalsData))]
            public void Optional_Inequality(Optional<int?> left, Optional<int?> right)
            {
                Assert.False(left.Equals((object)right));
                Assert.False(left.Equals(right));
                Assert.False(left == right);
                Assert.True(left != right);
                Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
            }

            public static TheoryData<Optional<int?>, int?> EqualValuesData => new TheoryData<Optional<int?>, int?>()
            {
                { new Optional<int?>(123), 123 },
                { new Optional<int?>(null), null },
            };
            
            public static TheoryData<Optional<int?>, int?> UnequalValuesData => new TheoryData<Optional<int?>, int?>()
            {
                { Optional<int?>.Empty, 123 },
                { new Optional<int?>(123), null },
                { new Optional<int?>(123), 456 },
            };

            [Theory]
            [MemberData(nameof(EqualValuesData))]
            public void Value_Equality(Optional<int?> left, int? right)
            {
                Assert.True(left.Equals((object?)right));
                Assert.True(left.Equals(right));
                Assert.True(left == right);
                Assert.True(right == left);
                Assert.False(left != right);
                Assert.False(right != left);
                Assert.Equal(left.GetHashCode(), right.GetHashCode());
            }

            [Theory]
            [MemberData(nameof(UnequalValuesData))]
            public void Value_Inequality(Optional<int?> left, int? right)
            {
                Assert.False(left.Equals((object?)right));
                Assert.False(left.Equals(right));
                Assert.False(left == right);
                Assert.False(right == left);
                Assert.True(left != right);
                Assert.True(right != left);
                Assert.NotEqual(left.GetHashCode(), right?.GetHashCode() ?? 0);
            }

        }

        public class ToStringTests
        {

            [Fact]
            public void Returns_Empty_String_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                Assert.Equal(string.Empty, opt.ToString());
            }
            
            [Fact]
            public void Returns_Empty_String_If_Held_Value_Is_Null()
            {
                var opt = new Optional<int?>(null);
                Assert.Equal(string.Empty, opt.ToString());
            }
            
            [Fact]
            public void Returns_ToString_Of_Held_Value()
            {
                var opt = new Optional<int>(123);
                Assert.Equal(123.ToString(), opt.ToString());
            }

        }

        public class ImplicitOperatorTests
        {

            [Fact]
            public void Initializes_Non_Empty_Optional()
            {
                Optional<int> opt = 123;
                Assert.True(opt.HasValue);
                Assert.Equal(123, opt.GetValue());
            }

        }

        public class ExplicitOperatorTests
        {

            [Fact]
            public void Returns_Value_For_Non_Empty_Optional()
            {
                var opt = new Optional<int>(123);
                var value = (int)opt;
                Assert.Equal(123, value);
            }

            [Fact]
            public void Throws_InvalidOperationException_For_Empty_Optional()
            {
                var opt = Optional<int>.Empty;
                Assert.Throws<InvalidOperationException>(() => (int)opt);
            }

        }

    }

}
