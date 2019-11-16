# Changelog

## NCommons.Monads v2.0.0

* **[Package]** Updated the NuGet package description.
* **[Breaking]** Removed `Either<TL, TR>`.
* **[Breaking]** Reworked `Variant<T1-8>` to follow the naming conventions of `Optional`.
* **[New]** Added new utility methods to the `Variant<T1-8>` classes.
* **[New]** Added `Optional<T>`.
* **[New]** Added `Optional`.
* **[Fix]** Updated the XML documentation.


## NCommons.Collections v2.0.0

* **[Package]** Updated the NuGet package description.
* **[Breaking]** Made the `WeakReferenceCollection.Enumerator` struct private.
* **[Fix]** Relaxed the `WeakReferenceCollection<T> where T : class` constraint to `WeakReferenceCollection<T> where T : class?`.
* **[Fix]** Updated the XML documentation.


## NCommons.Observables v1.2.4

* **[Package]** Updated the NuGet package description.
* **[Fix]** Updated the XML documentation.


## NCommons.Collections v1.2.2

* **[Fix]** Correctly flagged `PreviewingObservableCollection.CollectionChanging` as nullable.


## NCommons.Observables v1.2.3

* **[Fix]** Correctly flagged `ObservableObject.PropertyChanged` as nullable.
* **[Fix]** Correctly flagged `ObservableObject.PropertyChanging` as nullable.


## Older Versions

_Changes earlier than this have sadly not been documented. If you are interested in details, consider looking through old pull requests._
