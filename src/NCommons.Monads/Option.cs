using System;
using System.Diagnostics;
using NCommons.Monads.Resources;

namespace NCommons.Monads
{

    /// <summary>
    ///     A wrapper for a value that may or may not be present, similar to .NET's
    ///     <see cref="Nullable{T}"/>, but with support for both value- and reference-types.
    ///     
    ///     See remarks for details.
    /// </summary>
    /// <typeparam name="T">
    ///     The type that the option holds, if it has a value.
    /// </typeparam>
    /// <remarks>
    ///     The option class is inspired by .NET's <see cref="Nullable{T}"/> and by optional values
    ///     in functional programming languages, e.g. OCaml.
    ///     
    ///     An option can either be of type <c>Some</c> (i.e. it holds a value), or of 
    ///     type <c>None</c> (i.e. it doesn't hold a value).
    ///     Depending on this, an option gives you several ways of retrieving a value, depending
    ///     on whether it is <c>Some</c> or <c>None</c>. In fact, it forces you to consider both
    ///     cases, i.e. that it has a value or that it hasn't.
    ///     This is typically an improvement over returning <c>null</c> in code, because you
    ///     cannot forget to check for an inexisting value. You are forced to do so, which will,
    ///     in most cases, lead to more stable applications.
    ///      
    ///     The option's type can be determined by the <see cref="Type"/> property.
    ///     Alternatively, you can use the <see cref="HasValue"/> property to check whether the
    ///     option holds a value or not.
    ///     
    ///     An actual value can be retrieved from the option via functions 
    ///     like <see cref="ValueOr(T)"/>.
    ///     
    ///     There are several ways to construct an option from a value.
    ///     The most important ones are <see cref="Some"/>, which constructs an option that holds
    ///     a value, <see cref="None"/>, which constructs an option that doesn't hold a value and
    ///     <see cref="From{TRef}(TRef)"/>, which dynamically generates an option that is either
    ///     <c>Some</c> or <c>None</c>, depending on whether the specified value is <c>null</c> or
    ///     not.
    ///     
    ///     The option class provides support for C# 8.0's new pattern matching.
    ///     This means that code like this is possible:
    ///     
    ///     <code>
    ///         var value = myOption switch
    ///         {
    ///             (OptionType.Some, var value) => $"The option is Some with value {value}.",
    ///             _                            => $"The option is None."
    ///         };
    ///     </code>
    /// </remarks>
    public readonly struct Option<T> : IEquatable<Option<T>>
    {

        private readonly T _value;
        private readonly OptionType _type;

        /// <summary>
        ///     Gets a value indicating whether the option is of type 
        ///     <see cref="OptionType.Some"/> and thus holds a value.
        /// </summary>
        /// <remarks>
        ///     This property is just another way of checking the option's type and is added
        ///     for consistency with .NET's <see cref="Nullable{T}.HasValue"/> property.
        ///     
        ///     Have a look at the <see cref="Type"/> property for a more specific way of
        ///     determining whether the option is <see cref="OptionType.Some"/> or
        ///     <see cref="OptionType.None"/>.
        /// </remarks>
        public bool HasValue => _type == OptionType.Some;

        /// <summary>
        ///     Gets the type of the option, i.e. whether it is
        ///     <see cref="OptionType.Some"/> or <see cref="OptionType.None"/>.
        /// </summary>
        public OptionType Type => _type;

        private Option(T value)
        {
            Debug.Assert(
                !(value is null),
                "This constructor should only be used to create an option of type Some."
            );

            _type = OptionType.Some;
            _value = value;
        }

        /// <summary>
        ///     Dynamically constructs an option from a reference type.
        ///     The option's type depends on whether the <paramref name="value"/> is <c>null</c>
        ///     or not.
        /// </summary>
        /// <typeparam name="TRef">
        ///     The reference type from which the option should be constructed.
        /// </typeparam>
        /// <param name="value">
        ///     The object from which the option should be constructed.
        /// </param>
        /// <returns>
        ///     If <paramref name="value"/> is <c>null</c>, this returns an option of type
        ///     <see cref="OptionType.None"/>.
        ///     Otherwise, this returns an option of type <see cref="OptionType.Some"/> which holds
        ///     the specified <paramref name="value"/>.
        /// </returns>
        public static Option<T> From<TRef>(TRef value) where TRef : class, T
        {
            return value is null ?
                   None() :
                   Some(value);
        }

        /// <summary>
        ///     Dynamically constructs an option from a nullable value type.
        ///     The option's type depends on whether the <paramref name="value"/> is <c>null</c>
        ///     or not.
        /// </summary>
        /// <typeparam name="TVal">
        ///     The value type from which the option should be constructed.
        /// </typeparam>
        /// <param name="value">
        ///     The object from which the option should be constructed.
        /// </param>
        /// <returns>
        ///     If <paramref name="value"/> is <c>null</c>, this returns an option of type
        ///     <see cref="OptionType.None"/>.
        ///     Otherwise, this returns an option of type <see cref="OptionType.Some"/> which holds
        ///     the specified <paramref name="value"/>.
        /// </returns>
        public static Option<T> From<TVal>(TVal? value) where TVal : struct, T
        {
            return value is null ?
                   None() :
                   Some(value.Value);
        }

        /// <summary>
        ///     Creates an option of type <see cref="OptionType.Some"/> from the specified value.
        /// </summary>
        /// <param name="value">
        ///     The value from which the option will be created.
        ///     This must not be <c>null</c>.
        /// </param>
        /// <returns>
        ///     A new <see cref="Option{T}"/> of type <see cref="OptionType.Some"/>, which holds
        ///     the specified <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="value"/> is <c>null</c>.
        ///     That is, because an option of type <see cref="OptionType.Some"/> must hold a value.
        /// </exception>
        public static Option<T> Some(T value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(
                    nameof(value),
                    ExceptionStrings.Option_SomeValueMustNotBeNull
                );
            }

            return new Option<T>(value);
        }

