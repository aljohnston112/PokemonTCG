using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokemonTCG.Utilities
{
    /// <summary>
    /// Helper methods for the file system.
    /// </summary>
    public class FileUtil
    {

        /// <summary>
        /// Loads a file from the file system.
        /// </summary>
        /// <param name="fileUrl">The path to the file.</param>
        /// <returns>A <c>Task</c> that returns a <c>StorageFile</c> of the file when complete.</returns>
        public static async Task<StorageFile> GetFile(string fileUrl)
        {
            String sourcePath = Path.GetFullPath(
                Path.Combine(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory),
                    fileUrl
                    )
                );
            return await StorageFile.GetFileFromPathAsync(sourcePath);
        }

        /// <summary>
        /// Loads a folder from the file system.
        /// </summary>
        /// <param name="folderUrl">The path to the folder</param>
        /// <returns>A <c>Task</c> that returns a <c>StorageFolder</c> of the folder when complete</returns>
        public static async Task<StorageFolder> GetFolder(string folderUrl)
        {
            var sourcePath = Path.GetFullPath(folderUrl);
            return await StorageFolder.GetFolderFromPathAsync(sourcePath);
        }

    }

}
