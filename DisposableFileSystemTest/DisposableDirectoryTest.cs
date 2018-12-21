using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DisposableFileSystem;
using FluentAssertions;
using Xunit;

namespace DisposableFileSystemTest
{
    public class DisposableDirectoryTest : IDisposable
    {
        private readonly DisposableDirectory _sut;

        public DisposableDirectoryTest()
        {
            _sut = new DisposableDirectory();
        }

        [Fact]
        public void should_create_random_directories()
        {
            using (var directory1 = new DisposableDirectory())
            using (var directory2 = new DisposableDirectory())
            {
                directory1.Path.Should().NotBe(directory2.Path);
            }
        }

        [Fact]
        public void should_create_directories_in_the_system_temp_directory()
        {
            var directory = _sut.Path;

            var parent = Path.GetDirectoryName(directory);

            parent.Should().Be(Path.GetDirectoryName(Path.GetTempPath()));
        }
        
        [Fact]
        public void should_delete_the_directory_and_its_content_when_disposing_of_it()
        {
            string filePath;
            using (var sut = new DisposableDirectory())
            {
                filePath = Path.Combine(sut.Path, "some-file.txt");
                File.WriteAllText(filePath, "some text");
                File.Exists(filePath).Should().Be(true);
            }
            File.Exists(filePath).Should().Be(false);
        }

        [Fact]
        public void should_allow_the_creation_of_subdirectories()
        {
            var result = _sut.CreateDirectory("some_directory");

            var parentDirectory = Directory.GetParent(result).FullName;
            parentDirectory.Should().Be(_sut.Path);
        }

        [Fact]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void should_allow_the_creation_of_nested_subdirectories()
        {
            var result = _sut.CreateDirectory("dir1", "dir2", "dir3");

            var dir3 = new DirectoryInfo(result);
            var dir2 = dir3.Parent;
            var dir1 = dir2.Parent;
            var root = dir1.Parent;

            root.FullName.Should().Be(_sut.Path);
            dir1.Name.Should().Be("dir1");
            dir2.Name.Should().Be("dir2");
            dir3.Name.Should().Be("dir3");
        }

        public void Dispose()
        {
            _sut.Dispose();
        }
    }
}