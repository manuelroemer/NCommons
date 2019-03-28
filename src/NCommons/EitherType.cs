using System;

namespace NCommons
{

    /// <summary>
    ///     Defines whether an <see cref="Either{TL, TR}"/> holds a left or a right value.
    /// </summary>
    public enum EitherType
    {

        /// <summary>
        ///     The <see cref="Either{TL, TR}"/> holds a left value.
        /// </summary>
        Left,

        /// <summary>
        ///     The <see cref="Either{TL, TR}"/> holds a right value.
        /// </summary>
        Right,

    }

    internal static class EitherTypeExtensions
    {
        public static EitherType Invert(this EitherType type) => type switch
        {
            EitherType.Left  => EitherType.Right,
            EitherType.Right => EitherType.Left,
            _ => throw new NotImplementedException()
        };
    }

}
