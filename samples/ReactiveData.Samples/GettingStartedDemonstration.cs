using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static ReactiveData.ReactiveFactory;

namespace ReactiveData.Samples
{
    class GettingStartedDemonstration
    {
        static void Main(string[] args)
        {
            DemonstrateVarAndExpression();
            DemonstrateReactiveObject();
            DemonstrateReactiveInpcObject();
        }

        public static void DemonstrateVarAndExpression()
        {
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
        }

        /// <summary>
        /// Normally you'll have whole objects that are reactive like model objects or view model objects.
        /// The simplest way to create a reactive object is to derive from ReactiveObject.
        /// </summary>
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

        public static void DemonstrateReactiveObject()
        {
            var user = new User();

            Reactive<string> fullName = ReactiveExpression(() => $"{user.FirstName} {user.LastName}");

            // Print a message when the fullName changes
            OutputWhenChanged(fullName);

            Transaction.Start();
            user.FirstName = "John";
            user.LastName = "Smith";
            Console.WriteLine($"When transaction ends, fullName will change to 'John Smith'");
            Transaction.End();
        }

        /// <summary>
        /// If you want your object to both be reactive, supporting the IReactive interface, and support
        /// INotifyPropertyChange for interoperability with other code (like XAML bindings) that uses that,
        /// then derive from ReactiveInpcObject instead. You'll get both.
        /// </summary>
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

        /// <summary>
        /// As another variant, if you have objects that implement INotifyPropertyChange and don't want to change their
        /// definition, you can use the ReactiveInpc wrapper. Here's a standard INotifyPropertyChange type.
        /// </summary>
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

        public static void DemonstrateReactiveInpcObject()
        {
            var user = new UserSupportingOnlyInpc();

            Reactive<UserSupportingOnlyInpc> reactiveUser = ReactiveInpc(user);
            Reactive<string> fullName = new ReactiveExpression<string>(() => $"{reactiveUser.Value.FirstName} {reactiveUser.Value.LastName}");

            OutputWhenChanged(fullName);

            Transaction.Start();
            user.FirstName = "John";
            user.LastName = "Smith";
            Console.WriteLine($"When transaction ends, fullName will change to 'John Smith'");
            Transaction.End();
        }

        private static void OutputWhenChanged<T>(Reactive<T> reactiveObject)
        {
            reactiveObject.Changed += () => {
                string newValue = reactiveObject.Value.ToString();
                Console.WriteLine($"Value is now: {newValue}");
            };
        }
    }
}
