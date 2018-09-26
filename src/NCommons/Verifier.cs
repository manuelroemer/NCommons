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
        ///     Must be a reference type.
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
        public static T VerifyNull<T>(this T obj) where T : class
        {
            return VerifyNull(obj, null, Strings.Verifier_ObjectNotNullExMsg);
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is null,
        ///     by throwing an <see cref="ArgumentException"/> if it is not null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        ///     Must be a reference type.
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
        public static T VerifyNull<T>(this T obj, string paramName) where T : class
        {
            return VerifyNull(
                obj, 
                paramName, 
                string.Format(
                    Strings.Verifier_ObjectNotNullExMsgWithParamName,
                    paramName));
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is null,
        ///     by throwing an <see cref="ArgumentException"/> if it is not null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        ///     Must be a reference type.
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
            where T : class
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
        ///     Must be a reference type.
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
        public static T VerifyNotNull<T>(this T obj) where T : class
        {
            return VerifyNotNull(obj, null, null);
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is not null,
        ///     by throwing an <see cref="ArgumentNullException"/> if it is null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        ///     Must be a reference type.
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
        public static T VerifyNotNull<T>(this T obj, string paramName) where T : class
        {
            return VerifyNotNull(obj, paramName, null);
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="obj"/> is not null,
        ///     by throwing an <see cref="ArgumentNullException"/> if it is null.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object.
        ///     Must be a reference type.
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
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName, message);
            }
            return obj;
        }
        
        #endregion

    }

}
