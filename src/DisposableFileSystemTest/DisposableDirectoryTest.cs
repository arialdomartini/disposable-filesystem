using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using DisposableFileSystem;
using Xunit;
using static DisposableFileSystem.DisposableDirectory;

namespace DisposableFileSystemTest;

public class DisposableDirectoryTest
{
    [Fact]
    public void disposable_directories_are_independent()
    {
        using var directory1 = Create();
        using var directory2 = Create();

        Assert.NotEqual(directory1.Path, directory2.Path);
    }

    [Fact]
    public void directories_are_created_in_the_system_temp_directory()
    {
        using var disposableDirectory = Create();

        var systemTempDirectory = Path.GetTempPath();

        Assert.True(systemTempDirectory.Contains(disposableDirectory));
    }

    [Fact]
    public void temporary_files_are_deleted_after_use()
    {
        string tempFile;
        using (var directory = Create())
        {
            tempFile = directory
                .RandomFileName()
                .WithSomeContent();

            Assert.True(tempFile.Exists());
        }

        Assert.False(tempFile.Exists());
    }

    [Fact]
    public void allows_the_creation_of_subdirectories()
    {
        using var disposableDirectory = Create();

        var result = disposableDirectory.CreateDirectory("some_directory");

        var parentDirectory = Directory.GetParent(result)!.FullName;
        Assert.Equal(parentDirectory, disposableDirectory.Path);
    }

    [Fact]
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public void allows_the_creation_of_nested_subdirectories_providing_a_collection_of_directory_names()
    {
        using var disposableDirectory = Create();

        var result = disposableDirectory.CreateDirectory("dir1", "dir2", "dir3");

        var dir3 = new DirectoryInfo(result);
        var dir2 = dir3.Parent;
        var dir1 = dir2.Parent;
        var root = dir1.Parent;


        Assert.Equal(disposableDirectory.Path, root.FullName);
        Assert.Equal("dir1", dir1.Name);
        Assert.Equal("dir2", dir2.Name);
        Assert.Equal("dir3", dir3.Name);
    }

    [Fact]
    public void allows_the_creation_of_files()
    {
        using var disposableDirectory = Create();

        var fileName = disposableDirectory.RandomFileName();

        File.WriteAllText(fileName, "some text");

        Assert.Equal("some text", File.ReadAllText(fileName));
    }

    [Fact]
    public void created_files_are_deleted_during_disposal()
    {
        string fileName;
        using (var disposableDirectory = Create())
        {
            fileName = disposableDirectory.RandomFileName();
            File.WriteAllText(fileName, "some text");

            Assert.True(File.Exists(fileName));
        }

        Assert.False(File.Exists(fileName));
    }

    [Fact]
    public void file_names_are_random()
    {
        using var disposableDirectory = Create();

        var fileName1 = disposableDirectory.RandomFileName();
        var fileName2 = disposableDirectory.RandomFileName();

        Assert.NotEqual(fileName1, fileName2);
    }

    [Fact]
    public void calculates_paths_from_root()
    {
        using var disposableDirectory = Create();

        var path = disposableDirectory.Combine("one", "two", "three", "some-file.txt");

        Assert.Equal(Path.Combine(disposableDirectory.Path, "one", "two", "three", "some-file.txt"), path);
    }

    [Fact]
    void functional_syntax()
    {
        var fileName = "";

        InADisposable(directory =>
        {
            fileName = directory.RandomFileName();
            File.WriteAllText(fileName, "some content");

            Assert.True(File.Exists(fileName));
        });

        Assert.False(File.Exists(fileName));
    }
}

internal static class FileSystemHelpers
{
    private static string[] InnerDirectories(this string path) => Directory.GetDirectories(path);

    internal static bool Contains(this string container, DisposableDirectory disposableDirectory) =>
        container
            .InnerDirectories()
            .Contains(disposableDirectory.Path);

    internal static string WithSomeContent(this string filePath)
    {
        File.WriteAllText(filePath, "some text");
        return filePath;
    }

    internal static bool Exists(this string tempFile) =>
        File.Exists(tempFile);
}
