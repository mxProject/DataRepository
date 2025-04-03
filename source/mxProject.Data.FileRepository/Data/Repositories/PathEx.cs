using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace mxProject.Data.Repositories
{
    internal static class PathEx
    {
        /// <summary>
        /// Gets the full path for the specified path.
        /// If the path specified is not the root path, the full path relative to the current directory is obtained.
        /// </summary>
        /// <param name="filePath">The path.</param>
        /// <returns>The full path.</returns>
        internal static string GetFullPath(string filePath)
        {
            if (Path.IsPathRooted(filePath))
            {
                return filePath;
            }
            else
            {
                return Path.GetFullPath(filePath);
            }
        }

        /// <summary>
        /// Gets the absolute path of the specified path.
        /// </summary>
        /// <param name="filePath">The path, relative path is expected.</param>
        /// <param name="basePath">The base path.</param>
        /// <returns>The absolute path.</returns>
        internal static string ToAbsolutePath(string filePath, string basePath)
        {
            if (basePath.Last() != Path.DirectorySeparatorChar)
            {
                basePath += Path.DirectorySeparatorChar;
            }

            if (basePath.Contains("%"))
            {
                basePath = basePath.Replace("%", "%25");
            }

            if (filePath.Contains("%"))
            {
                filePath = filePath.Replace("%", "%25");
            }

            var baseUrl = new Uri(basePath);
            var url = new Uri(baseUrl, filePath);

            string absolutePath = url.LocalPath;

            if (absolutePath.Contains("%25"))
            {
                return absolutePath.Replace("%25", "%");
            }
            else
            {
                return absolutePath;
            }
        }

        /// <summary>
        /// Gets the relative path of the specified path.
        /// </summary>
        /// <param name="filePath">The path, absolute path is expected.</param>
        /// <param name="basePath">The base path.</param>
        /// <returns>The relative path.</returns>
        internal static string ToRalativePath(string filePath, string basePath)
        {
            if (basePath.Last() != Path.DirectorySeparatorChar)
            {
                basePath += Path.DirectorySeparatorChar;
            }

            if (basePath.Contains("%"))
            {
                basePath = basePath.Replace("%", "%25");
            }

            if (filePath.Contains("%"))
            {
                filePath = filePath.Replace("%", "%25");
            }

            var baseUrl = new Uri(basePath);
            var url = new Uri(filePath);
            Uri relativeUrl = baseUrl.MakeRelativeUri(url);

            string relativePath = Uri.UnescapeDataString(relativeUrl.ToString());

            if (relativePath.Contains("%25"))
            {
                return relativePath.Replace("%25", "%");
            }

            if (relativePath.Contains("/"))
            {
                return relativePath.Replace('/', Path.DirectorySeparatorChar);
            }
            else
            {
                return relativePath;
            }
        }

        /// <summary>
        /// Normalize the extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        internal static string NormalizeExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension)) { return string.Empty; }

            return extension[0] == '.' ? extension : '.' + extension;
        }
    }
}
