namespace NCommons.Monads
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NCommons.Monads.Resources;

    /// <summary>
    ///     A container which either holds a value of type <typeparamref name="T"/> or not.
    ///     See remarks for details.
    /// </summary>
    /// <typeparam name="T">
    ///     The type which may be held by the optional.
    /// </typeparam>
    /// <remarks>
    ///     The <see cref="Optional{T}"/> type is very similar to .NET's <see cref="Nullable{T}"/>
    ///     struct. In essence, it offers the same functionality: An <see cref="Optional{T}"/> instance
    ///     can be empty (i.e. it does not hold/contain a value) or it can be non-empty (i.e. it
    ///     holds/contains a value).
    ///     
    ///     The major difference to the <see cref="Nullable{T}"/> type is that <see cref="Optional{T}"/>
    ///     supports both value- and reference types, meaning that it can be used with both structures
    ///     and classes.
    ///     In addition, a held value of type <typeparamref name="T"/> can be <see langword="null"/>
    ///     which allows to model triple-state values.
    ///     For example, an <c>Optional&lt;int?&gt;</c> may be:
    ///     
    ///     <list type="number">
    ///         <item>
    ///             <description>Empty, i.e. no value is present</description>
    ///         </item>
    ///         <item>
    ///             <description><see langword="null"/></description>
    ///         </item>
    ///         <item>
    ///             <description>A number, e.g. <c>123</c></description>
    ///         </item>
    ///     </list>
    ///     
    ///     The <see cref="Optional{T}"/> type should usually only be used if you require this triple-state.
    ///     For dual state behavior, i.e. "Has a value" vs. "Doesn't have a value", 
    ///     <see langword="null"/> and <see cref="Nullable{T}"/> should still be used, especially
    ///     with C# 8.0's support for nullable reference types.
    ///     
    ///     As an example for when the previously mentioned triple-state is sensible, consider the following
    ///     code fragment using the <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
    ///     method:
    ///     
    ///     <code>
    ///     var items = new[] { null, "Second", "Third" };
    ///     var first = items.FirstOrDefault();
    ///     
    ///     if (first != null)
    ///     {
    ///         Console.WriteLine("First item: {0}", first);
    ///     }
    ///     else
    ///     {
    ///         Console.WriteLine("There were no items.");
    ///     }
    ///     
    ///     // Output:
    ///     // "There were no items."
    ///     </code>
    ///     
    ///     The code above displays a simplified, but common usage of <see cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>.
    ///     If the result is the default value (here: <see langword="null"/>), users often assume that
    ///     a list is empty.
    ///     This is obviously not true for the list above - the first value was simply the default
    ///     value of type <see cref="string"/>.
    ///     
    ///     This problem could be solved with an optional. By returning an <see cref="Optional{T}"/>,
    ///     the method could return no value (i.e. an empty optional), the default value (i.e. <see langword="null"/>)
    ///     or any other string.
    ///     The following example shows how the extension may look like, using an optional:
    ///     
    ///     <code>
    ///     static Optional&lt;string&gt; BetterFirstOrDefault(this string[] items)
    ///     {
    ///         if (items.Length == 0) return Optional&lt;string&gt;.Empty;
    ///         return new Optional&lt;string&gt;(items[0]);
    ///     }
    ///     
    ///     // Possible usage:
    ///     var items = new[] { null, "Second", "Third" };
    ///     var first = items.BetterFirstOrDefault();
    ///     
    ///     if (first.HasValue)
    ///     {
    ///         Console.WriteLine("First item: {0}", first.GetValue());
    ///     }
    ///     else
    ///     {
    ///         Console.WriteLine("There were no items.");
    ///     }
    ///     
    ///     // Output:
    ///     // ""
    ///     </code>
    ///     
    ///     In addition to the members defined in this class, the <see cref="Optional"/> class
    ///     defines additional utility functions which further enhance the interaction with the
    ///     <see cref="Optional{T}"/> type.
    /// </remarks>
    /// <seealso cref="Optional"/>
    public readonly struct Optional<T> : IEquatable<T>, IEquatable<Optional<T>>
    {

        /// <summary>
        ///     An empty <see cref="Optional{T}"/> instance which does not hold any value.
        ///     This value is equivalent to an instance created via the parameterless constructor.
        /// </summary>
        public static readonly Optional<T> Empty = new Optional<T>();

        private readonly T _value;
        private readonly bool _hasValue;

        /// <summary>
        ///     Gets a value indicating whether this optional holds a value,
        ///     i.e. whether it is non-empty.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the optional holds a value, i.e. it is non-empty.
        ///     <see langword="false"/> if the optional doesn't hold a value, i.e. it is empty.
        /// </returns>
        /// <remarks>
        ///     The <see cref="HasValue"/> property can be used to determine whether the optional
        ///     holds a value.
        ///     For example, the following code segment uses the <see cref="HasValue"/> property
        ///     to determine whether <see cref="GetValue"/> can safely be called. 
        ///     
        ///     <code>
        ///     Optional&lt;int&gt; opt = GetOptionalNumber();
        ///     if (opt.HasValue)
        ///     {
        ///         Console.WriteLine("The value is: {0}", opt.GetValue());
        ///     }
        ///     else
        ///     {
        ///         Console.WriteLine("The optional is empty.");
        ///     }
        ///     </code>
        /// </remarks>
        public bool HasValue => _hasValue;

        /// <summary>
        ///     Initializes a new <see cref="Optional{T}"/> instance which is non-empty and holds
        ///     the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to be held by the optional.</param>
        public Optional(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        ///     Returns the held value if the optional is non-empty and throws an exception for
        ///     an empty optional.
        /// </summary>
        /// <returns>
        ///     The held value if the optional holds a value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     The optional is empty. No value can be retrieved from an empty optional.
        /// </exception>
        /// <remarks>
        ///     The <see cref="GetValue"/> function should be used if you either know that an
        ///     optional holds a value or if you must ensure that an optional is not empty.
        ///     
        ///     The following code segment first checks if an optional holds a value by checking
        ///     the <see cref="HasValue"/> property. Afterwards, it prints a text depending on that
        ///     value.
        ///     
        ///     The following code segment shows under which circumstances <see cref="GetValue"/>
        ///     throws an exception:
        ///     
        ///     <code>
        ///     Optional&lt;int&gt; nonEmptyOptional = 123;
        ///     Optional&lt;int&gt; emptyOptional = new Optional&lt;int&gt;;
        ///     
        ///     Console.WriteLine(nonEmptyOptional.GetValue());
        ///     Console.WriteLine(emptyOptional.GetValue());
        ///     
        ///     // Output:
        ///     // "123"
        ///     // InvalidOperationException
        ///     </code>
        /// 
        ///     <see cref="GetValue"/> is most useful when you must ensure that an optional
        ///     holds a value before your code continues.
        ///     
        ///     As an example, the following code segment executes a critical piece of business logic
        ///     which absolutely requires an instance of type <c>RequiredValues</c>.
        ///     By calling <see cref="GetValue"/> before invoking the business logic, the code
        ///     ensures that the process doesn't start if the required values are missing:
        ///     
        ///     <code>
        ///     Optional&lt;RequiredValues&gt; opt = GetRequiredValues();
        ///     
        ///     // Throws if the optional is empty, i.e. if it doesn't hold a "RequiredValues" instance.
        ///     RequiredValues requiredValues = opt.GetValue(); 
        ///     
        ///     RunCriticalBusinessLogic(requiredValues);
        ///     </code>
        /// </remarks>
        public T GetValue()
        {
            if (!_hasValue)
            {
                throw new InvalidOperationException(ExceptionStrings.Optional_IsEmpty);
            }
            return _value;
        }

        /// <summary>
        ///     Returns the held value if the optional is non-empty or a default value of type
        ///     <typeparamref name="T"/> if the optional is empty.
        /// </summary>
        /// <returns>
        ///     The held value if the optional is non-empty;
        ///     a default value of type <typeparamref name="T"/> if the optional is empty.
        /// </returns>
        /// <remarks>
        ///     <see cref="GetValueOrDefault"/> is useful when you must retrieve a value from
        ///     an optional, independent of whether it is empty or not.
        ///     
        ///     The following code segment shows the results or calling the <see cref="GetValueOrDefault"/>
        ///     function on multiple <see cref="Optional{T}"/> instances:
        ///     
        ///     <code>
        ///     Optional&lt;string&gt; emptyStringOpt = Optional&lt;string&gt;.Empty;
        ///     Optional&lt;int&gt; emptyIntOpt = Optional&lt;int&gt;.Empty;
        ///     Optional&lt;string&gt; nonEmptyStringOpt = "Hello";
        ///     Optional&lt;int&gt; nonEmptyIntOpt = 123;
        ///     
        ///     Console.WriteLine(emptyStringOpt.GetValueOrDefault());
        ///     Console.WriteLine(emptyIntOpt.GetValueOrDefault());
        ///     Console.WriteLine(nonEmptyStringOpt.GetValueOrDefault());
        ///     Console.WriteLine(nonEmptyIntOpt.GetValueOrDefault());
        ///     
        ///     // Output:
        ///     // ""
        ///     // "0"
        ///     // "Hello"
        ///     // "123"
        ///     </code>
        /// </remarks>
        [return: MaybeNull]
        public T GetValueOrDefault()
        {
            return _value;
        }

        /// <summary>
        ///     Returns the held value if the optional is non-empty or the specified
        ///     <paramref name="substitute"/> value if the optional is empty.
        /// </summary>
        /// <param name="substitute">
        ///     A value to be returned if the optional is empty.
        /// </param>
        /// <returns>
        ///     The held value if the optional is non-empty;
        ///     <paramref name="substitute"/> if the optional is empty.
        /// </returns>
        /// <remarks>
        ///     <see cref="GetValueOr(T)"/> behaves similarly to <see cref="GetValueOrDefault"/>,
        ///     with the exception that you can choose the result of the function if the optional
        ///     is empty.
        ///     
        ///     If computing the <paramref name="substitute"/> parameter is a heavy operation, it
        ///     is recommended to use the <see cref="GetValueOr(Func{T})"/> overload which lazily
        ///     creates the substitute value.
        ///     
        ///     The following code segment shows how <see cref="GetValueOr(T)"/> can be used:
        ///     
        ///     <code>
        ///     Optional&lt;string&gt; emptyStringOpt = Optional&lt;string&gt;.Empty;
        ///     Optional&lt;string&gt; nonEmptyStringOpt = "Hello";
        ///     
        ///     Console.WriteLine(emptyStringOpt.GetValueOr("Substitute value"));
        ///     Console.WriteLine(nonEmptyStringOpt.GetValueOr("Substitute value"));
        ///     
        ///     // Output:
        ///     // "Substitute value"
        ///     // "Hello"
        ///     </code>
        /// </remarks>
        public T GetValueOr(T substitute)
        {
            return _hasValue ? _value : substitute;
        }

        /// <summary>
        ///     Returns the held value if the optional is non-empty or the value returned by the specified
        ///     <paramref name="substituteProvider"/> function if the optional is empty.
        /// </summary>
        /// <param name="substituteProvider">
        ///     A function which returns a value which is supposed to be returned if the optional is empty.
        /// </param>
        /// <returns>
        ///     The held value if the optional is non-empty;
        ///     a value returned by the specified <paramref name="substituteProvider"/> function if 
        ///     the optional is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="substituteProvider"/>
        /// </exception>
        /// <remarks>
        ///     <see cref="GetValueOr(Func{T})"/> behaves similarly to <see cref="GetValueOrDefault"/>,
        ///     with the exception that you can choose the result of the function if the optional
        ///     is empty.
        ///     
        ///     In comparison to the <see cref="GetValueOr(T)"/> overload, this method lazily
        ///     generates the substitute by only invoking the specified <paramref name="substituteProvider"/>
        ///     if the optional is actually empty.
        /// 
        ///     The following code segment shows how <see cref="GetValueOr(Func{T})"/> can be used:
        ///     
        ///     <code>
        ///     Optional&lt;string&gt; emptyStringOpt = Optional&lt;string&gt;.Empty;
        ///     Optional&lt;string&gt; nonEmptyStringOpt = "Hello";
        ///     
        ///     Console.WriteLine(emptyStringOpt.GetValueOr(() => "Substitute value"));
        ///     Console.WriteLine(nonEmptyStringOpt.GetValueOr(() => "Substitute value"));
        ///     
        ///     // Output:
        ///     // "Substitute value"
        ///     // "Hello"
        ///     </code>
        ///     
        ///     Another possible use case for this method is throwing custom exceptions if an
        ///     optional doesn't hold a value:
        ///     
        ///     <code>
        ///     Optional&lt;string&gt; opt = Optional&lt;string&gt;.Empty;
        ///     opt.GetValueOr(() => throw new Exception("Custom exception."));
        ///     </code>
        /// </remarks>
        public T GetValueOr(Func<T> substituteProvider)
        {
            _ = substituteProvider ?? throw new ArgumentNullException(nameof(substituteProvider));
            return _hasValue ? _value : substituteProvider();
        }

        /// <summary>
        ///     Attempts to retrieve the held value of the optional and returns a value indicating
        ///     whether the retrieval was successful, i.e. if the optional actually held a value.
        /// </summary>
        /// <param name="value">
        ///     A parameter which will hold the value that the optional held, if it is non-empty.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the optional is non-empty;
        ///     <see langword="false"/> if the optional is empty.
        /// </returns>
        public bool TryGetValue(out T value)
        {
            value = _value;
            return _hasValue;
        }

        /// <summary>
        ///     Executes the specified function if the optional is non-empty.
        /// </summary>
        /// <param name="onValue">
        ///     The function to be invoked if the optional is non-empty.
        ///     This function receives the optional's held value as a parameter.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onValue"/>
        /// </exception>
        /// <remarks>
        ///     <see cref="IfValue(Action{T})"/> is a utility function which allows you to run a
        ///     piece of code if the optional is non-empty.
        ///     
        ///     The following code segment shows two possible ways of printing an optional's held
        ///     value to the console:
        ///     
        ///     <code>
        ///     Optional&lt;int&gt; opt = 123;
        ///     
        ///     // The following two code segments do exactly the same:
        ///     // 1)
        ///     if (opt.HasValue)
        ///     {
        ///         Console.WriteLine(opt.GetValue());
        ///     }
        ///     
        ///     // 2)
        ///     opt.IfValue(value => Console.WriteLine(value));
        ///     </code>
        /// </remarks>
        public void IfValue(Action<T> onValue)
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            if (_hasValue)
            {
                onValue(_value);
            }
        }

        /// <summary>
        ///     Executes the specified function if the optional is empty.
        /// </summary>
        /// <param name="onEmpty">
        ///     The function to be invoked if the optional is empty.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onEmpty"/>
        /// </exception>
        /// <remarks>
        ///     <see cref="IfEmpty(Action)"/> is a utility function which allows you to run a
        ///     piece of code if the optional is empty.
        ///     
        ///     The following code segment shows two possible ways of printing an optional's empty
        ///     state to the console:
        ///     
        ///     <code>
        ///     Optional&lt;int&gt; opt = Optional&lt;int&gt;.Empty;
        ///     
        ///     // The following two code segments do exactly the same:
        ///     // 1)
        ///     if (!opt.HasValue)
        ///     {
        ///         Console.WriteLine("The optional is empty.");
        ///     }
        ///     
        ///     // 2)
        ///     opt.IfEmpty(value => Console.WriteLine("The optional is empty."));
        ///     </code>
        /// </remarks>
        public void IfEmpty(Action onEmpty)
        {
            _ = onEmpty ?? throw new ArgumentNullException(nameof(onEmpty));
            if (!_hasValue)
            {
                onEmpty();
            }
        }

        /// <summary>
        ///     Executes the <paramref name="onValue"/> function if the optional is non-empty or
        ///     the <paramref name="onEmpty"/> function if the optional is empty.
        /// </summary>
        /// <param name="onValue">
        ///     The function to be invoked if the optional is non-empty.
        /// </param>
        /// <param name="onEmpty">
        ///     The function to be invoked if the optional is empty.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onValue"/>
        ///     * <paramref name="onEmpty"/>
        /// </exception>
        /// <remarks>
        ///     <see cref="Match(Action{T}, Action)"/> is a utility function which allows you to
        ///     run two different pieces of code, depending on whether an optional holds a value or
        ///     not.
        ///     
        ///     The following code segment shows how <see cref="Match(Action{T}, Action)"/> can be
        ///     used to print an optional's state to the console.
        ///     
        ///     <code>
        ///     static void Print(Optional&lt;string&gt; opt) =>
        ///         opt.Match(
        ///             value => Console.WriteLine("Value: {0}", value),
        ///             ()    => Console.WriteLine("Empty")
        ///         );
        /// 
        ///     Optional&lt;string&gt; emptyOpt = Optional&lt;string&gt;.Empty;
        ///     Optional&lt;string&gt; nonEmptyOpt = "Hello";
        ///     
        ///     Print(emptyOpt);
        ///     Print(nonEmptyOpt);
        ///     
        ///     // Output:
        ///     // "Empty"
        ///     // "Value: Hello"
        ///     </code>
        /// </remarks>
        public void Match(Action<T> onValue, Action onEmpty)
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            _ = onEmpty ?? throw new ArgumentNullException(nameof(onEmpty));

            if (_hasValue) 
            {
                onValue(_value);
            } 
            else
            {
                onEmpty();
            }
        }

        /// <summary>
        ///     Executes the <paramref name="onValue"/> function if the optional is non-empty or
        ///     the <paramref name="onEmpty"/> function if the optional is empty.
        ///     This method returns the returned value of the invoked function.
        /// </summary>
        /// <param name="onValue">
        ///     The function to be invoked if the optional is non-empty.
        /// </param>
        /// <param name="onEmpty">
        ///     The function to be invoked if the optional is empty.
        /// </param>
        /// <returns>
        ///     The returned value of the <paramref name="onValue"/> function if the optional is non-empty;
        ///     the returned value of the <paramref name="onEmpty"/> function if the optional is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onValue"/>
        ///     * <paramref name="onEmpty"/>
        /// </exception>
        /// <remarks>
        ///     <see cref="Match{TResult}(Func{T, TResult}, Func{TResult})"/> is a utility function
        ///     which allows you to run two different pieces of code, depending on whether an 
        ///     optional holds a value or not.
        ///     
        ///     In comparison to the <see cref="Match(Action{T}, Action)"/> overload, this method
        ///     supports returning a value.
        ///     
        ///     The following code segment shows how <see cref="Match{TResult}(Func{T, TResult}, Func{TResult})"/>
        ///     can be used to print an optional's state to the console.
        ///     
        ///     <code>
        ///     static string Stringify(Optional&lt;string&gt; opt) =>
        ///         opt.Match(
        ///             value => $"Value: {value}",
        ///             ()    => "Empty"
        ///         );
        /// 
        ///     Optional&lt;string&gt; emptyOpt = Optional&lt;string&gt;.Empty;
        ///     Optional&lt;string&gt; nonEmptyOpt = "Hello";
        ///     
        ///     Console.WriteLine(Stringify(emptyOpt));
        ///     Console.WriteLine(Stringify(nonEmptyOpt));
        ///     
        ///     // Output:
        ///     // "Empty"
        ///     // "Value: Hello"
        ///     </code>
        /// </remarks>
        public TResult Match<TResult>(Func<T, TResult> onValue, Func<TResult> onEmpty)
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            _ = onEmpty ?? throw new ArgumentNullException(nameof(onEmpty));
            return _hasValue ? onValue(_value) : onEmpty();
        }

        /// <summary>
        ///     If the optional is non-empty, the specified <paramref name="onValue"/> function
        ///     is invoked with the optional's held value. The return value is then wrapped in a new
        ///     <see cref="Optional{TResult}"/> instance and returned by this method.
        ///     
        ///     If the optional is empty, the <paramref name="onValue"/> function is not invoked.
        ///     Instead, an empty <see cref="Optional{TResult}"/> instance is returned.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type held by the optional to be returned.
        /// </typeparam>
        /// <param name="onValue">
        ///     The function to be invoked if the optional is non-empty.
        ///     This function receives the optional's held value as a parameter.
        /// </param>
        /// <returns>
        ///     An <see cref="Optional{TResult}"/> instance holding the returned value of the
        ///     <paramref name="onValue"/> function if the optional is non-empty;
        ///     an empty <see cref="Optional{TResult}"/> instance if the optional is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onValue"/>
        /// </exception>
        /// <remarks>
        ///     <see cref="Map{TResult}(Func{T, TResult})"/> allows you to transform the held value
        ///     of a non-empty optional. Calling the method on an empty optional has no effect.
        ///     
        ///     The following code segment shows how map can be used to transform the value
        ///     of an <see cref="int"/> optional to a <see cref="string"/> optional:
        ///     
        ///     <code>
        ///     Optional&lt;int&gt; intOpt = 123;
        ///     Optional&lt;string&gt; stringOpt = intOpt.Map(value => value.ToString());
        ///     Console.WriteLine(stringOpt.GetValue());
        ///     
        ///     // Output:
        ///     // "123"
        ///     </code>
        ///     
        ///     In addition to the <see cref="Map{TResult}(Func{T, TResult})"/> function, there is
        ///     also a function called <see cref="FlatMap{TResult}(Func{T, Optional{TResult}})"/>.
        ///     The difference between the two functions is how return values of type <see cref="Optional{T}"/>
        ///     are handled. 
        ///     In essence, <see cref="FlatMap{TResult}(Func{T, Optional{TResult}})"/> doesn't wrap
        ///     a returned <see cref="Optional{T}"/> into another <see cref="Optional{T}"/> instance,
        ///     while <see cref="Map{TResult}(Func{T, TResult})"/> does exactly that.
        ///     The following code segment demonstrates this behavior:
        ///     
        ///     <code>
        ///     Optional&lt;int&gt; opt = 123;
        ///     Optional&lt;Optional&lt;string&gt;&gt; map = opt.Map(value => new Optional&lt;string&gt;(value.ToString()));
        ///     Optional&lt;Optional&lt;string&gt;&gt; flatMap = opt.FlatMap(value => new Optional&lt;string&gt;(value.ToString()));
        ///     </code>
        ///     
        ///     As you can see, <see cref="FlatMap{TResult}(Func{T, Optional{TResult}})"/>
        ///     <i>flattens</i> an optional which is returned by the mapper function, while
        ///     <see cref="Map{TResult}(Func{T, TResult})"/> does not.
        /// </remarks>
        /// <seealso cref="FlatMap{TResult}(Func{T, Optional{TResult}})"/>
        public Optional<TResult> Map<TResult>(Func<T, TResult> onValue)
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            return _hasValue ? onValue(_value) : Optional<TResult>.Empty;
        }

        /// <summary>
        ///     If the optional is non-empty, the specified <paramref name="onValue"/> function
        ///     is invoked with the optional's held value. The return value is then returned by this method.
        ///     
        ///     If the optional is empty, the <paramref name="onValue"/> function is not invoked.
        ///     Instead, an empty <see cref="Optional{TResult}"/> instance is returned.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type held by the optional to be returned.
        /// </typeparam>
        /// <param name="onValue">
        ///     The function to be invoked if the optional is non-empty.
        ///     This function receives the optional's held value as a parameter.
        /// </param>
        /// <returns>
        ///     The <see cref="Optional{TResult}"/> instance returned by the
        ///     <paramref name="onValue"/> function if the optional is non-empty;
        ///     an empty <see cref="Optional{TResult}"/> instance if the optional is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     * <paramref name="onValue"/>
        /// </exception>
        /// <remarks>
        ///     <see cref="FlatMap{TResult}(Func{T, Optional{TResult}})"/> allows you to transform
        ///     the held value of a non-empty optional. Calling the method on an empty optional has no effect.
        ///     
        ///     The following code segment shows how map can be used to transform the value
        ///     of an <see cref="int"/> optional to a <see cref="string"/> optional:
        ///     
        ///     <code>
        ///     Optional&lt;int&gt; intOpt = 123;
        ///     Optional&lt;string&gt; stringOpt = intOpt.FlatMap(value => new Optional&lt;string&gt;(value.ToString()));
        ///     Console.WriteLine(stringOpt.GetValue());
        ///     
        ///     // Output:
        ///     // "123"
        ///     </code>
        ///     
        ///     In addition to the <see cref="FlatMap{TResult}(Func{T, Optional{TResult}})"/> function, there is
        ///     also a function called <see cref="Map{TResult}(Func{T, TResult})"/>.
        ///     The difference between the two functions is how return values of type <see cref="Optional{T}"/>
        ///     are handled. 
        ///     In essence, <see cref="FlatMap{TResult}(Func{T, Optional{TResult}})"/> doesn't wrap
        ///     a returned <see cref="Optional{T}"/> into another <see cref="Optional{T}"/> instance,
        ///     while <see cref="Map{TResult}(Func{T, TResult})"/> does exactly that.
        ///     The following code segment demonstrates this behavior:
        ///     
        ///     <code>
        ///     Optional&lt;int&gt; opt = 123;
        ///     Optional&lt;Optional&lt;string&gt;&gt; map = opt.Map(value => new Optional&lt;string&gt;(value.ToString()));
        ///     Optional&lt;Optional&lt;string&gt;&gt; flatMap = opt.FlatMap(value => new Optional&lt;string&gt;(value.ToString()));
        ///     </code>
        ///     
        ///     As you can see, <see cref="FlatMap{TResult}(Func{T, Optional{TResult}})"/>
        ///     <i>flattens</i> an optional which is returned by the mapper function, while
        ///     <see cref="Map{TResult}(Func{T, TResult})"/> does not.
        /// </remarks>
        /// <seealso cref="Map{TResult}(Func{T, TResult})"/>
        public Optional<TResult> FlatMap<TResult>(Func<T, Optional<TResult>> onValue)
        {
            _ = onValue ?? throw new ArgumentNullException(nameof(onValue));
            return _hasValue ? onValue(_value) : Optional<TResult>.Empty;
        }

        /// <summary>
        ///     Returns a value indicating whether the optional or its held value is equal to
        ///     the specified object.
        /// </summary>
        /// <param name="obj">
        ///     An object to be compared with the optional.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified object is equal to the optional or its held value;
        ///     <see langword="false"/> if not.
        /// </returns>
        /// <remarks>
        ///     See <see cref="Equals(Optional{T})"/> and <see cref="Equals(T)"/> for additional
        ///     information how the optional equality comparison works.
        /// </remarks>
        /// <seealso cref="Equals(Optional{T})"/>
        /// <seealso cref="Equals(T)"/>
        public override bool Equals(object? obj)
        {
            return obj switch
            {
                T other           => Equals(other),
                Optional<T> other => Equals(other),
                null              => _hasValue && _value is null,
                _                 => false
            };
        }

        /// <summary>
        ///     Returns a value indicating whether the optional or its held value is equal to
        ///     another optional.
        /// </summary>
        /// <param name="other">
        ///     Another <see cref="Optional{T}"/> instance to be compared with the optional.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the two optionals are equal;
        ///     <see langword="false"/> if not.
        /// </returns>
        /// <remarks>
        ///     Two optionals are considered equal if:
        ///     
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 The two optionals are both empty.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 The two optionals are both non-empty and the two values held by the
        ///                 optionals are considered equal by a default <see cref="EqualityComparer{T}"/> instance.
        ///             </description>
        ///         </item>
        ///     </list>
        ///     
        ///     If the above conditions are not met, the two optionals are considered to be unequal.
        /// </remarks>
        public bool Equals(Optional<T> other)
        {
            if (_hasValue && other._hasValue)
            {
                return EqualityComparer<T>.Default.Equals(_value, other._value);
            }
            else
            {
                return _hasValue == other._hasValue;
            }
        }

        /// <summary>
        ///     Returns a value indicating whether the held value of a non-empty optional is equal
        ///     to the specified value.
        /// </summary>
        /// <param name="value">
        ///     A value to be compared with the held value of the optional.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the optional is non-empty and the held value is equal to the specified value;
        ///     <see langword="false"/> if the optional is empty or the values are unequal. 
        /// </returns>
        public bool Equals(T value)
        {
            return _hasValue && EqualityComparer<T>.Default.Equals(_value, value);
        }

        /// <summary>
        ///     Returns a unique hash code for the current instance.
        /// </summary>
        /// <returns>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 <c>-1</c> if the optional is empty.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <c>0</c> if the optional is non-empty, but the held value is <see langword="null"/>.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 The hash code of the held value if the optional is non-empty and the held value is not
        ///                 <see langword="null"/>.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
        public override int GetHashCode()
        {
            // Return -1 if the optional is empty, because 0 would collide with the hash code returned
            // for a value which is null. 0 is fixed for that, because .NET also returns 0 in
            // the Nullable<T> struct if it represents null. We want to match this behavior.
            return _hasValue ? (_value?.GetHashCode() ?? 0) : -1;
        }

        /// <summary>
        ///     Returns a string representation of the optional or its held value.
        /// </summary>
        /// <returns>
        ///     An empty string if the optional is empty or if it is non-empty, but the held value
        ///     is <see langword="null"/>.
        ///     Otherwise, returns the value of calling <see cref="object.ToString"/> on the
        ///     held value.
        /// </returns>
        public override string? ToString()
        {
            if (_hasValue && !(_value is null))
            {
                return _value.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        ///     Returns a new <see cref="Variant{T1}"/> which represents the same state as the optional,
        ///     i.e. it holds a value if the optional is non-empty or it is empty if the optional is empty.
        /// </summary>
        /// <returns>
        ///     A new <see cref="Variant{T1}"/> holding the optional's held value if the optional is non-empty.
        ///     An empty <see cref="Variant{T1}"/> if the optional is empty.
        /// </returns>
        /// <remarks>
        ///     The <see cref="Variant{T1}"/> essentially has the same functionality as an <see cref="Optional{T}"/>.
        ///     Both types either hold a value or are empty. Therefore, any <see cref="Optional{T}"/> can
        ///     easily be converted to a <see cref="Variant{T1}"/> and vice versa.
        ///     
        ///     Nevertheless, the two types serve different purposes. While an <see cref="Optional{T}"/> is mainly
        ///     designed for modeling the absence of a value, the variant family is designed for scenarios where
        ///     one needs to accept or provide an instance of one or many types, or none.
        ///     Depending on the use case, you may pick either type for your scenario. If you ever
        ///     need to switch between the two, you can use one of the available conversion methods.
        /// </remarks>
        public Variant<T> ToVariant()
        {
            return _hasValue ? new Variant<T>(_value) : new Variant<T>();
        }

        /// <summary>
        ///     Returns a value indicating whether the two optional instances are equal.
        ///     See <see cref="Equals(Optional{T})"/> for additional information how the optional
        ///     equality comparison works.
        /// </summary>
        /// <param name="left">The first optional instance.</param>
        /// <param name="right">The second optional instance.</param>
        /// <returns>
        ///     <see langword="true"/> if the two optional instances are equal;
        ///     <see langword="false"/> if not.
        /// </returns>
        /// <seealso cref="Equals(Optional{T})"/>
        public static bool operator ==(Optional<T> left, Optional<T> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Returns a value indicating whether the two optional instances are unequal.
        ///     See <see cref="Equals(Optional{T})"/> for additional information how the optional
        ///     equality comparison works.
        /// </summary>
        /// <param name="left">The first optional instance.</param>
        /// <param name="right">The second optional instance.</param>
        /// <returns>
        ///     <see langword="true"/> if the two optional instances are unequal;
        ///     <see langword="false"/> if not.
        /// </returns>
        /// <seealso cref="Equals(Optional{T})"/>
        public static bool operator !=(Optional<T> left, Optional<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified value is equal to the specified
        ///     optional instance.
        ///     See <see cref="Equals(T)"/> for additional information how the optional
        ///     equality comparison works.
        /// </summary>
        /// <param name="value">A value to be compared with the held value of the optional.</param>
        /// <param name="optional">The optional to be compared with the value.</param>
        /// <returns>
        ///     <see langword="true"/> if the value is equal to the optional;
        ///     <see langword="false"/> if not.
        /// </returns>
        /// <seealso cref="Equals(T)"/>
        public static bool operator ==(T value, Optional<T> optional)
        {
            return optional.Equals(value);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified value is unequal to the specified
        ///     optional instance.
        ///     See <see cref="Equals(T)"/> for additional information how the optional
        ///     equality comparison works.
        /// </summary>
        /// <param name="value">A value to be compared with the held value of the optional.</param>
        /// <param name="optional">The optional to be compared with the value.</param>
        /// <returns>
        ///     <see langword="true"/> if the value is unequal to the optional;
        ///     <see langword="false"/> if not.
        /// </returns>
        /// <seealso cref="Equals(T)"/>
        public static bool operator !=(T value, Optional<T> optional)
        {
            return !(value == optional);
        }

        /// <inheritdoc cref="operator ==(T, Optional{T})"/>
        public static bool operator ==(Optional<T> optional, T value)
        {
            return optional.Equals(value);
        }

        /// <inheritdoc cref="operator !=(T, Optional{T})"/>
        public static bool operator !=(Optional<T> optional, T value)
        {
            return !(optional == value);
        }

        /// <summary>
        ///     Returns a new <see cref="Optional{T}"/> instance which is non-empty and holds the
        ///     specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to be held by the optional.</param>
        public static implicit operator Optional<T>(T value)
        {
            return new Optional<T>(value);
        }

        /// <summary>
        ///     Returns the held value of the specified <paramref name="optional"/> if the optional
        ///     is non-empty and throws an exception for an empty optional.
        /// </summary>
        /// <returns>
        ///     The held value if the optional holds a value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     The optional is empty. No value can be retrieved from an empty optional.
        /// </exception>
        /// <seealso cref="GetValue"/>
        public static explicit operator T(Optional<T> optional)
        {
            return optional.GetValue();
        }

    }

}
