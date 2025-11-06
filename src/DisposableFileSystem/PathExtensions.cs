using System.IO;

namespace DisposableFileSystem
{
    internal static class PathExtensions
    {
        internal static string EnsureExists(this string path) =>
            Directory.CreateDirectory(path).FullName;
    }
}