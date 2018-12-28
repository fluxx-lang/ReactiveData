[![Build Status](https://dev.azure.com/reactive-data/ReactiveData/_apis/build/status/reactive-data.ReactiveData?branchName=master)](https://dev.azure.com/reactive-data/ReactiveData/_build/latest?definitionId=1?branchName=master)

# ReactiveData

[MobX](https://github.com/mobxjs/mobx) style reactive state management for .NET

## Introduction

ReactiveData lets you build data structures that are _reactive_ - that send notifications on change so that other data can automatically update or code automatically run.

With it you can create say UI that automatically updates when model or view model state changes. ReactiveData is especially a good choice for coded UIs, that don't use XAML.

ReactiveData brings a few advantages over the simple INotifyPropertyChanged:

- Dependencies are tracked automatically, fine grained, and updated dynamically as they change. That makes it easy to use and efficient.
- Updates, even to a complex graph where several pieces of data change, are guaranteed to be _glitch_ free, never capturing data in an intermediate state.

The magic of automatic dependencies and glitch free updates happen via a similar algorithm to MobX, described in detail [here](https://hackernoon.com/becoming-fully-reactive-an-in-depth-explanation-of-mobservable-55995262a254).

## Use cases

ReactiveData is especially good for building UIs that automatically update when the underlying data model changes.
React.js and Reactive Native have popularized this model for building UIs in the JavaScript world, and ReactiveData helps bring
similar functionality to .NET.

Here's an example, with a coded UI for Xamarin.Forms (using some Xamarin.Forms specific helper methods):

```
    new Label { } .Text(() => Settings.ShowAlias ? User.Alias : $"{User.FirstName} {User.LastName}"),
```

The key thing here is that the `Settings.UseAlias ? User.Alias : $"{User.FirstName} {User.LastName}"`, which is set as the Label text,
is reactive. If any component of that expression changes, it's automatically re-evaluated and the label text updated.
Normally the XAML binding is restricted to single properties, but ReactiveData allows arbitrary expressions to be "bound".
