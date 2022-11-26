using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using Image = Microsoft.UI.Xaml.Controls.Image;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Diagnostics;
using Windows.Foundation;
using PokemonTCG.Models;
using PokemonTCG.ViewModel;
using System;
using Type = PokemonTCG.Models.Type;
using System.Xml.Linq;

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
            this.InitializeComponent();
            this.DataContext = _viewModel;
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string deckFile = baseFolder + "Assets/Decks/0 - Base.json";
            _viewModel.LoadCards(deckFile);
            this.CardGridView.ItemsSource = _viewModel.Cards;
            SetUpCheckBoxes();
        }

        /// <summary>
        /// Sets up the callback for the check boxes, so they can be used to sift the cards.
        /// </summary>
        private void SetUpCheckBoxes()
        {

            // For the supertype checkboxes
            DependencyPropertyChangedCallback callback = (sender, property) =>
            {
                string name = (string)sender.GetValue(CheckBox.NameProperty);
                bool? isChecked = ((CheckBox)sender).IsChecked;
                if (isChecked.HasValue)
                {
                    if (name == "CheckBoxPokemon")
                    {
                        _viewModel.PokemonCheckBox(isChecked.Value);
                    }
                    else if (name == "CheckBoxTrainer")
                    {
                        _viewModel.TrainerCheckBox(isChecked.Value);
                    }
                    else if (name == "CheckBoxEnergy")
                    {
                        _viewModel.EnergyCheckBox(isChecked.Value);
                    }
                }
            };

            CheckBoxPokemon.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, callback);
            CheckBoxTrainer.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, callback);
            CheckBoxEnergy.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, callback);

            // For the type checkboxes
            DependencyPropertyChangedCallback callbackTypes = (sender, property) =>
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsChecked.HasValue)
                {
                    string text = cb.Content.ToString();
                    PokemonType type = Type.GetType(text);
                    _viewModel.TypeChange(type, cb.IsChecked.Value);
                }
            };

            foreach (FrameworkElement element in StackPanelTypes.Children)
            {
                element.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, callbackTypes);
            }

            // For the in deck checkbox
            DependencyPropertyChangedCallback callbackInDeck = (sender, property) =>
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsChecked.HasValue)
                {
                    _viewModel.InDeckChanged(cb.IsChecked.Value);
                }
            };
            CheckBoxInDeck.RegisterPropertyChangedCallback(CheckBox.IsCheckedProperty, callbackInDeck);

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
                args.RegisterUpdateCallback(ShowImage);
                args.Handled = true;
            }

        }

        /// <summary>
        /// The callback for updating a container in the GridView named CardGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ShowImage(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase == 1)
            {
                Grid templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;

                // Image image = templateRoot.FindName("EditorImage") as Image;
                CardItem item = args.Item as CardItem;

                /*image.Source = item.Image;*/

                TypedEventHandler<
                    NumberBox,
                    NumberBoxValueChangedEventArgs
                > handler =
                    (box, args) =>
                    {
                        if (!double.IsNaN(args.NewValue))
                        {
                            _viewModel.ChangeCount(args, item);
                        }
                    };

                item.SetHandler(handler);
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
            if (_viewModel.GetTotalCount() != 60)
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
                List<string> cards = new List<string>();
                // TODO Cards are sifted!
                foreach (CardItem item in _viewModel.GetSelectedCards())
                {
                    cards.Add(item.Id);
                }
                PokemonDeck deck = new PokemonDeck(name, cards);
                await PokemonDeck.SaveDeck(deck);
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