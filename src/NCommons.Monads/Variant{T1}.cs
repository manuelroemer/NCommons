namespace NCommons.Monads
{
    using System;
    using System.Diagnostics;
    using Dummy = System.Object;

    /// <summary>
    ///     A type which is able to hold a single value of type <typeparamref name="T1"/>
    ///     or no value at all and provides methods for matching and retrieving that value.
    /// </summary>
    /// <typeparam name="T1">The type of the first possible value that the variant can hold.</typeparam>
    [Serializable]
    public readonly struct Variant<T1> :
        IVariant,
        IEquatable<Variant<T1>>
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly object? _value;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly VariantType _type;

        /// <inheritdoc/>
        public object? Value => _value;

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        VariantType IVariant.Type => _type;

        /// <inheritdoc/>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Type[] IVariant.GenericValueTypes => new Type[] { typeof(T1) };

        /// <summary>
        ///     Gets a value indicating whether the variant holds a value of one of the specified
        ///     generic type parameters.
        ///     This can only be <see langword="false"/> if the variant was initialized with
        ///     the parameterless constructor.
        /// </summary>
        public bool IsEmpty => _type == VariantType.Empty;

        /// <summary>
        ///     Gets a value indicating whether the variant holds a value of type <typeparamref name="T1"/>.
        /// </summary>
        public bool IsFirst => _type == VariantType.V1;

        /// <summary>
        ///     Initializes a new variant instance which holds the specified <paramref name="value"/>
        ///     of type <typeparamref name="T1"/>.
        /// </summary>
        /// <param name="value">
        ///     The value which will be held by the variant.
        /// </param>
        public Variant(T1 value)
        {
            _value = value;
            _type = VariantType.V1;
        }

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T1"/> from the variant or throws
        ///     an exception if the variant holds another value.
        ///     The result of this function is both stored in <paramref name="value1"/> and returned.
        /// </summary>
        /// <param name="value1">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T1"/>.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value1"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T1"/>.
        /// </exception>
        public T1 GetValue(out T1 value1) =>
            this.GetValueInternal(VariantType.V1, out value1);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T1"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substitute"/>
        ///     is used instead.
        ///     The result of this function is both stored in <paramref name="value1"/> and returned.
        /// </summary>
        /// <param name="substitute">
        ///     An alternative value which is returned if the variant doesn't hold a value of type
        ///     <typeparamref name="T1"/>.
        /// </param>
        /// <param name="value1">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T1"/>, or to <paramref name="substitute"/>
        ///     if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value1"/>.
        /// </returns>
        public T1 GetValueOr(T1 substitute, out T1 value1) =>
            this.GetValueOrInternal(VariantType.V1, substitute, out value1);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T1"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substituteProvider"/>
        ///     function is used to retrieve an alternative value.
        ///     The result of this function is both stored in <paramref name="value1"/> and returned.
        /// </summary>
        /// <param name="substituteProvider">
        ///     A function which returns a substitute value which is returned if the variant
        ///     doesn't hold a value of type <typeparamref name="T1"/>.
        /// </param>
        /// <param name="value1">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T1"/>, or to a value provided by the specified
        ///     <paramref name="substituteProvider"/> function if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value1"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="substituteProvider"/>
        /// </exception>
        public T1 GetValueOr(Func<T1> substituteProvider, out T1 value1) =>
            this.GetValueOrInternal(
                VariantType.V1,
                substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider)),
                out value1
            );

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T1"/> from the variant.
        ///     If the variant holds another value, a default value is retrieved instead.
        ///     The result of this function is both stored in <paramref name="value1"/> and returned.
        /// </summary>
        /// <param name="value1">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T1"/>, or to a default value if the variant
        ///     holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value1"/>.
        /// </returns>
        public T1 GetValueOrDefault(out T1 value1) =>
            this.GetValueOrDefaultInternal(VariantType.V1, out value1);

        /// <summary>
        ///     Attempts to retrieve a value of type <typeparamref name="T1"/> from the variant.
        ///     If the variant holds a value of type <typeparamref name="T1"/>, this value is stored
        ///     in <paramref name="value1"/> and this function returns <see langword="true"/>.
        ///     Otherwise, <paramref name="value1"/> is set to a default value and this function
        ///     returns <see langword="false"/>.
        /// </summary>
        /// <param name="value1">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T1"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the variant holds a value of type <typeparamref name="T1"/>;
        ///     <see langword="false"/> if not.
        /// </returns>
        public bool TryGetValue(out T1 value1) =>
            this.TryGetValueInternal(VariantType.V1, out value1);

        /// <summary>
        ///     Executes one of the specified functions, depending on which value the variant holds.
        /// </summary>
        /// <param name="onValue1">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T1"/>.
        /// </param>
        /// <param name="onEmpty">
        ///     A function to be executed if the variant is empty.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if any of the parameters is <see langword="null"/>.
        /// </exception>
        public void Match(Action<T1> onValue1, Action onEmpty)
        {
            this.MatchInternal<T1, Dummy, Dummy, Dummy, Dummy, Dummy, Dummy, Dummy>(
                onValue1 ?? throw new ArgumentNullException(nameof(onValue1)),
                onValue2: null,
                onValue3: null,
                onValue4: null,
                onValue5: null,
                onValue6: null,
                onValue7: null,
                onValue8: null,
                onEmpty ?? throw new ArgumentNullException(nameof(onEmpty))
            );
        }

        /// <summary>
        ///     Executes one of the specified functions, depending on which value the variant holds,
        ///     and returns the result returned by the function.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the match's result, i.e. the type which all of the functions return.
        /// </typeparam>
        /// <param name="onValue1">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T1"/>.
        /// </param>
        /// <param name="onEmpty">
        ///     A function to be executed if the variant is empty.
        /// </param>
        /// <returns>
        ///     The result of type <typeparamref name="T"/> returned by the function which was executed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if any of the parameters is <see langword="null"/>.
        /// </exception>
        public T Match<T>(Func<T1, T> onValue1, Func<T> onEmpty)
        {
            return this.MatchInternal<T, T1, Dummy, Dummy, Dummy, Dummy, Dummy, Dummy, Dummy>(
                onValue1 ?? throw new ArgumentNullException(nameof(onValue1)),
                onValue2: null,
                onValue3: null,
                onValue4: null,
                onValue5: null,
                onValue6: null,
                onValue7: null,
                onValue8: null,
                onEmpty ?? throw new ArgumentNullException(nameof(onEmpty))
            );
        }

        /// <summary>
        ///     Returns a value indicating whether the <paramref name="obj"/> is a variant which
        ///     holds the same value as this variant.
        /// </summary>
        /// <param name="obj">An object to be compared with this variant.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="obj"/> is a variant of the same type as 
        ///     this one and if two variants hold the same value;
        ///     <see langword="false"/> if not.
        /// </returns>
        public override bool Equals(object obj) =>
            this.EqualsInternal(obj);

        /// <summary>
        ///     Returns a value indicating whether the <paramref name="other"/> variant
        ///     holds the same value as this variant.
        /// </summary>
        /// <param name="other">Another variant to be compared with this one.</param>
        /// <returns>
        ///     <see langword="true"/> if the two variants hold the same value;
        ///     <see langword="false"/> if not.
        /// </returns>
        public bool Equals(Variant<T1> other) =>
            this.EqualsInternal(other);

        /// <summary>
        ///     Returns a unique hash code for this variant.
        /// </summary>
        /// <returns>A unique hash code for this variant.</returns>
        public override int GetHashCode() =>
            this.GetHashCodeInternal();

        /// <summary>
        ///     Returns a string representation of the variant and the value which it holds.
        /// </summary>
        /// <returns>A string representing the variant.</returns>
        public override string ToString() =>
            this.ToStringInternal();

        /// <see cref="Equals(Variant{T1})"/>
        public static bool operator ==(
            Variant<T1> left,
            Variant<T1> right)
        {
            return left.EqualsInternal(right);
        }

        /// <see cref="Equals(Variant{T1})"/>
        public static bool operator !=(
            Variant<T1> left,
            Variant<T1> right)
        {
            return !(left.EqualsInternal(right));
        }

        /// <summary>
        ///     Returns the variant's value of type <typeparamref name="T1"/>, if it holds one.
        ///     Otherwise, throws an exception.
        /// </summary>
        /// <param name="variant">
        ///     The variant whose value should be returned.
        /// </param>
        /// <returns>
        ///     The value of type <typeparamref name="T1"/> which the variant holds.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T1"/>.
        /// </exception>
        public static explicit operator T1(Variant<T1> variant) =>
            variant.GetValueInternal<T1>(VariantType.V1, out _);

    }

}
