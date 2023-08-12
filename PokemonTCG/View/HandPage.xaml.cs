using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokemonTCG.Models;
using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for the player's hand during a game
    /// </summary>
    public sealed partial class HandPage : Page
    {
        private CardViewViewModel CardViewModel;

        public HandPage()
        {
            InitializeComponent();
        }

        internal void SetViewModels(HandViewModel handViewModel, CardViewViewModel cardViewModel)
        {
            handViewModel.Images.CollectionChanged += HandChanged;
            CardViewModel = cardViewModel;
        }

        private readonly List<ColumnDefinition> ColumnDefinitions = new();
        private readonly List<CardView> CardViews = new();
        private readonly List<string> ImagePaths = new();

        private void HandChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems != null)
            {
                foreach (string item in e.NewItems)
                {
                    HandGrid.ColumnDefinitions.Remove(ColumnDefinitions[^1]);
                    ColumnDefinitions.RemoveAt(ColumnDefinitions.Count - 1);
                    CardViews.RemoveAt(ImagePaths.IndexOf(item));
                    ImagePaths.Remove(item);
                }
            }

            if (e.NewItems != null)
            {
                foreach (string item in e.NewItems)
                {
                    ColumnDefinition cd = new()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    };
                    ColumnDefinitions.Add(cd);
                    HandGrid.ColumnDefinitions.Add(cd);

                    CardView cardView = new();
                    cardView.SetViewModel(CardViewModel);
                    CardViewModel.SetCardViewState(new CardViewState(item, true, true, true, true));
                    CardViews.Add(cardView);
                    ImagePaths.Add(item);

                    Grid.SetColumn(cardView, ImagePaths.Count - 1);
                    HandGrid.Children.Add(cardView);
                }
            }
        }


    }

}
