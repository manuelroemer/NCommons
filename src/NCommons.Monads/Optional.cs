namespace NCommons.Monads
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using NCommons.Monads.Resources;

    /// <summary>
    ///     Defines static utility functions and extension methods which assist with the creation of
    ///     and interaction with <see cref="Optional{T}"/> instances.
    /// </summary>
#pragma warning disable CA1716 // Identifiers should not match keywords
    public static class Optional
#pragma warning restore CA1716
    {

        /// <summary>
        ///     Creates a non-empty optional holding the specified value if is not <see langword="null"/>.
        ///     Otherwise, returns an empty optional.
        /// </summary>
        /// <typeparam name="T">The type to be held by the optional.</typeparam>
        /// <param name="value">
        ///     The value from which an optional should be created if it is not <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     A non-empty optional holding the value if it is not <see langword="null"/>.
        ///     Otherwise, an empty optional.
        /// </returns>
        public static Optional<T> FromNullable<T>(T? value) where T : struct
        {
            return value.HasValue
                ? new Optional<T>(value.Value)
                : Optional<T>.Empty;
        }

        /// <inheritdoc cref="FromNullable{T}(T?)"/>
        public static Optional<T> FromNullable<T>(T? value) where T : class
        {
            return !(value is null)
                ? new Optional<T>(value)
                : Optional<T>.Empty;
        }

        /// <summary>
        ///     Converts the optional to a <see cref="Nullable{T}"/>.
        ///     This returns the held value if the optional is non-empty or <see langword="null"/>
        ///     if the optional is empty.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional to be converted to a <see cref="Nullable{T}"/>.</param>
        /// <returns>
        ///     The held value if the optional is non-empty.
        ///     <see langword="null"/> if the optional is empty.
        /// </returns>
        public static T? ToNullable<T>(this Optional<T> optional) where T : struct
        {
            // GetValueOrDefault() doesn't do a HasValue comparison and is thus better than GetValue().
            return optional.HasValue ? optional.GetValueOrDefault() : (Nullable<T>)null;
        }

        /// <summary>
        ///     Returns an empty optional if the current optional's held value is <see langword="null"/>.
        ///     Otherwise, returns a non-empty optional holding the same value as the current optional.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <returns>
        ///     An empty optional if the current optional's held value is <see langword="null"/>.
        ///     Otherwise, a non-empty optional holding the same value as the current optional.
        /// </returns>
        public static Optional<T> ToEmptyIfNull<T>(this Optional<T?> optional) where T : struct
        {
            var value = optional.GetValueOrDefault();
            return !(value is null)
                ? new Optional<T>(value.Value)
                : Optional<T>.Empty;
        }

        /// <inheritdoc cref="ToEmptyIfNull{T}(Optional{T?})"/>
        public static Optional<T> ToEmptyIfNull<T>(this Optional<T?> optional) where T : class
        {
            var value = optional.GetValueOrDefault();
            return !(value is null)
                ? new Optional<T>(value)
                : Optional<T>.Empty;
        }

        /// <summary>
        ///     Returns the optional's held value if it is not <see langword="null"/>
        ///     and throws an exception if the optional is either empty or holds a value which is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <returns>
        ///     The held value if the optional holds a value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     The optional is empty or holds a value which is <see langword="null"/>.
        /// </exception>
        /// <seealso cref="Optional{T}.GetValue"/>
        public static T GetNonNullValue<T>(this Optional<T?> optional) where T : struct
        {
            return optional.GetValue() ?? throw new InvalidOperationException(ExceptionStrings.Optional_ValueIsNull);
        }

        /// <inheritdoc cref="GetNonNullValue{T}(Optional{T?})"/>
        public static T GetNonNullValue<T>(this Optional<T?> optional) where T : class
        {
            return optional.GetValue() ?? throw new InvalidOperationException(ExceptionStrings.Optional_ValueIsNull);
        }

        /// <summary>
        ///     Returns the optional's held value if it is not <see langword="null"/>
        ///     or the default value of type <typeparamref name="T"/> if the optional is either empty
        ///     or holds a value which is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <returns>
        ///     The held value if the optional holds a value which is not <see langword="null"/>
        ///     or the default value of type <typeparamref name="T"/> if the optional is either
        ///     empty or holds a value which is <see langword="null"/>.
        /// </returns>
        /// <seealso cref="Optional{T}.GetValueOrDefault"/>
        public static T GetNonNullValueOrDefault<T>(this Optional<T?> optional) where T : struct
        // This method only makes sense for structs, because default(class) == null.
        {
            return optional.GetValueOrDefault() ?? default;
        }

        /// <summary>
        ///     Returns the optional's held value if it is not <see langword="null"/>
        ///     or the specified <paramref name="substitute"/> value if the optional is either empty
        ///     or holds a value which is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <param name="substitute">
        ///     A value to be returned if the optional is empty or holds a value which is <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     The held value if the optional holds a value which is not <see langword="null"/>
        ///     or <paramref name="substitute"/> if the optional is either empty
        ///     or holds a value which is <see langword="null"/>.
        /// </returns>
        /// <seealso cref="Optional{T}.GetValueOr(T)"/>
        public static T GetNonNullValueOr<T>(this Optional<T?> optional, T substitute) where T : struct
        {
            return optional.GetValueOrDefault() ?? substitute;
        }

        /// <inheritdoc cref="GetNonNullValueOr{T}(Optional{T?}, T)"/>
        public static T GetNonNullValueOr<T>(this Optional<T?> optional, T substitute) where T : class
        {
            return optional.GetValueOrDefault() ?? substitute;
        }

        /// <summary>
        ///     Returns the optional's held value if it is not <see langword="null"/>
        ///     or a value returned by the specified <paramref name="substituteProvider"/> function
        ///     if the optional is either empty or holds a value which is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <param name="substituteProvider">
        ///     A function which returns a value which is supposed to be returned if the optional is empty.
        /// </param>
        /// <returns>
        ///     The held value if the optional holds a value which is not <see langword="null"/>
        ///     or a value returned by the specified <paramref name="substituteProvider"/> function
        ///     if the optional is either empty or holds a value which is <see langword="null"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="substituteProvider"/>
        /// </exception>
        /// <seealso cref="Optional{T}.GetValueOr(Func{T})"/>
        public static T GetNonNullValueOr<T>(this Optional<T?> optional, Func<T> substituteProvider) where T : struct
        {
            _ = substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider));
            return optional.GetValueOrDefault() ?? substituteProvider();
        }

        /// <inheritdoc cref="GetNonNullValueOr{T}(Optional{T?}, Func{T})"/>
        public static T GetNonNullValueOr<T>(this Optional<T?> optional, Func<T> substituteProvider) where T : class
        {
            _ = substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider));
            return optional.GetValueOrDefault() ?? substituteProvider();
        }

        /// <summary>
        ///     Attempts to retrieve a non-<see langword="null"/> value from the optional and
        ///     returns a value indicating whether the retrieval was successful, i.e. if the
        ///     optional actually held a non-<see langword="null"/> value.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <param name="value">
        ///     A parameter which will hold the optional's non-<see langword="null"/>-value, if it
        ///     held one.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the optional held a non-<see langword="null"/> value;
        ///     <see langword="false"/> if the optional is either empty or holds a value which is <see langword="null"/>.
        /// </returns>
        /// <seealso cref="Optional{T}.TryGetValue(out T)"/>
        public static bool TryGetNonNullValue<T>(this Optional<T?> optional, out T value) where T : struct
        {
            var heldValue = optional.GetValueOrDefault();
            value = heldValue ?? default;
            return !(heldValue is null);
        }

        /// <inheritdoc cref="TryGetNonNullValue{T}(Optional{T?}, out T)"/>
        public static bool TryGetNonNullValue<T>(this Optional<T?> optional, [NotNullWhen(true)] out T? value) where T : class
        {
            var heldValue = optional.GetValueOrDefault();
            value = heldValue ?? default!;
            return !(heldValue is null);
        }

        /// <summary>
        ///     Executes the <paramref name="onValue"/> function if the optional holds a value which
        ///     is not <see langword="null"/> or the <paramref name="onNullOrEmpty"/> function if
        ///     the optional is either empty or holds a value which is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <param name="onValue">
        ///     The function to be invoked if the optional holds a value which is not <see langword="null"/>.
        /// </param>
        /// <param name="onNullOrEmpty">
        ///     The function to be invoked if the optional is either empty or holds a value which is
        ///     <see langword="null"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onValue"/>
        ///     * <paramref name="onNullOrEmpty"/>
        /// </exception>
        /// <seealso cref="Optional{T}.Match(Action{T}, Action)"/>
        public static void MatchNonNull<T>(this Optional<T?> optional, Action<T> onValue, Action onNullOrEmpty) where T : struct
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            _ = onNullOrEmpty ?? throw new ArgumentNullException(nameof(onNullOrEmpty));

            var value = optional.GetValueOrDefault();
            if (!(value is null))
            {
                onValue(value.Value);
            }
            else
            {
                onNullOrEmpty();
            }
        }

        /// <inheritdoc cref="MatchNonNull{T}(Optional{T?}, Action{T}, Action)"/>
        public static void MatchNonNull<T>(this Optional<T?> optional, Action<T> onValue, Action onNullOrEmpty) where T : class
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            _ = onNullOrEmpty ?? throw new ArgumentNullException(nameof(onNullOrEmpty));

            var value = optional.GetValueOrDefault();
            if (!(value is null))
            {
                onValue(value);
            }
            else
            {
                onNullOrEmpty();
            }
        }

        /// <summary>
        ///     Executes the <paramref name="onValue"/> function if the optional holds a value which
        ///     is not <see langword="null"/> or the <paramref name="onNullOrEmpty"/> function if
        ///     the optional is either empty or holds a value which is <see langword="null"/>.
        ///     This method returns the returned value of the invoked function.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <typeparam name="TResult">
        ///     The type of the result which gets returned by the <paramref name="onValue"/> and
        ///     <paramref name="onNullOrEmpty"/> functions.
        /// </typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <param name="onValue">
        ///     The function to be invoked if the optional holds a value which is not <see langword="null"/>.
        /// </param>
        /// <param name="onNullOrEmpty">
        ///     The function to be invoked if the optional is either empty or holds a value which is
        ///     <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     The returned value of the <paramref name="onValue"/> function if the optional
        ///     holds a value which is not <see langword="null"/> or
        ///     the returned value of the <paramref name="onNullOrEmpty"/> function if the optional
        ///     is either empty or holds a value which is <see langword="null"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onValue"/>
        ///     * <paramref name="onNullOrEmpty"/>
        /// </exception>
        /// <seealso cref="Optional{T}.Match(Action{T}, Action)"/>
        public static TResult MatchNonNull<T, TResult>(
            this Optional<T?> optional,
            Func<T, TResult> onValue,
            Func<TResult> onNullOrEmpty) where T : struct
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            _ = onNullOrEmpty ?? throw new ArgumentNullException(nameof(onNullOrEmpty));

            var value = optional.GetValueOrDefault();
            return !(value is null)
                ? onValue(value.Value)
                : onNullOrEmpty();
        }

        /// <inheritdoc cref="MatchNonNull{T, TResult}(Optional{T?}, Func{T, TResult}, Func{TResult})"/>
        public static TResult MatchNonNull<T, TResult>(
            this Optional<T?> optional,
            Func<T, TResult> onValue,
            Func<TResult> onNullOrEmpty) where T : class
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            _ = onNullOrEmpty ?? throw new ArgumentNullException(nameof(onNullOrEmpty));

            var value = optional.GetValueOrDefault();
            return !(value is null)
                ? onValue(value)
                : onNullOrEmpty();
        }

        /// <summary>
        ///     If the optional holds a value which is not <see langword="null"/>, the specified 
        ///     <paramref name="mapValue"/> function is invoked with the optional's held value.
        ///     The returned value is then wrapped in a new <see cref="Optional{TResult}"/> instance
        ///     and returned by this method.
        ///     
        ///     If the optional is either empty or holds a value which is not <see langword="null"/>,
        ///     the <paramref name="mapValue"/> function is not invoked.
        ///     Instead, an empty <see cref="Optional{TResult}"/> instance is returned.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <typeparam name="TResult">
        ///     The type of the result which gets returned by the <paramref name="mapValue"/> function.
        /// </typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <param name="mapValue">
        ///     The function to be invoked if the optional holds a value which is not <see langword="null"/>.
        ///     This function receives the optional's held value as a parameter.
        /// </param>
        /// <returns>
        ///     An <see cref="Optional{TResult}"/> instance holding the returned value of the
        ///     <paramref name="mapValue"/> function if the optional holds a value which is not
        ///     <see langword="null"/> or an empty <see cref="Optional{TResult}"/> instance if the
        ///     optional is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="mapValue"/>
        /// </exception>
        /// <seealso cref="Optional{T}.Map{TResult}(Func{T, TResult})"/>
        public static Optional<TResult> MapNonNull<T, TResult>(
            this Optional<T?> optional,
            Func<T, TResult> mapValue) where T : struct
        {
            _ = mapValue ?? throw new ArgumentNullException(nameof(mapValue));
            var value = optional.GetValueOrDefault();
            return !(value is null)
                ? mapValue(value.Value)
                : Optional<TResult>.Empty;
        }

        /// <inheritdoc cref="MapNonNull{T, TResult}(Optional{T?}, Func{T, TResult})"/>
        public static Optional<TResult> MapNonNull<T, TResult>(
            this Optional<T?> optional,
            Func<T, TResult> mapValue) where T : class
        {
            _ = mapValue ?? throw new ArgumentNullException(nameof(mapValue));
            var value = optional.GetValueOrDefault();
            return !(value is null)
                ? mapValue(value)
                : Optional<TResult>.Empty;
        }

        /// <summary>
        ///     If the optional holds a value which is not <see langword="null"/>, the specified 
        ///     <paramref name="flatMapValue"/> function is invoked with the optional's held value.
        ///     The returned value is then returned by this method.
        ///     
        ///     If the optional is either empty or holds a value which is not <see langword="null"/>,
        ///     the <paramref name="flatMapValue"/> function is not invoked.
        ///     Instead, an empty <see cref="Optional{TResult}"/> instance is returned.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <typeparam name="TResult">
        ///     The type of the result which gets returned by the <paramref name="flatMapValue"/> function.
        /// </typeparam>
        /// <param name="optional">The optional which may hold a nullable value.</param>
        /// <param name="flatMapValue">
        ///     The function to be invoked if the optional holds a value which is not <see langword="null"/>.
        ///     This function receives the optional's held value as a parameter.
        /// </param>
        /// <returns>
        ///     The <see cref="Optional{TResult}"/> instance returned by the
        ///     <paramref name="flatMapValue"/> function if the optional holds a value which is not
        ///     <see langword="null"/> or an empty <see cref="Optional{TResult}"/> instance if the
        ///     optional is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="flatMapValue"/>
        /// </exception>
        /// <seealso cref="Optional{T}.FlatMap{TResult}(Func{T, Optional{TResult}})"/>
        public static Optional<TResult> FlatMapNonNull<T, TResult>(
            this Optional<T?> optional,
            Func<T, Optional<TResult>> flatMapValue) where T : struct
        {
            _ = flatMapValue ?? throw new ArgumentNullException(nameof(flatMapValue));
            var value = optional.GetValueOrDefault();
            return !(value is null)
                ? flatMapValue(value.Value)
                : Optional<TResult>.Empty;
        }

        /// <inheritdoc cref="FlatMapNonNull{T, TResult}(Optional{T?}, Func{T, Optional{TResult}})"/>
        public static Optional<TResult> FlatMapNonNull<T, TResult>(
            this Optional<T?> optional,
            Func<T, Optional<TResult>> flatMapValue) where T : class
        {
            _ = flatMapValue ?? throw new ArgumentNullException(nameof(flatMapValue));
            var value = optional.GetValueOrDefault();
            return !(value is null)
                ? flatMapValue(value)
                : Optional<TResult>.Empty;
        }

    }

}
