using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;


namespace PokemonTCG.Utilities
{
    /// <summary>
    /// Metrhods for loading Images from the file system.
    /// </summary>
    public class ImageLoader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">The path to the image</param>
        /// <returns>A <c>Task</c> that returns a stream of the image when complete</returns>
        public static async Task<IRandomAccessStream> OpenImage(string url)
        {
            // Load the file
            StorageFile file = await FileUtil.GetFile(url);
            IRandomAccessStream fileStream = await file.OpenReadAsync();
            return fileStream;
        }

    }

}
