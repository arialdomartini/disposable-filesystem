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

        private static string RandomPath() =>
            GetRandomFileName();

        public static DisposableDirectory Create()
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var randomPath =
                System.IO.Path.Combine(currentDirectory, RandomPath())
                    .EnsureExists();

            return new DisposableDirectory(randomPath);
        }

        public static void InADisposable(Action<DisposableDirectory> action)
        {
            using (var disposableDirectory = Create())
            {
                action(disposableDirectory);
            }
        }

        public string CreateDirectory(params string[] directories) =>
            Directory.CreateDirectory(Combine(directories)).FullName;

        public string RandomFileName() =>
            System.IO.Path.Combine(
                Path,
                GetRandomFileName());

        public string Combine(params string[] directories) =>
            System.IO.Path.Combine(directories.Prepend(Path).ToArray());

        private void RecursivelyDelete() =>
            Directory.Delete(Path, true);

        void IDisposable.Dispose()
        {
            RecursivelyDelete();
        }
    }
}
