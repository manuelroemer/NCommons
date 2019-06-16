using System;

namespace NCommons.Monads
{

    /// <summary>
    ///     Declares shared members of the generic variant structs, thus enabling extension methods
    ///     for shared implementation logic.
    /// </summary>
    internal interface IVariant
    {

        /// <summary>
        ///     Gets or sets the value which the variant holds as an <see cref="object"/>.
        ///     This may be <see langword="null"/>, depending on the value with which the variant
        ///     was initialized.
        ///     
        ///     If the variant is empty this is <see langword="null"/>.
        /// </summary>
        object? Value { get; }

        /// <summary>
        ///     Gets the type of the variant, i.e. the enum value which defines what value it holds.
        /// </summary>
        VariantType Type { get; }

        /// <summary>
        ///     Gets the types of the variant's generic type parameters (in order).
        ///     This is used for building appropriate exception message strings.
        /// </summary>
        Type[] GenericValueTypes { get; }

    }

}
