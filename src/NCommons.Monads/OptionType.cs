namespace NCommons.Monads
{

    /// <summary>
    ///     Defines the two different types that an <see cref="Option{T}"/> can be, i.e.
    ///     "Some" or "None".
    /// </summary>
    public enum OptionType
    {

        // None must come at position 0, so that it is the default value when initializing
        // an "empty" Option<T> without a value.

        /// <summary>
        ///     The option doesn't hold any value.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The option holds a value.
        /// </summary>
        Some = 1

    }

}
