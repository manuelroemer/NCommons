namespace NCommons.Monads
{

    /// <summary>
    ///     Defines the different types that the variant can be of.
    ///     This enum gets shared between all generic variant structs, even though not all
    ///     enum values get used by the corresponding variant implementations.
    /// </summary>
    internal enum VariantType : byte
    {
        // Empty must always come first, so that a default struct initialization leads to an empty variant.
        Empty = 0,

        // V stands for value.
        // The numbers here should not change, because they are used for certain operations (like ToString).
        V1 = 1,
        V2 = 2,
        V3 = 3,
        V4 = 4,
        V5 = 5,
        V6 = 6,
        V7 = 7,
        V8 = 8,
    }

}
