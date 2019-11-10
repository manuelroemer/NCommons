namespace NCommons.Monads
{
    using System;

    /// <summary>
    ///     Defines static functions which assist with the creation of and interaction with
    ///     <see cref="Optional{T}"/> instances.
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
        public static Optional<T> FromNullable<T>(Nullable<T> value) where T : struct
        {
            return value.HasValue
                ? new Optional<T>(value.Value)
                : Optional<T>.Empty;
        }

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
        public static Nullable<T> ToNullable<T>(this Optional<T> optional) where T : struct
        {
            // GetValueOrDefault() doesn't do a HasValue comparison and is thus better than GetValue().
            return optional.HasValue ? optional.GetValueOrDefault() : (Nullable<T>)null;
        }

        /// <summary>
        ///     Returns an empty optional if the current optional's held value is <see langword="null"/>.
        ///     Otherwise, returns a non-empty optional holding the same value as the current optional.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional holding a nullable value.</param>
        /// <returns>
        ///     An empty optional if the current optional's held value is <see langword="null"/>.
        ///     Otherwise, a non-empty optional holding the same value as the current optional.
        /// </returns>
        public static Optional<T> ToEmptyIfNull<T>(this Optional<Nullable<T>> optional) where T : struct
        {
            return optional.FlatMap(nullable => nullable.HasValue ? nullable.Value : Optional<T>.Empty);
        }

        /// <summary>
        ///     Returns an empty optional if the current optional's held value is <see langword="null"/>.
        ///     Otherwise, returns a non-empty optional holding the same value as the current optional.
        /// </summary>
        /// <typeparam name="T">The type which may be held by the optional.</typeparam>
        /// <param name="optional">The optional holding a nullable value.</param>
        /// <returns>
        ///     An empty optional if the current optional's held value is <see langword="null"/>.
        ///     Otherwise, a non-empty optional holding the same value as the current optional.
        /// </returns>
        /// <remarks>
        ///     <b>Recommendation:</b> This method is ideally used together with C# 8.0's
        ///     nullable reference types, because it strips the nullable type annotation
        ///     from a nullable reference type, making it obvious that a held value can no
        ///     longer be <see langword="null"/>.
        /// </remarks>
        public static Optional<T> ToEmptyIfNull<T>(this Optional<T?> optional) where T : class
        {
            return optional.FlatMap(value => !(value is null) ? value : Optional<T>.Empty);
        }

    }

}
