using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Image = Microsoft.UI.Xaml.Controls.Image;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using PokemonTCG.Models;
using PokemonTCG.ViewModel;
using PokemonType = PokemonTCG.Models.PokemonType;
using PokemonTCG.DataSources;
using System.Collections.Immutable;
using Microsoft.UI.Xaml.Navigation;

namespace PokemonTCG.View
{
    /// <summary>
    /// Displays a UI that lets the user edit a deck.
    /// </summary>
    public sealed partial class DeckEditorPage : Page
    {
        private readonly DeckEditorViewModel ViewModel = new();

        public DeckEditorPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            object deckName = e.Parameter;
            _ = ViewModel.OnNavigatedTo(deckName);
            CardGridView.ItemsSource = ViewModel.CardItemViews;
            SetUpCheckBoxes();
        }

        /// <summary>
        /// Sets up the callback for the check boxes, so they can be used to sift the cards.
        /// </summary>
        private void SetUpCheckBoxes()
        {

            // For the supertype checkboxes
            void superTypesCallback(DependencyObject sender, DependencyProperty property)
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsChecked.HasValue)
                {
                    string text = checkBox.Content.ToString();
                    CardSupertype supertype = PokemonCard.GetCardSuperType(text);
                    if (supertype == CardSupertype.Pokemon)
                    {
                        ViewModel.OnPokemonCheckBox(checkBox.IsChecked.Value);
                    }
                    else if (supertype == CardSupertype.Trainer)
                    {
                        ViewModel.OnTrainerCheckBox(checkBox.IsChecked.Value);
                    }
                    else if (supertype == CardSupertype.Energy)
                    {
                        ViewModel.OnEnergyCheckBox(checkBox.IsChecked.Value);
                    }
                }
            }

            CheckBoxPokemon.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, superTypesCallback);
            CheckBoxTrainer.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, superTypesCallback);
            CheckBoxEnergy.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, superTypesCallback);

            // For the type checkboxes
            void pokemonTypesCallback(DependencyObject sender, DependencyProperty property)
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsChecked.HasValue)
                {
                    string text = checkBox.Content.ToString();
                    PokemonType type = PokemonCard.GetPokemonType(text);
                    ViewModel.OnTypeCheckBox(type, checkBox.IsChecked.Value);
                }
            }

            foreach (UIElement element in StackPanelTypes.Children)
            {
                element.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, pokemonTypesCallback);
            }

            // For the in deck checkbox
            void inDeckCallback(DependencyObject sender, DependencyProperty property)
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsChecked.HasValue)
                {
                    ViewModel.InDeckCheckbox(checkBox.IsChecked.Value);
                }
            }
            CheckBoxInDeck.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, inDeckCallback);

        }

        /// <summary>
        /// The callback for the GridView named CardGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void UpdateListView(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.InRecycleQueue)
            {
                Grid templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;
                Image image = templateRoot.FindName("CardImage") as Image;
                image.Source = null;
                (templateRoot.FindName("NumberBox") as GridViewItem).Content = null;
            }

            if (args.Phase == 0)
            {
                args.RegisterUpdateCallback(UpdateListViewCard);
                args.Handled = true;
            }

        }

        /// <summary>
        /// The callback for updating an item in the GridView named CardGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void UpdateListViewCard(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase == 1)
            {
                CardItemView item = args.Item as CardItemView;
                void numberBoxHandler(NumberBox box, NumberBoxValueChangedEventArgs args)
                {
                    if (!double.IsNaN(args.NewValue))
                    {
                        ViewModel.ChangeCardItemCount((int)args.OldValue, (int)args.NewValue, item);
                    }
                }

                item.SetNumberBoxHandler(numberBoxHandler);
            }

        }
        /// <summary>
        /// For when the user clicks to submit a deck.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void SubmitDeckEvent(object sender, RoutedEventArgs args)
        {
            // TODO Warn that duplicate name overwrites
            string name = TextBlockDeckName.Text;
            if (name.Length == 0)
            {
                Flyout flyoutNoName = new();
                TextBlock text = new()
                {
                    Text = "A deck name is needed to make a deck."
                };
                flyoutNoName.Content = text;
                Flyout.SetAttachedFlyout(SubmitDeckButton, flyoutNoName);
                Flyout.ShowAttachedFlyout(SubmitDeckButton);
                Flyout.SetAttachedFlyout(SubmitDeckButton, null);
            }
            else if (ViewModel.NumberOfCardsInDeck != DeckDataSource.NUMBER_OF_CARDS_PER_DECK)
            {
                Flyout flyoutNotEnough = new();
                TextBlock text = new()
                {
                    Text = "60 cards are need to make a deck."
                };
                flyoutNotEnough.Content = text;
                Flyout.SetAttachedFlyout(SubmitDeckButton, flyoutNotEnough);
                Flyout.ShowAttachedFlyout(SubmitDeckButton);
                Flyout.SetAttachedFlyout(SubmitDeckButton, null);
            }
            else if (!ViewModel.HasBasicPokemon())
            {
                Flyout flyoutNotEnough = new();
                TextBlock text = new()
                {
                    Text = "At least one basic Pokemon is needed to make a deck."
                };
                flyoutNotEnough.Content = text;
                Flyout.SetAttachedFlyout(SubmitDeckButton, flyoutNotEnough);
                Flyout.ShowAttachedFlyout(SubmitDeckButton);
                Flyout.SetAttachedFlyout(SubmitDeckButton, null);
            } else
            {
                Flyout flyoutNotEnough = new();
                TextBlock text = new()
                {
                    Text = "Saving deck"
                };
                flyoutNotEnough.Content = text;
                Flyout.SetAttachedFlyout(SubmitDeckButton, flyoutNotEnough);
                Flyout.ShowAttachedFlyout(SubmitDeckButton);
                Flyout.SetAttachedFlyout(SubmitDeckButton, null);
                await ViewModel.CreateDeck(name);
                Frame.GoBack();
            }
        }

        private void CancelDeckEvent(object sender, RoutedEventArgs args)
        {
            this.Frame.GoBack();
        }

        private void ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void SearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string text = sender.Text;
            ViewModel.SearchStringUpdated(text);
        }

    }

}