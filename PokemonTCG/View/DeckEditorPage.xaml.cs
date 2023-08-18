using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;
using PokemonTCG.States;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{
    /// <summary>
    /// Displays a UI that lets the user edit a deck.
    /// </summary>
    public sealed partial class DeckEditorPage : Page
    {
        private readonly DeckEditorViewModel ViewModel = new();
        private readonly CardItemAdapter CardItemAdapter = new();

        public DeckEditorPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object deckName = e.Parameter;
            await ViewModel.OnNavigatedTo(deckName, CardItemAdapter);
            SetUpCheckBoxes();
        }

        /// <summary>
        /// Sets up the callback for the check boxes, so they can be used to sift the cards.
        /// </summary>
        private void SetUpCheckBoxes()
        {

            // Supertype checkboxes
            void superTypesCallback(DependencyObject sender, DependencyProperty property)
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsChecked.HasValue)
                {
                    string text = checkBox.Content.ToString();
                    _ = CardItemAdapter.IncludeSupertype(text, checkBox.IsChecked.Value);
                }
            }

            CheckBoxPokemon.RegisterPropertyChangedCallback(ToggleButton.IsCheckedProperty, superTypesCallback);
            CheckBoxTrainer.RegisterPropertyChangedCallback(ToggleButton.IsCheckedProperty, superTypesCallback);
            CheckBoxEnergy.RegisterPropertyChangedCallback(ToggleButton.IsCheckedProperty, superTypesCallback);

            // For the type checkboxes
            void pokemonTypesCallback(DependencyObject sender, DependencyProperty property)
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsChecked.HasValue)
                {
                    string text = checkBox.Content.ToString();
                    _ = CardItemAdapter.InludeType(text, checkBox.IsChecked.Value);
                }
            }

            foreach (UIElement checkBox in StackPanelTypes.Children)
            {
                checkBox.RegisterPropertyChangedCallback(ToggleButton.IsCheckedProperty, pokemonTypesCallback);
            }

            // For the in deck checkbox
            void inDeckCallback(DependencyObject sender, DependencyProperty property)
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsChecked.HasValue)
                {
                    _ = CardItemAdapter.IncludeOnlyCardsInDeck(checkBox.IsChecked.Value);
                }
            }
            CheckBoxInDeck.RegisterPropertyChangedCallback(ToggleButton.IsCheckedProperty, inDeckCallback);
        }

        private async void SearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            await CardItemAdapter.UpdateSearchString(sender.Text);
        }

        /// <summary>
        /// The callback for the CardGridView.
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
                image.Tapped -= FlyoutUtil.ImageTapped;
            }

            if (args.Phase == 0)
            {
                args.RegisterUpdateCallback(UpdateListViewCardItem);
                args.Handled = true;
            }

        }

        /// <summary>
        /// The callback for updating an item in the CardGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void UpdateListViewCardItem(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase == 1)
            {
                CardItem cardItem = args.Item as CardItem;
                Grid templateRoot = args.ItemContainer.ContentTemplateRoot as Grid;
                NumberBox numberBox = templateRoot.FindName("NumberBox") as NumberBox;
                numberBox.Maximum = DeckEditorViewModel.GetCardLimit(cardItem);
                numberBox.Value = cardItem.Count;
                numberBox.Tag = cardItem;

                Image image = templateRoot.FindName("CardImage") as Image;
                image.Tapped += FlyoutUtil.ImageTapped;
            }
        }

        private async void NumberBoxHandler(NumberBox box, NumberBoxValueChangedEventArgs args)
        {
            if (box.Tag is CardItem cardItem && args.OldValue != args.NewValue)
            {
                if (!double.IsNaN(args.NewValue))
                {
                    await ViewModel.ChangeCardItemCount(
                        cardItemAdapter: CardItemAdapter,
                        cardId: cardItem.Id,
                        newValue: (int)args.NewValue
                        );
                }
                else
                {
                    await ViewModel.ChangeCardItemCount(
                       cardItemAdapter: CardItemAdapter,
                       cardId: cardItem.Id,
                       newValue: 0
                       );
                    // No input does not update the binding when the previous input was 0, so it has to be set manually
                    box.Value = 0;
                }
            }
        }

        private async void SubmitDeck(object sender, RoutedEventArgs args)
        {
            string deckName = TextBlockDeckName.Text;
            string errorText = await ViewModel.CheckUserInput(deckName, CardItemAdapter);
            if(errorText != null)
            {
                FlyoutUtil.ShowTextFlyout(errorText, SubmitDeckButton);
            }
            else
            {
                FlyoutUtil.ShowTextFlyout("Saving deck", SubmitDeckButton);
                await DeckEditorViewModel.SaveDeck(deckName, CardItemAdapter);
                Frame.GoBack();
            }
        }

        private void CancelDeck(object sender, RoutedEventArgs args)
        {
            Frame.GoBack();
        }

    }

}