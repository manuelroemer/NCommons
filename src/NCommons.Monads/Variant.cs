namespace NCommons.Monads
{
    using System;
    using System.Globalization;
    using NCommons.Monads.Resources;

    /// <summary>
    ///     Defines shared methods for implementing the variant structs.
    ///     
    ///     Members in this class should have the "Internal" suffix so that the real methods
    ///     in the variant structs don't shadow them.
    /// </summary>
    internal static class Variant
    {

        internal static T GetValueInternal<T>(this IVariant variant, VariantType forType, out T value)
        {
            return variant.TryGetValueInternal(forType, out value)
                ? value
                : throw CreateException();

            Exception CreateException() =>
                new InvalidOperationException(GetExceptionMessage());

            string GetExceptionMessage()
            {
                if (variant.Type == VariantType.Empty)
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        ExceptionStrings.Variant_GetValue_Empty,
                        typeof(T).FullName
                    );
                }
                else
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        ExceptionStrings.Variant_GetValue_HoldsOtherValue,
                        typeof(T).FullName,
                        variant.GetGenericValueType().FullName
                    );
                }
            }
        }

        internal static T GetValueOrInternal<T>(this IVariant variant, VariantType forType, T substitute, out T value)
        {
            return variant.GetValueOrInternal(forType, () => substitute, out value);
        }

        internal static T GetValueOrInternal<T>(this IVariant variant, VariantType forType, Func<T> substituteProvider, out T value)
        {
            return variant.TryGetValueInternal(forType, out value)
                ? value
                : value = substituteProvider();
        }

        internal static T GetValueOrDefaultInternal<T>(this IVariant variant, VariantType forType, out T value)
        {
#nullable disable
            return variant.TryGetValueInternal(forType, out value)
                ? value
                : value = default;
#nullable restore
        }

        internal static bool TryGetValueInternal<T>(this IVariant variant, VariantType forType, out T value)
        {
#nullable disable
            if (variant.Type == forType)
            {
                value = (T)variant.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
#nullable restore
        }

        internal static void MatchInternal<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IVariant variant,
            Action<T1> onValue1, Action<T2> onValue2, Action<T3> onValue3, Action<T4> onValue4,
            Action<T5> onValue5, Action<T6> onValue6, Action<T7> onValue7, Action<T8> onValue8,
            Action onEmpty)
        {
            variant.MatchInternal<object?, T1, T2, T3, T4, T5, T6, T7, T8>(
                v1 => { onValue1(v1); return null; },
                v2 => { onValue2(v2); return null; },
                v3 => { onValue3(v3); return null; },
                v4 => { onValue4(v4); return null; },
                v5 => { onValue5(v5); return null; },
                v6 => { onValue6(v6); return null; },
                v7 => { onValue7(v7); return null; },
                v8 => { onValue8(v8); return null; },
                () => { onEmpty(); return null; }
            );
        }

        internal static TRes MatchInternal<TRes, T1, T2, T3, T4, T5, T6, T7, T8>(
            this IVariant variant,
            Func<T1, TRes>? onValue1,
            Func<T2, TRes>? onValue2,
            Func<T3, TRes>? onValue3,
            Func<T4, TRes>? onValue4,
            Func<T5, TRes>? onValue5,
            Func<T6, TRes>? onValue6,
            Func<T7, TRes>? onValue7,
            Func<T8, TRes>? onValue8,
            Func<TRes>? onEmpty)
        {
            return variant.Type switch
            {
#nullable disable
                VariantType.V1    => Invoke(onValue1, (T1)variant.Value),
                VariantType.V2    => Invoke(onValue2, (T2)variant.Value),
                VariantType.V3    => Invoke(onValue3, (T3)variant.Value),
                VariantType.V4    => Invoke(onValue4, (T4)variant.Value),
                VariantType.V5    => Invoke(onValue5, (T5)variant.Value),
                VariantType.V6    => Invoke(onValue6, (T6)variant.Value),
                VariantType.V7    => Invoke(onValue7, (T7)variant.Value),
                VariantType.V8    => Invoke(onValue8, (T8)variant.Value),
                VariantType.Empty => InvokeEmpty(onEmpty),
                _ => throw new NotImplementedException(),
#nullable restore
            };

            static TRes Invoke<T>(Func<T, TRes>? fn, T value)
            {
                fn ??= _ => throw CreateUnmatchedTypeException();
                return fn(value);
            }

            static TRes InvokeEmpty(Func<TRes>? fn)
            {
                fn ??= () => throw CreateUnmatchedTypeException();
                return fn();
            }

            static Exception CreateUnmatchedTypeException() =>
                new NotImplementedException(ExceptionStrings.Variant_Match_TypeShouldHaveBeenImplementedButWasnt);
        }

        internal static Type GetGenericValueType(this IVariant variant) => variant.Type switch
        {
            VariantType.V1    => variant.GenericValueTypes[0],
            VariantType.V2    => variant.GenericValueTypes[1],
            VariantType.V3    => variant.GenericValueTypes[2],
            VariantType.V4    => variant.GenericValueTypes[3],
            VariantType.V5    => variant.GenericValueTypes[4],
            VariantType.V6    => variant.GenericValueTypes[5],
            VariantType.V7    => variant.GenericValueTypes[6],
            VariantType.V8    => variant.GenericValueTypes[7],
            VariantType.Empty => throw new InvalidOperationException(),
            _ => throw new NotImplementedException(),
        };

        internal static bool EqualsInternal<TVariant>(this TVariant variant, object obj) where TVariant : IVariant
        {
            // Must be TVariant here, because a Variant<T1, T2> should not be comparable with a Variant<T1, T2, T3>.
            if (obj is TVariant other)
            {
                return variant.EqualsInternal(other);
            }
            return false;
        }

        internal static bool EqualsInternal<TVariant>(this TVariant variant, IVariant other) where TVariant : IVariant =>
            variant.Type == other.Type && 
            Equals(variant.Value, other.Value);

        internal static int GetHashCodeInternal(this IVariant variant) =>
            (variant.Type, variant.Value).GetHashCode();

        internal static string ToStringInternal(this IVariant variant) =>
            variant.Type == VariantType.Empty
                ? "Empty"
                : $"Value {(int)variant.Type}: {variant.Value ?? "null"}";

    }

}
