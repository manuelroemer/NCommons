# NCommons

_A set of several .NET libraries which provide common building blocks that
can be used by any kind of library or application._

[:books: Documentation](https://manuelroemer.github.io/NCommons) &nbsp; | &nbsp; [:package: NuGet](https://www.nuget.org/packages?q=ncommons)

<hr/>


NCommons is a growing set of several .NET libraries which aim to extend
the existing .NET Framework BCL.
In essence, NCommons could be described as a "utility library" which
defines common classes that may be useful in any kind of project.

While such libraries are certainly nothing new, NCommons has the following
goals and advantages:

* **Important members only:**<br/>
  NCommons is supposed to be a set of "no-nonsense" libraries.
  In essence, this means that there will be no library which contains
  members that can be written manually in a very short amount of time.
  Anything that gets added to one of these library will at least have a
  medium level of complexity or boilerplate - basically everything,
  that is reusable but tedious to write.
* **It feels like .NET:**<br/>
  A lot of time is spent on making NCommon's types feel like .NET.
  If you know .NET's BCL, you will feel at home when using one of
  these libraries.
* **Fully documented:**<br/>
  Nearly every public member exposed by the libraries is extensively
  documented with XML documentation comments.
* **C# 8.0/Nullable Reference Types Support:**<br/>
  The entire library has been built with support for Nullable Reference Types.


## Available Libraries

### NCommons.Collections [![Nuget](https://img.shields.io/nuget/v/NCommons.Collections)](https://www.nuget.org/packages/NCommons.Collections) ![netstandard2.0](https://img.shields.io/badge/netstandard2.0-lightgrey)

[:books: Documentation](https://manuelroemer.github.io/NCommons/api/NCommons.Collections.html) &nbsp; | &nbsp; [:package: NuGet ](https://www.nuget.org/packages/NCommons.Collections)

`NCommons.Collections` provides several specialized, generic collection-type
members which fill some open gaps in the `System.Collections.Generic` namespace.

Notable members include:

* [`PriorityQueue<T>`](https://manuelroemer.github.io/NCommons/api/NCommons.Collections.PriorityQueue-1.html)
* [`BinaryHeap<T>`](https://manuelroemer.github.io/NCommons/api/NCommons.Collections.BinaryHeap-1.html)
* [`PreviewingObservableCollection<T>`](https://manuelroemer.github.io/NCommons/api/NCommons.Collections.PreviewingObservableCollection-1.html)
* [`WeakReferenceCollection<T>`](https://manuelroemer.github.io/NCommons/api/NCommons.Collections.WeakReferenceCollection-1.html)


### NCommons.Monads [![Nuget](https://img.shields.io/nuget/v/NCommons.Monads)](https://www.nuget.org/packages/NCommons.Monads) ![netstandard2.0](https://img.shields.io/badge/netstandard2.0-lightgrey)

[:books: Documentation](https://manuelroemer.github.io/NCommons/api/NCommons.Monads.html) &nbsp; | &nbsp; [:package: NuGet ](https://www.nuget.org/packages/NCommons.Monads)

`NCommons.Monads` provides several members which assist in writing type-safe
and expressive code. Even though the package is called "Monads", the included
members don't necessarily have to be monadic.
In general, the goal of the package is to provide utility types, monadic or not,
which assist in writing precise code that still looks and behaves like traditional C#.
While inspired by functional programming languages, the provided types still look
and feel like types that could come straight from the .NET Framework's BCL.

Notable members include:

* [`Optional<T>`](https://manuelroemer.github.io/NCommons/api/NCommons.Monads.Optional-1.html)
* [`Variant<T1>, ..., Variant<T1, ..., T8>`](https://manuelroemer.github.io/NCommons/api/NCommons.Monads.Variant-2.html)


### NCommons.Observables [![Nuget](https://img.shields.io/nuget/v/NCommons.Observables)](https://www.nuget.org/packages/NCommons.Observables) ![netstandard2.0](https://img.shields.io/badge/netstandard2.0-lightgrey)

[:books: Documentation](https://manuelroemer.github.io/NCommons/api/NCommons.Observables.html) &nbsp; | &nbsp; [:package: NuGet ](https://www.nuget.org/packages/NCommons.Observables)

`NCommons.Observables` provides members which aim to reduce the amount of
required boilerplate code for correctly implement the MVVM pattern in UI-based
applications using frameworks like WPF or WinUI.

Notable members include:

* [`ObservableObject`](https://manuelroemer.github.io/NCommons/api/NCommons.Observables.ObservableObject.html)


## Installation

Each library described above is available on NuGet. You can install it via:

```sh
Install-Package [Package-Name]

--or--

dotnet add package [Package-Name]
```


## Documentation

While no hand-written articles are available, a lot of time has been
spent on writing XML documentation comments inside the source code.
The goal is to provide a nearly flawless documentation which
accurately describes each public member that is exposed by the libraries.

While the documentation can easily be viewed in your preferred IDE, you
can also browse it [online](https://manuelroemer.github.io/NCommons).

If you have any issues, feel free to open an issue.


## Contributing

* Do you feel that some crucial component is missing?
* Did you find a bug?
* Do you want to improve comments or code passages?

If any of this is true, feel free to open an issue or hit me with a pull request.
If you are going to put a larger amount of time into a PR, be sure to talk about the change first!
Otherwise, feel free to contribute or discuss!

**Be sure to also read the following notes:**


### Git and CI

Whenever a commit to `master` finishes, the project is automatically built and published to NuGet.
Changes to `master` must be in a 100% deployable state (i.e. tested and complete).

Changes should be created on `feature/` branches and collected on the `dev` branch.
Once there are enough changes to justify a new NuGet version, `dev` will be merged into `master`
and published.


## License

See the [LICENSE](./LICENSE) file for details.
