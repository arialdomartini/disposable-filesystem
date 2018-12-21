using System.IO;
using DisposableFileSystem;
using FluentAssertions;
using Xunit;

namespace DisposableFileSystemTest
{
    public class DisposableDirectoryTest
    {
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
            using (var sut = new DisposableDirectory())
            {
                var parent = Path.GetDirectoryName(sut.Path);

                parent.Should().Be(Path.GetDirectoryName(Path.GetTempPath()));
            }
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
    }
}