﻿using System;
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
            _sut = DisposableDirectory.Create();
        }

        public void Dispose()
        {
            _sut.Dispose();
        }

        [Fact]
        public void disposable_directories_have_random_names()
        {
            using (var directory1 = DisposableDirectory.Create())
            using (var directory2 = DisposableDirectory.Create())
            {
                directory1.Path.Should().NotBe(directory2.Path);
            }
        }

        [Fact]
        public void directories_are_created_in_the_system_temp_directory()
        {
            var directory = _sut.Path;

            var parent = Path.GetDirectoryName(directory);

            parent.Should().Be(Path.GetDirectoryName(Path.GetTempPath()));
        }

        [Fact]
        public void when_disposed_of_the_directory_and_its_content_are_deleted()
        {
            string filePath;
            using (var sut = DisposableDirectory.Create())
            {
                filePath = Path.Combine(sut.Path, "some-file.txt");
                File.WriteAllText(filePath, "some text");
                File.Exists(filePath).Should().Be(true);
            }

            File.Exists(filePath).Should().Be(false);
        }

        [Fact]
        public void allows_the_creation_of_subdirectories()
        {
            var result = _sut.CreateDirectory("some_directory");

            var parentDirectory = Directory.GetParent(result).FullName;
            parentDirectory.Should().Be(_sut.Path);
        }

        [Fact]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public void allows_the_creation_of_nested_subdirectories_providing_a_collection_of_directory_names()
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

        [Fact]
        public void allows_the_creation_of_files()
        {
            var fileName = _sut.RandomFileName();

            File.WriteAllText(fileName, "some text");

            File.ReadAllText(fileName).Should().Be("some text");
        }

        [Fact]
        public void created_files_are_deleted_during_disposal()
        {
            string fileName;
            using (var directory = DisposableDirectory.Create())
            {
                fileName = directory.RandomFileName();
                File.WriteAllText(fileName, "some text");

                File.Exists(fileName).Should().Be(true);
            }

            File.Exists(fileName).Should().Be(false);
        }

        [Fact]
        public void file_names_are_random()
        {
            var fileName1 = _sut.RandomFileName();
            var fileName2 = _sut.RandomFileName();

            fileName1.Should().NotBe(fileName2);
        }

        [Fact]
        public void calculates_paths_from_root()
        {
            var path = _sut.Combine("one", "two", "three", "some-file.txt");

            path.Should().Be(Path.Combine(_sut.Path, "one", "two", "three", "some-file.txt"));
        }
    }
}