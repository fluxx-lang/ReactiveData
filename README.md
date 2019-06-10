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

```csharp
    new Label { } .Text(() => Settings.ShowAlias ? User.Alias : $"{User.FirstName} {User.LastName}"),
```

The key thing here is that the `Settings.UseAlias ? User.Alias : $"{User.FirstName} {User.LastName}"`, which is set as the Label text,
is reactive. If any component of that expression changes, it's automatically re-evaluated and the label text updated.
Normally the XAML binding is restricted to single properties, but ReactiveData allows arbitrary expressions to be "bound".

## Usage

### Getting started

```csharp
// A ReactiveVar is a simple mutable object with a Set method to update it in its entirety. It can turn
// any existing type--often a primitive type--into a reactive type. You can create it with "new" or,
// more concisely, with the factory method
ReactiveVar<int> var1 = new ReactiveVar<int>(1);
ReactiveVar<int> var2 = ReactiveVar(2);

// A ReactiveExpression is computed based on other reactive data. When any of the components change, the expression itself changes
Reactive<int> sumExpression = ReactiveExpression(() => var1.Value + var2.Value);

// Expressions can depend on other expressions. They can call nested functions too, being arbitrarily complex
Reactive<int> doubleSumExpression = ReactiveExpression(() => sumExpression.Value * 2);

// React to changes in doubleSumExpression using the Changed event, showing a message when it changes.
// Changed is used to run code with side effects when reactive data changes. It can, for instance, update UI.
doubleSumExpression.Changed += () => {
    Console.WriteLine($"Double sum is now: {doubleSumExpression.Value}");
};

// All updates are made inside a transaction; if you try to update data outside a transaction, you'll get an error
Transaction.Start();
var1.Set(3);
var2.Set(2);

// Note that Changed notifications are only sent when the Transaction completes. That ensures all data is in it's final state and no "glitches" are possible.
Console.WriteLine($"About to commit transaction; should show a new value of 10");
Transaction.End();
```

Here's the output:

```
About to commit transaction; should show a new value of 10
Double sum is now: 10
```

### Reactive objects

Normally you'll have whole objects that are reactive like model objects or view model objects.
The simplest way to create a reactive object is to derive from ReactiveObject.

```csharp
public class User : ReactiveObject
{
    private string _firstName = "";
    private string _lastName = "";

    /// <summary>
    /// Property implementations should call the Get and Set methods. Set sends change notifications.
    /// Get is important because ReactiveData needs to know when objects are accessed in order to
    /// know what an expression depends on - that's where the magic happens.
    /// </summary>
    public string FirstName {
        get => Get(_firstName);
        set => Set(out _firstName, value);
    }

    public string LastName {
        get => Get(_lastName);
        set => Set(out _lastName, value);
    }
}
```

Here's code that uses the reactive object. 

```csharp
var user = new User();

Reactive<string> fullName = ReactiveExpression(() => $"{user.FirstName} {user.LastName}");

// Print a message when the fullName changes
fullName.Changed += () => {
    Console.WriteLine($"Value is now: {fullName.Value}");
};

Transaction.Start();
user.FirstName = "John";
user.LastName = "Smith";
Console.WriteLine($"When transaction ends, fullName will change to 'John Smith'");
Transaction.End();
```

Here's the output:
```
When transaction ends, fullName will change to 'John Smith'
Value is now: John Smith
```

### Supporting both reactivity and INotifyPropertyChanged

If you want your object to both be reactive, supporting the IReactive interface, and support
INotifyPropertyChange for interoperability with other code (like XAML bindings) that uses that,
then derive from ReactiveInpcObject instead. You'll get both.

```csharp
public class UserSupportingInpc : ReactiveInpcObject
{
    private string _firstName = "";
    private string _lastName = "";

    public string FirstName {
        get => Get(_firstName);
        set => Set(ref _firstName, value);
    }

    public string LastName {
        get => Get(_lastName);
        set => Set(ref _lastName, value);
    }
}
````

 As another variant, if you have objects that implement INotifyPropertyChange and don't want to change their  definition, you can use the ReactiveInpc wrapper.
 
 Here's a standard INotifyPropertyChange type:

```csharp
public class UserSupportingOnlyInpc : INotifyPropertyChanged
{
    private string _firstName = "";
    private string _lastName = "";

    public event PropertyChangedEventHandler PropertyChanged;

    protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!Equals(storage, value)) {
            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public string FirstName {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    public string LastName {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }
}
```

Use ReactiveInpc to turn it, or any INotifyPropertyChange object, into a reactive object.

```csharp
var user = new UserSupportingOnlyInpc();

Reactive<UserSupportingOnlyInpc> reactiveUser = ReactiveInpc(user);
Reactive<string> fullName = new ReactiveExpression<string>(() => $"{reactiveUser.Value.FirstName} {reactiveUser.Value.LastName}");

OutputWhenChanged(fullName);

Transaction.Start();
user.FirstName = "John";
user.LastName = "Smith";
Console.WriteLine($"When transaction ends, fullName will change to 'John Smith'");
Transaction.End();
```
