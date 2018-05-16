# ReactiveData

[MobX](https://github.com/mobxjs/mobx) style reactive state management for .NET

## Introduction

ReactiveData lets you build data structures that are _reactive_ - that send notifications on change so that other data can automatically update or code automatically run.

With it you can create say UI that automatically updates when model or view model state changes. ReactiveData is especially a good choice for coded UIs, that don't use XAML.

ReactiveData brings a few advantages over the simple INotifyPropertyChanged:

- Dependencies are tracked automatically, fine grained, and updated dynamically as they change. That makes it easy to use and very efficient.
- Updates, even to a complex graph where several pieces of data change, are guaranteed to be _glitch_ free, never capturing data in an intermediate state.

The magic of automatic dependences and glitch free updates both happen via the same algorithm used by MobX, described in detail [here](https://hackernoon.com/becoming-fully-reactive-an-in-depth-explanation-of-mobservable-55995262a254).