        /// <summary>
        ///     Creates an option of type <see cref="OptionType.None"/> which doesn't hold any
        ///     value.
        /// </summary>
        /// <returns>
        ///     A new <see cref="Option{T}"/> of type <see cref="OptionType.None"/>, which doesn't
        ///     hold any value.
        /// </returns>
        public static Option<T> None()
        {
            // The default constructor defaults to None.
            return new Option<T>();
        }

        /// <summary>
        ///     Returns either the value that the option holds, or the specified 
        ///     <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">
        ///     A default value to be returned in case the option doesn't hold a value.
        /// </param>
        /// <returns>
        ///     The value that the option holds, or the specified <paramref name="defaultValue"/>.
        /// </returns>
        public T ValueOr(T defaultValue)
        {
            return HasValue ? _value : defaultValue;
        }

        /// <summary>
        ///     Returns either the value that the option holds, or the value provided by the
        ///     specified <paramref name="valueProvider"/> function.
        /// </summary>
        /// <param name="valueProvider">
        ///     A function which provides a default value, in case the option doesn't hold a value.
        /// </param>
        /// <returns>
        ///     The value that the option holds, or a value provided by 
        ///     <paramref name="valueProvider"/>.
        /// </returns>
        public T ValueOr(Func<T> valueProvider)
        {
            if (valueProvider is null)
                throw new ArgumentNullException(nameof(valueProvider));

            return HasValue ? _value : valueProvider();
        }

        /// <summary>
        ///     Returns either the value that the option holds, or the default value of
        ///     <typeparamref name="T"/>, if the option doesn't hold a value.
        /// </summary>
        /// <returns>
        ///     The value that the option holds, or the default value of type 
        ///     <typeparamref name="T"/>.
        /// </returns>
        public T ValueOrDefault()
        {
#nullable disable
            return HasValue ? _value : default;
#nullable enable
        }

