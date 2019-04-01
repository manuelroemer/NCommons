using System;
using System.Globalization;
using System.Runtime.Serialization;
using NCommons.Monads.Resources;

namespace NCommons.Monads
{

#pragma warning disable CA1032

    /// <summary>
    ///     An exception which gets thrown by certain methods of the <see cref="Either{TL, TR}"/>
    ///     type, when an attempt was made to retrieve or match a value that the either does not
    ///     hold.
    /// </summary>
    [Serializable]
    public class UnexpectedEitherTypeException : Exception
    {

        /// <summary>
        ///     Gets the actual type of the either which was the cause of this exception.
        ///     This is the value that can be retrieved or matched.
        /// </summary>
        public EitherType ActualType { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnexpectedEitherTypeException"/>.
        /// </summary>
        /// <param name="actualType">The actual type of the either which was the cause of this exception.</param>
        public UnexpectedEitherTypeException(EitherType actualType)
            : this(actualType, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnexpectedEitherTypeException"/>.
        /// </summary>
        /// <param name="actualType">The actual type of the either which was the cause of this exception.</param>
        /// <param name="message">A message describing the error.</param>
        public UnexpectedEitherTypeException(EitherType actualType, string? message) 
            : this(actualType, message, null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnexpectedEitherTypeException"/>.
        /// </summary>
        /// <param name="actualType">The actual type of the either which was the cause of this exception.</param>
        /// <param name="message">A message describing the error.</param>
        /// <param name="innerException">An exception which was the cause of this exception.</param>
        public UnexpectedEitherTypeException(EitherType actualType, string? message, Exception? innerException)
            : base(message ?? BuildMessage(actualType), innerException)
        {
            ActualType = actualType;
        }

        protected UnexpectedEitherTypeException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }

        private static string BuildMessage(EitherType actualType)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                ExceptionStrings.UnexpectedEitherTypeException_DefaultMessage,
                actualType.Invert().ToString(),
                actualType.ToString()
            );
        }

    }

#pragma warning restore CA1032

}
