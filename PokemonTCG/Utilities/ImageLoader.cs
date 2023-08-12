using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;


namespace PokemonTCG.Utilities
{
    /// <summary>
    /// Metrhods for loading Images from the file system.
    /// </summary>
    internal class ImageLoader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">The path to the image</param>
        /// <returns>A <c>Task</c> that returns a stream of the image when complete</returns>
        internal static async Task<IRandomAccessStream> OpenImage(string url, bool absolutePath = false)
        {
            StorageFile file = await FileUtil.GetFile(url, absolutePath);
            IRandomAccessStream fileStream = await file.OpenReadAsync();
            return fileStream;
        }

    }

}
