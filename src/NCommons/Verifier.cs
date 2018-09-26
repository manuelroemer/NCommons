using NCommons.Resources;
using System;
using System.Diagnostics;

namespace NCommons
{

    /// <summary>
    ///     Provides static extension methods in the form of
    ///     <code>Verify[Condition](...)</code>
    ///     which will throw exceptions if the condition is not met.
    /// </summary>
    [DebuggerStepThrough]
    public static class Verifier
    {

        #region Null

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is null,
        ///     by throwing an <see cref="ArgumentException"/> if it is not null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to be checked.
        /// </param>
        /// <returns>
        ///     If no exception is thrown, this returns a reference to the specified
        ///     <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="obj"/> is not null.
        /// </exception>
        public static T VerifyNull<T>(this T obj)
        {
            return VerifyNull(obj, null, Strings.Verifier_ObjectNotNullExMsg);
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is null,
        ///     by throwing an <see cref="ArgumentException"/> if it is not null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to be checked.
        /// </param>
        /// <param name="paramName">
        ///     If the object to be checked is a parameter in a method, this can be set
        ///     to the parameter's name.
        ///     This results in a more detailed error message.
        /// </param>
        /// <returns>
        ///     If no exception is thrown, this returns a reference to the specified
        ///     <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="obj"/> is not null.
        /// </exception>
        public static T VerifyNull<T>(this T obj, string paramName)
        {
            return VerifyNull(
                obj, paramName, string.Format(
                    Strings.Verifier_ObjectNotNullExMsgWithParamName,
                    paramName));
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is null,
        ///     by throwing an <see cref="ArgumentException"/> if it is not null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to be checked.
        /// </param>
        /// <param name="paramName">
        ///     If the object to be checked is a parameter in a method, this can be set
        ///     to the parameter's name.
        ///     This results in a more detailed error message.
        /// </param>
        /// <param name="message">
        ///     An error message which gets passed to the thrown exception.
        /// </param>
        /// <returns>
        ///     If no exception is thrown, this returns a reference to the specified
        ///     <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="obj"/> is not null.
        /// </exception>
        public static T VerifyNull<T>(this T obj, string paramName, string message)
        {
            if (obj != null)
            {
                throw new ArgumentException(message, paramName);
            }
            return obj;
        }

        #endregion

        #region NotNull

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is not null,
        ///     by throwing an <see cref="ArgumentNullException"/> if it is null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to be checked.
        /// </param>
        /// <returns>
        ///     If no exception is thrown, this returns a reference to the specified
        ///     <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="obj"/> is null.
        /// </exception>
        public static T VerifyNotNull<T>(this T obj)
        {
            return VerifyNotNull(obj, null, null);
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is not null,
        ///     by throwing an <see cref="ArgumentNullException"/> if it is null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to be checked.
        /// </param>
        /// <param name="paramName">
        ///     If the object to be checked is a parameter in a method, this can be set
        ///     to the parameter's name.
        ///     This results in a more detailed error message.
        /// </param>
        /// <returns>
        ///     If no exception is thrown, this returns a reference to the specified
        ///     <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="obj"/> is null.
        /// </exception>
        public static T VerifyNotNull<T>(this T obj, string paramName)
        {
            return VerifyNotNull(obj, paramName, null);
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is not null,
        ///     by throwing an <see cref="ArgumentNullException"/> if it is null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to be checked.
        /// </param>
        /// <param name="paramName">
        ///     If the object to be checked is a parameter in a method, this can be set
        ///     to the parameter's name.
        ///     This results in a more detailed error message.
        /// </param>
        /// <param name="message">
        ///     An error message which gets passed to the thrown exception.
        /// </param>
        /// <returns>
        ///     If no exception is thrown, this returns a reference to the specified
        ///     <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="obj"/> is null.
        /// </exception>
        public static T VerifyNotNull<T>(this T obj, string paramName, string message)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
            return obj;
        }

        #endregion

        #region Verify

        /// <summary>
        ///     Ensures that a specific condition, provided via a delegate,
        ///     is fulfilled for the specified <paramref name="obj"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to be checked.
        /// </param>
        /// <param name="condition">
        ///     A function which checks a certain condition which must apply to the object.
        /// </param>
        /// <param name="exceptionFactory">
        ///     A factory function which creates an <see cref="Exception"/> to be thrown if the
        ///     <paramref name="condition"/> doesn't apply to the object.
        /// </param>
        /// <returns>
        ///     If no exception is thrown, this returns a reference to the specified
        ///     <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="condition"/> or <paramref name="exceptionFactory"/>
        ///     is null.
        /// </exception>
        public static T Verify<T>(
            this T obj, Func<bool> condition, Func<Exception> exceptionFactory)
        {
            condition.VerifyNotNull(nameof(condition));
            exceptionFactory.VerifyNotNull(nameof(exceptionFactory));

            if (!condition())
            {
                throw exceptionFactory();
            }
            return obj;
        }

        #endregion

    }

}
