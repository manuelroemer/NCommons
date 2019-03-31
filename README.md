# NCommons [![Build Status](https://dev.azure.com/ManuelRoemer/NCommons/_apis/build/status/NCommons%20(NuGet%20CI)?branchName=master)](https://dev.azure.com/ManuelRoemer/NCommons/_build/latest?definitionId=3&branchName=master) ![Nuget](https://img.shields.io/nuget/v/NCommons.svg)

A reusable set of common members that are missing in the .NET Framework.


## Description

This library is my take on the one that everybody writes himself: 
A collection of common utility members that may be used in every single .NET project.

I am, however, trying to not include "small things" like helper functions so that the library
doesn't become cluttered.
For instance, if you are looking for a `string.EqualsIgnoreCase` extension, you are looking in the
wrong place.
Anything that gets added to this library will at least have a medium level of complexity or 
boilerplate - basically everything, that is reusable and tedious to write.
Common examples are specialized collections (e.g. the `WeakReferenceCollection`) - they take time to
write but may be useful in any kind of larger project.


## Installation and development notes

Install the library via NuGet:

```
Install-Package NCommons

-- or --

dotnet add package NCommons
```

The project is using .NET Standard 2.0 as a compilation target.
The library is also using C# 8.0 with *Nullable Reference Types*, if you have that feature flag
enabled.

If you want to work with the source code, you will need an editor/IDE that supports C# 8.0.


## Documentation / Usage

Feel free to browse this documentation online for a full list of all members and their documentation:
[To be done](./README)

The main way to learn about the library's members is via XML documentation.
I spend a non-trivial amount of time on writing these. If you aren't sure what a class does, it is
usually worth it to look at its XML documentation (including hidden parts, like *remarks* :wink:).


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
