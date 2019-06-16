namespace NCommons.Monads
{
    using System;
    using Dummy = System.Object;

    /// <summary>
    ///     A type which is able to hold one of up to 5 possible values and provides methods for
    ///     matching and retrieving that value.
    /// </summary>
    /// <typeparam name="T1">The type of the first possible value that the variant can hold.</typeparam>
    /// <typeparam name="T2">The type of the second possible value that the variant can hold.</typeparam>
    /// <typeparam name="T3">The type of the third possible value that the variant can hold.</typeparam>
    /// <typeparam name="T4">The type of the fourth possible value that the variant can hold.</typeparam>
    /// <typeparam name="T5">The type of the fifth possible value that the variant can hold.</typeparam>
    public readonly struct Variant<T1, T2, T3, T4, T5> :
        IVariant,
        IEquatable<Variant<T1, T2, T3, T4, T5>>
    {

        private readonly object? _value;
        private readonly VariantType _type;

        /// <inheritdoc/>
        public object? Value => _value;

        /// <inheritdoc/>
        VariantType IVariant.Type => _type;

        /// <inheritdoc/>
        Type[] IVariant.GenericValueTypes => new Type[]
        {
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5),
        };

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
        ///     Gets a value indicating whether the variant holds a value of type <typeparamref name="T2"/>.
        /// </summary>
        public bool IsSecond => _type == VariantType.V2;

        /// <summary>
        ///     Gets a value indicating whether the variant holds a value of type <typeparamref name="T3"/>.
        /// </summary>
        public bool IsThird => _type == VariantType.V3;

        /// <summary>
        ///     Gets a value indicating whether the variant holds a value of type <typeparamref name="T4"/>.
        /// </summary>
        public bool IsFourth => _type == VariantType.V4;

        /// <summary>
        ///     Gets a value indicating whether the variant holds a value of type <typeparamref name="T5"/>.
        /// </summary>
        public bool IsFifth => _type == VariantType.V5;

        /// <summary>
        ///     Initializes a new variant instance which holds the specified <paramref name="value"/>
        ///     of type <typeparamref name="T1"/>.
        /// </summary>
        /// <param name="value">
        ///     The value which will be held by the variant.
        /// </param>
        public Variant(T1 value)
            : this(value, VariantType.V1) { }

        /// <summary>
        ///     Initializes a new variant instance which holds the specified <paramref name="value"/>
        ///     of type <typeparamref name="T2"/>.
        /// </summary>
        /// <param name="value">
        ///     The value which will be held by the variant.
        /// </param>
        public Variant(T2 value)
            : this(value, VariantType.V2) { }

        /// <summary>
        ///     Initializes a new variant instance which holds the specified <paramref name="value"/>
        ///     of type <typeparamref name="T3"/>.
        /// </summary>
        /// <param name="value">
        ///     The value which will be held by the variant.
        /// </param>
        public Variant(T3 value)
            : this(value, VariantType.V3) { }

        /// <summary>
        ///     Initializes a new variant instance which holds the specified <paramref name="value"/>
        ///     of type <typeparamref name="T4"/>.
        /// </summary>
        /// <param name="value">
        ///     The value which will be held by the variant.
        /// </param>
        public Variant(T4 value)
            : this(value, VariantType.V4) { }

        /// <summary>
        ///     Initializes a new variant instance which holds the specified <paramref name="value"/>
        ///     of type <typeparamref name="T5"/>.
        /// </summary>
        /// <param name="value">
        ///     The value which will be held by the variant.
        /// </param>
        public Variant(T5 value)
            : this(value, VariantType.V5) { }

        private Variant(object? value, VariantType type)
        {
            _value = value;
            _type = type;
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
        ///     Retrieves a value of type <typeparamref name="T2"/> from the variant or throws
        ///     an exception if the variant holds another value.
        ///     The result of this function is both stored in <paramref name="value2"/> and returned.
        /// </summary>
        /// <param name="value2">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T2"/>.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value2"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T2"/>.
        /// </exception>
        public T2 GetValue(out T2 value2) =>
            this.GetValueInternal(VariantType.V2, out value2);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T3"/> from the variant or throws
        ///     an exception if the variant holds another value.
        ///     The result of this function is both stored in <paramref name="value3"/> and returned.
        /// </summary>
        /// <param name="value3">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T3"/>.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value3"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T3"/>.
        /// </exception>
        public T3 GetValue(out T3 value3) =>
            this.GetValueInternal(VariantType.V3, out value3);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T4"/> from the variant or throws
        ///     an exception if the variant holds another value.
        ///     The result of this function is both stored in <paramref name="value4"/> and returned.
        /// </summary>
        /// <param name="value4">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T4"/>.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value4"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T4"/>.
        /// </exception>
        public T4 GetValue(out T4 value4) =>
            this.GetValueInternal(VariantType.V4, out value4);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T5"/> from the variant or throws
        ///     an exception if the variant holds another value.
        ///     The result of this function is both stored in <paramref name="value5"/> and returned.
        /// </summary>
        /// <param name="value5">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T5"/>.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value5"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T5"/>.
        /// </exception>
        public T5 GetValue(out T5 value5) =>
            this.GetValueInternal(VariantType.V5, out value5);

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
        ///     Retrieves a value of type <typeparamref name="T2"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substitute"/>
        ///     is used instead.
        ///     The result of this function is both stored in <paramref name="value2"/> and returned.
        /// </summary>
        /// <param name="substitute">
        ///     An alternative value which is returned if the variant doesn't hold a value of type
        ///     <typeparamref name="T2"/>.
        /// </param>
        /// <param name="value2">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T2"/>, or to <paramref name="substitute"/>
        ///     if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value2"/>.
        /// </returns>
        public T2 GetValueOr(T2 substitute, out T2 value2) =>
            this.GetValueOrInternal(VariantType.V2, substitute, out value2);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T3"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substitute"/>
        ///     is used instead.
        ///     The result of this function is both stored in <paramref name="value3"/> and returned.
        /// </summary>
        /// <param name="substitute">
        ///     An alternative value which is returned if the variant doesn't hold a value of type
        ///     <typeparamref name="T3"/>.
        /// </param>
        /// <param name="value3">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T3"/>, or to <paramref name="substitute"/>
        ///     if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value3"/>.
        /// </returns>
        public T3 GetValueOr(T3 substitute, out T3 value3) =>
            this.GetValueOrInternal(VariantType.V3, substitute, out value3);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T4"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substitute"/>
        ///     is used instead.
        ///     The result of this function is both stored in <paramref name="value4"/> and returned.
        /// </summary>
        /// <param name="substitute">
        ///     An alternative value which is returned if the variant doesn't hold a value of type
        ///     <typeparamref name="T4"/>.
        /// </param>
        /// <param name="value4">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T4"/>, or to <paramref name="substitute"/>
        ///     if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value4"/>.
        /// </returns>
        public T4 GetValueOr(T4 substitute, out T4 value4) =>
            this.GetValueOrInternal(VariantType.V4, substitute, out value4);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T5"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substitute"/>
        ///     is used instead.
        ///     The result of this function is both stored in <paramref name="value5"/> and returned.
        /// </summary>
        /// <param name="substitute">
        ///     An alternative value which is returned if the variant doesn't hold a value of type
        ///     <typeparamref name="T5"/>.
        /// </param>
        /// <param name="value5">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T5"/>, or to <paramref name="substitute"/>
        ///     if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value5"/>.
        /// </returns>
        public T5 GetValueOr(T5 substitute, out T5 value5) =>
            this.GetValueOrInternal(VariantType.V5, substitute, out value5);

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
        public T1 GetValueOr(Func<T1> substituteProvider, out T1 value1) =>
            this.GetValueOrInternal(
                VariantType.V1,
                substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider)),
                out value1
            );

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T2"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substituteProvider"/>
        ///     function is used to retrieve an alternative value.
        ///     The result of this function is both stored in <paramref name="value2"/> and returned.
        /// </summary>
        /// <param name="substituteProvider">
        ///     A function which returns a substitute value which is returned if the variant
        ///     doesn't hold a value of type <typeparamref name="T2"/>.
        /// </param>
        /// <param name="value2">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T2"/>, or to a value provided by the specified
        ///     <paramref name="substituteProvider"/> function if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value2"/>.
        /// </returns>
        public T2 GetValueOr(Func<T2> substituteProvider, out T2 value2) =>
            this.GetValueOrInternal(
                VariantType.V2,
                substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider)),
                out value2
            );

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T3"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substituteProvider"/>
        ///     function is used to retrieve an alternative value.
        ///     The result of this function is both stored in <paramref name="value3"/> and returned.
        /// </summary>
        /// <param name="substituteProvider">
        ///     A function which returns a substitute value which is returned if the variant
        ///     doesn't hold a value of type <typeparamref name="T3"/>.
        /// </param>
        /// <param name="value3">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T3"/>, or to a value provided by the specified
        ///     <paramref name="substituteProvider"/> function if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value3"/>.
        /// </returns>
        public T3 GetValueOr(Func<T3> substituteProvider, out T3 value3) =>
            this.GetValueOrInternal(
                VariantType.V3,
                substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider)),
                out value3
            );

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T4"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substituteProvider"/>
        ///     function is used to retrieve an alternative value.
        ///     The result of this function is both stored in <paramref name="value4"/> and returned.
        /// </summary>
        /// <param name="substituteProvider">
        ///     A function which returns a substitute value which is returned if the variant
        ///     doesn't hold a value of type <typeparamref name="T4"/>.
        /// </param>
        /// <param name="value4">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T4"/>, or to a value provided by the specified
        ///     <paramref name="substituteProvider"/> function if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value4"/>.
        /// </returns>
        public T4 GetValueOr(Func<T4> substituteProvider, out T4 value4) =>
            this.GetValueOrInternal(
                VariantType.V4,
                substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider)),
                out value4
            );

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T5"/> from the variant.
        ///     If the variant holds another value, the specified <paramref name="substituteProvider"/>
        ///     function is used to retrieve an alternative value.
        ///     The result of this function is both stored in <paramref name="value5"/> and returned.
        /// </summary>
        /// <param name="substituteProvider">
        ///     A function which returns a substitute value which is returned if the variant
        ///     doesn't hold a value of type <typeparamref name="T5"/>.
        /// </param>
        /// <param name="value5">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T5"/>, or to a value provided by the specified
        ///     <paramref name="substituteProvider"/> function if the variant holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value5"/>.
        /// </returns>
        public T5 GetValueOr(Func<T5> substituteProvider, out T5 value5) =>
            this.GetValueOrInternal(
                VariantType.V5,
                substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider)),
                out value5
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
        ///     Retrieves a value of type <typeparamref name="T2"/> from the variant.
        ///     If the variant holds another value, a default value is retrieved instead.
        ///     The result of this function is both stored in <paramref name="value2"/> and returned.
        /// </summary>
        /// <param name="value2">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T2"/>, or to a default value if the variant
        ///     holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value2"/>.
        /// </returns>
        public T2 GetValueOrDefault(out T2 value2) =>
            this.GetValueOrDefaultInternal(VariantType.V2, out value2);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T3"/> from the variant.
        ///     If the variant holds another value, a default value is retrieved instead.
        ///     The result of this function is both stored in <paramref name="value3"/> and returned.
        /// </summary>
        /// <param name="value3">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T3"/>, or to a default value if the variant
        ///     holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value3"/>.
        /// </returns>
        public T3 GetValueOrDefault(out T3 value3) =>
            this.GetValueOrDefaultInternal(VariantType.V3, out value3);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T4"/> from the variant.
        ///     If the variant holds another value, a default value is retrieved instead.
        ///     The result of this function is both stored in <paramref name="value4"/> and returned.
        /// </summary>
        /// <param name="value4">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T4"/>, or to a default value if the variant
        ///     holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value4"/>.
        /// </returns>
        public T4 GetValueOrDefault(out T4 value4) =>
            this.GetValueOrDefaultInternal(VariantType.V4, out value4);

        /// <summary>
        ///     Retrieves a value of type <typeparamref name="T5"/> from the variant.
        ///     If the variant holds another value, a default value is retrieved instead.
        ///     The result of this function is both stored in <paramref name="value5"/> and returned.
        /// </summary>
        /// <param name="value5">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T5"/>, or to a default value if the variant
        ///     holds another value.
        /// </param>
        /// <returns>
        ///     The final result which was assigned to <paramref name="value5"/>.
        /// </returns>
        public T5 GetValueOrDefault(out T5 value5) =>
            this.GetValueOrDefaultInternal(VariantType.V5, out value5);

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
        ///     Attempts to retrieve a value of type <typeparamref name="T2"/> from the variant.
        ///     If the variant holds a value of type <typeparamref name="T2"/>, this value is stored
        ///     in <paramref name="value2"/> and this function returns <see langword="true"/>.
        ///     Otherwise, <paramref name="value2"/> is set to a default value and this function
        ///     returns <see langword="false"/>.
        /// </summary>
        /// <param name="value2">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T2"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the variant holds a value of type <typeparamref name="T2"/>;
        ///     <see langword="false"/> if not.
        /// </returns>
        public bool TryGetValue(out T2 value2) =>
            this.TryGetValueInternal(VariantType.V2, out value2);

        /// <summary>
        ///     Attempts to retrieve a value of type <typeparamref name="T3"/> from the variant.
        ///     If the variant holds a value of type <typeparamref name="T3"/>, this value is stored
        ///     in <paramref name="value3"/> and this function returns <see langword="true"/>.
        ///     Otherwise, <paramref name="value3"/> is set to a default value and this function
        ///     returns <see langword="false"/>.
        /// </summary>
        /// <param name="value3">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T3"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the variant holds a value of type <typeparamref name="T3"/>;
        ///     <see langword="false"/> if not.
        /// </returns>
        public bool TryGetValue(out T3 value3) =>
            this.TryGetValueInternal(VariantType.V3, out value3);

        /// <summary>
        ///     Attempts to retrieve a value of type <typeparamref name="T4"/> from the variant.
        ///     If the variant holds a value of type <typeparamref name="T4"/>, this value is stored
        ///     in <paramref name="value4"/> and this function returns <see langword="true"/>.
        ///     Otherwise, <paramref name="value4"/> is set to a default value and this function
        ///     returns <see langword="false"/>.
        /// </summary>
        /// <param name="value4">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T4"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the variant holds a value of type <typeparamref name="T4"/>;
        ///     <see langword="false"/> if not.
        /// </returns>
        public bool TryGetValue(out T4 value4) =>
            this.TryGetValueInternal(VariantType.V4, out value4);

        /// <summary>
        ///     Attempts to retrieve a value of type <typeparamref name="T5"/> from the variant.
        ///     If the variant holds a value of type <typeparamref name="T5"/>, this value is stored
        ///     in <paramref name="value5"/> and this function returns <see langword="true"/>.
        ///     Otherwise, <paramref name="value5"/> is set to a default value and this function
        ///     returns <see langword="false"/>.
        /// </summary>
        /// <param name="value5">
        ///     An out parameter which will be set to the variant's value if the variant holds
        ///     a value of type <typeparamref name="T5"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the variant holds a value of type <typeparamref name="T5"/>;
        ///     <see langword="false"/> if not.
        /// </returns>
        public bool TryGetValue(out T5 value5) =>
            this.TryGetValueInternal(VariantType.V5, out value5);

        /// <summary>
        ///     Executes one of the specified functions, depending on which value the variant holds.
        /// </summary>
        /// <param name="onValue1">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T1"/>.
        /// </param>
        /// <param name="onValue2">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T2"/>.
        /// </param>
        /// <param name="onValue3">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T3"/>.
        /// </param>
        /// <param name="onValue4">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T4"/>.
        /// </param>
        /// <param name="onValue5">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T5"/>.
        /// </param>
        /// <param name="onEmpty">
        ///     A function to be executed if the variant is empty.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if any of the parameters is <see langword="null"/>.
        /// </exception>
        public void Match(
            Action<T1> onValue1, Action<T2> onValue2, Action<T3> onValue3, Action<T4> onValue4,
            Action<T5> onValue5,
            Action onEmpty)
        {
            this.MatchInternal<T1, T2, T3, T4, T5, Dummy, Dummy, Dummy>(
                onValue1 ?? throw new ArgumentNullException(nameof(onValue1)),
                onValue2 ?? throw new ArgumentNullException(nameof(onValue2)),
                onValue3 ?? throw new ArgumentNullException(nameof(onValue3)),
                onValue4 ?? throw new ArgumentNullException(nameof(onValue4)),
                onValue5 ?? throw new ArgumentNullException(nameof(onValue5)),
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
        /// <param name="onValue2">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T2"/>.
        /// </param>
        /// <param name="onValue3">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T3"/>.
        /// </param>
        /// <param name="onValue4">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T4"/>.
        /// </param>
        /// <param name="onValue5">
        ///     A function to be executed if the variant holds a value of type <typeparamref name="T5"/>.
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
        public T Match<T>(
            Func<T1, T> onValue1, Func<T2, T> onValue2, Func<T3, T> onValue3, Func<T4, T> onValue4,
            Func<T5, T> onValue5,
            Func<T> onEmpty)
        {
            return this.MatchInternal<T, T1, T2, T3, T4, T5, Dummy, Dummy, Dummy>(
                onValue1 ?? throw new ArgumentNullException(nameof(onValue1)),
                onValue2 ?? throw new ArgumentNullException(nameof(onValue2)),
                onValue3 ?? throw new ArgumentNullException(nameof(onValue3)),
                onValue4 ?? throw new ArgumentNullException(nameof(onValue4)),
                onValue5 ?? throw new ArgumentNullException(nameof(onValue5)),
                onValue6: null,
                onValue7: null,
                onValue8: null,
                onEmpty
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
        public bool Equals(Variant<T1, T2, T3, T4, T5> other) =>
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

        /// <see cref="Equals(Variant{T1, T2, T3, T4, T5})"/>
        public static bool operator ==(
            Variant<T1, T2, T3, T4, T5> left,
            Variant<T1, T2, T3, T4, T5> right)
        {
            return left.EqualsInternal(right);
        }

        /// <see cref="Equals(Variant{T1, T2, T3, T4, T5})"/>
        public static bool operator !=(
            Variant<T1, T2, T3, T4, T5> left,
            Variant<T1, T2, T3, T4, T5> right)
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
        public static explicit operator T1(Variant<T1, T2, T3, T4, T5> variant) =>
            variant.GetValueInternal<T1>(VariantType.V1, out _);

        /// <summary>
        ///     Returns the variant's value of type <typeparamref name="T2"/>, if it holds one.
        ///     Otherwise, throws an exception.
        /// </summary>
        /// <param name="variant">
        ///     The variant whose value should be returned.
        /// </param>
        /// <returns>
        ///     The value of type <typeparamref name="T2"/> which the variant holds.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T2"/>.
        /// </exception>
        public static explicit operator T2(Variant<T1, T2, T3, T4, T5> variant) =>
            variant.GetValueInternal<T2>(VariantType.V2, out _);

        /// <summary>
        ///     Returns the variant's value of type <typeparamref name="T3"/>, if it holds one.
        ///     Otherwise, throws an exception.
        /// </summary>
        /// <param name="variant">
        ///     The variant whose value should be returned.
        /// </param>
        /// <returns>
        ///     The value of type <typeparamref name="T3"/> which the variant holds.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T3"/>.
        /// </exception>
        public static explicit operator T3(Variant<T1, T2, T3, T4, T5> variant) =>
            variant.GetValueInternal<T3>(VariantType.V3, out _);

        /// <summary>
        ///     Returns the variant's value of type <typeparamref name="T4"/>, if it holds one.
        ///     Otherwise, throws an exception.
        /// </summary>
        /// <param name="variant">
        ///     The variant whose value should be returned.
        /// </param>
        /// <returns>
        ///     The value of type <typeparamref name="T4"/> which the variant holds.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T4"/>.
        /// </exception>
        public static explicit operator T4(Variant<T1, T2, T3, T4, T5> variant) =>
            variant.GetValueInternal<T4>(VariantType.V4, out _);

        /// <summary>
        ///     Returns the variant's value of type <typeparamref name="T5"/>, if it holds one.
        ///     Otherwise, throws an exception.
        /// </summary>
        /// <param name="variant">
        ///     The variant whose value should be returned.
        /// </param>
        /// <returns>
        ///     The value of type <typeparamref name="T5"/> which the variant holds.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the variant is empty or if it holds a value which is not of type
        ///     <typeparamref name="T5"/>.
        /// </exception>
        public static explicit operator T5(Variant<T1, T2, T3, T4, T5> variant) =>
            variant.GetValueInternal<T5>(VariantType.V5, out _);

    }

}
