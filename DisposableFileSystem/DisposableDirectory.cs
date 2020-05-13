using System;
using System.IO;
using System.Linq;

namespace DisposableFileSystem
{
    public class DisposableDirectory : IDisposable
    {
        public string Path { get; }

        private DisposableDirectory(string path)
        {
            Path = path;
        }

        public static DisposableDirectory Create() =>
            new DisposableDirectory(RandomPath().EnsureExists());

        private static string RandomPath() =>
            System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                System.IO.Path.GetRandomFileName());

        public void Dispose()
        {
            RecursivelyDelete();
        }

        private void RecursivelyDelete() =>
            Directory.Delete(Path, true);

        public string CreateDirectory(params string[] directories)
        {
            var enumerable = directories.ToList().Prepend(Path).ToArray();
            var directory = System.IO.Path.Combine(enumerable);
            Directory.CreateDirectory(directory);

            return directory;
        }

        public string RandomFileName() =>
            System.IO.Path.Combine(
                Path,
                System.IO.Path.GetRandomFileName());
    }
}