        /// <summary>
        ///     Returns a value indicating whether this option equals the specified object.
        /// </summary>
        /// <param name="obj">
        ///     Another object to be compared with this option.
        /// </param>
        /// <returns>
        ///     The comparison done depends on the type of the <paramref name="obj"/> parameter:
        ///     
        ///     * If <paramref name="obj"/> is of type <see cref="Option{T}"/>, this method
        ///       compares the two options with the <see cref="Equals(Option{T})"/> method.
        ///     * Otherwise, this method returns <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Option<T> other)
            {
                return Equals(other);
            }
            return false;
        }

        /// <summary>
        ///     Returns a value indicating whether this option equals the 
        ///     <paramref name="other"/> specified option.
        /// </summary>
        /// <param name="other">Another option to be compared with this option.</param>
        /// <returns>
        ///     <c>true</c> if the options are considered to be equal; <c>false</c> otherwise.
        ///     
        ///     Two options are considered to be equal if they have the same type, i.e. either
        ///     <see cref="OptionType.Some"/> or <see cref="OptionType.None"/> and, if they both
        ///     hold a value, the values are equal.
        /// </returns>
        public bool Equals(Option<T> other)
        {
#nullable disable
            return (_type == other._type) &&
                    _value.Equals(other._value);
#nullable enable
        }

        /// <summary>
        ///     Returns a hash code for this option.
        /// </summary>
        /// <returns>
        ///     An hash code of this instance.
        /// </returns>
        public override int GetHashCode() => this switch
        {
            (OptionType.Some, var value) => value.GetHashCode(),
            _                            => typeof(T).GetHashCode()
        };

        /// <summary>
        ///     Returns a string representation of the option and, if present, it's value.
        /// </summary>
        /// <returns>
        ///     A string representation of the option, similar to
        ///     <c>Some(123)</c> or <c>None()</c>
        /// </returns>
        public override string ToString() => this switch
        {
            (OptionType.Some, var value) => $"{nameof(OptionType.Some)}({value})",
            _                            => $"{nameof(OptionType.None)}()"
        };

        /// <summary>
        ///     Deconstructs the option into its type and, if present, its value for pattern
        ///     matching.
        /// </summary>
        /// <param name="type">
        ///     A parameter which will hold the option's type, i.e.
        ///     <see cref="OptionType.Some"/> or <see cref="OptionType.None"/>.
        /// </param>
        /// <param name="value">
        ///     A parameter which will hold the option's value, if the option was of type
        ///     <see cref="OptionType.Some"/>.
        ///     If the option was of type <see cref="OptionType.None"/>, this will be the default
        ///     value of <typeparamref name="T"/>.
        /// </param>
        public void Deconstruct(out OptionType type, out T value)
        {
            type = _type;
            value = _value;
        }

        /// <summary>
        ///     A shortcut operator for the <see cref="ValueOr(T)"/> function which 
        ///     returns either the value that the <paramref name="option"/> holds, or the specified 
        ///     <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="option">
        ///     The option from which a value should be retrieved.
        /// </param>
        /// <param name="defaultValue">
        ///     A default value to be returned in case the option doesn't hold a value.
        /// </param>
        /// <returns>
        ///     The value that the option holds, or the specified <paramref name="defaultValue"/>.
        /// </returns>
        public static T operator |(Option<T> option, T defaultValue)
        {
            return option.ValueOr(defaultValue);
        }

        /// <summary>
        ///     A shortcut operator for the <see cref="ValueOr(Func{T})"/> function which 
        ///     returns either the value that the <paramref name="option"/> holds, or the value 
        ///     provided by the specified <paramref name="valueProvider"/> function.
        /// </summary>
        /// <param name="option">
        ///     The option from which a value should be retrieved.
        /// </param>
        /// <param name="valueProvider">
        ///     A function which provides a default value, in case the option doesn't hold a value.
        /// </param>
        /// <returns>
        ///     The value that the option holds, or a value provided by 
        ///     <paramref name="valueProvider"/>.
        /// </returns>
        public static T operator |(Option<T> option, Func<T> valueProvider)
        {
            return option.ValueOr(valueProvider);
        }

        /// <summary>
        ///     Returns a value indicating whether the two specified options are equal to each
        ///     other.
        /// </summary>
        /// <param name="left">The first option.</param>
        /// <param name="right">The second option.</param>
        /// <returns>
        ///     <c>true</c> if the options are considered to be equal; <c>false</c> otherwise.
        ///     
        ///     Two options are considered to be equal if they have the same type, i.e. either
        ///     <see cref="OptionType.Some"/> or <see cref="OptionType.None"/> and, if they both
        ///     hold a value, the values are equal.
        /// </returns>
        public static bool operator ==(Option<T> left, Option<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Returns a value indicating whether the two specified options are unequal to each
        ///     other.
        /// </summary>
        /// <param name="left">The first option.</param>
        /// <param name="right">The second option.</param>
        /// <returns>
        ///     <c>true</c> if the options are considered to be unequal; <c>false</c> otherwise.
        ///     
        ///     Two options are considered to be unequal if they have different types, e.g.
        ///     <see cref="OptionType.Some"/> and <see cref="OptionType.None"/> or, if they both
        ///     hold a value, the values are unequal.
        /// </returns>
        public static bool operator !=(Option<T> left, Option<T> right)
        {
            return !(left == right);
        }

    }

}
