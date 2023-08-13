using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokemonTCG.Utilities
{
    /// <summary>
    /// Helper methods for the file system.
    /// </summary>
    internal class FileUtil
    {

        private static readonly string BASE_PATH = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);


        /// <summary>
        /// Loads a file from the file system.
        /// </summary>
        /// <param name="fileUrl">The path to the file.</param>
        /// <returns>A <c>Task</c> that returns a <c>StorageFile</c> of the file when complete.</returns>
        internal static async Task<StorageFile> GetFile(string fileUrl)
        {
            string sourcePath = BASE_PATH + fileUrl;
            return await StorageFile.GetFileFromPathAsync(sourcePath);
        }

        internal static string GetFullPath(string fileUrl)
        {
            return BASE_PATH + fileUrl;
        }

    }

}
