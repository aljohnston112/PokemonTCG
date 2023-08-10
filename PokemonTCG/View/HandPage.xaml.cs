using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using PokemonTCG.Utilities;
using PokemonTCG.ViewModel;

namespace PokemonTCG.View
{
    /// <summary>
    /// The UI for the player's hand during a game
    /// </summary>
    public sealed partial class HandPage : Page
    {

        public HandPage()
        {
            InitializeComponent();
        }

        public void SetViewModel(HandViewModel viewModel)
        {
            viewModel.images.CollectionChanged += HandChanged;
        }

        private readonly List<ColumnDefinition> ColumnDefinitions = new();
        private readonly List<string> ImagePaths = new();
        private readonly List<Image> Images = new();

        private void HandChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems != null)
            {
                foreach (string item in e.NewItems)
                {
                    HandGrid.ColumnDefinitions.Remove(ColumnDefinitions[^1]);
                    ColumnDefinitions.RemoveAt(ColumnDefinitions.Count - 1);
                    Images.RemoveAt(ImagePaths.IndexOf(item));
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

                    Image image = new() {
                        Source = new BitmapImage(new Uri(FileUtil.GetFullPath(item))),
                        Stretch = Stretch.Uniform,
                    };
                    image.Tapped += ImageTapped;
                    FlyoutBase.SetAttachedFlyout(image, ImagePreviewFlyout);

                    Images.Add(image);
                    ImagePaths.Add(item);
                    Grid.SetColumn(image, Images.Count - 1);
                    HandGrid.Children.Add(image);
                }
            }
        }

        private void ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            (((Flyout.GetAttachedFlyout(sender as Image)) as Flyout).Content as Image).Source = (sender as Image).Source;
            Flyout.ShowAttachedFlyout(sender as Image);
        }
    }

}
