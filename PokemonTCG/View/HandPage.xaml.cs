using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Models;
using PokemonTCG.Utilities;
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
        private readonly List<Image> CardImages = new();
        private readonly List<string> ImagePaths = new();

        private void HandChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems != null)
            {
                foreach (string item in e.NewItems)
                {
                    HandGrid.ColumnDefinitions.Remove(ColumnDefinitions[^1]);
                    ColumnDefinitions.RemoveAt(ColumnDefinitions.Count - 1);
                    CardImages.RemoveAt(ImagePaths.IndexOf(item));
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

                    Image image = new()
                    {
                        Source = new BitmapImage(new Uri(FileUtil.GetFullPath(item)))
                    };
                    image.Tapped += FlyoutUtil.ImageTapped;

                    Flyout flyout = Resources["ImagePreviewFlyout"] as Flyout;
                    FlyoutBase.SetAttachedFlyout(image, flyout);

                    IImmutableList<string> commands = ImmutableList.Create<string>();
                    image.ContextFlyout = FlyoutUtil.CreateCommandBarFlyout(commands);

                    CardImages.Add(image);
                    ImagePaths.Add(item);

                    Grid.SetColumn(image, ImagePaths.Count - 1);
                    HandGrid.Children.Add(image);
                }
            }
        }


    }

}
