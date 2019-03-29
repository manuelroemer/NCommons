using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace NCommons.Monads
{

#pragma warning disable CA1066 // Should implement IEquatable<T>.    It is implemented?? Prob. confused by the explicit implementation.

    /// <summary>
    ///     Holds a single value which can either be of type <typeparamref name="TL"/>
    ///     (i.e. a left value) or of type <typeparamref name="TR"/> (i.e. a right value).
    /// </summary>
    /// <typeparam name="TL">
    ///     The type of the either's left value.
    ///     In functional programming languages, this is typically an error state.
    /// </typeparam>
    /// <typeparam name="TR">
    ///     The type of the either's right value.
    ///     In functional programming languages, this is typically a successful result.
    /// </typeparam>
    /// <remarks>
    ///     **Important**:
    ///     Since this member is a struct, it automatically exposes a default constructor.
    ///     Be aware that this is not intended and may, if used, lead to undesired results.
    ///     An either constructed via the parameterless constructor will always hold a default left
    ///     value.
    /// </remarks>
    [Serializable]
    public readonly struct Either<TL, TR> : IEquatable<Either<TL, TR>>, ISerializable
    {

        private readonly EitherType _type;
        private readonly TL _left;
        private readonly TR _right;

        /// <summary>
        ///     Gets the either's type, indicating whether it holds a left or a right value.
        /// </summary>
        public EitherType Type => _type;

        /// <summary>
        ///     Gets a value indicating whether the either holds a left value.
        /// </summary>
        public bool IsLeft => Type == EitherType.Left;

        /// <summary>
        ///     Gets a value indicating whether the either holds a right value.
        /// </summary>
        public bool IsRight => Type == EitherType.Right;

        /// <summary>
        ///     Initializes a new <see cref="Either{TL, TR}"/> which holds the specified
        ///     <paramref name="left"/> value.
        /// </summary>
        /// <param name="left">
        ///     The left value for the either to hold.
        /// </param>
        /// <seealso cref="Left(TL)"/>
        public Either(TL left)
        {
#nullable disable
            _type = EitherType.Left;
            _left = left;
            _right = default;
#nullable enable
        }

        /// <summary>
        ///     Initializes a new <see cref="Either{TL, TR}"/> which holds the specified
        ///     <paramref name="right"/> value.
        /// </summary>
        /// <param name="right">
        ///     The right value for the either to hold.
        /// </param>
        /// <seealso cref="Right(TR)"/>
        public Either(TR right)
        {
#nullable disable
            _type = EitherType.Right;
            _left = default;
            _right = right;
#nullable enable
        }

        private Either(SerializationInfo serializationInfo, StreamingContext context)
        {
#nullable disable
            if (serializationInfo is null)
                throw new ArgumentNullException(nameof(serializationInfo));

            _type = (EitherType)serializationInfo.GetInt32(nameof(_type));
            if (_type == EitherType.Left)
            {
                _left = (TL)serializationInfo.GetValue(nameof(_left), typeof(TL));
                _right = default;
            }
            else
            {
                _left = default;
                _right = (TR)serializationInfo.GetValue(nameof(_right), typeof(TR));
            }
#nullable enable
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info is null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(_type), (int)_type);
            if (IsLeft)
            {
                info.AddValue(nameof(_left), _left);
            }
            else
            {
                info.AddValue(nameof(_right), _right);
            }
        }

        /// <summary>
        ///     Returns a new <see cref="Either{TL, TR}"/> which holds the specified
        ///     <paramref name="left"/> value.
        /// </summary>
        /// <param name="left">
        ///     The left value for the either to hold.
        /// </param>
        /// <returns>
        ///     A newly created <see cref="Either{TL, TR}"/> which holds the specified
        ///     <paramref name="left"/> value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Either<TL, TR> Left(TL left)
        {
            return new Either<TL, TR>(left);
        }

        /// <summary>
        ///     Returns a new <see cref="Either{TL, TR}"/> which holds the specified
        ///     <paramref name="right"/> value.
        /// </summary>
        /// <param name="right">
        ///     The right value for the either to hold.
        /// </param>
        /// <returns>
        ///     A newly created <see cref="Either{TL, TR}"/> which holds the specified
        ///     <paramref name="right"/> value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Either<TL, TR> Right(TR right)
        {
            return new Either<TL, TR>(right);
        }

        /// <summary>
        ///     Matches the two specified functions to the either's <see cref="Type"/> and executes
        ///     the matching function.
        /// </summary>
        /// <param name="onLeft">
        ///     A function to be executed if the either holds a left value.
        ///     The function receives the left value as a parameter.
        /// </param>
        /// <param name="onRight">
        ///     A function to be executed if the either holds a right value.
        ///     The function receives the right value as a parameter.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onLeft"/>
        ///     * <paramref name="onRight"/>
        /// </exception>
        public void Match(Action<TL> onLeft, Action<TR> onRight)
        {
            if (onLeft is null)
                throw new ArgumentNullException(nameof(onLeft));
            if (onRight is null)
                throw new ArgumentNullException(nameof(onRight));

            if (IsLeft)
            {
                onLeft(_left);
            }
            else
            {
                onRight(_right);
            }
        }

        /// <summary>
        ///     Matches the two specified functions to the either's <see cref="Type"/>, executes
        ///     the matching function and returns its result.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type that <paramref name="onLeft"/> and <paramref name="onRight"/> return.
        /// </typeparam>
        /// <param name="onLeft">
        ///     A function to be executed if the either holds a left value.
        ///     The function receives the left value as a parameter.
        /// </param>
        /// <param name="onRight">
        ///     A function to be executed if the either holds a right value.
        ///     The function receives the right value as a parameter.
        /// </param>
        /// <returns>
        ///     The result of <paramref name="onLeft"/> or <paramref name="onRight"/>, depending
        ///     on the either's type.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onLeft"/>
        ///     * <paramref name="onRight"/>
        /// </exception>
        /// <seealso cref="Transform{TResL, TResR}(Func{TL, TResL}, Func{TR, TResR})"/>
        public TResult Match<TResult>(Func<TL, TResult> onLeft, Func<TR, TResult> onRight)
        {
            if (onLeft is null)
                throw new ArgumentNullException(nameof(onLeft));
            if (onRight is null)
                throw new ArgumentNullException(nameof(onRight));

            if (IsLeft)
            {
                return onLeft(_left);
            }
            else
            {
                return onRight(_right);
            }
        }

        /// <summary>
        ///     Returns the either's left value if it holds one.
        ///     Otherwise, returns the specified <paramref name="substitute"/>.
        /// </summary>
        /// <param name="substitute">
        ///     A substitute value to be returned if the either holds a right value.
        /// </param>
        /// <returns>
        ///     The either's left value or <paramref name="substitute"/>, if the either holds
        ///     a right value.
        /// </returns>
        public TL LeftOr(TL substitute)
        {
            return IsLeft ? _left : substitute;
        }

        /// <summary>
        ///     Returns the either's left value if it holds one.
        ///     Otherwise, returns the value provided by the specified 
        ///     <paramref name="substituteProvider"/> function.
        /// </summary>
        /// <param name="substituteProvider">
        ///     A function which provides a substitute value to be returned 
        ///     if the either holds a right value.
        /// </param>
        /// <returns>
        ///     The either's left value or the value returned by the 
        ///     <paramref name="substituteProvider"/> if the either holds a right value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="substituteProvider"/>
        /// </exception>
        public TL LeftOr(Func<TL> substituteProvider)
        {
            if (substituteProvider is null)
                throw new ArgumentNullException(nameof(substituteProvider));

            return IsLeft ? _left : substituteProvider();
        }

        /// <summary>
        ///     Returns the either's right value if it holds one.
        ///     Otherwise, returns the specified <paramref name="substitute"/>.
        /// </summary>
        /// <param name="substitute">
        ///     A substitute value to be returned if the either holds a left value.
        /// </param>
        /// <returns>
        ///     The either's right value or <paramref name="substitute"/>, if the either holds
        ///     a left value.
        /// </returns>
        public TR RightOr(TR substitute)
        {
            return IsRight ? _right : substitute;
        }

        /// <summary>
        ///     Returns the either's right value if it holds one.
        ///     Otherwise, returns the value provided by the specified 
        ///     <paramref name="substituteProvider"/> function.
        /// </summary>
        /// <param name="substituteProvider">
        ///     A function which provides a substitute value to be returned 
        ///     if the either holds a left value.
        /// </param>
        /// <returns>
        ///     The either's right value or the value returned by the 
        ///     <paramref name="substituteProvider"/> if the either holds a left value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="substituteProvider"/>
        /// </exception>
        public TR RightOr(Func<TR> substituteProvider)
        {
            if (substituteProvider is null)
                throw new ArgumentNullException(nameof(substituteProvider));

            return IsRight ? _right : substituteProvider();
        }

        /// <summary>
        ///     Returns the either's left value if it holds one.
        ///     Otherwise, an <see cref="UnexpectedEitherTypeException"/> is thrown.
        /// </summary>
        /// <returns>The either's left value.</returns>
        /// <exception cref="UnexpectedEitherTypeException">
        ///     Thrown if the either holds a right value.
        /// </exception>
        public TL LeftOrThrow()
        {
            return IsLeft ? _left : throw new UnexpectedEitherTypeException(Type);
        }

        /// <summary>
        ///     Returns the either's right value if it holds one.
        ///     Otherwise, an <see cref="UnexpectedEitherTypeException"/> is thrown.
        /// </summary>
        /// <returns>The either's right value.</returns>
        /// <exception cref="UnexpectedEitherTypeException">
        ///     Thrown if the either holds a left value.
        /// </exception>
        public TR RightOrThrow()
        {
            return IsRight ? _right : throw new UnexpectedEitherTypeException(Type);
        }

        /// <summary>
        ///     Executes the specified <paramref name="action"/> if the either holds a left value.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="action"/>
        /// </exception>
        public void IfLeft(Action<TL> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            if (IsLeft)
            {
                action(_left);
            }
        }

        /// <summary>
        ///     Executes the specified <paramref name="action"/> if the either holds a right value.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="action"/>
        /// </exception>
        public void IfRight(Action<TR> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            if (IsRight)
            {
                action(_right);
            }
        }

        /// <summary>
        ///     Returns a new either whose left value, if it holds one, is the result of the
        ///     specified <paramref name="transformLeft"/> function that was applied to this
        ///     either's left value, if it holds one.
        /// </summary>
        /// <typeparam name="TResult">The result of the transformation.</typeparam>
        /// <param name="transformLeft">
        ///     The transformation function to be applied to this either's left value, if it holds one.
        /// </param>
        /// <returns>
        ///     A new either which either holds the transformation result as its left value,
        ///     or this either's right value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="transformLeft"/>
        /// </exception>
        public Either<TResult, TR> TransformLeft<TResult>(Func<TL, TResult> transformLeft)
        {
            return Transform(transformLeft, r => r);
        }

        /// <summary>
        ///     Returns a new either whose right value, if it holds one, is the result of the
        ///     specified <paramref name="transformRight"/> function that was applied to this
        ///     either's right value, if it holds one.
        /// </summary>
        /// <typeparam name="TResult">The result of the transformation.</typeparam>
        /// <param name="transformRight">
        ///     The transformation function to be applied to this either's right value, if it holds one.
        /// </param>
        /// <returns>
        ///     A new either which either holds the transformation result as its right value,
        ///     or this either's left value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="transformRight"/>
        /// </exception>
        public Either<TL, TResult> TransformRight<TResult>(Func<TR, TResult> transformRight)
        {
            return Transform(l => l, transformRight);
        }

        /// <summary>
        ///     Returns a new either whose left or right value is the result of one of the
        ///     two specified transformation functions.
        /// </summary>
        /// <typeparam name="TResL">
        ///     The result of the transformation, if this either holds a left value.
        /// </typeparam>
        /// <typeparam name="TResR">
        ///     The result of the transformation, if this either holds a right value.
        /// </typeparam>
        /// <param name="transformLeft">
        ///     The transformation function to be applied to this either's left value, if it holds
        ///     one.
        /// </param>
        /// <param name="transformRight">
        ///     The transformation function to be applied to this either's right value, if it holds
        ///     one.
        /// </param>
        /// <returns>
        ///     A new either which holds the value returned by one of the transformation functions.
        ///     The new either holds the value on the same side as this either.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="transformLeft"/>
        ///     * <paramref name="transformRight"/>
        /// </exception>
        /// <remarks>
        ///     This method is nothing more than an alias for the match function with a result
        ///     (<see cref="Match{TResult}(Func{TL, TResult}, Func{TR, TResult})"/>).
        ///     
        ///     It exists for two reasons:
        ///     
        ///     a) For consistency with <see cref="TransformLeft{TResult}(Func{TL, TResult})"/> and
        ///        <see cref="TransformRight{TResult}(Func{TR, TResult})"/>.
        ///     b) For the sake of readability. Try to explicitly use this method (and not Match)
        ///        whenever you want to actually generate a new either with transformed values.
        ///        The intent of this method (in comparison to Match) is much clearer.
        ///        Furthermore, it simplifies writing the transformation. See the following code
        ///        example for details.
        ///        
        ///     <code>
        ///         // With match:
        ///         var newEither = e.Match(
        ///             l => Either&lt;int, double&gt;.Left(l + 10),
        ///             r => Either&lt;int, double&gt;.Right(r - 50.0)
        ///         );
        ///         
        ///         // With transform - much more readable:
        ///         var newEither = e.Transform(
        ///             l => l + 10,
        ///             r => r - 50.0
        ///         );
        ///     </code>
        /// </remarks>
        public Either<TResL, TResR> Transform<TResL, TResR>(
            Func<TL, TResL> transformLeft, Func<TR, TResR> transformRight)
        {
            // This method is nothing more than an alias for Match. See remarks for why it is included.
            if (transformLeft is null)
                throw new ArgumentNullException(nameof(transformLeft));
            if (transformRight is null)
                throw new ArgumentNullException(nameof(transformRight));

            return Match(
                l => new Either<TResL, TResR>(transformLeft(l)),
                r => new Either<TResL, TResR>(transformRight(r))
            );
        }

        /// <summary>
        ///     Returns a value indicating whether the specified <paramref name="obj"/> is equal
        ///     to this either.
        ///     This is <c>true</c> if <paramref name="obj"/> is an either and
        ///     if the two eithers both hold an equal value on the same side.
        /// </summary>
        /// <param name="obj">The object to be compared with this either.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="obj"/> is an equal either; <c>false</c> if not.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Either<TL, TR> either)
            {
                return Equals(either);
            }
            return false;
        }

        /// <summary>
        ///     Returns a value indicating whether the specified either is equal to this either.
        ///     This is <c>true</c> if the two eithers both hold an equal value on the same side.
        /// </summary>
        /// <param name="other">Another either to be compared with this either.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="other"/> is an equal either; <c>false</c> if not.
        /// </returns>
        bool IEquatable<Either<TL, TR>>.Equals(Either<TL, TR> other)
        {
            return Equals(other);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified either is equal to this either.
        ///     This is <c>true</c> if the two eithers both hold an equal value on the same side.
        /// </summary>
        /// <param name="other">Another either to be compared with this either.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="other"/> is an equal either; <c>false</c> if not.
        /// </returns>
        public bool Equals(in Either<TL, TR> other)
        {
            return other.Type == Type &&
                   IsLeft ? Equals(_left, other._left)
                          : Equals(_right, other._right);
        }

        /// <summary>
        ///     Returns a unique hash code of this either.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 31 + Type.GetHashCode();
                hash = hash * 31 + Match(l => l?.GetHashCode(), r => r?.GetHashCode()) ?? 0;
                return hash;
            }
        }

        /// <summary>
        ///     Returns a string representation of the either and it's value.
        /// </summary>
        /// <returns>
        ///     A string similar to <c>Left(System.Object)</c> or <c>Right(null)</c>.
        /// </returns>
        public override string ToString()
        {
            var valStr = Match(l => l?.ToString(), r => r?.ToString()) ?? "null";
            return $"{Type.ToString()}({valStr})";
        }

        public static bool operator ==(in Either<TL, TR> left, in Either<TL, TR> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(in Either<TL, TR> left, in Either<TL, TR> right)
        {
            return !(left == right);
        }

        public static implicit operator Either<TL, TR>(TL left)
        {
            return new Either<TL, TR>(left);
        }

        public static implicit operator Either<TL, TR>(TR right)
        {
            return new Either<TL, TR>(right);
        }

    }

#pragma warning restore CA1066

}
