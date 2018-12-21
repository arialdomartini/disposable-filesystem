using System;
using System.IO;

namespace DisposableFileSystem
{
    public class DisposableDirectory : IDisposable
    {
        public DisposableDirectory()
        {
            Path = GetRandomPath();
            Directory.CreateDirectory(Path);
        }

        private string GetRandomPath() =>
            System.IO.Path.Combine(
                System.IO.Path.GetTempPath(), 
                System.IO.Path.GetRandomFileName());

        public string Path { get; }
        public void Dispose()
        {
            RecursivelyDelete();
        }

        private void RecursivelyDelete()
        {
            Directory.Delete(Path, true);
        }
    }
}