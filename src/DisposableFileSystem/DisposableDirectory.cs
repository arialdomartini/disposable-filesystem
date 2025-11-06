using System;
using System.IO;
using System.Linq;
using static System.IO.Path;

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
                GetTempPath(),
                GetRandomFileName());

        void IDisposable.Dispose()
        {
            RecursivelyDelete();
        }

        private void RecursivelyDelete() =>
            Directory.Delete(Path, true);

        public string CreateDirectory(params string[] directories) =>
            Directory.CreateDirectory(Combine(directories)).FullName;

        public string Combine(params string[] directories) =>
            System.IO.Path.Combine(directories.Prepend(Path).ToArray());

        public string RandomFileName() =>
            System.IO.Path.Combine(
                Path,
                GetRandomFileName());

        public static void InADisposable(Action<DisposableDirectory> action)
        {
            using (var disposableDirectory = Create())
            {
                action(disposableDirectory);
            }
        }
    }
}
