using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Image = Microsoft.UI.Xaml.Controls.Image;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using PokemonTCG.Models;
using PokemonTCG.ViewModel;
using System;
using PokemonType = PokemonTCG.Models.PokemonType;
using PokemonTCG.DataSources;
using System.Collections.Immutable;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Navigation;

namespace PokemonTCG.View
{
    /// <summary>
    /// Displays a UI that lets the user edit a deck.
    /// </summary>
    public sealed partial class DeckEditorPage : Page
    {
        private readonly DeckEditorViewModel _viewModel = new();

        public DeckEditorPage()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            object deckName = e.Parameter;
            ISet<string> sets = new HashSet<string>();
            if (deckName != null)
            {
                ImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
                foreach (string id in decks[deckName as string].CardIds)
                {
                    PokemonCard card = CardDataSource.GetCardById(id);
                    sets.Add(card.SetId);
                }
            }
            else
            {
                sets.Add("base1");
            }

            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            foreach (string set in sets)
            {
                string setFile = baseFolder + "Assets/sets/" + set + ".json";
                await _viewModel.LoadCardsItemsForSet(setFile);
            }

            CardGridView.ItemsSource = _viewModel.CardItems;
            if (deckName != null)
            {
                ImmutableDictionary<string, PokemonDeck> decks = DeckDataSource.GetDecks();
                foreach (string id in decks[deckName as string].CardIds)
                {
                    _viewModel.IncrementCardCountForCardWithId(id);
                }
            }

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
                        _viewModel.OnPokemonCheckBox(checkBox.IsChecked.Value);
                    }
                    else if (supertype == CardSupertype.Trainer)
                    {
                        _viewModel.OnTrainerCheckBox(checkBox.IsChecked.Value);
                    }
                    else if (supertype == CardSupertype.Energy)
                    {
                        _viewModel.OnEnergyCheckBox(checkBox.IsChecked.Value);
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
                    _viewModel.TypeChange(type, checkBox.IsChecked.Value);
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
                    _viewModel.InDeckChanged(checkBox.IsChecked.Value);
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

                Image image = templateRoot.FindName("EditorImage") as Image;
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
                Grid templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;
                CardItem item = args.Item as CardItem;

                void numberBoxHandler(NumberBox box, NumberBoxValueChangedEventArgs args)
                {
                    if (!double.IsNaN(args.NewValue))
                    {
                        _viewModel.ChangeCount((int)args.OldValue, (int)args.NewValue, item);
                    }
                }

                item.SetHandler(numberBoxHandler);
            }

        }
        /// <summary>
        /// For when the user clicks to submit a deck.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void SubmitDeckEvent(object sender, RoutedEventArgs args)
        {

            string name = TextBlockDeckName.Text;
            if (_viewModel.NumberOfCardsInDeck != 60)
            {
                Flyout flyoutNotEnough = new();
                TextBlock text = new()
                {
                    Text = "60 cards are need to make a deck."
                };
                flyoutNotEnough.Content = text;
                Flyout.SetAttachedFlyout(SubmitDeckButton, flyoutNotEnough);
                Flyout.ShowAttachedFlyout(SubmitDeckButton);

            }
            else if (name.Length == 0)
            {
                Flyout flyoutNotEnough = new();
                TextBlock text = new()
                {
                    Text = "A deck name is needed to make a deck."
                };
                flyoutNotEnough.Content = text;
                Flyout.SetAttachedFlyout(SubmitDeckButton, flyoutNotEnough);
                Flyout.ShowAttachedFlyout(SubmitDeckButton);
            }
            else
            {
                Flyout flyoutNotEnough = new();
                TextBlock text = new()
                {
                    Text = "Saving deck"
                };
                flyoutNotEnough.Content = text;
                Flyout.SetAttachedFlyout(SubmitDeckButton, flyoutNotEnough);
                Flyout.ShowAttachedFlyout(SubmitDeckButton);
                await _viewModel.CreateDeck(name);
                Frame.GoBack();
            }
        }

        /// <summary>
        /// For when the user clicks to cancel a deck.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CancelDeckEvent(object sender, RoutedEventArgs args)
        {
            this.Frame.GoBack();
        }

        /// <summary>
        /// To show a larger image when the user taps one of the Images in the GridView named CardGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        /// <summary>
        /// Sifts the cards that are shown when the user updates the search field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            string text = sender.Text;
            _viewModel.SearchStringUpdated(text);
        }

    }

}