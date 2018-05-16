using ReactiveData;
using ReactiveData.ReactiveSequence;
using Xamarin.Forms;

namespace ReactiveXamarinForms.Views
{
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
                        .Reactive(me => me.Text = $"Counter value is: {_counter.Value}"),

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

                        new TextCell
                        {
                            Text = "OS Version",
                            Detail = "OS verson"
                        }
                    },

                    new TableSection("My Device Info")
                    .WithReactiveItems(new ReactiveItemsBuilder<Cell> {
                        new TextCell {Text = "OS Version", Detail = "OS verson"},
                        new TextCell {Text = "OS Version", Detail = "OS verson"},
                        new ReactiveOptionalItems<Cell>(
                            () => _counter.Value > 3,
                            new TextCell {Text = "OS Version", Detail = "OS verson"},
                            new TextCell {Text = "OS Version", Detail = "OS verson"}
                        )
                    }),

                    new TableSection("My Device Info")
                    .WithReactiveItems(new ReactiveItemsBuilder<Cell> {
                        new TextCell {Text = "OS Version", Detail = "OS verson"},
                        new TextCell {Text = "OS Version", Detail = "OS verson"},
                        new ReactiveOptionalItems<Cell>(
                            () => _counter.Value > 3,
                            new TextCell {Text = "OS Version", Detail = "OS verson"},
                            new TextCell {Text = "OS Version", Detail = "OS verson"}
                        )
                    }),

                    new TableSection
                    {
                        new TextCell
                        {
                            Text = "Another Cell",
                            Detail = "VersionNumber"
                        }
                    }
                }
            };

            incrementButton.Clicked +=
                (sender, e) => { _counter.UpdateValue(_counter.CurrentValue + 1); };
            decrementButton.Clicked +=
                (sender, e) => { _counter.UpdateValue(_counter.CurrentValue - 1); };
        }
    }
}