using System;
using System.Collections.Generic;
using ReactiveData;
using ReactiveData.Sequence;
using Xamarin.Forms;
using static ReactiveXamarinForms.Views.ViewSequenceUtils;

namespace ReactiveXamarinForms.Views
{
    public static class LayoutExtensions {
        public static TLayout Children<TLayout>(this TLayout layout, params ISequence<View>[] sequences) where TLayout : Layout<View>
        {
            Sequences(sequences);
            return layout;
        }

        public static TLayout Children<TLayout>(this TLayout layout, SequenceGenerator sequences) where TLayout : Layout<View>
        {
            //Sequences(sequences);
            return layout;
        }

        public static void test(List<String> strings)
        {

        }

        public static void test2()
        {
            test( new List<string>{"abc", "def"} );

            test(new List<string> { "abc", "def" });
        }

        public delegate IEnumerable<IReactive<IEnumerable<View>>> SequenceGenerator();
    }

    public static class ViewSequenceUtils
    {
        public static ItemsSequence<View> Items(params View[] items)
        {
            return new ItemsSequence<View>(items);
        }

        public static ParentSequence<View> Sequences<View>(params ISequence<View>[] sequences)
        {
            return new ParentSequence<View>(sequences);
        }

        public static ExpressionReactiveSequence<View> Expression(Func<INonreactiveSequence<View>> expressionFunction)
        {
            return new ExpressionReactiveSequence<View>(expressionFunction);
        }

        public static IfBuilder If(Func<bool> condition, params View[] items)
        {
            return null;
        }

        public class IfBuilder
        {
            public IfBuilder ElseIf(Func<bool> condition, params View[] items)
            {
                return null;
            }

            public ExpressionReactiveSequence<View> Else(params View[] items)
            {
                return null;
            }

            public ExpressionReactiveSequence<View> End()
            {
                return null;
            }

        }
    }

    public class CodeTestPage : ContentPage
    {
        private readonly ReactiveVar<int> _counter = new ReactiveVar<int>(1);
        private readonly ReactiveVar<bool> _showMore = new ReactiveVar<bool>(false);


        public CodeTestPage()
        {
            Button incrementButton;
            Button decrementButton;

            Content = new TableView
            {
                HasUnevenRows = true,
                Root = new TableRoot
                {
                    new TableSection("Table Section 1")
                    {
                        new TextCell
                        {
                            Detail = "detail"
                        }
                        .React(me => me.Text = $"Counter value is: {_counter.Value}"),

                        new ViewCell
                        {
                            View = new StackLayout {
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    (incrementButton = new Button {Text = "Increment Counter"}),
                                    (decrementButton = new Button {Text = "Decrement Counter"})
                                }
                            }
                        },

                        new ViewCell
                        {
                            View = new StackLayout {
                                Orientation = StackOrientation.Horizontal
                            }
#if false
                            .Children(new ReactiveListBuilder<View> {
                                new Label {Text = "OS Version"},
                                new Label {Text = "OS Version"},
                                If(() => _counter.Value > 3,
                                    new Label {Text = "OS Version"},
                                    new Label {Text = "OS Version"}
                                ),
                                new Label {Text = "OS Version"},
                            })
                            .Children( Items(
                                new Label {Text = "OS Version"},
                                new Label {Text = "OS Version"} ),
                                If(() => _counter.Value > 3, Items(
                                    new Label {Text = "OS Version"},
                                    new Label {Text = "OS Version"} ),
                                Else: Items(
                                    new Label {Text = "OS Version"},
                                    new Label {Text = "OS Version"} )), Items(
                                new Label {Text = "OS Version"}
                            )),

                            .Children( Items(
                                new Label {Text = "OS Version"},
                                new Label {Text = "OS Version"}  ), Expr(() => {
                                if (_counter.Value > 3)  return Items(
                                    new Label { Text = "OS Version" },
                                    new Label { Text = "OS Version" }  );
                                else return Items(
                                    new Label { Text = "OS Version" },
                                    new Label { Text = "OS Version" } ); }), Items(
                                new Label { Text = "OS Version" } )
                             ),
#endif
                            .Children( Items(
                                new Label {Text = "OS Version"},
                                new Label {Text = "OS Version"}  ), Expression(() => {
                                if (_counter.Value > 3)  return Items(
                                    new Label { Text = "OS Version" },
                                    new Label { Text = "OS Version" }  );
                                else return Items(
                                    new Label { Text = "OS Version" },
                                    new Label { Text = "OS Version" }); }), Items(
                                new Label { Text = "OS Version" } )
                             )
                            .Children( Items(
                                new Label {Text = "OS Version"},
                                new Label {Text = "OS Version"}  ), Expression(() => 
                                (_counter.Value > 3) ? Items(
                                    new Label { Text = "OS Version" },
                                    new Label { Text = "OS Version1" }  )
                                : Items(
                                    new Label { Text = "OS Version" },
                                    new Label { Text = "OS Version" }) ), Items(
                                new Label { Text = "OS Version" } )
                             )
                            .Children( Items(
                                new Label {Text = "OS Version"},
                                new Label {Text = "OS Version"}  ),
                                If (() => _counter.Value > 3,
                                    new Label { Text = "OS Version" },
                                    new Label { Text = "OS Version" })
                                .Else(
                                    new Label { Text = "OS Version" },
                                    new Label { Text = "OS Version" }), Items(
                                new Label { Text = "OS Version" } )
                            )
                            .Children(
                                Items(
                                    new Label {Text = "OS Version"},
                                    new Label {Text = "OS Version"} ),
                                Expression(() => {
                                    if (_counter.Value > 3) return Items(
                                        new Label { Text = "OS Version" },
                                        new Label { Text = "OS Version" }  );
                                    else return Items(
                                        new Label { Text = "OS Version" },
                                        new Label { Text = "OS Version" } ); }),
                                Items(
                                    new Label { Text = "OS Version" } )
                             )
                            .Children(
                                Items(
                                    new Label {Text = "OS Version"},
                                    new Label {Text = "OS Version"}
                                ),
                                Expression(() => {
                                    if (_counter.Value > 3)
                                        return Items(
                                            new Label { Text = "OS Version" },
                                            new Label { Text = "OS Version" }
                                        );
                                    else
                                        return Items(
                                            new Label { Text = "OS Version" },
                                            new Label { Text = "OS Version" }
                                        );
                                }),
                                Items(
                                    new Label { Text = "OS Version" }
                                )
                            )
                        }
                    },
                }
            };

            incrementButton.Clicked +=
                (sender, e) => { _counter.Set(_counter.Value + 1); };
            decrementButton.Clicked +=
                (sender, e) => { _counter.Set(_counter.Value - 1); };
        }
    }
}